using Microsoft.AspNetCore.Mvc;
using Landing.PL.Areas.Dashboard.ViewModels;

namespace Landing.PL.Controllers
{
    public class CheckOutController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            // Returning a single instance of OrderViewModel
            var orderViewModel = new OrderViewModel();
            return View(orderViewModel);  // Pass a single OrderViewModel
        }

        [HttpPost]
        public IActionResult CompleteCheckout(OrderViewModel orderViewModel)
        {
            if (ModelState.IsValid)
            {
                // Process the order here (e.g., save to database, etc.)
                return RedirectToAction("ThankYou");
            }

            // If there are validation errors, return to the form view
            return View("Index", orderViewModel);
        }

        public IActionResult ThankYou()
        {
            return View();
        }
    }
}
