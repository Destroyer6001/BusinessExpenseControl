using BusinessExpenseControl.Presentation.DTOs;

namespace BusinessExpenseControl.Aplication.Interfaces
{
    public interface ICategoryService
    {
        Task<ApiResponseDTO<List<CategoryDTO>>> GetAllCategories();
    }
}
