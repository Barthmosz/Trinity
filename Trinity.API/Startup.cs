using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Trinity.Persistence;
using Trinity.Persistence.ConnectionConfig;
using Trinity.Persistence.Contexts;

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
      services.Configure<DbOptions>(options =>
      {
        options.Connection = this.Configuration.GetSection("DatabaseSettings:Connection").Value;
        options.Name = this.Configuration.GetSection("DatabaseSettings:Name").Value;
      });
      services.AddScoped<IConnectionConfig, ConnectionConfig>();
      services.AddScoped<IMongoDbContext, MongoDbContext>();
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
