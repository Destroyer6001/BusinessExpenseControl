using System.ComponentModel.DataAnnotations;

namespace BusinessExpenseControl.Presentation.DTOs
{
    public class JourneyDTO
    {
        [Required(ErrorMessage = "El campo titulo de viaje es un campo requerido")]
        [MaxLength(100, ErrorMessage = "El campo titulo de viaje no debe superar los 100 caracteres")]
        public string? Title { get; set; }

        [MaxLength(200, ErrorMessage = "El campo descripcion no debe superar los 200 caracteres")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "El campo fecha de inicio es un campo requerido")]
        public DateTime DepertureDate { get; set; }

        [Required(ErrorMessage = "El campo fecha fin es un campo requerido")]
        public DateTime ReturnDate { get; set; }

        [Required(ErrorMessage = "El campo presupuesto es un campo requerido")]
        public double Budget { get; set; }

        [Required(ErrorMessage = "El campo responsable es un campo requerido")]
        public int ResponsibleId { get; set; }

        [Required(ErrorMessage = "El campo destino es un campo requerido")]
        [MaxLength(150, ErrorMessage ="El campo destino no debe superar los 150 caracteres")]
        public string? Destination { get; set; }
    }
}
