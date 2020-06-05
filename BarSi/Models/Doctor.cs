using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarSi.Models
{
    public class Doctor: Person
    {
        public Hospital Hospital { get; set; }
        public IEnumerable<Patient> patients { get; set; }
    }
}
