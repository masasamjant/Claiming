using Masasamjant.Claiming.Http;
using Masasamjant.Claiming.Memory;
using Masasamjant.Claiming.SqlServer;

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
            
            // Get configuration what kind of claim manager should be used.
            var managerType = managerSection["Type"];

            // Claim manager type configuration is mandatory.
            if (string.IsNullOrWhiteSpace(managerType))
                throw new InvalidOperationException("Masasamjant.Claming.Manager.Type configuration is mandatory.");

            // Register correct claim manager factory implementation.
            switch (managerType)
            {
                case ClaimManagerType.MemoryClaimManager:
                    builder.Services.AddTransient<IClaimManagerFactory, MemoryClaimManagerFactory>();
                    break;
                case ClaimManagerType.HttpClaimManager:
                    builder.Services.AddHttpClient();
                    builder.Services.AddTransient<IClaimManagerFactory, HttpClaimManagerFactory>();
                    break;
                case ClaimManagerType.EntityClaimManager:
                    builder.Services.AddTransient<IClaimManagerFactory, EntityClaimManagerFactory>();
                    break;
                default:
                    throw new NotSupportedException($"'{managerType}' is not supported claim manager.");
            }

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
