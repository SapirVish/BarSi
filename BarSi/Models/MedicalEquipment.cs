using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace BarSi.Models
{
    public class MedicalEquipment
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public int Price { get; set; }

        [Required]
        [RegularExpression(@"^[0-9\s]*$", ErrorMessage = "Please enter positive value")]
        public int Quantity { get; set; }

        public IEnumerable<MedicalEquipmentSupply> medicalEquipmentSupplies { get; set; }
    }
}
