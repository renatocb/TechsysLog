using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TechsysLog.Application.Services;
using TechsysLog.Domain.Interfaces;
using TechsysLog.Infrastructure.Data;
using TechsysLog.Infrastructure.Repositories;
using TechsysLog.Infrastructure.External.ViaCep.Services;
using TechsysLog.Infrastructure.External.ViaCep.Interfaces;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Identity;
using TechsysLog.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TechsysLog.Infrastructure.Services;
using TechsysLog.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TechsysLog.API.Services;
using Microsoft.Extensions.Logging;
using Serilog;
using Microsoft.Net.Http.Headers;
using TechsysLog.Application.Hubs;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console() // Escreve logs no console
    .WriteTo.File("logs/efcore.log", rollingInterval: RollingInterval.Day) // Escreve logs em um arquivo
    .CreateLogger();

builder.Host.UseSerilog(); // Usa o Serilog como provedor de logs

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
              .EnableSensitiveDataLogging() // Habilita o log de dados sensíveis
              .LogTo(Log.Logger.Information, LogLevel.Information));

builder.Services.AddScoped(typeof(IRepositorio<>), typeof(Repositorio<>));
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IEntregaService, EntregaService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPedidoEntregaService, PedidoEntregaService>();
builder.Services.AddScoped<PedidoService>();
builder.Services.AddScoped<INotificationRepositorio, NotificationRepositorio>();

builder.Services.AddHttpClient<IViaCepService, ViaCepService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TechsysLog.API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
    {
        new OpenApiSecurityScheme{
            Reference = new OpenApiReference{
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[]{}
    }});
});

var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            if (!string.IsNullOrEmpty(accessToken))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:64249")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials(); // Permite credenciais
    });
});

builder.Services.AddControllers();
builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TechsysLog.API v1"));
    app.UseRewriter(new RewriteOptions().AddRedirect("^$", "swagger"));
}

app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    app.MapControllers();
    endpoints.MapHub<PedidoEntregaHub>("/pedidoEntregaHub");
});

await app.RunAsync();