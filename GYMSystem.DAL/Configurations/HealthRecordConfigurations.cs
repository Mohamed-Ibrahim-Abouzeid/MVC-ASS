using GYMSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GYMSystem.DAL.Configurations
{
    public class HealthRecordConfigurations : IEntityTypeConfiguration<HealthRecord>
    {
        void IEntityTypeConfiguration<HealthRecord>.Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            builder.Property(x => x.BloodType)
                    .HasMaxLength(5);
            builder.Property(x => x.Note)
                    .HasMaxLength(500);
        }
    }
}
