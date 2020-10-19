using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Models
{
    public class NationalPark
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="You Should write name of park")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You Should write state of park")]
        public string State { get; set; }

        public byte[] Picture { get; set; }
        public DateTime Create { get; set; }

        public DateTime Established { get; set; }

        public List<Trail> Trails { get; set; }
    }
}
