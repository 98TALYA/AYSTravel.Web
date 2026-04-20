using AYSTravel.Data;
using AYSTravel.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace AYSTravel.Logic
{
    public class RestaurationManager
    {
        private readonly ApplicationDbContext _context;

        public RestaurationManager(ApplicationDbContext context)
        {
            _context = context;
        }

        // -------------------------
        // GET ALL
        // -------------------------
        public List<Restauration> GetAll()
        {
            return _context.Restaurations.ToList();
        }

        // -------------------------
        // GET BY ID
        // -------------------------
        public Restauration GetById(int id)
        {
            return _context.Restaurations
                           .FirstOrDefault(r => r.Id == id);
        }

        // -------------------------
        // ADD
        // -------------------------
        public void Add(Restauration restauration)
        {
            _context.Restaurations.Add(restauration);
            _context.SaveChanges();
        }

        // -------------------------
        // UPDATE (IMPORTANT)
        // -------------------------
        public void Update(Restauration restauration)
        {
            var existing = _context.Restaurations.Find(restauration.Id);

            if (existing != null)
            {
                existing.Nom = restauration.Nom;
                existing.Type = restauration.Type;
                existing.Budget = restauration.Budget;
                existing.Adresse = restauration.Adresse;
                existing.VilleId = restauration.VilleId;

                // ⚠️ ne pas écraser image si vide
                if (!string.IsNullOrEmpty(restauration.ImageUrl))
                {
                    existing.ImageUrl = restauration.ImageUrl;
                }

                _context.SaveChanges();
            }
        }

        // -------------------------
        // DELETE
        // -------------------------
        public void Delete(int id)
        {
            var restauration = GetById(id);

            if (restauration != null)
            {
                _context.Restaurations.Remove(restauration);
                _context.SaveChanges();
            }
        }
    }
}