using graduationProject.Api;
using graduationProject.Api.Contracts.Authentication;
using graduationProject.Api.Hubs;
using graduationProject.Api.Persistence;
using graduationProject.Api.Seeds;
using Microsoft.Extensions.FileProviders;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


//call AddDependencies
builder.Services.AddDependencies(builder.Configuration);

//add SignalR
builder.Services.AddSignalR();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//new StaticFileOptions
//{
//	FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "images")),
//	RequestPath = "/Resources"
//}

app.UseStaticFiles();

//before Authorization
app.UseCors("Default");

//add Roles and admin
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using var scope = scopeFactory.CreateScope();

var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

await DefaultRoles.SeedAsync(roleManager);
await DefaultUsers.SeedAdminUserAsync(userManager);

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

//add SignalR
app.MapHub<ChatHub>("/hubs/chat");

app.Run();
