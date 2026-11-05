using System.ComponentModel.DataAnnotations;

namespace BusinessExpenseControl.Presentation.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "El campo email es un campo requerido")]
        [EmailAddress(ErrorMessage = "El campo email debe ser un email valido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "El campo contraseña es un campo requerido")]
        public string? Password { get; set; }

    }
}
