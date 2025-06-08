using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using UnitsConverterWebApp.Data;
using UnitsConverterWebApp.Models;

namespace UnitsConverterWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UnitsConverterWebAppContext _context;

        public AccountController(UnitsConverterWebAppContext context)
        {
            _context = context;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users
              .Include(u => u.Role)
              .FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role.Name);
                return RedirectToAction("Index", "Converter");

            }

            ViewBag.Error = "Invalid credentials";
            return View();
        }

        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(string username, string password)
        {
            if (_context.Users.Any(u => u.Username == username))
            {
                ViewBag.Error = "Username already taken";
                return View();
            }

            var role = _context.Roles.FirstOrDefault(r => r.Name == "User");  
            var user = new User { Username = username, PasswordHash = password, Role = role };

            _context.Users.Add(user);
            _context.SaveChanges();

            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role.Name);

            return RedirectToAction("Index", "Converter");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }

}
