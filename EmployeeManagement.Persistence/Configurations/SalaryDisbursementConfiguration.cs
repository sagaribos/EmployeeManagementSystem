using EmployeeManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagement.Persistence.Configurations;

public class SalaryDisbursementConfiguration : IEntityTypeConfiguration<SalaryDisbursement>
{
    public void Configure(EntityTypeBuilder<SalaryDisbursement> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Amount)
            .HasColumnType("decimal(18,2)");

        builder.Property(s => s.Adjustment)
            .HasColumnType("decimal(18,2)");

        builder.Property(s => s.Status)
            .HasConversion<string>();
    }
}