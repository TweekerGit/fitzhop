using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FittShop.Data.Entities.Core;

namespace FittShop.Data.Entities
{
    public class Category : IEntity<int>
    {
        public int Id { get; set; }

        [MaxLength(32)] 
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public Category()
        {
            this.Products = new HashSet<Product>();
        }
    }
}