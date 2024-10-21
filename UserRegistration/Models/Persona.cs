using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRegistration.Models
{
    internal class Persona
    {
        [Key] public int Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Surname { get; set; }
        public DateTimeOffset BirthDay { get; set; }
        public string FormatBirthDay { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int Cap { get; set; }
        public int PhoneNumber { get; set; }
    }
}
