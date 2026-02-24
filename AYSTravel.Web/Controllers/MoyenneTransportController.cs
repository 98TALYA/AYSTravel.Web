using AYSTravel.Logic;
using Microsoft.AspNetCore.Mvc;

namespace AYSTravel.Web.Controllers
{
    
    public class MoyenTransportController : Controller
    {
        private readonly MoyenTransportManager _manager;

        public MoyenTransportController(MoyenTransportManager manager)
        {
            _manager = manager;
        }

        public IActionResult Index()
        {
            return View(_manager.GetAll());
        }

        public IActionResult Details(int id)
        {
            var transport = _manager.GetById(id);
            if (transport == null) return NotFound();
            return View(transport);
        }
    }
}