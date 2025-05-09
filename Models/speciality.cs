using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Speciality
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Image { get; set; }
        public int ServiceId { get; set; }
        public virtual Service? Service { get; set; }

    }
}