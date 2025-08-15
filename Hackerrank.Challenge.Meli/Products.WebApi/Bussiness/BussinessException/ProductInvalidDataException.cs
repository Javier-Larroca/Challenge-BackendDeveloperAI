namespace Products.WebApi.Bussiness.BussinessException
{
    public class ProductInvalidDataException : BussinessException
    {
        public ProductInvalidDataException(string message) : base(message) { }
    }

}
