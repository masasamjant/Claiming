using DemoApp.Core;
using Masasamjant.Claiming;
using Masasamjant.Http;
using Masasamjant.Http.Abstractions;

namespace DemoApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register repositories
            var productRepository = new ProductRepository();
            var userRepository = new UserRepository();
            builder.Services.AddSingleton<IRepository<Product>>(productRepository);
            builder.Services.AddSingleton<IRepository<User>>(userRepository);

            // Register JSON Http client builder
            builder.Services.AddJsonHttpClientBuilder(new HttpClientConfiguration(), new ConfigurationHttpBaseAddressProviderFactory(builder.Configuration, "Masasamjant"), HttpCacheManager.Default);

            // Register claim manager.
            builder.Services.AddClaimManagerFactory(builder.Configuration);

            // Register services.
            builder.Services.AddTransient<IService<string, User>, UserService>();
            builder.Services.AddTransient<IService<Guid, Product>, ProductService>();

            builder.Services.AddMemoryCache();
            builder.Services.AddSession();

            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            productRepository.Initialize(app.Environment.WebRootPath);
            userRepository.Initialize(app.Environment.WebRootPath);

            app.Run();
        }
    }
}
