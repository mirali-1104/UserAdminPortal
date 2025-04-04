﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using UserAdminPortal.Data;
using UserAdminPortal.Models;
using UserAdminPortal.ViewModel;

namespace UserAdminPortal.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly AppDbContext _context;


        public AccountController(AppDbContext context, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            _context = context;
        }

        public IActionResult UserAccount()
        {
            var userEmail = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user != null)
            {
                var model = new UserViewModel
                {
                    Name = user.FullName,
                    Email = user.Email
                };
                return View(model);
            }

            return RedirectToAction("Login");
        }
        [HttpPost]
        public IActionResult UpdateProfile(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userEmail = User.Identity.Name;
                var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);

                if (user != null)
                {
                    user.FullName = model.Name;

                    if (!string.IsNullOrEmpty(model.Password) && model.Password == model.ConfirmPassword)
                    {
                        user.Password = model.Password;  // Ensure password hashing here
                    }

                    _context.SaveChanges();
                    return RedirectToAction("UserAccount");
                }
            }

            return View("UserAccount", model);
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Email or password is incorrect.");
                    return View(model);
                }
            }
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {

            if (!ModelState.IsValid)
            {
                User user = new User
                {
                    FullName = model.Name,
                    Email = model.Email,
                    UserName = model.Email,
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    Console.WriteLine("User registered successfully"); // Debugging
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    Console.WriteLine("User registration failed"); // Debugging
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine(error.Description);
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(model);
                }
            }
            else
            {
                Console.WriteLine("User registration failed");
            }

            return View(model);
        }


        public IActionResult VerifyEmail()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Email);

                if (user == null)
                {
                    ModelState.AddModelError("", "Something is wrong!");
                    return View(model);
                }
                else
                {
                    return RedirectToAction("ChangePassword", "Account", new { username = user.UserName });
                }
            }
            return View(model);
        }

        public IActionResult ChangePassword(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("VerifyEmail", "Account");
            }
            return View(new ChangePasswordViewModel { Email = username });
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    var result = await userManager.RemovePasswordAsync(user);
                    if (result.Succeeded)
                    {
                        result = await userManager.AddPasswordAsync(user, model.NewPassword);
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email not found!");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong. try again.");
                return View(model);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}