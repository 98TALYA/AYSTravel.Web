using AYSTravel.Logic;
using AYSTravel.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // nécessaire pour Include

namespace AYSTravel.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MatchController : Controller
    {
        private readonly MatchManager _manager;
        

        public MatchController(MatchManager manager)
        {
            _manager = manager;
        }

        // Liste de tous les matchs
        public IActionResult Index()
        {
            var matchs = _manager.GetAll() // retourne List<Match>
                .Select(m => new MatchViewModel
                {
                    Id = m.Id,
                    Equipe1 = m.Equipe1,
                    Equipe2 = m.Equipe2,
                    Stade = m.Stade,
                    Date = m.Date,
                    VilleNom = m.Ville != null ? m.Ville.Nom : "Ville inconnue"
                })
                .ToList();

            return View(matchs);
        }

        // Détails d’un match spécifique
        public IActionResult Details(int id)
        {
            var matchEntity = _manager.GetById(id);

            if (matchEntity == null)
                return NotFound();

            var match = new MatchViewModel
            {
                Id = matchEntity.Id,
                Equipe1 = matchEntity.Equipe1,
                Equipe2 = matchEntity.Equipe2,
                Stade = matchEntity.Stade,
                Date = matchEntity.Date,
                VilleNom = matchEntity.Ville != null
                           ? matchEntity.Ville.Nom
                           : "Ville inconnue"
            };

            return View(match);
        }
    }
}