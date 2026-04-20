using AYSTravel.Data;
using AYSTravel.Data.Entities;
using AYSTravel.Logic.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AYSTravel.Logic
{
    public class HotelManager
    {
        private readonly ApplicationDbContext _context;

        public HotelManager(ApplicationDbContext context)
        {
            _context = context;
        }

        // -------------------------
        // GET ALL
        // -------------------------
        public List<Hotel> GetAll()
        {
            return _context.Hotels.ToList();
        }

        // -------------------------
        // GET BY ID
        // -------------------------
        public Hotel GetById(int id)
        {
            return _context.Hotels
                           .FirstOrDefault(h => h.Id == id);
        }

        public void Add(Hotel hotel)
        {
            _context.Hotels.Add(hotel);
            _context.SaveChanges();
        }

        // -------------------------
        // UPDATE (IMPORTANT)
        // -------------------------
        public void Update(Hotel hotel)
        {
            var existing = _context.Hotels.Find(hotel.Id);

            if (existing != null)
            {
                // Préserver l'image actuelle
                var existingImage = existing.ImageUrl;
                existing.ImageUrl = existingImage;
                existing.Nom = hotel.Nom;
                existing.Description = hotel.Description;
                existing.Note = hotel.Note;
                existing.BookingUrl = hotel.BookingUrl;
                existing.Population = hotel.Population;

                // Copier toutes les valeurs de hotel vers existing
                _context.Entry(existing).CurrentValues.SetValues(hotel);

                // ⚠️ ne pas écraser image si vide
                if (string.IsNullOrEmpty(hotel.ImageUrl))
                {
                    existing.ImageUrl = hotel.ImageUrl;
                }

                _context.SaveChanges();
            }
        }

        // -------------------------
        // DELETE
        // -------------------------
        public void Delete(int id)
        {
            var hotel = GetById(id);
            if (hotel != null)
            {
                _context.Hotels.Remove(hotel);
                _context.SaveChanges();
            }
        }
    }
}