using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SnippetManagement.Data;
using System.Text;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SnippetManagement.Api.Middlewares;
using SnippetManagement.Api.Service;
using SnippetManagement.Service.Repositories;
using SnippetManagement.Service.Repositories.Implementation;
using SnippetManagement.Service.Services;
using SnippetManagement.Service.Services.Implementation;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddDbContext<SnippetManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["IssuerSigningKey"] ?? throw new InvalidOperationException())),
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidAudience = jwtSection["ValidAudience"],
        ValidIssuer = jwtSection["ValidIssuer"],
    };
});
builder.Services.Configure<JwtConfiguration>(options => builder.Configuration.GetSection("Jwt").Bind(options));

var portsSection = builder.Configuration.GetSection("Ports");
builder.Services.AddCors(opts =>
{
    opts.AddPolicy(name: "_myAllowSpecificOrigins",
        policy =>
        {
            policy.WithOrigins($"http://localhost:{portsSection["ReactAppPort"]}")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddControllers(opts => { opts.Filters.Add(new HandleApiExceptionAttribute()); }).AddFluentValidation(
    opts =>
    {
        // Validate child properties and root collection elements
        opts.ImplicitlyValidateChildProperties = true;
        opts.ImplicitlyValidateRootCollectionElements = true;

        // Automatic registration of validators in assembly
        opts.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    });
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<IIdentityService, IdentityService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme()
    {
        Description = "Standard Authorization header using the Bearer scheme",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Name = "Bearer",
                In = ParameterLocation.Header,
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    if (scope.ServiceProvider.GetService<SnippetManagementDbContext>()?.Database.ProviderName !=
        "Microsoft.EntityFrameworkCore.InMemory")
        scope.ServiceProvider.GetService<SnippetManagementDbContext>()?.Database.Migrate();
}

app.UseCors("_myAllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
var folderSection = builder.Configuration.GetSection("Folder");
app.UseFileServer(new FileServerOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), folderSection["Assets"] ?? throw new InvalidOperationException())),
    RequestPath = $"/{folderSection["Assets"]}",
    EnableDefaultFiles = true
});

app.Run();

public partial class Program
{
}