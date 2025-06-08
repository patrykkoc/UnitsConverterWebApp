using Microsoft.EntityFrameworkCore;
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
              
                // Look for any categories
                if (context.Categories.Any())
                {
                    return; // DB has been seeded
                }

               // Seed categories
                var distanceCategory = new Category { Name = "Distance" };
                var temperatureCategory = new Category { Name = "Temperature" };

                context.Categories.AddRange(distanceCategory, temperatureCategory);
                context.SaveChanges();
                // Seed units
                context.Units.AddRange(
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
        }
    }
}
