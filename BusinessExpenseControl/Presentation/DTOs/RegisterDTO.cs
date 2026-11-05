using System.ComponentModel.DataAnnotations;

namespace BusinessExpenseControl.Presentation.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "El campo razon social es un campo requirido")]
        [MaxLength(100, ErrorMessage = "El campo razon social no debe ser mayor a 100 caracteres")]
        public string? CompanyName { get; set; }

        [Required(ErrorMessage = "El campo nit es un campo requerido")]
        public int Nit { get; set; }

        [MaxLength(150, ErrorMessage ="El campo nombre comercial no debe superar los 150 caracteres")]
        public string? TradeName { get; set; }

        [MaxLength(50, ErrorMessage = "El campo telefono no debe superar los 50 caractares")]
        public string? Phone { get; set; }

        [MaxLength(100, ErrorMessage = "El campo direccion no debe superar los 100 caracteres")]
        public string? Address { get; set; }

        [MaxLength(150, ErrorMessage = "El campo representate legal no debe superar los 150 caracteres")]
        [Required(ErrorMessage = "El campo representante legal es un campo requerido")]
        public string? LegalRepresentative { get; set; }

        [Required(ErrorMessage = "El campo documento de identidad es un campo requerido")]
        public int DocumentNumberLegalRepresentative { get; set; }

        [Required(ErrorMessage = "El campo nombres del administrador es un campo requerido")]
        [MaxLength(150, ErrorMessage = "El campo nombres del administrador no debe superar los 150 caracteres")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "El campo apellidos del administrador es un campo requerido")]
        [MaxLength(150, ErrorMessage = "El campo apellidos del administrador no debe superar los 150 caracteres")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "El campo email del administrador es un campo requerido")]
        [MaxLength(100, ErrorMessage = "El campo email del administrador no debe superar los 100 caracteres")]
        [EmailAddress(ErrorMessage = "El campo email del administrador debe ser un email valido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "El campo contraseña del administrador es un campo requerido")]
        public string? Password { get; set; }
    }
}
