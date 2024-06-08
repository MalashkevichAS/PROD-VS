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

        public IActionResult AdminPanel()
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
        public async Task<IActionResult> CreateUser(string UserName, string FirstName, string LastName, string Password, string AccessLevel)
        {
            // Проверка обязательных полей
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName))
            {
                return BadRequest("All fields are required");
            }

            var newUser = new User
            {
                UserName = UserName,              
                FirstName = FirstName,
                LastName = LastName,
                AccessLevel = AccessLevel,
                IsDeveloper = false // Ensure new user is not marked as developer
            };

            var passwordHasher = new PasswordHasher<User>();
            newUser.PasswordHash = passwordHasher.HashPassword(newUser, Password); // Assuming Password contains the plain password

            try
            {
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Логирование ошибки или вывод подробностей для отладки
                Console.WriteLine($"Error saving changes: {ex.Message}");
                var innerException = ex.InnerException;
                while (innerException != null)
                {
                    Console.WriteLine(innerException.Message);
                    innerException = innerException.InnerException;
                }
                return StatusCode(500, "Internal server error");
            }

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
