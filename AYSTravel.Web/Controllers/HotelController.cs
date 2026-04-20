using AYSTravel.Logic;
using AYSTravel.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AYSTravel.Web.Controllers
{
    public class HotelController : Controller
    {
        private readonly HotelManager _manager;

        public HotelController(HotelManager manager)
        {
            _manager = manager;
        }

        // Liste des hôtels
        public IActionResult Index()
        {
            var hotels = _manager.GetAll()
                .Select(h => new HotelViewModel
                {
                    Id = h.Id,
                    Nom = h.Nom,
                    ImageUrl = h.ImageUrl,
                    Description = h.Description,
                    Note = h.Note,
                    BookingUrl = h.BookingUrl,
                  
                })
                .ToList();

            return View(hotels);
        }

        // Détails d’un hôtel
        public IActionResult Details(int id)
        {
            var hotelEntity = _manager.GetById(id);

            if (hotelEntity == null)
                return NotFound();

            var hotel = new HotelViewModel
            {
                Id = hotelEntity.Id,
                Nom = hotelEntity.Nom,
                ImageUrl = hotelEntity.ImageUrl,
                Description = hotelEntity.Description,
                Note = hotelEntity.Note,
                BookingUrl = hotelEntity.BookingUrl,
            
              
            };

            return View(hotel);
        }
    }
}