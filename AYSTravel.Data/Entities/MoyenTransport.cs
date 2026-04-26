using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYSTravel.Data.Entities
{
    public class MoyenTransport
    {
        public int Id { get; set; }

        [Required]
        public string Nom { get; set; }
        // Ex: Bus, Taxi, Tram, Train

        public string Type { get; set; }
        // Public, Privé, Urbain, Interurbain...

        public string Description { get; set; }
        public string HeureDepart { get; set; }

        public string HeureArrivee { get; set; }

        public string Tarif { get; set; }
         public string VilleNom { get; set; }



        // Relation avec Ville
        public int VilleId { get; set; }

        [ForeignKey("VilleId")]
        public Ville Ville { get; set; }
    }
}
