using AYSTravel.Data;
using AYSTravel.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AYSTravel.Logic
{
    public class VilleManager
    {
        private readonly ApplicationDbContext _context;

        public VilleManager(ApplicationDbContext context)
        {
            _context = context;
        }

        // Récupère toutes les villes avec leurs données
        public List<Ville> GetAll()
        {
            return _context.Villes
                .Include(v => v.Hotels)
                .Include(v => v.Monuments)
                .Include(v => v.Activites)
                .Include(v => v.MoyensTransports)
                .Include(v => v.Restaurations)
                .Include(v => v.Stades)
                    .ThenInclude(s => s.Matchs)
                .ToList();
        }

        // Récupère une ville par Id
        public Ville GetById(int id)
        {
            return _context.Villes
                .Include(v => v.Hotels)
                .Include(v => v.Monuments)
                .Include(v => v.Activites)
                .Include(v => v.MoyensTransports)
                .Include(v => v.Restaurations)
                .Include(v => v.Stades)
                    .ThenInclude(s => s.Matchs)
                .FirstOrDefault(v => v.Id == id);
        }

        // Ajouter une ville
        public void Add(Ville ville)
        {
            _context.Villes.Add(ville);
            _context.SaveChanges();
        }

        // Mettre à jour une ville
        public void Update(Ville ville)
        {
            var existing = _context.Villes.Find(ville.Id);

            if (existing != null)
            {
                existing.Nom = ville.Nom;
                existing.Description = ville.Description;
                existing.ImageUrl = ville.ImageUrl;

                _context.SaveChanges();
            }
        }



        // Supprimer une ville
        public void Delete(int id)
        {
            var ville = _context.Villes.Find(id);

            if (ville != null)
            {
                _context.Villes.Remove(ville);
                _context.SaveChanges();
            }
        }

        // Villes similaires
        public List<Ville> GetSimilarCities(int villeId, int take = 3)
        {
            return _context.Villes
                .Where(v => v.Id != villeId)
                .Take(take)
                .ToList();
        }
    }
}