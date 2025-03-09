using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using UserAdminPortal.Data;
using UserAdminPortal.Models;
using UserAdminPortal.ViewModel;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace UserAdminPortal.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public AdminController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
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
                // Validate admin credentials based on email and password
                var admin = _context.Admins.FirstOrDefault(a => a.Email == model.Email && a.Password == model.Password);
                if (admin != null)
                {
                    // Save the admin's email in session
                    HttpContext.Session.SetString("AdminEmail", admin.Email);
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
            // Ensure the admin is logged in by checking session
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminEmail")))
            {
                return RedirectToAction("Login");
            }

            ViewBag.AdminEmail = HttpContext.Session.GetString("AdminEmail");

            // Fetch users for display on the dashboard
            var users = _userManager.Users.ToList();
            return View(users);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult Account()
        {
            // Retrieve the logged-in admin's email from session
            string adminEmail = HttpContext.Session.GetString("AdminEmail");
            if (string.IsNullOrEmpty(adminEmail))
            {
                return RedirectToAction("Login");
            }

            // Fetch the admin details from the database using the email
            var loggedInAdmin = _context.Admins.FirstOrDefault(a => a.Email == adminEmail);
            if (loggedInAdmin == null)
            {
                return RedirectToAction("Login");
            }

            ViewBag.AdminEmail = loggedInAdmin.Email;
            return View(loggedInAdmin);
        }

        [HttpPost]
        public IActionResult SaveAccount(Admin updatedAdmin)
        {
            // Update admin details in the database, including password if provided
            var existingAdmin = _context.Admins.Find(updatedAdmin.Id);
            if (existingAdmin != null)
            {
                existingAdmin.Name = updatedAdmin.Name;
                existingAdmin.UserName = updatedAdmin.UserName;
                existingAdmin.Email = updatedAdmin.Email;

                // Only update the password if a new one is provided.
                if (!string.IsNullOrWhiteSpace(updatedAdmin.Password))
                {
                    existingAdmin.Password = updatedAdmin.Password;
                }

                _context.SaveChanges();
            }
            return RedirectToAction("Account");
        }
    }
}
