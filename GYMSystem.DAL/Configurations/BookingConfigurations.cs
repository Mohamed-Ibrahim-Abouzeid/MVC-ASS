using GYMSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GYMSystem.DAL.Configurations
{
    public class BookingConfigurations : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.Ignore(b => b.Id);
            builder.Property(x => x.CreatedAt)
                .HasColumnName("BookingDate")
                .HasDefaultValueSql("GETDATE()");
            #region Relations
            builder.HasOne(b => b.Member)
                .WithMany(m => m.MemberSessions)
                .HasForeignKey(b => b.MemberId);
             builder.HasOne(b => b.Session)
                .WithMany(s => s.TrainerSessions)
                .HasForeignKey(b => b.SessionId);
            builder.HasKey(b => new { b.MemberId, b.SessionId });
            #endregion
        }
    }
}
