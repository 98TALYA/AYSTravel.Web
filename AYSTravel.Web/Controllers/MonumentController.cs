using AYSTravel.Logic;
using Microsoft.AspNetCore.Mvc;

namespace AYSTravel.Web.Controllers
{
    public class MonumentController : Controller
    {
        private readonly MonumentManager _manager;

        public MonumentController(MonumentManager manager)
        {
            _manager = manager;
        }

        public IActionResult Index()
        {
            return View(_manager.GetAll());
        }

        public IActionResult Details(int id)
        {
            var monument = _manager.GetById(id);
            if (monument == null) return NotFound();
            return View(monument);
        }
    }
}