namespace ShopBridgeInventory
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using ShopBridgeInventory.Models;
    using ShopBridgeInventory.Repositories;
    using ShopBridgeInventory.Services;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ShopInventoryContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:InventoryDB"]));            
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped< IDataRepostiory<Product>, ProductRepostiory >();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ShopInventoryContext>();
                context.Database.EnsureCreated();
            }
        }
    }
}
