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
    public class CategoriesController : Controller
    {
        private readonly IRepository<int, Category> repository;

        public CategoriesController(IRepository<int, Category> repository)
        {
            this.repository = repository;
        }

        //Read Types
        [HttpGet]
        public async Task<IActionResult> All()
        {
            IEnumerable<Category> data = await this.repository.GetAllAsync();
            return View(data.Select(c => new CategoryDto(c)));
        }

        //Create Item
        [HttpGet]
        public IActionResult One()
        {
            return View(new CategoryDto());
        }
        
        //Update Item
        [HttpGet("{id}")]
        public async Task<IActionResult> One(int id)
        {
            Category data = await repository.GetByIdAsync(id);
            if (data is null) return this.LocalRedirect("All"); // 404

            return View(new CategoryDto(data));
        }

        //Update in Form
        [HttpPost]
        public async Task<IActionResult> One([FromForm] CategoryDto category)
        {
            if (ModelState.IsValid)
            {
                this.repository.Create(category.ToEntity());
                await this.repository.SaveAsync();
                return RedirectToAction("All");
            }

            return View(category);
        }

        //Delete Item
        [HttpGet("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            this.repository.Delete(id);
            await this.repository.SaveAsync();
            return RedirectToAction("All");
        }
    }
}