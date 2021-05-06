using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FittShop.Data.Entities;

namespace FittShop.Models.Dto
{
    public class ProductDto
    {
        public int? Id { get; set; }
        
        [Display(Name = "Назва (заголовок)")] 
        public string Title { get; set; }
        
        [Display(Name = "Опис")] 
        public string About { get; set; }

        [Display(Name = "Розміри")] 
        public string Sizes { get; set; }

        [Display(Name = "Ціна")] 
        public decimal Price { get; set; }

        public CategoryDto Category { get; set; }
        public IEnumerable<string> PhotoUrls { get; set; }


        public ProductDto(Product product)
        {
            this.Id = product.Id;
            this.Title = product.Title;
            this.About = product.Title;
            this.Sizes = product.Sizes;
            this.Price = product.Price;
            this.Category = new CategoryDto(product.Category);
            this.PhotoUrls = product.Photos?.Select(p => p.Url);
        }

        public Product ToEntity()
        {
            return new Product
            {
                Id = this.Id ?? throw new ArgumentNullException(nameof(Id)),
                Title = this.Title,
                About = this.About,
                Sizes = this.Sizes,
                Price = this.Price,
                CategoryId = this.Category?.Id ?? throw new ArgumentNullException(nameof(Category.Id)),
                // TODO: add photos
            };
        }
    }
}