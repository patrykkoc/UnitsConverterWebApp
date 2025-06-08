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
       

        public async Task<IActionResult> Index(int? categoryId)
        {
            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            ViewBag.SelectedCategory = categoryId;

            IQueryable<Unit> unitsQuery = _context.Units.Include(u => u.Category);

            if (categoryId.HasValue)
            {
                unitsQuery = unitsQuery.Where(u => u.CategoryId == categoryId.Value);
            }

            var units = await unitsQuery.ToListAsync();

            return View(units);
        }

        // POST: Converter/Convert
        [HttpPost]
        public IActionResult Convert(int fromUnitId, int toUnitId, double value, int? categoryId)
        {
            var fromUnit = _context.Units.Find(fromUnitId);
            var toUnit = _context.Units.Find(toUnitId);

            if (fromUnit == null || toUnit == null)
            {
                return BadRequest("Nieprawidłowe jednostki");
            }

            // Konwersja wartości do bazy, potem na docelową jednostkę
            double valueInBase = fromUnit.ToBase(value);
            double convertedValue = toUnit.FromBase(valueInBase);

            _context.Histories.Add(new HistoryEntry
            {
                FromUnitId = fromUnitId,
                ToUnitId = toUnitId,
                InputValue = value,
                OutputValue = convertedValue,
                Time = DateTime.UtcNow
            });

            _context.SaveChanges();




            ViewBag.Result = convertedValue;
            ViewBag.FromUnit = fromUnit.Name;
            ViewBag.ToUnit = toUnit.Name;
            ViewBag.Value = value;

            var categories = _context.Categories.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", categoryId);
            ViewBag.SelectedCategory = categoryId;

            IQueryable<Unit> unitsQuery = _context.Units.Include(u => u.Category);
            if (categoryId.HasValue)
            {
                unitsQuery = unitsQuery.Where(u => u.CategoryId == categoryId.Value);
            }

            var units = unitsQuery.ToList();

            return View("Index",units);
        }
    }
}
