using AYSTravel.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYSTravel.Logic.Models
{
    public class Hotel
    {
        [Key]
        public int Id { get; set; }

        public string Nom { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public double Note { get; set; }
        public string BookingUrl { get; set; }
        public string Population { get; set; }  

        // Relation avec la ville
        public int VilleId { get; set; }
        public Ville Ville { get; set; }
    }
}
