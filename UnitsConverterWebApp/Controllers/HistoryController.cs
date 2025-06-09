using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnitsConverterWebApp.Data;

namespace UnitsConverterWebApp.Controllers
{
    public class HistoryController : Controller
    {
        private readonly UnitsConverterWebAppContext _context;

        public HistoryController(UnitsConverterWebAppContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? categoryId, int pageNumber = 1, int pageSize = 10)
        {
            string? username = HttpContext.Session.GetString("Username");
            string? role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(username))
            {
                // Brak zalogowanego użytkownika - przekieruj na login lub pokaż pustą listę
                return RedirectToAction("Login", "Account");
            }

            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                // Nie znaleziono użytkownika w bazie - możesz obsłużyć to odpowiednio
                return RedirectToAction("Login", "Account");
            }

            var query = _context.Histories
                .Include(h => h.FromUnit).ThenInclude(u => u.Category)
                .Include(h => h.ToUnit).ThenInclude(u => u.Category)
                .Include(h => h.User)   
                .OrderByDescending(h => h.Time)
                .AsQueryable();


            if (role != "Admin")
            {
                query = query.Where(h => h.UserId == user.Id);
            }

            if (categoryId.HasValue)
            {
                query = query.Where(h => h.FromUnit.CategoryId == categoryId.Value);
            }

            query = query.OrderByDescending(h => h.Time);

            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var histories = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewData["PageNumber"] = pageNumber;
            ViewData["PageSize"] = pageSize;
            ViewData["TotalPages"] = totalPages;
            ViewData["SelectedCategoryId"] = categoryId;

            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name");

            return View(histories);
        }


        public IActionResult Statistics()
        {
            var totalConversions = _context.Histories.Count();

            var mostUsedUnit = _context.Histories
                .GroupBy(h => h.ToUnitId)
                .OrderByDescending(g => g.Count())
                .Select(g => new
                {
                    UnitId = g.Key,
                    Count = g.Count(),
                    UnitName = _context.Units.FirstOrDefault(u => u.Id == g.Key)!.Name
                })
                .FirstOrDefault();
            var categoryUnitCounts = _context.Categories
              .Select(c => new
              {
                  CategoryName = c.Name,
                  UnitCount = _context.Units.Count(u => u.CategoryId == c.Id)
              })
              .ToList();


            ViewBag.TotalConversions = totalConversions;
            ViewBag.MostUsedUnitName = mostUsedUnit?.UnitName ?? "N/A";
            ViewBag.MostUsedCount = mostUsedUnit?.Count ?? 0;
            ViewBag.CategoryUnitCounts = categoryUnitCounts;

            return View();
        }
        public IActionResult Delete(int id)
        {
            var entry = _context.Histories.Find(id);
            if (entry == null)
                return NotFound();

            
            if (HttpContext.Session.GetString("Role") != "Admin")
                return Forbid();

            _context.Histories.Remove(entry);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
