﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarSi.Models
{
    public class MedicalEquipment
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public IEnumerable<MedicalEquipmentSupply> medicalEquipmentSupplies { get; set; }
    }
}
