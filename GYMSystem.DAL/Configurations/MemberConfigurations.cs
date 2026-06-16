using GYMSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GYMSystem.DAL.Configurations
{
    public class MemberConfigurations : GymUserConfigurations<Member>, IEntityTypeConfiguration<Member>
    {
        public new void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.Property(x => x.CreatedAt)
                .HasColumnName("JoinDate")
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(M => M.HealthRecord)
                .WithOne(HR => HR.Member)
                .HasForeignKey<HealthRecord>(HR => HR.MemberId);

            base.Configure(builder);
        }
    }
}
