using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AYSTravel.Data.Entities
{
    public class Stade
    {
        public int Id { get; set; }

        
        public string Nom { get; set; }

        public string? ImageUrl { get; set; }

        

        // relation Ville
        public int VilleId { get; set; }

        [ForeignKey("VilleId")]
        public Ville? Ville { get; set; }

        // relation Match
        public List<Match> Matchs { get; set; } = new ();
    }
}