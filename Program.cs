using Microsoft.EntityFrameworkCore;
using PortfolioOpgave.Repositories;
using PortfolioOpgave.Models;
using PortfolioOpgave.Data;
using PortfolioOpgave.Interfaces;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using PortfolioOpgave.Services;
using AutoMapper;
using PortfolioOpgave.DTOs;

Console.Clear();

Console.WriteLine("Program starting...");
Console.WriteLine("Go to http://localhost:5000 to see the application.");
Console.WriteLine("Go to http://localhost:5000/swagger to see the API documentation.");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder => builder
            .WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PortfolioOpgave API", Version = "v1" });
});

// Configure DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

// Register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ISkillRepository, SkillRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Register services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<IEducationService, EducationService>();
builder.Services.AddScoped<IWorkExperienceService, WorkExperienceService>();
builder.Services.AddScoped<IProjectCategoryService, ProjectCategoryService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Add AutoMapper configuration
builder.Services.AddAutoMapper(config =>
{
    // Project mappings
    config.CreateMap<Project, ProjectDto>();
    config.CreateMap<CreateProjectDto, Project>();
    config.CreateMap<UpdateProjectDto, Project>();

    // Skill mappings
    config.CreateMap<Skill, SkillDto>();
    config.CreateMap<CreateSkillDto, Skill>();
    config.CreateMap<UpdateSkillDto, Skill>();

    // User and Auth mappings
    config.CreateMap<User, AuthResponseDto>();
    config.CreateMap<RegisterDto, User>();
    config.CreateMap<User, UserDto>();
    config.CreateMap<CreateUserDto, User>();
    config.CreateMap<UpdateUserDto, User>();

    // Education mappings
    config.CreateMap<Education, EducationDto>();
    config.CreateMap<CreateEducationDto, Education>();
    config.CreateMap<UpdateEducationDto, Education>();

    // Work Experience mappings
    config.CreateMap<WorkExperience, WorkExperienceDto>();
    config.CreateMap<CreateWorkExperienceDto, WorkExperience>();
    config.CreateMap<UpdateWorkExperienceDto, WorkExperience>();

    // Project Category mappings
    config.CreateMap<ProjectCategory, ProjectCategoryDto>();
});

var app = builder.Build();

// Add detailed request logging
app.Use(async (context, next) =>
{
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
    Console.WriteLine($"Headers: {string.Join(", ", context.Request.Headers.Select(h => $"{h.Key}={h.Value}"))}");
    Console.WriteLine($"Content-Type: {context.Request.ContentType}");
    Console.WriteLine($"Accept: {context.Request.Headers["Accept"]}");

    try
    {
        await next();
        Console.WriteLine($"Response: {context.Response.StatusCode}");
        Console.WriteLine($"Response Content-Type: {context.Response.ContentType}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
        throw;
    }
});

// Use CORS
app.UseCors("AllowAngularApp");

// Enable Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PortfolioOpgave API V1");
});

// Use static files
app.UseStaticFiles();

// Map API controllers first
app.MapControllers();

// Serve index.html for all other routes
app.MapFallbackToFile("index.html");

app.Run();
