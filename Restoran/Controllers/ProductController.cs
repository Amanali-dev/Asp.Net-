using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Restoran.Data;
using Restoran.Models;

namespace Restoran.Controllers
{
    public class ProductController : Controller
    {
        private Repository<Product> products;
        private Repository <Ingredient> ingredients;
        private Repository<Category> categories;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            products = new Repository<Product>(context);
            ingredients = new Repository<Ingredient>(context);
            categories = new Repository<Category>(context);
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {

            var data = await products.GetAllAsync();
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)

        {   ViewBag.Ingredients = await ingredients.GetAllAsync();
            ViewBag.Categories = await categories.GetAllAsync();
            if (id == 0)
            {
                ViewBag.operation = "Add";
                return View(new Product());
            }
            else
            {
                Product product = await products.GetByIdAsync(id, new QueryOptions<Product>
                {
                    Includes = "ProductIngredients.Ingredient,Category",
                    Where = p => p.ProductId == id
                });
                ViewBag.operation = "Edit";
                return View(product);
               
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddEdit(Product product, int[] ingredientIds, int catId)
        {      
            // ---------------- IMAGE UPLOAD ----------------
            // Agar user ne image upload ki hai to server me save karo

            if (product.ImageFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                string uniqueFileName = Guid.NewGuid() + "_" + product.ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // File ko copy kar ke ImageUrl me unique name store karo
                using var fileStream = new FileStream(filePath, FileMode.Create);
                await product.ImageFile.CopyToAsync(fileStream);

                product.ImageUrl = uniqueFileName;
            }

            // ---------------- ADD NEW PRODUCT ----------------
            if (product.ProductId == 0)
            {
                // ViewBags populate karna optional, agar Add view me use ho raha ho
                ViewBag.Ingredients = await ingredients.GetAllAsync();
                ViewBag.Categories = await categories.GetAllAsync();

                product.CategoryId = catId;

                // Ingredients select kiye gaye ids ke hisab se add karo
                foreach (int id in ingredientIds)
                {
                    product.ProductIngredients?.Add(new ProductIngredient
                    {
                        IngredientId = id,
                        ProductId = product.ProductId
                    });
                }

                // Product add karo database me
                await products.AddAsync(product);
                return RedirectToAction("Index", "Product");
            }
            // ---------------- EDIT EXISTING PRODUCT ----------------
            else
            {
                // Existing product load karo with ingredients
                var existingProduct = await products.GetByIdAsync(product.ProductId,
                    new QueryOptions<Product> { Includes = "ProductIngredients" });

                // Basic fields update
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.Stock = product.Stock;
                existingProduct.CategoryId = catId;

                // Agar new image uploaded hai to update karo
                if (!string.IsNullOrEmpty(product.ImageUrl))
                    existingProduct.ImageUrl = product.ImageUrl;

                // ❗ Ingredient update fix:
                // Pehle old ingredients clear karo, phir new add karo
                existingProduct.ProductIngredients = new List<ProductIngredient>();
                foreach (int id in ingredientIds)
                {
                    existingProduct.ProductIngredients.Add(new ProductIngredient
                    {
                        IngredientId = id,
                        ProductId = existingProduct.ProductId
                    });
                }

                // Save changes to database
                await products.UpdateAsync(existingProduct);
                return RedirectToAction("Index", "Product");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await products.DeleteAsync(id);
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("","Product not found.");
                return RedirectToAction("Index");
            }
        }




    }
}
