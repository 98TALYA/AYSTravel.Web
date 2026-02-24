using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AYSTravel.Data.Entities
{
    public class Match
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Equipe1 { get; set; }
        public string Equipe2 { get; set; }

        public string Stade { get; set; }

        // Relation Ville
        public int VilleId { get; set; }

        [ForeignKey("VilleId")]
        public Ville Ville { get; set; }
    }
}
