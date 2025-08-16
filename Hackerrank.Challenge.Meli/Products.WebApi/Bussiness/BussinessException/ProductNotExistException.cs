namespace Products.WebApi.Bussiness.BussinessException
{
    /// <summary>
    /// Excepción lanzada cuando se intenta acceder a un producto que no existe
    /// </summary>
    public class ProductNotExistException : BussinessException
    {
        public ProductNotExistException(int id) : base($"Product with id {id} does not exist.") { }
        public ProductNotExistException(string message) : base(message) { }
    }
}
