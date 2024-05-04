using Jobby.Infrastructure.Identity.Entities;
using Jobby.Infrastructure.Identity.Seeds;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var provider = scope.ServiceProvider;
    try
    {
        var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();

        await DefaultRoles.SeedAsync(userManager, roleManager);
        await DefaultEnterprise.SeedAsync(userManager, roleManager);
        await DefaultBasicUser.SeedAsync(userManager, roleManager);
    }
    catch (Exception e)
    {

    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
