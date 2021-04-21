using System.ComponentModel.DataAnnotations.Schema;
using FittShop.Data.Entities.Core;

namespace FittShop.Data.Entities
{
    public class Photo : IEntity<int>
    {
        public int Id { get; set; }
        
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        
        public string Url { get; set; }

        public virtual Product Product { get; set; }
    }
}