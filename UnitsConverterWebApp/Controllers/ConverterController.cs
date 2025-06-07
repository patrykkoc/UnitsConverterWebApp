using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnitsConverterWebApp.Data;

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
        public async Task<IActionResult> Index()
        {
            // Pobierz wszystkie jednostki z kategoriami, żeby wyświetlić dropdowny
            var units = await _context.Units.Include(u => u.Category).ToListAsync();
            return View(units);
        }

        // POST: Converter/Convert
        [HttpPost]
        public IActionResult Convert(int fromUnitId, int toUnitId, double value)
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

            ViewBag.Result = convertedValue;
            ViewBag.FromUnit = fromUnit.Name;
            ViewBag.ToUnit = toUnit.Name;
            ViewBag.Value = value;

            // Do dropdownów ponownie przesyłamy listę jednostek
            var units = _context.Units.Include(u => u.Category).ToList();
            ViewBag.Units = units;

            return View("Index",units);
        }
    }
}
