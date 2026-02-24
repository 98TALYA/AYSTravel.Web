

using AYSTravel.Data;
using AYSTravel.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace AYSTravel.Logic
{
    
    public class ActiviteManager
    {
        private readonly ApplicationDbContext _context;
        public ActiviteManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Activite> GetAll()
        {
            return _context.Activites.ToList();
        }

        public Activite GetById(int id)
        {
            return _context.Activites.FirstOrDefault(a => a.Id == id);
        }

        public void Add(Activite activite)
        {
            _context.Activites.Add(activite);
            _context.SaveChanges();
        }

        public void Update(Activite activite)
        {
            _context.Activites.Update(activite);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var activite = GetById(id);
            if (activite != null)
            {
                _context.Activites.Remove(activite);
                _context.SaveChanges();
            }
        }
    }
}