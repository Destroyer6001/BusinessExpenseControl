using BusinessExpenseControl.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BusinessExpenseControl.Domain.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    public class UserEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string? FirstName { get; set; }

        [Required]
        [MaxLength(150)]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

        public ICollection<JourneyEntity>? JourneysCreate { get; set; }

        public ICollection<JourneyEntity>? JourneysAsigned { get; set; }

        public ICollection<CompanyUserEntity>? CompanyUsers {  get; set; }
    }
}
