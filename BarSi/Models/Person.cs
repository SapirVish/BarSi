using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BarSi.Models
{
    public class Person
    {
        public int Id { get; set; }

        [RegularExpression(@"^[A-Za-z\s]*$")]
        [StringLength(50)]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        [RegularExpression(@"^[A-Za-z\s]*$")]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName ="date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime Birthdate { get; set; }

        public City City { get; set; }
    }
}
