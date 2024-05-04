using Jobby.Core.Application.DTOS.Account;
using Jobby.Core.Application.Enums;
using Jobby.Core.Application.Helpers;
using Jobby.Core.Application.Interfaces.Services;
using Jobby.Core.Application.Wrappers;
using Jobby.Core.Domain.Settings;
using Jobby.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Jobby.Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JWTSetting _jwtSetting;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, Option<JWTSetting> jwtSetting)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSetting = jwtSetting.DefaultValue;
        }

        public async Task<Response<AuthenticationResponse>> AuthenticateAsyncApi(AuthenticationRequest request, string ipAddress)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new Response<AuthenticationResponse>("No se han encontrado usuario con el email: " + request.Email);
            }

            var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return new Response<AuthenticationResponse>("Hubo un error al intentar iniciar sesion");
            }

            JwtSecurityToken jwtSecurityToken = await GenerateSecurityTokenAsync(user);
            AuthenticationResponse response = new();
            response.Id = user.Id;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.Email = user.Email;
            response.UserName = user.UserName;

            var roleList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            response.Roles = roleList.ToList();
            response.IsVerified = user.EmailConfirmed;

            var refreshToken = GenerateRefreshToken(ipAddress);
            response.RefreshToken = refreshToken.Token;
            return new Response<AuthenticationResponse>(response, "Autenticado");

        }

        public async Task<Response<string>> RegisterAsyncApi(RegisterRequest request, string origin)
        {
            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                return new Response<string>("Ya hay un usuario con el mismo username");
            }

            var user = new ApplicationUser()
            {
                FirstName = request.Name,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.UserName,
                EmailConfirmed = true
            };

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail != null)
            {
                return new Response<string>("Ya hay un usuario con el mismo email");
            }

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.User.ToString());
            }

            return new Response<string>(user.Id, message: "Usuario registrado exitosamente");

        }

        private async Task<JwtSecurityToken> GenerateSecurityTokenAsync(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }

            string ipAddress = IpHelper.GetIp();
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
                new Claim("ip", ipAddress)

            }.Union(userClaims)
            .Union(roleClaims);

            var symetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Key));
            var signingCredentials = new SigningCredentials(symetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSetting.Issuer,
                audience: _jwtSetting.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSetting.DurationInMinutes),
                signingCredentials: signingCredentials
                );

            return jwtSecurityToken;
        }

        private RefreshToken GenerateRefreshToken(string IpAddress)
        {
            return new RefreshToken()
            {
                Token = RandomTokenString(),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now,
                CreatedByIp = IpAddress

                
            };
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var ramdomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(ramdomBytes);

            return BitConverter.ToString(ramdomBytes).Replace("-", "");
        }

    }
}

