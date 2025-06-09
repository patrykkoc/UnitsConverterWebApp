using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using UnitsConverterWebApp.Data;

namespace UnitsConverterWebApp.Models
{
    public static class SeedData
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {
           
            using (var context = new UnitsConverterWebAppContext(
                serviceProvider.GetRequiredService<DbContextOptions<UnitsConverterWebAppContext>>()))
            {
                Category distanceCategory;
                Category temperatureCategory;

                // Look for any categories
                if (!context.Categories.Any())
                {
                      distanceCategory = new Category { Name = "Distance" };
                      temperatureCategory = new Category { Name = "Temperature" };

                    context.Categories.AddRange(distanceCategory, temperatureCategory);
                    context.SaveChanges();

                    
                }
                else
                {
                    distanceCategory = context.Categories.First(c => c.Name == "Distance");
                    temperatureCategory = context.Categories.First(c => c.Name == "Temperature");
                }
                //Seed categories
                if (!context.Units.Any())
                {
                    context.Units.AddRange(
                        // DISTANCE
                        new Unit
                        {
                            Name = "Meter",
                            Symbol = "m",
                            MultiplierToBase = 1,
                            OffsetToBase = 0,
                            IsBaseUnit = true,
                            CategoryId = distanceCategory.Id
                        },
                        new Unit
                        {
                            Name = "Centimeter",
                            Symbol = "cm",
                            MultiplierToBase = 0.01,
                            OffsetToBase = 0,
                            IsBaseUnit = false,
                            CategoryId = distanceCategory.Id
                        },

                        // TEMPERATURE
                        new Unit
                        {
                            Name = "Celsius",
                            Symbol = "°C",
                            MultiplierToBase = 1,
                            OffsetToBase = 0,
                            IsBaseUnit = true,
                            CategoryId = temperatureCategory.Id
                        },
                        new Unit
                        {
                            Name = "Fahrenheit",
                            Symbol = "°F",
                            MultiplierToBase = 5.0 / 9.0,           
                            OffsetToBase = -32 * (5.0 / 9.0),       
                            IsBaseUnit = false,
                            CategoryId = temperatureCategory.Id
                        }
                    );

                    context.SaveChanges();
                }



                if (!context.Roles.Any())
                {
                    context.Roles.AddRange(
                        new Role {   Name = "Admin" },
                        new Role {  Name = "User" }
                    );
                    context.SaveChanges();
                }


                if (!context.Users.Any())
                {
                    var adminRole = context.Roles.FirstOrDefault(r => r.Name == "Admin");
                    var userRole = context.Roles.FirstOrDefault(r => r.Name == "User");

                    context.Users.AddRange(
                        new User
                        {

                            Username = "admin",
                            PasswordHash = "admin",
                            RoleId = adminRole.Id

                        },
                        new User
                        {

                            Username = "user",
                            PasswordHash = "user",
                            RoleId = userRole.Id

                        }
                    );

                    context.SaveChanges();
                }


            }
        }
    }
}
