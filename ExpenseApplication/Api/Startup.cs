using System.Reflection;
using System.Text;
using Api.Middlewares;
using Application.Cqrs;
using Application.Mapper;
using Application.Services;
using Application.Validators;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.SqlServer;
using IbanNet.DependencyInjection.ServiceProvider;
using Infrastructure.Data.DbContext;
using Infrastructure.Dtos;
using Infrastructure.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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
        
        // Hangfire
        
        services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(Configuration.GetConnectionString("MsSqlConnection"), new SqlServerStorageOptions()
            {
                TransactionTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.FromMinutes(5),
            }));
        services.AddHangfireServer();


        
        services.AddEndpointsApiExplorer();
        services.AddHttpContextAccessor();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<INotificationService, NotificationService>();
        
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
        
        services.AddIbanNet();
        services.AddFluentValidationAutoValidation();
        services.AddScoped<IValidator<TokenRequest>, CreateTokenValidator>();
        
        services.AddScoped<IValidator<CreateExpenseRequest>, CreateExpenseValidator>();
        services.AddScoped<IValidator<UpdateExpenseRequest>, UpdateExpenseValidator>();
        
        services.AddScoped<IValidator<CreateExpenseCategoryRequest>, CreateExpenseCategoryValidator>();
        services.AddScoped<IValidator<UpdateExpenseCategoryRequest>, UpdateExpenseCategoryValidator>();
        services.AddScoped<IValidator<GetExpenseByParameterRequest>, GetExpenseByParameterRequestValidator>();

        services.AddScoped<IValidator<CreateUserRequest>, CreateUserValidator>();
        services.AddScoped<IValidator<UpdateUserRequest>, UpdateUserValidator>();
        
        services.AddScoped<IHandlerValidator, HandlerValidator>();


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

        app.UseHangfireDashboard();

        app.UseAuthentication();
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