using AYSTravel.Logic;
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
            return View(_manager.GetAll());
        }

        public IActionResult Details(int id)
        {
            var restauration = _manager.GetById(id);
            if (restauration == null) return NotFound();
            return View(restauration);
        }
    }
}