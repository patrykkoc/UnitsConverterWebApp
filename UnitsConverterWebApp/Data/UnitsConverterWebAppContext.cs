using Microsoft.EntityFrameworkCore;
using UnitsConverterWebApp.Models;

namespace UnitsConverterWebApp.Data
{
    public class UnitsConverterWebAppContext : DbContext
    {
        public UnitsConverterWebAppContext (DbContextOptions<UnitsConverterWebAppContext> options)
            : base(options)
        {
        }

      
        public DbSet<Category> Categories { get; set; } = default!;
        public DbSet<Unit> Units { get; set; } = default!;
        public DbSet<HistoryEntry> Histories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unit -> Category
            modelBuilder.Entity<Unit>()
                .HasOne(u => u.Category)
                .WithMany(c => c.Units)
                .HasForeignKey(u => u.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // HistoryEntry -> FromUnit
            modelBuilder.Entity<HistoryEntry>()
                .HasOne(h => h.FromUnit)
                .WithMany()
                .HasForeignKey(h => h.FromUnitId)
                .OnDelete(DeleteBehavior.Restrict);  

            // HistoryEntry -> ToUnit
            modelBuilder.Entity<HistoryEntry>()
                .HasOne(h => h.ToUnit)
                .WithMany()
                .HasForeignKey(h => h.ToUnitId)
                .OnDelete(DeleteBehavior.Restrict);  
        }



    }
}
