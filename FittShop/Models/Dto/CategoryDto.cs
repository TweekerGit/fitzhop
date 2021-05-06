using System;
using System.ComponentModel.DataAnnotations;
using FittShop.Data.Entities;

namespace FittShop.Models.Dto
{
    public class CategoryDto
    {
        public int? Id { get; set; }
        
        [Display(Name = "Тип товару")]
        public string Name { get; set; }


        public CategoryDto() { }

        public CategoryDto(Category category)
        {
            this.Id = category.Id;
            this.Name = category.Name;
        }

        public Category ToEntity()
        {
            return new Category
            {
                Id = this.Id ?? throw new ArgumentNullException(nameof(Id)),
                Name = this.Name
            };
        }
    }
}