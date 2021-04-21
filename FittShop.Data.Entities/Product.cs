using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FittShop.Data.Entities.Core;

namespace FittShop.Data.Entities
{
    public class Product : IEntity<int>
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Category))] // table relationship
        public int CategoryId { get; set; }

        [MaxLength(64)]
        public string Title { get; set; }
        
        [MaxLength(256)]
        public string About { get; set; }
        
        [MaxLength(32)]
        public string Sizes { get; set; }

        [Column(TypeName = "money")] 
        public decimal Price { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        
        
        public Product()
        {
            this.Photos = new HashSet<Photo>();
        }
    }
}