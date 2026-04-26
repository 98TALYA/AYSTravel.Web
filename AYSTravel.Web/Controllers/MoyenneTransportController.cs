using AYSTravel.Logic;
using AYSTravel.Web.Models.API;
using AYSTravel.Web.Services;
using AYSTravel.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AYSTravel.Web.Controllers
{
    
    public class MoyenTransportController : Controller
    {
        private readonly MoyenTransportManager _manager;
        private readonly ApiService _apiService;

        public MoyenTransportController(MoyenTransportManager manager, ApiService apiService)
        {
            _manager = manager;
            _apiService = apiService;
        }

        public async Task<IActionResult> Index(int? villeId)
        {
            var query = _manager.GetAll().AsEnumerable();

            if (villeId.HasValue)
                query = query.Where(t => t.VilleId == villeId.Value);

            var transports = query.Select(t => new MoyenTransportViewModel
            {
                Id = t.Id,
                Nom = t.Nom,
                Type = t.Type,
                Description = t.Description,
                Tarif = t.Tarif,
                HeureDepart = t.HeureDepart,
                HeureArrivee = t.HeureArrivee,
                VilleNom = t.Ville?.Nom ?? "Inconnu"
            }).ToList();

            // TransitLand
            try
            {
                var villeNom = transports.FirstOrDefault()?.VilleNom ?? "Casablanca";
                var routes = await _apiService.GetTransitRoutes(villeNom);

                ViewBag.TransitBus = routes?.Where(r => r.Type.Contains("Bus")).ToList();
                ViewBag.TransitTrain = routes?.Where(r => r.Type.Contains("Train") ||
                                                           r.Type.Contains("Métro") ||
                                                           r.Type.Contains("Tram")).ToList();
                ViewBag.TransitFerry = routes?.Where(r => r.Type.Contains("Ferry")).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TRANSIT] Erreur : {ex.Message}");
                ViewBag.TransitBus = null;
                ViewBag.TransitTrain = null;
                ViewBag.TransitFerry = null;
            }

            return View(transports);
        }

        public IActionResult Details(int id)
        {
            var transport = _manager.GetById(id);
            if (transport == null) return NotFound();
            return View(transport);
        }
    }
}