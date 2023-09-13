using Ecommerce.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

        builder.Services.AddDbContext<MyContext>();

        // Cookie authentication configuration
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
        {
            options.LogoutPath = "/Account/Login";
            options.Cookie.Name = "Home"; // any name
        });

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<MyContext>();

            // Ensure the database is created (if it doesn't exist) and apply migrations
            dbContext.Database.EnsureCreated();

            // Seed users
            SeedUsers(dbContext);
        }

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }

    private static void SeedUsers(MyContext dbContext)
    {
        if (!dbContext.Users.Any())
        {
            var users = new List<User>
            {
                new User
                {
                    email = "nada.fcai@gmail.com",
                    userName = "nadaabutaleb",
                    password = "Nada2001#",
                    Role = "Admin",
                    Age= 22
                },
                new User
                {
                    email = "osama@gmail.com",
                    userName = "osamaabutaleb",
                    password = "Osama2006#",
                    Role = "Admin",
                    Age= 18
                }
            };

            dbContext.Users.AddRange(users);
            dbContext.SaveChanges();
        }
    }
}
