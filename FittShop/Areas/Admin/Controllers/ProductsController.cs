using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FittShop.Data.Abstracts;
using FittShop.Data.Entities;
using FittShop.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace FittShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IRepository<int, Product> productsRepository;
        private readonly IRepository<int, Category> categoriesRepository;

        public ProductsController(IRepository<int, Product> productsRepository, IRepository<int, Category> categoriesRepository)
        {
            this.productsRepository = productsRepository;
            this.categoriesRepository = categoriesRepository;
        }

        //Read Items
        [HttpGet]
        public async Task<IActionResult> All()
        {
            IEnumerable<Product> data = await this.productsRepository.GetAllAsync(p => p.Photos, p => p.Category);

            foreach (Product product in data)
                product.Category = await this.categoriesRepository.GetByIdAsync(product.CategoryId);

            return View(data.Select(p => new ProductDto(p)));
        }

        //Create and Update Item
        [HttpGet("{id}")]
        public async Task<IActionResult> One(int id)
        {
            Product data = await this.productsRepository.GetByIdAsync(id, p => p.Photos, p => p.Category);
            if (data is null) return this.NotFound(); // 404

            return View(new ProductDto(data));
        }
        
        //Update in Form
        [HttpPost]
        public async Task<IActionResult> One([FromForm] ProductDto product)
        {
            if (ModelState.IsValid)
            {
                this.productsRepository.Create(product.ToEntity());
                await this.productsRepository.SaveAsync();
                return RedirectToAction("All");
            }
            return View(product);
        }
        
        //Delete Item
        [HttpGet("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            this.productsRepository.Delete(id);
            await this.productsRepository.SaveAsync();
            return RedirectToAction("All");
        }
    }
}