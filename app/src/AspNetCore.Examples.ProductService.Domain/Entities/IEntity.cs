namespace AspNetCore.Examples.ProductService.Entities
{
    public interface IEntity<out T>
    {
        public T Id { get; }
    }
}