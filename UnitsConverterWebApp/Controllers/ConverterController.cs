using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnitsConverterWebApp.Data;
using UnitsConverterWebApp.Models;

namespace UnitsConverterWebApp.Controllers
{
    public class ConverterController : Controller
    {
        private readonly UnitsConverterWebAppContext _context;

        public ConverterController(UnitsConverterWebAppContext context)
        {
            _context = context;
        }

        // GET: Converter


        public async Task<IActionResult> Index(int categoryId = 1)
        {
            var categories = await _context.Categories.ToListAsync();

            if (!categories.Any(c => c.Id == categoryId))
            {
                // Jeśli podany categoryId jest niepoprawny, ustaw na pierwszy z listy
                categoryId = categories.FirstOrDefault()?.Id ?? 1;
            }

            ViewBag.Categories = new SelectList(categories, "Id", "Name", categoryId);
            ViewBag.SelectedCategory = categoryId;

            var units = await _context.Units
                .Include(u => u.Category)
                .Where(u => u.CategoryId == categoryId)
                .ToListAsync();

            return View(units);
        }


        // POST: Converter/Convert
        [HttpPost]
        public IActionResult Convert(int fromUnitId, int toUnitId, double value, int? categoryId)
        {
            var fromUnit = _context.Units.Find(fromUnitId);
            var toUnit = _context.Units.Find(toUnitId);

            if (fromUnit == null || toUnit == null)
                return BadRequest("Nieprawidłowe jednostki");

            double valueInBase = fromUnit.ToBase(value);
            double convertedValue = toUnit.FromBase(valueInBase);

            var username = HttpContext.Session.GetString("Username");
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            // Zapis historii tylko jeśli użytkownik jest zalogowany
            if (user != null)
            {
                _context.Histories.Add(new HistoryEntry
                {
                    FromUnitId = fromUnitId,
                    ToUnitId = toUnitId,
                    InputValue = value,
                    OutputValue = convertedValue,
                    Time = DateTime.UtcNow,
                    UserId = user.Id
                });

                _context.SaveChanges();
            }

            ViewBag.Result = convertedValue;
            ViewBag.FromUnit = fromUnit.Name;
            ViewBag.ToUnit = toUnit.Name;
            ViewBag.Value = value;

            var categories = _context.Categories.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", categoryId);
            ViewBag.SelectedCategory = categoryId;

            IQueryable<Unit> unitsQuery = _context.Units.Include(u => u.Category);
            if (categoryId.HasValue)
                unitsQuery = unitsQuery.Where(u => u.CategoryId == categoryId.Value);

            var units = unitsQuery.ToList();

            return View("Index", units);
        }


    }
}
