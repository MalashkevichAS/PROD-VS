using Microsoft.AspNetCore.Mvc;
using System;

namespace PROD_VS.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LoginCheck()
        {
            var lastActivityTime = HttpContext.Session.GetString("LastActivityTime");
            if (!string.IsNullOrEmpty(lastActivityTime) && DateTime.TryParse(lastActivityTime, out var lastActivity))
            {
                if (lastActivity.AddHours(2) > DateTime.Now)
                {
                    // User has been active within the last 2 hours
                    return RedirectToAction("Index", "Home");
                }
            }

            // User has not been active, show login form
            return RedirectToAction("ShowLoginForm");
        }

        public IActionResult ShowLoginForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Authenticate(string username, string password)
        {
            // Здесь должна быть ваша логика аутентификации
            if (username == "admin" && password == "password")
            {
                HttpContext.Session.SetString("LastActivityTime", DateTime.Now.ToString());
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid username or password");
            return View("ShowLoginForm");
        }
    }
}
