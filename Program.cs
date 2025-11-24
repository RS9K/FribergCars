using FribergCars.Services;

namespace FribergCars
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // HttpContextAccessor
            builder.Services.AddHttpContextAccessor();

            // Storing session in memory
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // HttpClient for CarApiClient
            builder.Services.AddHttpClient<CarApiClient>((sp, client) =>
            {
                var cfg= sp.GetRequiredService<IConfiguration>();
                var baseUrl = cfg["ApiBaseUrl"];

                if (string.IsNullOrWhiteSpace(baseUrl))
                    throw new InvalidOperationException("APIBaseUrl is missing in appsettings.json");

                client.BaseAddress = new Uri(baseUrl);
            });

            // HttpClient for CustomerApiClient
            builder.Services.AddHttpClient<CustomerApiClient>((sp, client) =>
            {
                var cfg = sp.GetRequiredService<IConfiguration>();
                var baseUrl = cfg["ApiBaseUrl"];
                if (string.IsNullOrWhiteSpace(baseUrl))
                    throw new InvalidOperationException("APIBaseUrl is missing in appsettings.json");
                client.BaseAddress = new Uri(baseUrl);
            });

            // HttpClient for BookingApiClient
            builder.Services.AddHttpClient<BookingApiClient>((sp, client) =>
            {
                var cfg = sp.GetRequiredService<IConfiguration>();
                var baseUrl = cfg["ApiBaseUrl"];
                if (string.IsNullOrWhiteSpace(baseUrl))
                    throw new InvalidOperationException("APIBaseUrl is missing in appsettings.json");
                client.BaseAddress = new Uri(baseUrl);
            });

            // HttpClient for AdminApiClient
            builder.Services.AddHttpClient<AdminApiClient>((sp, client) =>
            {
                var cfg = sp.GetRequiredService<IConfiguration>();
                var baseUrl = cfg["ApiBaseUrl"];
                if (string.IsNullOrWhiteSpace(baseUrl))
                    throw new InvalidOperationException("APIBaseUrl is missing in appsettings.json");
                client.BaseAddress = new Uri(baseUrl);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
