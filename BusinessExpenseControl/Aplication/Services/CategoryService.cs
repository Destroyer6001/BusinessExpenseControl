using BusinessExpenseControl.Aplication.Interfaces;
using BusinessExpenseControl.Infraestructure.Data;
using BusinessExpenseControl.Presentation.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BusinessExpenseControl.Aplication.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDBContext _dbContext;

        public CategoryService(ApplicationDBContext context)
        {
            this._dbContext = context;
        }

        public async Task<ApiResponseDTO<List<CategoryDTO>>> GetAllCategories()
        {
            var categories = await _dbContext.Category
                                .Select(p => new CategoryDTO()
                                {
                                    Id = p.Id,
                                    Name = p.Name,
                                }).ToListAsync();

            return ApiResponseDTO<List<CategoryDTO>>.SuccessResponse("Se ha obtenido con exito la lista de categorias", categories);
        }
    }
}
