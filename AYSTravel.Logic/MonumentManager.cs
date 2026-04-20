
using AYSTravel.Data;
using AYSTravel.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace AYSTravel.Logic
{

    public class MonumentManager
    {
        private readonly ApplicationDbContext _context;

        public MonumentManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Monument> GetAll() => _context.Monuments.ToList();

        public Monument GetById(int id) =>
            _context.Monuments.FirstOrDefault(m => m.Id == id);

        public void Add(Monument monument)
        {
            _context.Monuments.Add(monument);
            _context.SaveChanges();
        }

        public void Update(Monument monument)
        {
            _context.Monuments.Update(monument);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var monument = GetById(id);
            if (monument != null)
            {
                _context.Monuments.Remove(monument);
                _context.SaveChanges();
            }
        }
    }
}