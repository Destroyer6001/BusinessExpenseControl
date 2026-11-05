using BusinessExpenseControl.Presentation.DTOs;

namespace BusinessExpenseControl.Aplication.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponseDTO<List<UserDTO>>> GetAdminUsers();

        Task<ApiResponseDTO<List<UserDTO>>> GetEmployeeUsers();

        Task<ApiResponseDTO<UserDTO>> Register(RegisterDTO register);

        Task<ApiResponseDTO<string>> Login(LoginDTO login);

        Task<ApiResponseDTO<UserDTO>> GetUserById(int id);

        Task<ApiResponseDTO<UserDTO>> RegisterUser(RegisterUserDTO registerUser);

        Task<ApiResponseDTO<UserDTO>> EditUser (EditUserDTO editUser, int id);

        Task<ApiResponseDTO<UserDTO>> ChangeRole(int id);

        Task<ApiResponseDTO<UserDTO>> ChangeStateUser(int id);

        Task<ApiResponseDTO<List<UserDTO>>> GetEmployeesActive();
    }
}
