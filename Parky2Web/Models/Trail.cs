using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Parky2Web.Models
{
    public class Trail
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public double Elevation { get; set; }

        [Required]
        public double Distance { get; set; }

        public enum DificultyType { Eazy, Moderate, Difficult, Expeert }
        public DificultyType Dificulty { get; set; }

        [Required]
        public int NationalParkId { get; set; }

        public NationalPark NationalPark { get; set; }
    }
}
