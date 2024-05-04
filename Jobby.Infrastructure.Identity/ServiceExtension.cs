using Jobby.Core.Application.Wrappers;
using Jobby.Core.Domain.Settings;
using Jobby.Infrastructure.Identity.Context;
using Jobby.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobby.Infrastructure.Identity
{
    public static class ServiceExtension
    {
        public static void AddInfrastructureIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityContext>(op => op.UseSqlServer(configuration.GetConnectionString("IdentityConnction"), m=>m.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)));

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();

            services.Configure<JWTSetting>(configuration.GetSection("JWTSetting"));

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JWTSetting:Issuer"],
                    ValidAudience = configuration["JWTSetting:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSetting:Key"]))
                };

                o.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();
                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "text/plain";
                        return c.Response.WriteAsync(c.Exception.ToString());
                    },

                    OnChallenge = c =>
                    {
                        c.HandleResponse();
                        c.Response.StatusCode = 401;
                        c.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new Response<string>("No estas autorizado"));
                        return c.Response.WriteAsync(result);
                    },

                    OnForbidden = c =>
                    {
                        c.Response.StatusCode = 400;
                        c.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new Response<string>("No tienes acceso a este recurso"));
                        return c.Response.WriteAsync(result);
                    }
                };
                
                
            });
        }
    }
}
