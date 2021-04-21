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
        private readonly IRepository<int, Product> repository;

        public ProductsController(IRepository<int, Product> repository) => this.repository = repository;


        [HttpGet("{categoryId?}")]
        public async Task<IActionResult> All(int? categoryId = null)
        {
            IEnumerable<Product> data = categoryId is null
                ? await repository.GetAllAsync()
                : await repository.GetByFilterAsync(p => p.CategoryId == categoryId);

            if (data is null) return this.NotFound(); // 404

            return View(data.Select(e => new ProductDto(e)));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> One([FromRoute] int id)
        {
            Product data = await repository.GetByIdAsync(id);
            if (data is null) return this.NotFound(); // 404

            return View(new ProductDto(data));
        }

        [HttpPost]
        public async Task<StatusCodeResult> Create([FromBody] ProductDto product)
        {
            if (!ModelState.IsValid) return this.BadRequest();
            
            product.Id = null;
            this.repository.Create(product.ToEntity());
            await this.repository.SaveAsync();

            return this.StatusCode(201); // Created
        }

        [HttpPost]
        public async Task<StatusCodeResult> Update([FromBody] ProductDto product)
        {
            if (!ModelState.IsValid) return this.BadRequest();

            if (await this.repository.CountAsync(e => e.Id == product.Id) == 0)
                return this.NotFound();

            this.repository.Update(product.ToEntity());
            await this.repository.SaveAsync();

            return this.StatusCode(204); // No Content
        }

        [HttpPost("{id}")]
        public async Task<StatusCodeResult> Delete([FromRoute] int id)
        {
            if (await this.repository.CountAsync(e => e.Id == id) == 0)
                return this.NotFound();

            this.repository.Delete(id);
            await this.repository.SaveAsync();

            return this.StatusCode(204); // No Content
        }
    }
}