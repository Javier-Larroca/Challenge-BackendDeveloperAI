namespace Products.WebApi.Bussiness.BussinessException
{
    /// <summary>
    /// Excepción lanzada cuando los datos del producto son inválidos
    /// </summary>
    public class ProductInvalidDataException : BussinessException
    {
        public ProductInvalidDataException(string message) : base(message) { }
    }

}
