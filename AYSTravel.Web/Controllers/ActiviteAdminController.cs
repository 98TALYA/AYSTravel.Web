using AYSTravel.Data;
using AYSTravel.Data.Entities;
using AYSTravel.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AYSTravel.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ActiviteAdminController : Controller
    {
        private readonly ActiviteManager _manager;
        private readonly ApplicationDbContext _context;

        public ActiviteAdminController(ActiviteManager manager, ApplicationDbContext context)
        {
            _manager = manager;
            _context = context;
        }

        // LISTE
        public IActionResult Index()
        {
            var activites = _manager.GetAll();
            return View(activites);
        }

        // CREATE
        public IActionResult Create()
        {
            ViewBag.Villes = new SelectList(_context.Villes, "Id", "Nom");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Activite activite)
        {
            if (ModelState.IsValid)
            {
                _manager.Add(activite);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Villes = new SelectList(_context.Villes, "Id", "Nom");
            return View(activite);
        }

        // EDIT
        public IActionResult Edit(int id)
        {
            var activite = _manager.GetById(id);
            if (activite == null) return NotFound();

            ViewBag.Villes = new SelectList(_context.Villes, "Id", "Nom", activite.VilleId);
            return View(activite);
        }

        [HttpPost]
        public IActionResult Edit(Activite activite)
        {
            if (ModelState.IsValid)
            {
                _manager.Update(activite);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Villes = new SelectList(_context.Villes, "Id", "Nom", activite.VilleId);
            return View(activite);
        }

        // DELETE
        public IActionResult Delete(int id)
        {
            var activite = _manager.GetById(id);
            if (activite == null) return NotFound();

            return View(activite);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _manager.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}