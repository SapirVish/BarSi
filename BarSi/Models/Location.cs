using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BarSi.Models
{
    public class Location
    {
        public int Id { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }
    }
}
