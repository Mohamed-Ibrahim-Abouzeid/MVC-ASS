using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GYMSystem.DAL.Models
{
    public class Plan:BaseEntity
    {
        [Required, MaxLength(50)]
        public string Name { get; set; } = null!;
        [Required, MaxLength(100)]
        public string Description { get; set; }
        [Column(TypeName ="decimal(10,2)")]
        public decimal Price { get; set; }
        [Range(1,365)]
        public int DurationInDays { get; set; }
        public bool IsActive { get; set; }
    public ICollection<MemberShip> MemberShips { get; set; } = null!;
    }
}
