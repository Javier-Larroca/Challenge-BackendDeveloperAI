using System.ComponentModel.DataAnnotations;

namespace Products.WebApi.Models
{
    /// <summary>
    /// Modelo de dominio para un producto
    /// Representa la entidad principal del sistema con todas sus propiedades
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Identificador único del producto (clave primaria)
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nombre del producto (requerido, máximo 50 caracteres)
        /// </summary>
        [Required]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder 50 caracteres")]
        public string Name { get; set; }

        /// <summary>
        /// Descripción detallada del producto (requerido, máximo 250 caracteres)
        /// </summary>
        [Required]
        [StringLength(250, ErrorMessage = "La descripción no puede exceder 250 caracteres")]
        public string Description { get; set; }

        /// <summary>
        /// URL de la imagen del producto (requerido)
        /// </summary>
        [Required]
        [Url(ErrorMessage = "La URL de la imagen debe ser válida")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Precio del producto en formato decimal (requerido, debe ser positivo)
        /// </summary>
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Price { get; set; }

        /// <summary>
        /// Calificación del producto de 0 a 5 estrellas (requerido)
        /// </summary>
        [Required]
        [Range(0, 5, ErrorMessage = "La calificación debe estar entre 0 y 5")]
        public double Rating { get; set; }

        /// <summary>
        /// Especificaciones técnicas del producto (requerido, máximo 250 caracteres)
        /// </summary>
        [Required]
        [StringLength(250, ErrorMessage = "Las especificaciones no pueden exceder 250 caracteres")]
        public string Specifications { get; set; }
    }
}
