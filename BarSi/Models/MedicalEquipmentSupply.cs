using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace BarSi.Models
{
    public class MedicalEquipmentSupply
    {
        public int HospitalId { get; set; }

        public Hospital Hospital { get; set; }

        public int MedicalEquipmentId { get; set; }

        [Display(Name = "Medical Equipment")]
        public MedicalEquipment MedicalEquipment { get; set; }

        [Display(Name = "Quantity Supplied")]
        [Required]
        public int SupplyQuantity { get; set; }


        public MedicalEquipmentSupply()
        {
        }

        public MedicalEquipmentSupply(Hospital hospital, MedicalEquipment medicalEquipmentSupply, int supplyQuantity)
        {
            this.Hospital = hospital;
            this.HospitalId = hospital.Id;
            this.MedicalEquipment = medicalEquipmentSupply;
            this.MedicalEquipmentId = medicalEquipmentSupply.Id;
            this.SupplyQuantity = supplyQuantity;
        }
    }
}
