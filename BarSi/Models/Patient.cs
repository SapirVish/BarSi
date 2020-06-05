using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarSi.Models
{
    public class Patient: Person
    {
        public string MedicalBackgroundHispory { get; set; }

        public Hospital hospital { get; set; }

    }
}
