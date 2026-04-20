using AYSTravel.Data;
using AYSTravel.Data.Entities;
using AYSTravel.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AYSTravel.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VilleAdminController : Controller
    {
        private readonly VilleManager _manager;
        private readonly ApplicationDbContext _context;

        public VilleAdminController(VilleManager manager, ApplicationDbContext context)
        {
            _manager = manager;
            _context = context;
        }

        // LISTE
        public IActionResult Index()
        {
            var ville = _context.Villes.ToList();
            return View(ville);
        }

        // CREATE
        public IActionResult Create()
        {
            ViewBag.Villes = new SelectList(_context.Villes, "Id", "Nom");
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Ville ville)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Villes = new SelectList(_context.Villes, "Id", "Nom");
                return View(ville);
            }

            ville.ImageUrl ??= "default.jpg";

            _manager.Add(ville);

            return RedirectToAction(nameof(Index));
        }

        //EEDIIT

        public IActionResult Edit(int id)
        {
            var ville = _manager.GetById(id);
            if (ville == null) return NotFound();

            ViewBag.Villes = new SelectList(_context.Villes, "Id", "Nom", ville.Id);
            return View(ville);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Ville ville)
        {
            if (id != ville.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.Villes = new SelectList(_context.Villes, "Id", "Nom", ville.Id);
                return View(ville);
            }

            var existingVille = _manager.GetById(id);
            if (existingVille == null)
                return NotFound();

            // mise à jour
            existingVille.Nom = ville.Nom;
            existingVille.Description = ville.Description;

            // 🔥 SAUVEGARDE
            _manager.Update(existingVille);

            return RedirectToAction(nameof(Index));
        }


        // DELETE
        public IActionResult Delete(int id)
        {
            var ville = _manager.GetById(id);
            if (ville == null) return NotFound();
            return View(ville);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _manager.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}