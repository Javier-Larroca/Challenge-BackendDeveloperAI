namespace Products.WebApi.Bussiness.BussinessException
{
    /// <summary>
    /// Excepción base para errores de negocio
    /// Proporciona una jerarquía de excepciones específicas del dominio
    /// </summary>
    public class BussinessException : Exception
    {
        public BussinessException(string message) : base(message) { }
    }
}
