using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restoran.Data;
using Restoran.Helpers;
using Restoran.Models;

namespace Restoran.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Show products + cart
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel") ?? new OrderViewModel
            {
                Products = await _context.Products.ToListAsync(),
                OrderItems = new List<OrderItemViewModel>()
            };
            return View(model);
        }

        // Add item to cart
        [HttpPost]
        public async Task<IActionResult> AddItem(int prodId, int prodQty)
        {
            var product = await _context.Products.FindAsync(prodId);
            if (product == null) return NotFound();

            var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel") ?? new OrderViewModel
            {
                Products = await _context.Products.ToListAsync(),
                OrderItems = new List<OrderItemViewModel>()
            };

            var existingItem = model.OrderItems.FirstOrDefault(i => i.ProductId == prodId);
            if (existingItem != null)
                existingItem.Quantity += prodQty;
            else
                model.OrderItems.Add(new OrderItemViewModel
                {
                    ProductId = product.ProductId,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = prodQty
                });

            model.TotalAmount = model.OrderItems.Sum(i => i.Price * i.Quantity);
            HttpContext.Session.Set("OrderViewModel", model);

            return RedirectToAction("Create");
        }

        // View cart
        [HttpGet]
        public IActionResult Cart()
        {
            var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel");
            if (model == null || !model.OrderItems.Any())
                return RedirectToAction("Create");
            return View(model);
        }

        // Remove item from cart
        [HttpPost]
        public IActionResult RemoveItem(int prodId)
        {
            var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel");
            if (model != null)
            {
                var item = model.OrderItems.FirstOrDefault(i => i.ProductId == prodId);
                if (item != null)
                    model.OrderItems.Remove(item);

                model.TotalAmount = model.OrderItems.Sum(i => i.Price * i.Quantity);
                HttpContext.Session.Set("OrderViewModel", model);
            }
            return RedirectToAction("Cart");
        }

        // Place order
        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
        {
            var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel");
            if (model == null || !model.OrderItems.Any())
                return RedirectToAction("Create");

            var order = new Order
            {
                OrderDate = DateTime.Now,
                TotalAmount = model.TotalAmount,
                UserId = _userManager.GetUserId(User),
                OrderItems = new List<OrderItem>()
            };

            foreach (var item in model.OrderItems)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove("OrderViewModel");

            return RedirectToAction("ViewOrders");
        }

        // View user orders
        [HttpGet]
        public async Task<IActionResult> ViewOrders()
        {
            var userId = _userManager.GetUserId(User);
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .ToListAsync();

            return View(orders);
        }

        
    }
}
