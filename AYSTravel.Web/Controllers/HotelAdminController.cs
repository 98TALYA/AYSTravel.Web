using AYSTravel.Data;
using AYSTravel.Data.Entities;
using AYSTravel.Logic;
using AYSTravel.Logic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace AYSTravel.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HotelAdminController : Controller
    {
        private readonly HotelManager _manager;
        private readonly ApplicationDbContext _context;

        public HotelAdminController(HotelManager manager, ApplicationDbContext context)
        {
            _manager = manager;
            _context = context;
        }

        // -------------------------
        // LISTE
        // -------------------------
        public IActionResult Index()
        {
            var hotels = _context.Hotels.ToList();
            return View(hotels);
        }

        // CREATE

        // CREATE - GET
        public IActionResult Create()
        {
            ViewBag.Villes = new SelectList(_context.Villes, "Id", "Nom");
            return View();
        }

        // CREATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Hotel hotel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Villes = new SelectList(_context.Villes, "Id", "Nom");
                return View(hotel);
            }

            hotel.ImageUrl ??= "default.jpg";

            _manager.Add(hotel);

            return RedirectToAction(nameof(Index));
        }

        // -------------------------
        // EDIT - GET
        // -------------------------
        public IActionResult Edit(int id)
        {
            // Récupérer l'hôtel par son identifiant via le manager
            var hotel = _manager.GetById(id);
            if (hotel == null) return NotFound();

            // Préparer la liste des villes pour le dropdown, sélectionner la ville courante de l'hôtel
            ViewBag.Villes = new SelectList(_context.Villes, "Id", "Nom", hotel.VilleId);
            return View(hotel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id,Hotel hotel)
        {
            if (id != hotel.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.Villes = new SelectList(_context.Villes, "Id", "Nom", hotel.VilleId);
                return View(hotel);
            }

            var existingHotel = _manager.GetById(id);
            if (existingHotel == null)
                return NotFound();

            // mise à jour
            existingHotel.Nom = hotel.Nom;
            existingHotel.Description = hotel.Description;
            existingHotel.VilleId = hotel.VilleId;
            existingHotel.Note = hotel.Note;
            existingHotel.BookingUrl = hotel.BookingUrl;
            existingHotel.Population = hotel.Population;
            

            // 🔥 SAUVEGARDE
            _manager.Update(existingHotel);

            return RedirectToAction(nameof(Index));
        }

        // -------------------------
        // DELETE - GET
        // -------------------------
        public IActionResult Delete(int id)
        {
            var hotel = _manager.GetById(id);
            if (hotel == null) return NotFound();

            return View(hotel);
        }

        // -------------------------
        // DELETE - POST
        // -------------------------
        [HttpPost, ActionName("Delete")]
        
        public IActionResult DeleteConfirmed(int id)
        {
            _manager.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}