using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Market.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Market.Models;


namespace Market.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductDbContext _context;

        public ProductController(ProductDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.Include("Category").ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();
                   
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();

                return RedirectToAction("index");
            }
            ViewBag.Categories = _context.Categories.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();

            return View(product);
        }

        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null) return NotFound();

            _context.Products.Remove(product);

            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            var product = _context.Products.Include("Category").FirstOrDefault(p => p.Id == id);

            if (product == null) return NotFound();

            ViewBag.Categories = _context.Categories.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {

            if (ModelState.IsValid)
            {
                _context.Entry(product).State = EntityState.Modified;

                _context.SaveChanges();

                return RedirectToAction("index");
            }

            ViewBag.Categories = _context.Categories.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();

            return View(product);
        }
    }
}