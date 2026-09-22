using Microsoft.EntityFrameworkCore;
using SoftwareAccountingService.Api.Domain.Enums;
using SoftwareAccountingService.Api.Domain.Entities;

namespace SoftwareAccountingService.Api.Data;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<InspectionObject> InspectionObjects
        => Set<InspectionObject>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var inspectionObject =
        modelBuilder.Entity<InspectionObject>();

        inspectionObject.ToTable("InspectionObjects", tableBuilder =>
        {
            tableBuilder.HasCheckConstraint(
                "CK_InspectionObjects_Type",
                EnumBuider(
                    "Type",
                    typeof(InspectionType)));

            tableBuilder.HasCheckConstraint(
                "CK_InspectionObjects_Result",
                EnumBuider(
                    "Result",
                    typeof(InspectionResult)));
        });

        inspectionObject.Property(x => x.Type)
            .HasConversion<string>()
            .HasMaxLength(20);

        inspectionObject.Property(x => x.Result)
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(InspectionResult.InProgress);
    }

    private static string EnumBuider(string columnName, Type enumType)
    {
        var enumNames = Enum.GetNames(enumType);
        var values = new List<string>();

        foreach (var name in enumNames) { values.Add($"'{name}'"); }
        var stringValues = string.Join(", ", values);
        var fullString = $"\"{columnName}\" IN ({stringValues})";
        return fullString;

    }
}