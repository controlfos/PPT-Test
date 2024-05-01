using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PPT_Test.Model;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// ADD... services
var connectionString = "Data Source=Data\\data.db";
builder.Services.AddDbContext<ImagesContext>(options =>
{
    options.UseSqlite(connectionString);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// REG... HttpClientFactory
builder.Services.AddHttpClient();

var app = builder.Build();
var env = app.Environment;

// CONF... HTTP request
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Awesome PPT Api Test");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseStaticFiles();

// CONF... root
app.MapGet("/", async context =>
{
    await context.Response.SendFileAsync(Path.Combine(env.ContentRootPath, "wwwroot", "index.html"));
});

app.MapControllers();
app.Run();
