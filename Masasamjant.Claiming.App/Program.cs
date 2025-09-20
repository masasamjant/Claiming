using Masasamjant.Http;

namespace Masasamjant.Claiming.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var topSection = builder.Configuration.GetRequiredSection("Masasamjant");
            var claimingSection = topSection.GetRequiredSection("Claiming");
            var managerSection = claimingSection.GetRequiredSection("Manager");

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register claim manager factory.
            builder.Services.AddClaimManagerFactory(builder.Configuration, new HttpClientConfiguration());
            
            var app = builder.Build();

            // Check if HTTPS requirement is configured.
            if (!bool.TryParse(claimingSection["RequireHttps"], out var requireHttps))
            {
                // If not configured, then require HTTPS in production environment.
                requireHttps = app.Environment.IsProduction();
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                
                if (requireHttps)
                {
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }
            }

            if (requireHttps)
            {
                app.UseHttpsRedirection();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
