using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROD_VS.Data;
using PROD_VS.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PROD_VS.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Users()
        {
            var users = await _context.Users
                                      .Where(u => !u.IsDeveloper)
                                      .ToListAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User newUser)
        {
            newUser.IsDeveloper = false; // Ensure new user is not marked as developer
            var passwordHasher = new PasswordHasher<User>();
            newUser.PasswordHash = passwordHasher.HashPassword(newUser, newUser.PasswordHash); // Assuming PasswordHash contains the plain password
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return RedirectToAction("Users");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null && !user.IsDeveloper) // Prevent deletion of developer
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Users");
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(string id, string newPassword)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null && !user.IsDeveloper) // Prevent password change for developer
            {
                user.PasswordHash = new PasswordHasher<User>().HashPassword(user, newPassword);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Users");
        }
    }
}
