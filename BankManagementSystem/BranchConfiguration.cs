using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankManagementSystem.Configurations
{
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.HasKey(b => b.Code);
            builder.Property(b => b.Code).HasMaxLength(20);
            builder.Property(b => b.Name).IsRequired().HasMaxLength(100);
            builder.Property(b => b.Address).IsRequired().HasMaxLength(200);
            builder.Property(b => b.PhoneNumber).IsRequired().HasMaxLength(20);

            // Seed Branches
            builder.HasData(
                new Branch { Code = "CAI-01", Name = "Cairo Main Branch", Address = "Tahrir Square, Cairo", PhoneNumber = "0223456789" },
                new Branch { Code = "ALX-02", Name = "Alexandria Branch", Address = "Corniche St, Alexandria", PhoneNumber = "0354123456" }
            );
        }
    }
}
