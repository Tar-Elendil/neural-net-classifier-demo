using ML.NET.API.Services;
using ML.NET.API.Validators;

namespace ML.NET.API;

public class Startup
{
    private const string CorsPolicy = "ContainerServicePolicy";

    public void ConfigureServices(IServiceCollection services)
    {
        // Enables the framework to discover your API controllers
        services.AddControllers();

        // Register Swagger for API documentation
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // Register Services
        services.AddSingleton<IPredictClassifications, PredictionService>();
        services.AddSingleton<CardTransactionValidator>();

        // CORS
        services.AddCors(opt =>
        {
            opt.AddPolicy(CorsPolicy, builder =>
            {
            });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors(CorsPolicy);

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}