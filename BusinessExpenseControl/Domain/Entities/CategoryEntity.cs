using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BusinessExpenseControl.Domain.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class CategoryEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        [MaxLength(150)]
        public string? Description { get; set; }

        public ICollection<JourneyLogsEntity>? JourneyLogs { get; set; }
    }
}
