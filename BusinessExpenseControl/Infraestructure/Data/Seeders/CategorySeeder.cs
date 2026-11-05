using BusinessExpenseControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessExpenseControl.Infraestructure.Data.Seeders
{
    public class CategorySeeder
    {
        private readonly ApplicationDBContext _dbContext;

        public CategorySeeder(ApplicationDBContext context)
        {
            this._dbContext = context;
        }

        public async Task SeedAsync()
        {
            if (!await _dbContext.Category.AnyAsync())
            {
                await _dbContext.AddRangeAsync(

                    new CategoryEntity
                    {
                        Name = "Transporte",
                        Description = "Categoria para transporte"
                    },

                    new CategoryEntity
                    {
                        Name = "Hospedaje",
                        Description = "Categoria para el hospedaje"
                    },

                    new CategoryEntity
                    {
                        Name = "Alimentacion",
                        Description = "Categoria para la alimentacion"
                    },

                    new CategoryEntity
                    {
                        Name = "Imprevistos",
                        Description = "Categoria para imprevistos"
                    }
                );

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
