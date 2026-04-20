using AYSTravel.Data;
using AYSTravel.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AYSTravel.Logic
{
    public class MatchManager
    {
        private readonly ApplicationDbContext _context;

        public MatchManager(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET ALL
        public List<Match> GetAll()
        {
            return _context.Matchs
                .Include(m => m.Stade)
                .ThenInclude(s => s.Ville)
                .ToList();
        }

        // GET BY ID
        public Match? GetById(int id)
        {
            return _context.Matchs
                .Include(m => m.Stade)
                .ThenInclude(s => s.Ville)
                .FirstOrDefault(m => m.Id == id);
        }

        // ADD
        public void Add(Match match)
        {
            match.Stade = null; // ✅ évite doublon EF
            _context.Matchs.Add(match);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }

        // UPDATE
        public void Update(Match match)
        {
            var existing = _context.Matchs.FirstOrDefault(m => m.Id == match.Id);
            if (existing != null)
            {
                existing.Equipe1 = match.Equipe1;
                existing.Equipe2 = match.Equipe2;
                existing.Date = match.Date;
                existing.StadeId = match.StadeId;
                existing.Stade = null; // ✅ évite doublon EF

                try
                {
                    _context.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    throw new Exception(ex.InnerException?.Message ?? ex.Message);
                }
            }
        }

        // DELETE
        public void Delete(int id)
        {
            var match = _context.Matchs.FirstOrDefault(m => m.Id == id);
            if (match != null)
            {
                _context.Matchs.Remove(match);
                try
                {
                    _context.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    throw new Exception(ex.InnerException?.Message ?? ex.Message);
                }
            }
        }
    }
}