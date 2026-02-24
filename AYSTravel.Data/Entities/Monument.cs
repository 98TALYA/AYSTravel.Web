using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYSTravel.Data.Entities
{
    public class Monument
    {
        public int Id { get; set; }

        [Required]
        public string Nom { get; set; }

        public string Description { get; set; }

        public string Adress { get; set; }

        public string ImageUrl { get; set; }



        // Relation Ville
        public int VilleId { get; set; }

        [ForeignKey("VilleId")]
        public Ville Ville { get; set; }
    }
}
