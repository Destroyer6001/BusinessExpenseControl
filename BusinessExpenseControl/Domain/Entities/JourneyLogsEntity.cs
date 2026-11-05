using System.ComponentModel.DataAnnotations;

namespace BusinessExpenseControl.Domain.Entities
{
    public class JourneyLogsEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Title { get; set; }

        [Required]
        [MaxLength(150)]
        public string? Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public CategoryEntity Category { get; set; } = null!;

        [Required]
        public int JourneyId { get; set; }

        public JourneyEntity Journey { get; set; } = null!;
    }
}
