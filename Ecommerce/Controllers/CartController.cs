using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.Controllers
{
    public class CartController : Controller
    {
        private readonly MyContext _dbContext;

        public CartController(MyContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        //[Authorize]
        public IActionResult AddToCart(int productId, int quantity)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var product = _dbContext.Products.FirstOrDefault(p => p.Id == productId);

                if (product == null)
                {
                    return NotFound();
                }

                if (quantity <= 0 || quantity > product.QuantityAvailable)
                {
                    TempData["ErrorMessage"] = "Invalid quantity or not enough stock.";
                    return RedirectToAction("Details", "Product", new { id = productId });
                }

                // Check if the product is already in the user's cart
                var cartItem = _dbContext.Carts
                    .FirstOrDefault(c => c.UserId == int.Parse(userId) && c.ProductId == productId);

                if (cartItem != null)
                {
                    // If the product is already in the cart, update the quantity
                    cartItem.Quantity += quantity;
                }
                else
                {
                    // Otherwise, create a new cart item
                    var newCartItem = new Cart
                    {
                        UserId = int.Parse(userId),
                        ProductId = productId,
                        Quantity = quantity
                    };

                    _dbContext.Carts.Add(newCartItem);
                }

                product.QuantityAvailable -= quantity;

                _dbContext.SaveChanges();

                TempData["SuccessMessage"] = "Product added to cart successfully.";
                return RedirectToAction("Details", "Product", new { id = productId });
            }
            else
            {
                TempData["ErrorMessage"] = "You have to log in first.";
                return RedirectToAction("Details", "Product", new { id = productId });
            }
        }

        [Authorize]
        public IActionResult ShowCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cartItems = _dbContext.Carts
                .Include(c => c.Product)
                .Where(c => c.UserId == int.Parse(userId))
                .ToList();

            return View(cartItems);
        }

        [HttpPost]
        //[Authorize]
        public IActionResult RemoveFromCart(int cartItemId)
        {
            var cartItem = _dbContext.Carts.FirstOrDefault(c => c.Id == cartItemId);

            if (cartItem != null)
            {
                var product = _dbContext.Products.FirstOrDefault(p => p.Id == cartItem.ProductId);
                product.QuantityAvailable += cartItem.Quantity;

                _dbContext.Carts.Remove(cartItem);
                _dbContext.SaveChanges();
            }

            return RedirectToAction("ShowCart");
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int cartItemId, int quantity)
        {
            Console.WriteLine($"You entered new quantity = {quantity} ");
            var cartItem = _dbContext.Carts.FirstOrDefault(c => c.Id == cartItemId);

            if (cartItem != null)
            {
                var product = _dbContext.Products.FirstOrDefault(p => p.Id == cartItem.ProductId);

                if (quantity <= 0 || quantity > product.QuantityAvailable)
                {
                    TempData["ErrorMessage"] = "Invalid quantity or not enough stock.";
                }
                else
                {
                    product.QuantityAvailable += cartItem.Quantity - quantity;
                    cartItem.Quantity = quantity;

                    _dbContext.SaveChanges();
                }
            }

            return RedirectToAction("ShowCart");
        }

        public IActionResult Checkout(int id)
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Checkout()
        {
            // Clear the Carts
            _dbContext.Carts.RemoveRange(_dbContext.Carts);
            _dbContext.SaveChanges();

            return RedirectToAction("SuccessOrder");
        }

        public IActionResult SuccessOrder()
        {
            return View();
        }
    }
}
