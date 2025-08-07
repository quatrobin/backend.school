using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using WebApplication3.Data;
using WebApplication3.Models.Common;
using WebApplication3.Services.Interfaces;
using WebApplication3.Services.Implementations;
using WebApplication3.Middleware;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Настройка Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();

// Добавляем CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.SetIsOriginAllowed(origin => 
        {
            var isAllowed = origin.StartsWith("http://localhost:") && 
                (origin.Contains("4200") || origin.Contains("4201") || origin.Contains("4202") || 
                 origin.Contains("4203") || origin.Contains("4204") || origin.Contains("4205") ||
                 origin.Contains("4206") || origin.Contains("4207") || origin.Contains("4208") ||
                 origin.Contains("4209") || origin.Contains("4210"));
            
            Console.WriteLine($"CORS: Origin {origin} is {(isAllowed ? "allowed" : "denied")}");
            return isAllowed;
        })
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
    
    // Добавляем более простую политику для разработки
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { 
        Title = "School API", 
        Version = "v1",
        Description = "API для системы управления образовательным процессом с интеграцией Elasticsearch",
        Contact = new OpenApiContact
        {
            Name = "School API Support",
            Email = "support@school.com"
        }
    });
    
    // Добавляем поддержку JWT в Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    // Включаем аннотации Swagger
    c.EnableAnnotations();
    
    // Группируем endpoints по тегам
    c.TagActionsBy(api =>
    {
        if (api.GroupName != null)
        {
            return new[] { api.GroupName };
        }

        var controllerActionDescriptor = api.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;
        if (controllerActionDescriptor != null)
        {
            return new[] { controllerActionDescriptor.ControllerName };
        }

        return new[] { api.ActionDescriptor.RouteValues["controller"] };
    });
});

// Добавляем DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Добавляем JWT аутентификацию
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not found"))),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// Добавляем авторизацию
builder.Services.AddAuthorization();

// Настройка Prometheus метрик
Metrics.DefaultRegistry.SetStaticLabels(new Dictionary<string, string>
{
    { "app", "school-api" },
    { "version", "1.0.0" }
});

// Конфигурация Elasticsearch
builder.Services.Configure<ElasticsearchSettings>(
    builder.Configuration.GetSection("Elasticsearch"));

// Регистрируем сервисы
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IAssignmentService, AssignmentService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ILessonService, LessonService>();
builder.Services.AddScoped<ISubmissionService, SubmissionService>();
builder.Services.AddScoped<IElasticsearchService, ElasticsearchService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

// Добавляем CORS middleware
app.UseCors("AllowAll");

// Добавляем логирование запросов
app.UseRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

// Добавляем endpoint для Prometheus метрик
app.UseMetricServer();
app.UseHttpMetrics();

app.MapControllers();

// Настройка завершения приложения
app.Lifetime.ApplicationStopped.Register(Log.CloseAndFlush);

try
{
    Log.Information("Запуск School API...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "School API завершился с ошибкой");
}
finally
{
    Log.CloseAndFlush();
}