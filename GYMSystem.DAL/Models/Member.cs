using System;
using System.Collections.Generic;
using System.Text;

namespace GYMSystem.DAL.Models
{
    public class Member:GymUser
    {
        public string? Photo { get; set; } = null!;
        public HealthRecord HealthRecord { get; set; } = null!;
        public ICollection<Booking> MemberSessions { get; set; } = null!;
        public ICollection<MemberShip> MemberPlans { get; set; } = null!;

    }
}
