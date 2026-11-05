using BusinessExpenseControl.Domain.Entities;

namespace BusinessExpenseControl.Aplication.Interfaces
{
    public interface IJwtService
    {
        public string GenerateToken(UserEntity user);
    }
}
