using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarSi.Models
{
    public class MedicalEquipmentSupply
    {
        public int HospitalId { get; set; }

        public Hospital Hospital { get; set; }

        public int MedicalEquipmentId { get; set; }

        public MedicalEquipment MedicalEquipment { get; set; }
    }
}
