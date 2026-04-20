using AYSTravel.Data;
using AYSTravel.Data.Entities;
using AYSTravel.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AYSTravel.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MatchAdminController : Controller
    {
        private readonly MatchManager _manager;
        private readonly ApplicationDbContext _context;

        public MatchAdminController(MatchManager manager, ApplicationDbContext context)
        {
            _manager = manager;
            _context = context;
        }

        // LISTE
        public IActionResult Index()
        {
            var matchs = _context.Matchs
                .Include(m => m.Stade)
                .ToList();

            return View(matchs);
        }

        // CREATE GET
        public IActionResult Create()
        {
            ViewBag.Stades = new SelectList(_context.Stades, "Id", "Nom");
            return View();
        }

        // CREATE POST
        [HttpPost]
        public IActionResult Create(Match match)
        {
            ModelState.Remove("Stade"); // ✅ ignore validation navigation property

            if (!ModelState.IsValid)
            {
                ViewBag.Stades = new SelectList(_context.Stades, "Id", "Nom");
                return View(match);
            }

            try
            {
                _manager.Add(match);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erreur : {ex.Message}");
                ViewBag.Stades = new SelectList(_context.Stades, "Id", "Nom");
                return View(match);
            }
        }

        // EDIT GET
        public IActionResult Edit(int id)
        {
            var match = _manager.GetById(id);
            if (match == null) return NotFound();

            ViewBag.Stades = new SelectList(_context.Stades, "Id", "Nom", match.StadeId);
            return View(match);
        }

        // EDIT POST
        [HttpPost]
        public IActionResult Edit(Match match)
        {
            if (ModelState.IsValid)
            {
                _context.Update(match);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Stades = new SelectList(_context.Stades, "Id", "Nom", match.StadeId);
            return View(match);
        }

        // DELETE GET
        public IActionResult Delete(int id)
        {
            var match = _context.Matchs
                .Include(m => m.Stade)
                .FirstOrDefault(m => m.Id == id);

            if (match == null) return NotFound();
            return View(match);
        }

        // DELETE POST
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _manager.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erreur : {ex.Message}");
                var match = _context.Matchs
                    .Include(m => m.Stade)
                    .FirstOrDefault(m => m.Id == id);
                return View(match);
            }
        }
    }
}