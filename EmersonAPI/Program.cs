using EmersonAPI;
using EmersonAPI.BusinessService;
using EmersonAPI.Interfaces;
using EmersonAPI.Repositories;
using EmersonDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using System.Linq.Expressions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<EmersonContext>(opt => opt.UseSqlServer("Server=tcp:emerson-testdb.database.windows.net,1433;Initial Catalog=EmersonTest;Persist Security Info=False;User ID=EmersonAdmin;Password=emerson123!.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICityRepo, CityRepo>();
builder.Services.AddScoped<IVariableRepo, VariableRepo>();
builder.Services.AddScoped<IVariableService, VariableService>();
builder.Services.AddScoped<ICityService, CityService>();

/*
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen(x => x.SwaggerDoc("v1", new OpenApiInfo() { Title = "Emerson API v1", Version = "V1" }));
}
*/


builder.Services.AddCors(x => x.AddPolicy("ExclusivePolicy", y => {
    y.WithOrigins("https://emerson-web.azurewebsites.net").AllowAnyHeader().AllowAnyMethod();
    y.WithOrigins("https://localhost:7023").AllowAnyHeader().AllowAnyMethod();
}));

var app = builder.Build();

/*
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseSwagger(x=> { x.SerializeAsV2 = true; });
    //app.UseSwaggerUI(opt => { opt.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"); opt.RoutePrefix = string.Empty; });
}
*/

app.UseMiddleware<CustomExceptionHandling>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("ExclusivePolicy");

app.Run();
