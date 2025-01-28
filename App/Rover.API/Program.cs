using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Rover.Infrastructure;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Rover.API;
using Rover.Application.BikeInfo;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Настройка аутентификации
builder.Services.AddAuthentication("TestScheme")
    .AddCookie("TestScheme", options =>
    {
        options.LoginPath = "/Account/Login"; // Задайте путь для логина, если потребуется
        options.Cookie.Name = "TestAuthCookie";
    });

builder.Services.AddControllers(opt =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
});

// Регистрация MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<BikeList>());

// Регистрация DbContext
builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("AzureDb"));
});

builder.Services.AddScoped(typeof(IAppLogger<>), typeof(AppLogger<>));

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .WithOrigins("http://localhost:7057");
    });
});

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

// Подключение аутентификации и авторизации
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Проверка подключения к базе данных
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<DataContext>();

    await context.Database.MigrateAsync();
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}

app.Run();
