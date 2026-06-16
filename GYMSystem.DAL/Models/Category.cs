using System;
using System.Collections.Generic;
using System.Text;

namespace GYMSystem.DAL.Models
{
    public class Category:BaseEntity
    {
        public string CategoryName { get; set; } = null!;
        public ICollection<Session> Sessions { get; set; } = null!;
    }
}
