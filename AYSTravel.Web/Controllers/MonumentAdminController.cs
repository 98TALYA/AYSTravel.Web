using AYSTravel.Data;
using AYSTravel.Data.Entities;
using AYSTravel.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AYSTravel.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MonumentAdminController : Controller
    {
        private readonly MonumentManager _manager;
        private readonly ApplicationDbContext _context;

        public MonumentAdminController(MonumentManager manager, ApplicationDbContext context)
        {
            _manager = manager;
            _context = context;
        }

        // LISTE
        public IActionResult Index()
        {
            // Inclure la ville associée pour affichage si besoin
            var monuments = _context.Monuments
                                   .Include(m => m.Ville)
                                   .ToList();
            return View(monuments);
        }

        // CREATE (GET)
        public IActionResult Create()
        {
            ViewBag.Villes = new SelectList(_context.Villes, "Id", "Nom");
            return View();
        }

        // CREATE (POST)
        [HttpPost]
        public IActionResult Create(Monument monument)
        {
            if (ModelState.IsValid)
            {
                _manager.Add(monument);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Villes = new SelectList(_context.Villes, "Id", "Nom", monument?.VilleId);
            return View(monument);
        }

        // EDIT (GET)
        public IActionResult Edit(int id)
        {
            var monument = _manager.GetById(id);
            if (monument == null) return NotFound();

            ViewBag.Villes = new SelectList(_context.Villes, "Id", "Nom", monument.VilleId);
            return View(monument);
        }

        // EDIT (POST)
        [HttpPost]
        public IActionResult Edit(Monument monument)
        {
            if (ModelState.IsValid)
            {
                _manager.Update(monument); // ✔ CORRECT
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Villes = new SelectList(_context.Villes, "Id", "Nom", monument.VilleId);
            return View(monument);
        }

        // DELETE (GET)
        public IActionResult Delete(int id)
        {
            var monument = _manager.GetById(id);
            if (monument == null) return NotFound();
            return View(monument);
        }

        // DELETE (POST)
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var monument = _context.Monuments.Find(id);

            if (monument == null) return NotFound();

            _context.Monuments.Remove(monument);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}