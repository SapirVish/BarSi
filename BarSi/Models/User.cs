using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BarSi.Models
{
    public class User
    {
        public int Id { get; set; }

        [Display(Name = "User Name")]
        public string Name { get; set; }

        public string Password { get; set; }
    }
}
