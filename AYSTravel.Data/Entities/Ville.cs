using AYSTravel.Logic.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AYSTravel.Data.Entities
{
    public class Ville
    {
        public int Id { get; set; }

        [Required]
        public string Nom { get; set; }

        public string Description { get; set; }

       public string ImageUrl { get; set; }
       


        public List<Restauration> Restaurations { get; set; }
        public List<Monument> Monuments { get; set; }
        public List<Match> Matchs { get; set; }
        public List<Activite> Activites { get; set; }
        public List<MoyenTransport> MoyensTransports { get; set; }
        public List<Hotel> Hotels { get; set; }
    }
}
