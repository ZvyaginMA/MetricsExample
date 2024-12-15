using Microsoft.AspNetCore.Mvc;
using Prometheus;

namespace MetricsExample
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c => 
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Simple API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapMetrics(); // Эндпоинт для метрик Prometheus
            });
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class MetricsController : ControllerBase
    {
        private static readonly Counter requestCounter = Metrics.CreateCounter("request_count", "Number of requests received");

        // Простой метод, который увеличивает счетчик при каждом запросе
        [HttpGet]
        public IActionResult Get()
        {
            requestCounter.Inc();
            return Ok("Hello, world!");
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}