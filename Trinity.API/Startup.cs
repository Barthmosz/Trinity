using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Trinity.Application.Contracts;
using Trinity.Application.Mapping;
using Trinity.Application.Services;
using Trinity.Domain;
using Trinity.Persistence;
using Trinity.Persistence.ConnectionConfig;
using Trinity.Persistence.Contexts;
using Trinity.Persistence.Contracts;
using Trinity.Persistence.Persistence;

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

            byte[] key = Encoding.ASCII.GetBytes(ConfigurationApp.JwtKey);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddTransient<TokenService>();

            services.AddAutoMapper(typeof(DomainToMappingProfile));

            services.AddScoped<IConnectionConfig, ConnectionConfig>();
            services.AddScoped<IMongoDbContext, MongoDbContext>();

            services.AddScoped<IStaticPersistence<Products>, StaticPersistence<Products>>();
            services.AddScoped<IStaticPersistence<Users>, StaticPersistence<Users>>();
            services.AddScoped<IBasePersistence<Products>, BasePersistence<Products>>();
            services.AddScoped<IBasePersistence<Users>, BasePersistence<Users>>();

            services.AddScoped<IProductsService, ProductsService>();
            services.AddScoped<IUsersService, UsersService>();
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
            application.UseAuthentication();
            application.UseAuthorization();
            application.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
