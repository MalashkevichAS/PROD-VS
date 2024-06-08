using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PROD_VS.Data;
using PROD_VS.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace PROD_VS.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AuthorizationController> _logger;

        public AuthorizationController(ApplicationDbContext context, SignInManager<User> signInManager, UserManager<User> userManager, ILogger<AuthorizationController> logger)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Authorization()
        {
            ViewData["HideNavbar"] = true;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string pass, bool rememberMe)
        {
            _logger.LogInformation("Attempting to log in user: {Username}", username);

            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                _logger.LogWarning("User not found: {Username}", username);
                ViewBag.ErrorMessage = "Invalid username or password";
                return View("Authorization");
            }

            _logger.LogInformation("User found: {Username}", username);

            var result = await _signInManager.PasswordSignInAsync(user, pass, rememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation("Login successful for user: {Username}", username);

                if (user.AccessLevel == "Full" || user.AccessLevel == "Admin")
                {
                    _logger.LogInformation("Redirecting user {Username} to AdminPanel", username);
                    return RedirectToAction("AdminPanel", "Admin");
                }
                else
                {
                    _logger.LogInformation("Redirecting user {Username} to Home/Index", username);
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                _logger.LogWarning("Invalid login attempt for user: {Username}", username);
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out: {Username}", username);
                }
                if (result.IsNotAllowed)
                {
                    _logger.LogWarning("User not allowed to sign in: {Username}", username);
                }
                if (result.RequiresTwoFactor)
                {
                    _logger.LogWarning("User requires two-factor authentication: {Username}", username);
                }
                ViewBag.ErrorMessage = "Invalid username or password";
                return View("Authorization");
            }
        }
    }
}
