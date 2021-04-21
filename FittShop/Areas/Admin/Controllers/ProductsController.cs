using System;
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
    [Route("admin/[action]")]
    public class ProductsController : Controller
    {
        private readonly IRepository<Product> repository;

        public ProductsController(IRepository<Product> repository)
        {
            this.repository = repository;
        }

        //Read Items
        [HttpGet]
        public async Task<IActionResult> All()
        {
            IEnumerable<Product> data = await this.repository.GetAllAsync();
            return View(data.Select(p => new ProductDto(p)));
        }
        
        //Create and Update Item
        [HttpGet("{id}")]
        public async Task<IActionResult> One(int id)
        {
            Product data = await repository.GetByIdAsync(id);
            if (data is null) return this.NotFound(); // 404

            return View(new ProductDto(data));
        }
        
        //Update in Form
        [HttpPost]
        public async Task<IActionResult> One([FromBody] ProductDto product)
        {
            if (ModelState.IsValid)
            {
                this.repository.Create(product.ToEntity());
                await this.repository.SaveAsync();
                return RedirectToAction("All");
            }
            return View(product);
        }
        
        //Delete Item
        [HttpGet("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            this.repository.Delete(id);
            await this.repository.SaveAsync();
            return RedirectToAction("All");
        }
    }
}