using Microsoft.EntityFrameworkCore;
using WebstoreAIU.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using WebstoreAIU.Models;
using WebstoreAIU.Services;

namespace WebstoreAIU
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? "Host=localhost;Database=webstore;Username=postgres;Password=password;Port=5432";

            builder.Services.AddDbContext<WebstoreDbContext>(options =>
                options.UseNpgsql(connectionString));

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login";
                    options.LogoutPath = "/Logout";
                    options.AccessDeniedPath = "/Login";
                });

            builder.Services.AddAuthorization();
            builder.Services.AddRazorPages();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<WebstoreDbContext>();
                dbContext.Database.EnsureCreated();
                
                var testUser = dbContext.Users.FirstOrDefault(u => u.Login == "test");
                if (testUser == null)
                {
                    testUser = new User
                    {
                        Login = "test",
                        PasswordHash = PasswordHasher.HashPassword("test")
                    };
                    dbContext.Users.Add(testUser);
                    dbContext.SaveChanges();
                }
                
                if (!dbContext.Items.Any())
                {
                    var laptopImage = ImageGenerator.GeneratePlaceholderImage("Laptop", 400, 300);
                    var mouseImage = ImageGenerator.GeneratePlaceholderImage("Mouse", 400, 300);
                    var keyboardImage = ImageGenerator.GeneratePlaceholderImage("Keyboard", 400, 300);
                    var monitorImage = ImageGenerator.GeneratePlaceholderImage("Monitor", 400, 300);
                    var headphonesImage = ImageGenerator.GeneratePlaceholderImage("Headphones", 400, 300);
                    
                    var laptopAdditional = new List<ItemImage>
                    {
                        new ItemImage { Base64Data = Convert.ToBase64String(ImageGenerator.GeneratePlaceholderImage("Laptop Side", 400, 300)), ContentType = "image/svg+xml" },
                        new ItemImage { Base64Data = Convert.ToBase64String(ImageGenerator.GeneratePlaceholderImage("Laptop Open", 400, 300)), ContentType = "image/svg+xml" }
                    };
                    
                    var monitorAdditional = new List<ItemImage>
                    {
                        new ItemImage { Base64Data = Convert.ToBase64String(ImageGenerator.GeneratePlaceholderImage("Monitor Side", 400, 300)), ContentType = "image/svg+xml" }
                    };
                    
                    dbContext.Items.AddRange(
                        new Item 
                        { 
                            Name = "Laptop", 
                            Price = 999.99m, 
                            Description = "High-performance laptop with latest processor and 16GB RAM. Perfect for work and gaming.",
                            MainImage = laptopImage,
                            AdditionalImagesJson = ImageService.SerializeAdditionalImages(laptopAdditional),
                            OwnerId = testUser.Id
                        },
                        new Item 
                        { 
                            Name = "Mouse", 
                            Price = 29.99m, 
                            Description = "Wireless ergonomic mouse with precision tracking. Comfortable for long hours of use.",
                            MainImage = mouseImage,
                            AdditionalImagesJson = "[]",
                            OwnerId = testUser.Id
                        },
                        new Item 
                        { 
                            Name = "Keyboard", 
                            Price = 79.99m, 
                            Description = "Mechanical keyboard with RGB backlighting. Tactile keys for the best typing experience.",
                            MainImage = keyboardImage,
                            AdditionalImagesJson = "[]",
                            OwnerId = testUser.Id
                        },
                        new Item 
                        { 
                            Name = "Monitor", 
                            Price = 299.99m, 
                            Description = "27-inch 4K monitor with HDR support. Crystal clear display for professional work.",
                            MainImage = monitorImage,
                            AdditionalImagesJson = ImageService.SerializeAdditionalImages(monitorAdditional),
                            OwnerId = testUser.Id
                        },
                        new Item 
                        { 
                            Name = "Headphones", 
                            Price = 149.99m, 
                            Description = "Noise-cancelling wireless headphones with premium sound quality. Perfect for music lovers.",
                            MainImage = headphonesImage,
                            AdditionalImagesJson = "[]",
                            OwnerId = testUser.Id
                        }
                    );
                    dbContext.SaveChanges();
                }
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
