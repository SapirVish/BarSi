using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BarSi.Models
{
    public class Patient: Person
    {
        [Display(Name = "Medical History")]
        public string MedicalBackgroundHispory { get; set; }

        public Hospital Hospital { get; set; }

        public PatientStatus Status { get; set; }

        public Doctor Doctor { get; set; }
    }
}
