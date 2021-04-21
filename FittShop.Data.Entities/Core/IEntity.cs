namespace FittShop.Data.Entities.Core
{
    public interface IEntity<TKey>
    {
        public TKey Id { get; set; }
    }
}