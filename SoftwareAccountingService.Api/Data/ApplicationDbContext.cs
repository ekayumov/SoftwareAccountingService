using Microsoft.EntityFrameworkCore;
using SoftwareAccountingService.Api.Domain.Entities;

namespace SoftwareAccountingService.Api.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<InspectionObject> InspectionObjects { get; set; }
    }
}
