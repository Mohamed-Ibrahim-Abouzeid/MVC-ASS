using GYMSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GYMSystem.DAL.Configurations
{
    public class GymUserConfigurations<T> : IEntityTypeConfiguration<T> where T : GymUser
    {


        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(u => u.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50);
            builder.Property(u => u.Email)
               .HasColumnType("varchar")
               .HasMaxLength(100);
            builder.OwnsOne(u => u.Address, a =>
            { 
                a.Property(ad => ad.Street)
                    .HasColumnType("varchar")
                    .HasMaxLength(30);
                a.Property(ad => ad.City)
                    .HasColumnType("varchar")
                    .HasMaxLength(30);
            });
            builder.Property(u => u.Phone)
                .HasColumnType("varchar")
                .HasMaxLength(11);
            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.Phone).IsUnique();
            builder.ToTable(t =>
            {
                t.HasCheckConstraint("GymUsers_PhoneCheck", "Phone LIKE '010%' OR Phone LIKE '011%' OR Phone LIKE '012%' OR Phone LIKE '015%'");
                t.HasCheckConstraint("GymUsers_EmailCheck", "Email LIKE '%@%.%'");
            }
            );
        }
    }
}
