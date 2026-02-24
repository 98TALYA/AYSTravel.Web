

using AYSTravel.Data.Entities;
using AYSTravel.Logic;
using Microsoft.AspNetCore.Mvc;

namespace AYSTravel.Web.Controllers
{
    public class ActiviteController : Controller
    {
        private readonly ActiviteManager _manager;

        public ActiviteController(ActiviteManager manager)
        {
            _manager = manager;
        }

        // READ ALL
        public IActionResult Index()
        {
            return View(_manager.GetAll());
        }

        // READ ONE
        public IActionResult Details(int id)
        {
            var activite = _manager.GetById(id);
            if (activite == null)
                return NotFound();

            return View(activite);
        }

        // CREATE GET
        public IActionResult Create()
        {
            return View();
        }

        // CREATE POST
        [HttpPost]
        public IActionResult Create(Activite activite)
        {
            if (ModelState.IsValid)
            {
                _manager.Add(activite);
                return RedirectToAction(nameof(Index));
            }
            return View(activite);
        }

        // EDIT GET
        public IActionResult Edit(int id)
        {
            var activite = _manager.GetById(id);
            if (activite == null)
                return NotFound();

            return View(activite);
        }

        // EDIT POST
        [HttpPost]
        public IActionResult Edit(Activite activite)
        {
            if (ModelState.IsValid)
            {
                _manager.Update(activite);
                return RedirectToAction(nameof(Index));
            }
            return View(activite);
        }

        // DELETE GET
        public IActionResult Delete(int id)
        {
            var activite = _manager.GetById(id);
            if (activite == null)
                return NotFound();

            return View(activite);
        }

        // DELETE POST
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _manager.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}