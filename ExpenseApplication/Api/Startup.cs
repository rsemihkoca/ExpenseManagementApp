using System.Reflection;
using System.Text;
using Api.Middlewares;
using Application.Cqrs;
using Application.Mapper;
using Application.Validators;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Data.DbContext;
using Infrastructure.Dtos;
using Infrastructure.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

//
// using FluentValidation;
// using FluentValidation.AspNetCore;
// using Api.Middleware;
// using Api.Schema;
// using Api.Service;
// using Api.Validator;

namespace Api;

public class Startup
{
    public IConfiguration Configuration;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        
        
        services.AddDbContext<ExpenseDbContext>(options =>
        {
            options.UseSqlServer(Configuration.GetConnectionString("MsSqlConnection"));
        });
        
        // MediatR
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
        }

        // AutoMapper
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MapperConfig()));
        services.AddSingleton(mapperConfig.CreateMapper());
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Expense Management Api", Version = "v1.0" });

            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Expense Management Api for IT Company",
                Description = "Enter JWT Bearer token **_only_**",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
            c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, new string[] { } }
            });
        });


        JwtConfig jwtConfig = Configuration.GetSection("JwtConfig").Get<JwtConfig>();
        services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = true;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtConfig.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.Secret)),
                ValidAudience = jwtConfig.Audience,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(2)
            };
        });
        
        services.AddHealthChecks();
        services.AddControllers();
        
        
        services.AddFluentValidationAutoValidation();
        services.AddScoped<IValidator<TokenRequest>, CreateTokenValidator>();
        
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHealthChecks("/health");
        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

        app.UseRouting();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}