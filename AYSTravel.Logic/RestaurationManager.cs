
using AYSTravel.Data;
using   AYSTravel.Data.Entities;
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

        public List<Restauration> GetAll() =>
            _context.Restaurations.ToList();

        public Restauration GetById(int id) =>
            _context.Restaurations.FirstOrDefault(r => r.Id == id);

        public void Add(Restauration restauration)
        {
            _context.Restaurations.Add(restauration);
            _context.SaveChanges();
        }

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