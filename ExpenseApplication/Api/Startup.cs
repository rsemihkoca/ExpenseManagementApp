using Api.Middlewares;
using Infrastructure.Data.DbContext;
using Microsoft.EntityFrameworkCore;
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

        services.AddHealthChecks();
        services.AddControllers();
        // services.AddFluentValidationAutoValidation();
        // services.AddScoped<IValidator<CalculateInterestRequest>, CalculateInterestRequestValidator>();
        services.AddDbContext<ExpenseDbContext>(options =>
        {
            options.UseSqlServer(Configuration.GetConnectionString("MsSqlConnection"));
        });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
    
    public void Configure(IApplicationBuilder app,IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHealthChecks("/health");
        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        
        app.UseDefaultFiles();
        app.UseStaticFiles();
        // app.UseHttpsRedirection();

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