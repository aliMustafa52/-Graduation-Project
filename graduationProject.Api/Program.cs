using graduationProject.Api;
using graduationProject.Api.Persistence;

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

app.UseAuthorization();



app.MapControllers();

app.Run();
