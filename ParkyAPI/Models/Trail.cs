using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Models
{
    public class Trail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Distance { get; set; }

        [Required]
        public double Elevation { get; set; }

        public enum DificultyType { Eazy, Moderate,Difficult,Expeert }

        public DificultyType Dificulty { get; set; }

        [Required]
        public int NationalParkId { get; set; }

        [ForeignKey(nameof(NationalParkId))]

        public NationalPark NationalPark { get; set; }
        public DateTime Created { get; set; }
    }
}
