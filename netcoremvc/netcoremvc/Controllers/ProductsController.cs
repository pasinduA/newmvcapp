using Microsoft.AspNetCore.Mvc;
using netcoremvc.Models;
using netcoremvc.Services;

namespace netcoremvc.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext context;

        public ProductsController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            var products = context.Products.OrderByDescending(p=>p.Id).ToList();
            return View(products);
        }

        public IActionResult Create()
        {
            return View(); 
        }
        [HttpPost]
        public IActionResult Create(ProductDto productDto)
        {
            if(!ModelState.IsValid)
            {
                return View(productDto);
            }

            Product product = new Product()
            {
                Name=productDto.Name,
                Description=productDto.Description,
                Category=productDto.Category,
                Price=productDto.Price
            };

            context.Products.Add(product);
            context.SaveChanges();
            return RedirectToAction("Index", "Products");
        }
        public IActionResult Edit(int id)
        {
            var product = context.Products.Find(id);
            if(product == null)
            {
                return RedirectToAction("Index", "Products");
            }
            // create productDto from product
            var productDto = new ProductDto()
            {
                Name = product.Name,
                Description = product.Description,
                Category = product.Category,
                Price = product.Price
            };

            ViewData["ProductId"] = product.Id;

            return View(productDto);
        }
        [HttpPost]
        public IActionResult Edit(int id, ProductDto productDto)
        {
            var product = context.Products.Find(id);
            if(product == null)
            {
                return RedirectToAction("Index", "Products");
            }
            if (!ModelState.IsValid)
            {
                ViewData["ProductId"]=product.Id;

                return View(productDto);
            }
            //update the product in the database
            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Category = productDto.Category;
            product.Price = productDto.Price;

            context.SaveChanges();

            return RedirectToAction("Index", "Products");
        }
        public IActionResult Delete(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }
            context.Products.Remove(product);
            context.SaveChanges(true);

            return RedirectToAction("Index", "Products");
        }
    }
}
