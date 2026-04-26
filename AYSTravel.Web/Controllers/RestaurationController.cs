using AYSTravel.Logic;
using AYSTravel.Web.Services;
using AYSTravel.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AYSTravel.Web.Controllers
{
    public class RestaurationController : Controller
    {
        private readonly RestaurationManager _manager;
        private readonly ApiService _apiService;

        public RestaurationController(RestaurationManager manager, ApiService apiService)
        {
            _manager = manager;
            _apiService = apiService;
        }

        public async Task<IActionResult> Index(int? villeId)
        {
            var query = _manager.GetAll().AsEnumerable();

            if (villeId.HasValue)
                query = query.Where(r => r.VilleId == villeId.Value);

            var restos = query.Select(r => new RestaurationViewModel
            {
                Id = r.Id,
                Nom = r.Nom,
                ImageUrl = r.ImageUrl,
                Description = r.Description,
                Type = r.Type,            // ← AJOUTÉ
                Adresse = r.Adresse,         // ← AJOUTÉ
                Budget = r.Budget,          // ← AJOUTÉ
                VilleId = r.VilleId,         // ← AJOUTÉ
                VilleNom = r.Ville?.Nom ?? "Inconnu"
            }).ToList();

            // Enrichir avec Foursquare
            try
            {
                var villeRecherche = restos.FirstOrDefault()?.VilleNom ?? "Tanger";
                ViewBag.RestosApi = await _apiService.GetRestaurants(villeRecherche);
                ViewBag.CafesApi = await _apiService.GetCafes(villeRecherche);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RESTO] Erreur : {ex.Message}");
                ViewBag.RestosApi = null;
                ViewBag.CafesApi = null;
            }

            return View(restos);
        }

        public async Task<IActionResult> Details(int id)
        {
            var r = _manager.GetById(id);
            if (r == null)
                return NotFound();

            var resto = new RestaurationViewModel
            {
                Id = r.Id,
                Nom = r.Nom,
                Type = r.Type,
                Adresse = r.Adresse,
                Budget = r.Budget,
                ImageUrl = r.ImageUrl,
                VilleId = r.VilleId,
                Description = r.Description,
                VilleNom = r.Ville?.Nom ?? "Tanger"
            };

            // Enrichir avec Foursquare
            try
            {
                var apiRestos = await _apiService.GetRestaurants(resto.VilleNom);

                var apiMatch = apiRestos?.FirstOrDefault(a =>
                    a.Nom != null &&
                    a.Nom.Contains(resto.Nom, StringComparison.OrdinalIgnoreCase));

                if (apiMatch != null)
                {
                    resto.ApiPhotoUrl = apiMatch.PhotoUrl;
                    resto.ApiNote = apiMatch.Note;
                    resto.ApiAdresse = apiMatch.Adresse;
                    resto.ApiWebsite = apiMatch.Website;
                }

                ViewBag.RestaurantsSimilaires = apiRestos?
                    .Where(a => a.Nom != resto.Nom)
                    .Take(3)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RESTO DETAILS] Erreur : {ex.Message}");
                ViewBag.RestaurantsSimilaires = null;
            }

            return View(resto);
        }
    }
}