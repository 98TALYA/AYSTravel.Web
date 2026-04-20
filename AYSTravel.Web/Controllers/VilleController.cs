using System.Linq;
using AYSTravel.Data.Entities;
using AYSTravel.Logic;
using AYSTravel.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AYSTravel.Web.Controllers
{
    
    public class VilleController : Controller
    {
        private readonly VilleManager _villeManager;

        public VilleController(VilleManager manager)
        {
            _villeManager = manager;
        }

        public IActionResult Index()
        {
            return View(GetListForIndex());
        }

        public IActionResult DetailsVille(int id)
        {
            var vvm = GetVilleForDetails(id);
            if (vvm == null)
            {
                return NotFound();
            }
            return View(vvm);
        }

        private VilleViewModel GetVilleForDetails(int id)
        {
            var ville = _villeManager.GetById(id);
            if (ville == null) return null;

            var vvm = new VilleViewModel
            {
                Id = ville.Id,
                Description = ville.Description,
                ImageUrl = ville.ImageUrl,
                NombreMonuments = ville.Monuments?.Count ?? 0,
                NombreActivites = ville.Activites?.Count ?? 0,
                NombreRestaurations = ville.Restaurations?.Count ?? 0,
                NombreMatchs = ville.Stades?.SelectMany(s => s.Matchs)?.Count() ?? 0,
                NombreTransports = ville.MoyensTransports?.Count ?? 0,
                
                

                Monuments = ville.Monuments?.Select(m => new MonumentViewModel
                {
                    Nom = m.Nom,
                    Description = m.Description,
                    ImageUrl = m.ImageUrl
                }).ToList(),

                Activites = ville.Activites?.Select(a => new ActiviteViewModel
                {
                    Nom = a.Nom,
                    Description = a.Description,
                    ImageUrl = a.ImageUrl

                }).ToList(),

                MoyensTransports = ville.MoyensTransports?.Select(t => new MoyenTransportViewModel
                {
                    Nom = t.Nom,
                    Description = t.Description, 
                    Type = t.Type, 
                    HeureArrivee = t.HeureArrivee,
                    HeureDepart = t.HeureDepart,
                }).ToList(),

                Matchs = ville.Stades?
    .SelectMany(s => s.Matchs)
    .Select(ma => new MatchViewModel
    {
        Equipe1 = ma.Equipe1,
        Equipe2 = ma.Equipe2,
        Date = ma.Date,
        StadeNom = ma.Stade?.Nom
    }).ToList(),

                Hotels = ville.Hotels?.Select(h => new HotelViewModel
                {
                    Nom = h.Nom,
                    Description = h.Description,
                    ImageUrl = h.ImageUrl,
                    Note = h.Note,
                    BookingUrl = h.BookingUrl
                }).ToList()
            };

            // Villes similaires
            vvm.VillesSimilaires = _villeManager.GetAll()
                .Where(v => v.Id != ville.Id)
                .Take(3)
                .Select(v => new VilleViewModel
                {
                    Id = v.Id,
                    Nom = v.Nom,
                    Description = v.Description
                }).ToList();

            return vvm;
        }

        private AccueilVilleViewModel GetListForIndex()
        {
            var villes = _villeManager.GetAll();
            var villeViewModels = villes.Select(v => new VilleViewModel
            {
                Id = v.Id,
                Nom = v.Nom,
                Description = v.Description,
                ImageUrl = v.ImageUrl,
                NombreMonuments = v.Monuments?.Count ?? 0,
                NombreActivites = v.Activites?.Count ?? 0,
                NombreRestaurations = v.Restaurations?.Count ?? 0,
                NombreMatchs = v.Stades?.SelectMany(s => s.Matchs)?.Count() ?? 0,
                NombreTransports = v.MoyensTransports?.Count ?? 0
            }).ToList();

            var vm = new AccueilVilleViewModel
            {
                Villes = villeViewModels,
                NombreVilles = villes.Count
            };

            return vm;
        }

    }
}