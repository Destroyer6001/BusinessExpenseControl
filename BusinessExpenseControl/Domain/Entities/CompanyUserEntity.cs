using BusinessExpenseControl.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BusinessExpenseControl.Domain.Entities
{
    public class CompanyUserEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int CompanyId { get; set; }

        public bool State { get; set; }

        [Required]
        public RoleEnum Role { get; set; }

        public CompanyEntity Company { get; set; } = null!;

        public UserEntity User { get; set; } = null!;
    }
}
