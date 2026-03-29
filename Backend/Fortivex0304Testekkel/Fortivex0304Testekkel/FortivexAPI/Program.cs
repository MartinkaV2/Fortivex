using FortivexAPI.Data;
using FortivexAPI.Helpers;
using FortivexAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .SetIsOriginAllowed(origin => true)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Register DbContext and application services
// Use Pomelo MySQL provider (UseMySql) instead of UseMySQL to ensure compatibility.
builder.Services.AddDbContext<FortivexContext>(options =>
{
    var connStr = builder.Configuration.GetConnectionString("DefaultConnection")!;

    // Az AutoDetect-et TÖRÖLD ki, és használd ezt helyette:
    // Ez kifejezetten a te 10.11.15-ös MariaDB verziódra van beállítva.
    var serverVersion = new MariaDbServerVersion(new Version(10, 11, 15));

    options.UseMySql(connStr, serverVersion);
});

// Register application services and helpers
builder.Services.AddScoped<JwtHelper>();
builder.Services.AddScoped<UserService>();

// JWT Authentication configuration
//var jwtKey = builder.Configuration["Jwt:Key"] ?? "YourSuperSecretKeyForDevelopment12345";
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = false,
//            ValidateAudience = false,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
//        };
//    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Use Swagger only in development for API documentation
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseRouting();

app.Use(async (context, next) =>
{
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.StatusCode = 200;
        await context.Response.CompleteAsync();
    }
    else
    {
        await next();
    }
});

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();

public partial class Program { }

