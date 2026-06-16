using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GYMSystem.DAL.Models
{
    public class MemberShip: BaseEntity 
    {
       
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }

        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;
        public int PlanId { get; set; }
        public Plan Plan { get; set; } = null!;
        [NotMapped]
        public string Status => EndDate > DateTime.UtcNow ? "Active" : "Expired";
        [NotMapped]
        public bool IsActive => EndDate > DateTime.UtcNow;

    }
}
