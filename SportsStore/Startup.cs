using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Models;

namespace SportsStore
{
    public class Startup
    {
        public Startup(IConfiguration config)
        {
            Configuration = config;
        }

        private IConfiguration Configuration { get; set; }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseNpgsql(
                    "Host=localhost;Port=5432;Database=SportsStore;Username=postgres;Password=root");
            });
            services.AddScoped<IStoreRepository, EfStoreRepository>();
            services.AddScoped<IOrderRepository, EfOrderRepository>();
            services.AddRazorPages();
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddServerSideBlazor();

            services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseNpgsql(
                    "Host=localhost;Port=5432;Database=AuthDb;Username=postgres;Password=root");
            });
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("catpage",
                    "{category}/Page/{productPage:int}",
                    new {Controller = "Home", action = "Index"});

                endpoints.MapControllerRoute("page", "Page{productPage:int}",
                    new {Controller = "Home", action = "Index", productPage = 1});

                endpoints.MapControllerRoute("category", "{category}",
                    new {Controller = "Home", action = "Index", productPage = 1});

                endpoints.MapControllerRoute("pagination",
                    "Products/Page{productPage}",
                    new {Controller = "Home", action = "Index", productPage = 1});

                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/admin/{*catchall}", "/Admin/Index");
            });
            
            SeedData.EnsurePopulated(app);
            IdentitySeedData.EnsurePopulated(app);
        }
    }
}