using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using UserAdminPortal.Data;
using UserAdminPortal.Models;
using UserAdminPortal.ViewModel;

namespace UserAdminPortal.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = _context.Admins.FirstOrDefault(a => a.Email == model.Email && a.Password == model.Password);

                if (admin != null)
                {
                    HttpContext.Session.SetString("AdminEmail", admin.Email);
                    HttpContext.Session.SetString("AdminName", admin.Name);
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid email or password.");
                }
            }
            return View(model);
        }


        public IActionResult Dashboard()
        {
            // Check if admin session exists
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminEmail")))
            {
                return RedirectToAction("Login");
            }

            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

    }
}
