using AYSTravel.Logic;
using AYSTravel.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AYSTravel.Web.Controllers
{
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
            var matchs = _manager.GetAll()
                .Select(m => new MatchViewModel
                {
                    Id = m.Id,
                    Equipe1 = m.Equipe1,
                    Equipe2 = m.Equipe2,
                    Date = m.Date,
                    StadeImageUrl = m.Stade.ImageUrl,

                    StadeNom = m.Stade != null ? m.Stade.Nom : "Stade inconnu",

                    VilleNom = m.Stade != null && m.Stade.Ville != null
                        ? m.Stade.Ville.Nom
                        : "Ville inconnue"
                })
                .ToList();

            return View(matchs);
        }

        // Détails d’un match
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
                Date = matchEntity.Date,
                StadeImageUrl = matchEntity.Stade.ImageUrl,
                StadeNom = matchEntity.Stade != null
                    ? matchEntity.Stade.Nom
                    : "Stade inconnu",

                VilleNom = matchEntity.Stade != null && matchEntity.Stade.Ville != null
                    ? matchEntity.Stade.Ville.Nom
                    : "Ville inconnue"
            };

            return View(match);
        }
    }
}