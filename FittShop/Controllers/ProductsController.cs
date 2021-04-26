using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FittShop.Data.Abstracts;
using FittShop.Data.Entities;
using FittShop.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace FittShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IRepository<int, Product> productsRepository;
        private readonly IRepository<int, Category> categoriesRepository;

        public ProductsController(IRepository<int, Product> productsRepository, IRepository<int, Category> categoriesRepository)
        {
            this.productsRepository = productsRepository;
            this.categoriesRepository = categoriesRepository;
        }


        [HttpGet]
        public async Task<IActionResult> All(int? categoryId = null)
        {
            Category category = categoryId is null ? null : await this.categoriesRepository.GetByIdAsync(categoryId.Value);

            IEnumerable<Product> data = categoryId is null
                ? await this.productsRepository.GetAllAsync(p => p.Photos, p => p.Category)
                : await this.productsRepository.GetByFilterAsync(p => p.CategoryId == categoryId, p => p.Photos, p => p.Category);

            if (data is null) return this.NotFound(); // 404

            return View(data.Select(p => new ProductDto(p)));
        }

        [HttpGet("products/{id}")]
        public async Task<IActionResult> One([FromRoute] int id)
        {
            Product data = await productsRepository.GetByIdAsync(id, p => p.Photos, p => p.Category);
            if (data is null) return this.NotFound(); // 404
            
            return View(new ProductDto(data));
        }

        [HttpPost("products")]
        public async Task<StatusCodeResult> Create([FromBody] ProductDto product)
        {
            if (!ModelState.IsValid) return this.BadRequest();
            
            product.Id = null;
            this.productsRepository.Create(product.ToEntity());
            await this.productsRepository.SaveAsync();

            return this.StatusCode(201); // Created
        }

        [HttpPost("products")]
        public async Task<StatusCodeResult> Update([FromBody] ProductDto product)
        {
            if (!ModelState.IsValid) return this.BadRequest();

            if (await this.productsRepository.CountAsync(e => e.Id == product.Id) == 0)
                return this.NotFound();

            this.productsRepository.Update(product.ToEntity());
            await this.productsRepository.SaveAsync();

            return this.StatusCode(204); // No Content
        }

        [HttpPost("products/{id}")]
        public async Task<StatusCodeResult> Delete([FromRoute] int id)
        {
            if (await this.productsRepository.CountAsync(e => e.Id == id) == 0)
                return this.NotFound();

            this.productsRepository.Delete(id);
            await this.productsRepository.SaveAsync();

            return this.StatusCode(204); // No Content
        }
    }
}