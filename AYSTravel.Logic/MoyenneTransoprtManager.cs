
using AYSTravel.Data;
using AYSTravel.Data.Entities;


namespace AYSTravel.Logic
{
    public class MoyenTransportManager
    {
        private readonly ApplicationDbContext _context;

        public MoyenTransportManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<MoyenTransport> GetAll() =>
            _context.MoyensTransports.ToList();

        public MoyenTransport GetById(int id) =>
            _context.MoyensTransports.FirstOrDefault(t => t.Id == id);

        public void Add(MoyenTransport transport)
        {
            _context.MoyensTransports.Add(transport);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var transport = GetById(id);
            if (transport != null)
            {
                _context.MoyensTransports.Remove(transport);
                _context.SaveChanges();
            }
        }
    }
}