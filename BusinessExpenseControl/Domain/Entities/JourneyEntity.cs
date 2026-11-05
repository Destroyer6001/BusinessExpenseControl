using BusinessExpenseControl.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BusinessExpenseControl.Domain.Entities
{
    public class JourneyEntity
    {
        
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Title { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        [Required]
        public DateTime DepartureDate { get; set; }

        [Required]
        public DateTime ReturnDate { get; set; }

        [Required]
        public double Budget { get; set; }

        public JourneyState State { get; set; }

        [Required]
        [MaxLength(150)]
        public string? Destination { get; set; }

        [Required]
        public int ResponsibleId { get; set; }

        public UserEntity Responsible { get; set; } = null!;

        [Required]
        public int CreatorId { get; set; }

        public UserEntity Creator { get; set; } = null!;

        [Required]
        public int CompanyId { get; set; }

        public CompanyEntity Company { get; set; } = null!;

        public ICollection<JourneyLogsEntity>? JourneyLogs { get; set; }

    }
}
