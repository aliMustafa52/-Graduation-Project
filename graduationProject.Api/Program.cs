using graduationProject.Api;
using graduationProject.Api.Contracts.Authentication;
using graduationProject.Api.Persistence;
using Microsoft.Extensions.FileProviders;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


//call AddDependencies
builder.Services.AddDependencies(builder.Configuration);

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
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
