using AssetsAPI.AssetsAPI.Crypto.v1;
using AssetsAPI.AssetsAPI.Crypto.v1.Mappers;
using AssetsAPI.AssetsAPI.Crypto.v1.Providers.Coinbase;
using AssetsAPI.AssetsAPI.Crypto.v1.Proxies;
using AssetsAPI.AssetsAPI.Crypto.v1.Repository;
using AssetsAPI.Data;
using CoinbasePro;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AssetsAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            // Crypto Ioc
            services.AddScoped<ICryptoRepository, CryptoRepository>();
            services.AddScoped<ICryptoManager, CryptoManager>();
            services.AddScoped<ICryptoCurrencyPriceProvider, CryptoCurrencyPriceProvider>();
            services.AddScoped<ICryptoProviderProxy, CryptoProviderProxy>();
            services.AddScoped<ICoinbaseProxy, CoinbaseProxy>();
            services.AddScoped<ICryptoPriceMapper, CryptoPriceMapper>();
            services.AddScoped<IPerformanceResultMapper, PerformanceResultMapper>();

            var authenticator = new CoinbasePro.Network.Authentication.Authenticator("credentials",
                "were", "here");
            services.AddSingleton<ICoinbaseProClient>(new CoinbaseProClient(authenticator));
            services.AddDbContext<CurrencyContext>(p => p.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));


            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AssetsAPI v1");
                c.RoutePrefix = string.Empty;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
