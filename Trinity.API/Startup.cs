using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Trinity.API
{
  public class Startup
  {
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
      this.Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();
      services.AddEndpointsApiExplorer();
      services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder application, IWebHostEnvironment environment)
    {
      if (environment.IsDevelopment())
      {
        application.UseSwagger();
        application.UseSwaggerUI();
      }

      application.UseHttpsRedirection();
      application.UseRouting();
      application.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
      application.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
