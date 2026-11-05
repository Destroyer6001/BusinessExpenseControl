using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BusinessExpenseControl.Domain.Entities
{
    [Index(nameof(CompanyName), IsUnique = true)]
    [Index(nameof(Nit), IsUnique = true)]
    public class CompanyEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? CompanyName { get; set; }

        [Required]
        public int Nit { get; set; }

        [MaxLength(150)]
        public string? TradeName { get; set; }

        [MaxLength(50)]
        public string? Phone { get; set; }

        [MaxLength(100)]
        public string? Address { get; set; }

        [MaxLength(150)]
        [Required]
        public string? LegalRepresentative { get; set; }

        [Required]
        public int DocumentNumberLegalRepresentative { get; set; }

        public ICollection<JourneyEntity>? Journeys { get; set; }

        public ICollection<CompanyUserEntity>? CompanyUsers { get; set; } 
    }
}
