using GYMSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GYMSystem.DAL.Configurations
{
    internal class SessionConfigurations : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(T =>

            {
                T.HasCheckConstraint("SessionCapacityConstraint", "Capacity between 1 and 25");
                T.HasCheckConstraint("SessionEndDateConstraint", "EndDate > StartDate");


            });
            builder.HasOne(X => X.Trainer)
                .WithMany(X => X.TrainerSessions)
                .HasForeignKey(X => X.TrainerId);

            builder.HasOne(X => X.Category)
                .WithMany(X => X.Sessions)
                .HasForeignKey(X => X.CategoryId);
        }
    }
}
