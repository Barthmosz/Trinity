using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Trinity.Application.Contracts;
using Trinity.Application.Mapping;
using Trinity.Application.Services;
using Trinity.Application.Wrappers;
using Trinity.Domain.Entities.Accounts;
using Trinity.Domain.Entities.Products;
using Trinity.Persistence;
using Trinity.Persistence.ConnectionConfig;
using Trinity.Persistence.Contexts;
using Trinity.Persistence.Contracts;
using Trinity.Persistence.Persistence;

namespace Trinity.API
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.Configure<DbOptions>(options =>
            {
                options.Connection = this.Configuration.GetSection("DatabaseSettings:Connection").Value;
                options.Name = this.Configuration.GetSection("DatabaseSettings:Name").Value;
            });

            byte[] key = Encoding.ASCII.GetBytes(this.Configuration.GetSection("JwtKey").Value!);

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


            services.AddAutoMapper(typeof(DomainToMappingProfile));

            services.AddScoped<IConnectionConfig, ConnectionConfig>();
            services.AddScoped<IMongoDbContext, MongoDbContext>();

            services.AddScoped<IPasswordHasherWrapper, PasswordHasherWrapper>();
            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<IStaticPersistence<Products>, StaticPersistence<Products>>();
            services.AddScoped<IStaticPersistence<Accounts>, StaticPersistence<Accounts>>();
            services.AddScoped<IBasePersistence<Products>, BasePersistence<Products>>();
            services.AddScoped<IBasePersistence<Accounts>, BasePersistence<Accounts>>();

            services.AddScoped<IProductsService, ProductsService>();
            services.AddScoped<IAccountsService, AccountsService>();
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
