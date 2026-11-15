using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankManagementSystem.Configurations
{
    public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
    {
        public void Configure(EntityTypeBuilder<Manager> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.FullName).IsRequired().HasMaxLength(100);
            builder.Property(m => m.Email).IsRequired().HasMaxLength(150);
            builder.Property(m => m.PhoneNumber).IsRequired().HasMaxLength(20);
            builder.Property(m => m.HireDate).IsRequired();

            // One-to-One with Branch
            builder.HasOne(m => m.Branch)
                   .WithOne(b => b.Manager)
                   .HasForeignKey<Manager>(m => m.BranchCode)
                   .OnDelete(DeleteBehavior.Restrict);

            // Seed Managers
            builder.HasData(
                new Manager { Id = 1, FullName = "Hassan Mahmoud", Email = "hassan.manager@nbe.com", PhoneNumber = "01001234567", HireDate = new DateTime(2015, 6, 1), BranchCode = "CAI-01" },
                new Manager { Id = 2, FullName = "Mervat Ibrahim", Email = "mervat.manager@nbe.com", PhoneNumber = "01229876543", HireDate = new DateTime(2018, 9, 15), BranchCode = "ALX-02" }
            );
        }
    }
}
