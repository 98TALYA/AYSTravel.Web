using AYSTravel.Data;
using AYSTravel.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;


namespace AYSTravel.Logic
{
    public class MatchManager
    {
        private readonly ApplicationDbContext _context;

        public MatchManager(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🔹 Récupérer tous les matchs
        public List<Match> GetAll()
        {
            return _context.Matchs
                .Include(m => m.Ville)
                .ToList();
        }

        // 🔹 Récupérer un match par Id
        public Match GetById(int id)
        {
            return _context.Matchs
                .Include(m => m.Ville)
                .FirstOrDefault(m => m.Id == id);
        }

        // 🔹 Ajouter un match
        public void Add(Match match)
        {
            _context.Matchs.Add(match);
            _context.SaveChanges();
        }

        // 🔹 Mettre à jour un match
        public void Update(Match match)
        {
            _context.Matchs.Update(match);
            _context.SaveChanges();
        }

        // 🔹 Supprimer un match
        public void Delete(int id)
        {
            var match = GetById(id);
            if (match != null)
            {
                _context.Matchs.Remove(match);
                _context.SaveChanges();
            }
        }
    }
}