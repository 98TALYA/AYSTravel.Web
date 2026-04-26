using AYSTravel.Logic;
using AYSTravel.Web.Services;
using AYSTravel.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AYSTravel.Web.Controllers
{
    public class HotelController : Controller
    {
        private readonly HotelManager _manager;
        private readonly ApiService _apiService;

        public HotelController(HotelManager manager, ApiService apiService)
        {
            _manager = manager;
            _apiService = apiService;
        }

        // ===== LISTE DES HÔTELS =====
        public async Task<IActionResult> Index(int? villeId)
        {
            var query = _manager.GetAll().AsEnumerable();

            // Filtrer par ville si demandé
            if (villeId.HasValue)
                query = query.Where(h => h.VilleId == villeId.Value);

            var hotels = query.Select(h => new HotelViewModel
            {
                Id = h.Id,
                Nom = h.Nom,
                ImageUrl = h.ImageUrl,
                Description = h.Description,
                Note = h.Note,
                BookingUrl = h.BookingUrl,
                VilleNom = h.Ville?.Nom ?? "Ville inconnue"
            }).ToList();

            // Enrichir avec Booking API
            try
            {
                // Prendre la première ville des hôtels pour la recherche API
                var villeRecherche = hotels.FirstOrDefault()?.VilleNom ?? "Maroc";
                var apiHotels = await _apiService.GetHotels(villeRecherche);

                // Combiner : enrichir les hôtels DB avec données API si nom similaire
                foreach (var hotel in hotels)
                {
                    var apiMatch = apiHotels?.FirstOrDefault(a =>
                        a.Name != null &&
                        a.Name.Contains(hotel.Nom, StringComparison.OrdinalIgnoreCase));

                    if (apiMatch != null)
                    {
                        hotel.ApiPhotoUrl = apiMatch.PhotoUrl;
                        hotel.ApiPrixParNuit = apiMatch.PrixParNuit;
                        hotel.ApiDevise = apiMatch.Devise;
                        hotel.ApiNote = apiMatch.ReviewScoreWord;
                        hotel.ApiNoteScore = apiMatch.ReviewScore;
                    }
                }

                // Ajouter les hôtels API qui ne sont pas dans la DB
                ViewBag.HotelsApiOnly = apiHotels?
                    .Where(a => !hotels.Any(h =>
                        a.Name != null &&
                        a.Name.Contains(h.Nom, StringComparison.OrdinalIgnoreCase)))
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[HOTEL INDEX] Erreur API : {ex.Message}");
                ViewBag.HotelsApiOnly = null;
            }

            return View(hotels);
        }

        // ===== DÉTAILS D'UN HÔTEL =====
        public async Task<IActionResult> Details(int id)
        {
            var hotelEntity = _manager.GetById(id);
            if (hotelEntity == null)
                return NotFound();

            var hotel = new HotelViewModel
            {
                Id = hotelEntity.Id,
                Nom = hotelEntity.Nom,
                ImageUrl = hotelEntity.ImageUrl,
                Description = hotelEntity.Description,
                Note = hotelEntity.Note,
                BookingUrl = hotelEntity.BookingUrl,
                VilleNom = hotelEntity.Ville?.Nom ?? "Ville inconnue"
            };

            // Enrichir avec Booking API
            try
            {
                var apiHotels = await _apiService.GetHotels(hotel.VilleNom);
                var apiMatch = apiHotels?.FirstOrDefault(a =>
                    a.Name != null &&
                    a.Name.Contains(hotel.Nom, StringComparison.OrdinalIgnoreCase));

                if (apiMatch != null)
                {
                    hotel.ApiPhotoUrl = apiMatch.PhotoUrl;
                    hotel.ApiPrixParNuit = apiMatch.PrixParNuit;
                    hotel.ApiDevise = apiMatch.Devise;
                    hotel.ApiNote = apiMatch.ReviewScoreWord;
                    hotel.ApiNoteScore = apiMatch.ReviewScore;
                }

                ViewBag.HotelsProches = apiHotels?.Take(3).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[HOTEL DETAILS] Erreur API : {ex.Message}");
                ViewBag.HotelsProches = null;
            }

            return View(hotel);
        }
    }
}