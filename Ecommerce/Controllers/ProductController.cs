using Microsoft.AspNetCore.Mvc;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Linq;

namespace Ecommerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly MyContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(MyContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details([FromRoute] int Id)
        {
            Product product = _context.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == Id);

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int Id)
        {
            Product product = _context.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == Id);

            ViewBag.categories = _context.Categories.ToList();

            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product model, [FromRoute] int id, IFormFile ImageFile)
        {
            var product = _context.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            string OldImage = product.Image;

            // Temporary variable to store the value of ImageFile
            var tempImageFile = ImageFile;

            if (tempImageFile != null && tempImageFile.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    tempImageFile.CopyTo(stream);
                    byte[] imageBytes = stream.ToArray();
                    string base64String = Convert.ToBase64String(imageBytes);
                    model.Image = base64String;
                }
            }
            else
            {
                ModelState.Remove("ImageFile");
                ImageFile = new FormFile(Stream.Null, 0, 0, "name", "filename");
                model.Image = OldImage;
            }

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.QuantityAvailable = model.QuantityAvailable;
            product.CategoryId = model.CategoryId;
            product.Image = model.Image;

            if (ModelState.IsValid)
            {
                _context.Products.Update(product);
                _context.SaveChanges();

                return RedirectToAction("Details", "Product", new { id = product.Id });
            }

            ViewBag.categories = _context.Categories.ToList();
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AddNew()
        {
            ViewBag.categories = _context.Categories.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult SaveNew(Product model, IFormFile imageFile)
        {
            if (imageFile != null)
            {
                // Check for valid image file types if needed
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                string fileExtension = Path.GetExtension(imageFile.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("ImageFileName", "Invalid image file format.");
                }
                else
                {
                    model.Image = imageFile.FileName;
                    ModelState.Remove("Image");
                    imageFile = new FormFile(Stream.Null, 0, 0, "name", "filename");
                }
            }
            else
            {
                ModelState.AddModelError("ImageFileName", "Please select a product image.");
            }

            if (ModelState.IsValid)
            {
                // Save the product and process the image file
                _context.Products.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            ViewBag.categories = _context.Categories.ToList();
            return View("AddNew", model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            Product p = _context.Products.FirstOrDefault(s => s.Id == id);
            _context.Products.Remove(p);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}
