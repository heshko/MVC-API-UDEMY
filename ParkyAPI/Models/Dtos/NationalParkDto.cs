using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Models.Dtos
{
    public class NationalParkDto
    {
   
        public int Id { get; set; }

        [Required]   
        public string Name { get; set; }

        [Required]
        public string State { get; set; }

        public byte[] Picture { get; set; }
        public DateTime Create { get; set; }

        public DateTime Established { get; set; }

       public List<TrailDto> trailDtos { get; set; }
    }
}
