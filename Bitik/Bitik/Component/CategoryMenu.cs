using Bitik.Data;
using Bitik.Models;
using Microsoft.AspNetCore.Mvc;


namespace Bitik.Component
{
    public class CategoryMenu : ViewComponent

    {
        private readonly ApplicationDbContext _context;

        public CategoryMenu(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }
    }
}
