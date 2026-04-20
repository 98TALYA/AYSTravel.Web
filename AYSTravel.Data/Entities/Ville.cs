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

        
        public string Nom { get; set; }

        public string? Description { get; set; }

        // ✅ nullable
        public string? ImageUrl { get; set; }

        // ✅ initialiser les listes
        public List<Restauration> Restaurations { get; set; } = new();
        public List<Monument> Monuments { get; set; } = new();
        public List<Stade> Stades { get; set; } = new();
        public List<Activite> Activites { get; set; } = new();
        public List<MoyenTransport> MoyensTransports { get; set; } = new();
        public List<Hotel> Hotels { get; set; } = new();
    }
}
