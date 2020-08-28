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
        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
