using System.Linq;
using AYSTravel.Data;
using AYSTravel.Data.Entities;
using AYSTravel.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AYSTravel.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RestaurationAdminController : Controller
    {
        private readonly RestaurationManager _manager;
        private readonly ApplicationDbContext _context;

        public RestaurationAdminController(RestaurationManager manager, ApplicationDbContext context)
        {
            _manager = manager;
            _context = context;
        }

        // LISTE
        public IActionResult Index()
        {
            var restaurations = _context.Restaurations.ToList();
            return View(restaurations);
        }

        // CREATE - GET
        public IActionResult Create()
        {
            ViewBag.Villes = new SelectList(_context.Villes, "Id", "Nom");
            return View();
        }

        // CREATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Restauration restauration)
        {
            if (ModelState.IsValid)
            {
                _manager.Add(restauration);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Villes = new SelectList(_context.Villes, "Id", "Nom", restauration?.VilleId);
            return View(restauration);
        }

        // EDIT - GET
        public IActionResult Edit(int id)
        {
            var restauration = _manager.GetById(id);
            if (restauration == null) return NotFound();

            ViewBag.Villes = new SelectList(_context.Villes, "Id", "Nom", restauration.VilleId);
            return View(restauration);
        }

        // EDIT - POST
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Restauration restauration)
        {
            if (id != restauration.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.Villes = new SelectList(_context.Villes, "Id", "Nom", restauration.VilleId);
                return View(restauration);
            }

            try
            {
                _manager.Update(restauration); // ✅ CORRECT
            }
            catch (Exception ex)
            {
                return Content(ex.InnerException?.Message ?? ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }

        // DELETE - GET
        public IActionResult Delete(int id)
        {
            var restauration = _manager.GetById(id);
            if (restauration == null) return NotFound();
            return View(restauration);
        }

        // DELETE - POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _manager.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}