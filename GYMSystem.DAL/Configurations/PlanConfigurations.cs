using GYMSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GYMSystem.DAL.Configurations
{
    internal class PlanConfigurations : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(x => x.Name)
                  .HasColumnType("varchar")
                  .HasMaxLength(50);

            builder.Property(x => x.Description)
                .HasMaxLength(200);

            builder.Property(x => x.Price)
                .HasPrecision(10, 2);

            builder.Property(x => x.CreatedAt)
               .HasDefaultValueSql("GETDATE()");

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("PlanDurationCheck", "DurationInDays Between 1 and 365");

            });


        }
    }
}
