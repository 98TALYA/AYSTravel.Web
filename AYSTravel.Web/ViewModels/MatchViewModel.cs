using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AYSTravel.Web.ViewModels
{
    public class MatchViewModel
    {
        public int Id { get; set; }

        public string Equipe1 { get; set; }
        public string Equipe2 { get; set; }

        
        public string Stade { get; set; }
        [Url]
        public string StadeImageUrl { get; set; }
        public string StadeNom { get; set; }
        public DateTime Date { get; set; }
         public string VilleNom { get; set; }

       
        

        

       

    }
}