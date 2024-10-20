using Landing.DAL.Data;
using Landing.DAL.Models;
using Landing.PL.Areas.Dashboard.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Landing.PL.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private static List<CartItemViewModel> Cart = new List<CartItemViewModel>();

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

       
        public IActionResult Index()
        {
            return View(Cart);
        }

        
        [HttpPost]
        public async Task<IActionResult> AddToCart(int id)
        {
            
            var product = await _context.Shops.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

           
            var existingCartItem = Cart.FirstOrDefault(c => c.Id == product.Id);
            if (existingCartItem == null)
            {
                
                Cart.Add(new CartItemViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    ImageName = product.ImageName,
                    Quantity = 1
                });
            }
            else
            {
               
                existingCartItem.Quantity++;
            }

            // Redirect to the cart index
            return RedirectToAction("Index");
        }
    }
}

