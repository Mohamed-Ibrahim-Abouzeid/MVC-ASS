using GYMSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GYMSystem.DAL.Configurations
{
    public class CategoryConfigurations : IEntityTypeConfiguration<Category>
    {
      public  void Configure(EntityTypeBuilder<Category> builder)
        {
        builder.Property(c => c.CategoryName)
            .HasColumnType("varchar")
            .HasMaxLength(20);
            builder.HasData(
                new Category { Id = 1, CategoryName = "Yoga" },
                new Category { Id = 2, CategoryName = "Cardio" },
                new Category { Id = 3, CategoryName = "Strength Training" },
                new Category { Id = 4, CategoryName = "Pilates" },
                new Category { Id = 5, CategoryName = "CrossFit" }
            );
        }
    }
}
