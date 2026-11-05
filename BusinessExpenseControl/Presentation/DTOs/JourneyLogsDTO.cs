using System.ComponentModel.DataAnnotations;

namespace BusinessExpenseControl.Presentation.DTOs
{
    public class JourneyLogsDTO
    {
        [Required(ErrorMessage="El campo titulo es un campo requerido")]
        [MaxLength(100, ErrorMessage="El campo titulo no debe superar los 100 caracteres")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "El campo descripcion es un campo requerido")]
        [MaxLength(150, ErrorMessage = "El campo descripcion no debe superar los 150 caracteres")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El campo fecha es un campo requerido")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "El campo precio es un campo requerido")]
        public double Price { get; set; }

        [Required(ErrorMessage = "El campo categoria es un campo requerido")]
        public int CategoryId { get; set; }

        public int JourneyId { get; set; }
    }
}
