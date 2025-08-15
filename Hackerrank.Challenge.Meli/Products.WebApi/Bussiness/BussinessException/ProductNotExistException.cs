namespace Products.WebApi.Bussiness.BussinessException
{
    public class ProductNotExistException : BussinessException
    {
        public ProductNotExistException(int id) : base($"Product with id {id} does not exist.") { }
    }
}
