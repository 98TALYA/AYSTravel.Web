using AYSTravel.Data;
using AYSTravel.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AYSTravel.Web.Controllers
{
    [Authorize(Roles = "Admin")] // 🔒 Seul Admin peut accéder
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // -------------------------
        // Dashboard Admin
        // -------------------------
        public async Task<IActionResult> Index()
        {
            var totalUsers = await _context.Users.CountAsync();
            var totalMatches = await _context.Matchs.CountAsync();
            var totalActivities = await _context.Activites.CountAsync();
            var totalMonuments = await _context.Monuments.CountAsync();
            
            var totalTransports = await _context.MoyensTransports.CountAsync();

            var model = new AdminDashboardViewModel
            {
                TotalUsers = totalUsers,
                TotalMatchs = totalMatches,
                TotalActivites = totalActivities,
                TotalMonuments = totalMonuments,
                
                TotalTransports = totalTransports
            };

            return View(model);
        }

        // -------------------------
        // Liste des utilisateurs
        // -------------------------
        public async Task<IActionResult> Users()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

       
        // -------------------------
        // Détails d’un utilisateur
        // -------------------------
        public async Task<IActionResult> DetailsUser(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(string id, IdentityUser updatedUser)
        {
            if (id != updatedUser.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                _context.Update(updatedUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Users));
            }

            return View(updatedUser);
        }

        // -------------------------
        // Supprimer un utilisateur
        // -------------------------
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (id == null) return NotFound();

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Users));
        }
    }

    // -------------------------
    // ViewModel pour Dashboard
    // -------------------------
   
}