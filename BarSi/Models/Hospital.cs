using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BarSi.Models
{
    public class Hospital
    {

        public int Id { get; set; }

        [StringLength(50)]
        [RegularExpression(@"^[A-Za-z\s]*$")]
        public string Name { get; set; }

        public City City { get; set; }

        public IEnumerable<Patient> Patient { get; set; }

        public IEnumerable<Doctor> Doctors { get; set; }
        
    }
}
