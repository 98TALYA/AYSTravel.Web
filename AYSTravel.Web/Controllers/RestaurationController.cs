using AYSTravel.Logic;
using AYSTravel.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AYSTravel.Web.Controllers
{
    public class RestaurationController : Controller
    {
        private readonly RestaurationManager _manager;

        public RestaurationController(RestaurationManager manager)
        {
            _manager = manager;
        }

        public IActionResult Index()
        {
            var restos = _manager.GetAll()
                .Select(r => new RestaurationViewModel
                {
                    Id = r.Id,
                    Nom = r.Nom,
                    Type = r.Type,
                    Adresse = r.Adresse,
                    Budget = r.Budget,
                    ImageUrl = r.ImageUrl,
                    VilleId = r.VilleId
                })
                .ToList();

            return View(restos);
        }

        public IActionResult Details(int id)
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
                VilleId = r.VilleId
            };

            return View(resto);
        }
    }
}