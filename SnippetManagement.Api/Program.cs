using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SnippetManagement.Data;
using System.Text;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SnippetManagement.Service;
using SnippetManagement.Service.Implementation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<SnippetManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:IssuerSigningKey"])),
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidAudience = builder.Configuration["Jwt:ValidAudience"],
            ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
        };
        
    });
builder.Services.Configure<JwtConfiguration>(options => builder.Configuration.GetSection("Jwt").Bind(options));

builder.Services.AddControllers().AddFluentValidation(opts =>
{
    // Validate child properties and root collection elements
    opts.ImplicitlyValidateChildProperties = true;
    opts.ImplicitlyValidateRootCollectionElements = true;
    
    // Automatic registration of validators in assembly
    opts.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
});

builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISnippetTagService, SnippetTagService>();
builder.Services.AddScoped<ISnippetService, SnippetService>();
builder.Services.AddScoped<ITagService, TagService>();

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

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();