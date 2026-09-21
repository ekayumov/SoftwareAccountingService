using Microsoft.EntityFrameworkCore;
using SoftwareAccountingService.Api.Domain.Emuns;
using SoftwareAccountingService.Api.Domain.Entities;

namespace SoftwareAccountingService.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<InspectionObject> InspectionObjects => Set<InspectionObject>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InspectionObject>()
                .Property(x => x.Type)
                .HasConversion<string>()
                .HasMaxLength(3);

            modelBuilder.Entity<InspectionObject>()
                .Property(x => x.Result)
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(InspectionsResult.InProgress);
        }
    }
}
