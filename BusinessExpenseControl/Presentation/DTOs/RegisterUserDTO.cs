using System.ComponentModel.DataAnnotations;

namespace BusinessExpenseControl.Presentation.DTOs
{
    public class RegisterUserDTO
    {
        [Required(ErrorMessage = "El campo nombre es un campo requerido")]
        [MaxLength(150, ErrorMessage = "El campo nombres no debe superar los 150 caracteres")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "El campo apellidos es un campo requerido")]
        [MaxLength(150, ErrorMessage = "El campo apellidos no debe superar los 150 caracteres")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "El campo email es un campo requerido")]
        [MaxLength(100, ErrorMessage = "El campo email no debe superar los 100 caracteres")]
        [EmailAddress(ErrorMessage = "El campo email debe ser un email valido")]
        public string? Email { get; set; }

        public int role { get; set; }

        [Required(ErrorMessage = "El campo contraseña es un campo requerido")]
        public string? Password { get; set; }
    }
}
