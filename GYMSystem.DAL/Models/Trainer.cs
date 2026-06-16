using GYMSystem.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GYMSystem.DAL.Models
{
    public class Trainer:GymUser
    {
        public Specialities Specialities { get; set; }
        public ICollection<Session> TrainerSessions { get; set; } = null!;
    }
}
