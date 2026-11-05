using BusinessExpenseControl.Aplication.Interfaces;
using BusinessExpenseControl.Domain.Entities;
using BusinessExpenseControl.Domain.Enums;
using BusinessExpenseControl.Infraestructure.Data;
using BusinessExpenseControl.Presentation.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BusinessExpenseControl.Aplication.Services
{
    public class JourneyLogsService : IJourneyLogsService
    {
        private readonly ApplicationDBContext _dbContext;

        public JourneyLogsService(ApplicationDBContext context)
        {
            this._dbContext = context;
        }

        public async Task<ApiResponseDTO<JourneyLogsResponseDTO>> CreateJourneyLog(JourneyLogsDTO journeyLogsDTO)
        {
            var categoryExist = await _dbContext.Category.AnyAsync(p => p.Id == journeyLogsDTO.CategoryId);

            if (!categoryExist)
            {
                return ApiResponseDTO<JourneyLogsResponseDTO>.ErrorResponse("La categoria seleccionada no ha sido registrada en el sistema");
            }

            var journey = await _dbContext.Journey.FirstOrDefaultAsync(p => p.Id == journeyLogsDTO.JourneyId);

            if (journey == null)
            {
                return ApiResponseDTO<JourneyLogsResponseDTO>.ErrorResponse("El viaje seleccionado no ha sido registrado en el sistema");
            }

            if (journeyLogsDTO.Date < journey.DepartureDate || journeyLogsDTO.Date > journey.ReturnDate)
            {
                return ApiResponseDTO<JourneyLogsResponseDTO>.ErrorResponse("La fecha del gasto no se encuentra en el rango de fechas del viaje");
            }

            if (journey.State == JourneyState.Cancel || journey.State == JourneyState.Completed)
            {
                return ApiResponseDTO<JourneyLogsResponseDTO>.ErrorResponse("No puedes registar un gasto a un viaje que fue cancelado o completado");
            }

            var journeyLog = new JourneyLogsEntity();
            journeyLog.Title = journeyLogsDTO.Title;
            journeyLog.Description = journeyLogsDTO.Descripcion;
            journeyLog.Price = journeyLogsDTO.Price;
            journeyLog.Date = journeyLogsDTO.Date;
            journeyLog.CategoryId = journeyLogsDTO.CategoryId;
            journeyLog.JourneyId = journeyLogsDTO.JourneyId;

            _dbContext.Add(journeyLog);
            await _dbContext.SaveChangesAsync();
            var logResponse = this.GetJourneyLogDTO(journeyLog);
            return ApiResponseDTO<JourneyLogsResponseDTO>.SuccessResponse("Se ha creado con exito el gasto del viaje", logResponse);
        }

        public async Task<ApiResponseDTO<List<JourneysLogsDTO>>> GetAllJourneysLogs(int journeyId)
        {
            var journeyLogs = await _dbContext.JourneyLog.Where(p => p.JourneyId ==  journeyId)
                                .Select(p => new JourneysLogsDTO
                                {
                                    Id = p.Id,
                                    Price = p.Price,
                                    Date = p.Date,
                                    Title = p.Title,
                                    Description = p.Description,
                                    Category = p.Category.Name
                                }).ToListAsync();

            return ApiResponseDTO<List<JourneysLogsDTO>>.SuccessResponse("Se ha obtenido con exito la lista de los gastos del viaje", journeyLogs);
        }

        public async Task<ApiResponseDTO<JourneyLogsResponseDTO>> GetById(int id)
        {
            var journeyLog = await _dbContext.JourneyLog.Where(p => p.Id == id)
                            .Select(p => new JourneyLogsResponseDTO
                            {
                                Id = p.Id,
                                Title = p.Title,
                                Description = p.Description,
                                CategoryId = p.CategoryId,
                                Date = p.Date,
                                Price = p.Price,
                                JourneyId = p.JourneyId
                            }).FirstOrDefaultAsync();

            if (journeyLog == null)
            {
                return ApiResponseDTO<JourneyLogsResponseDTO>.ErrorResponse("No hay un gasto registrado con ese Id");
            }

            return ApiResponseDTO<JourneyLogsResponseDTO>.SuccessResponse("Gasto encontrado con exito", journeyLog);
        }

        public async Task<ApiResponseDTO<JourneyLogsResponseDTO>> UpdateJourneyLog(JourneyLogsDTO journeyLogsDTO, int id)
        {
            var journeyLog = await _dbContext.JourneyLog.FirstOrDefaultAsync(p => p.Id == id);

            if (journeyLog == null)
            {
                return ApiResponseDTO<JourneyLogsResponseDTO>.ErrorResponse("El gasto seleccionado no se encuentra registrado en el sistema");
            }

            var journey = await _dbContext.Journey.FirstOrDefaultAsync(p => p.Id == journeyLogsDTO.JourneyId);

            if (journey == null)
            {
                return ApiResponseDTO<JourneyLogsResponseDTO>.ErrorResponse("El viaje registrado en este gasto no se encuentra registrado en el sistema");
            }

            if (journeyLogsDTO.Date < journey.DepartureDate || journeyLogsDTO.Date > journey.ReturnDate)
            {
                return ApiResponseDTO<JourneyLogsResponseDTO>.ErrorResponse("La fecha del gasto no se encuentra en el rango de fechas del viaje");
            }

            if (journey.State == JourneyState.Completed || journey.State == JourneyState.Cancel)
            {
                return ApiResponseDTO<JourneyLogsResponseDTO>.ErrorResponse("No puedes registar un gasto a un viaje que fue cancelado o completado");
            }

            journeyLog.Title = journeyLogsDTO.Title;
            journeyLog.Description = journeyLogsDTO.Descripcion;
            journeyLog.Price = journeyLogsDTO.Price;
            journeyLog.Date = journeyLogsDTO.Date;
            journeyLog.CategoryId = journeyLog.CategoryId;

            await _dbContext.SaveChangesAsync();

            var journeyLogResponse = this.GetJourneyLogDTO(journeyLog);

            return ApiResponseDTO<JourneyLogsResponseDTO>.SuccessResponse("Se ha actualizado la informacion del gasto del viaje", journeyLogResponse);
        }

        private JourneyLogsResponseDTO GetJourneyLogDTO(JourneyLogsEntity log)
        {
            var journeyLog = new JourneyLogsResponseDTO
            {
                Id = log.Id,
                Title = log.Title,
                Description = log.Description,
                Date = log.Date,
                CategoryId = log.CategoryId,
                Price = log.Price,
                JourneyId = log.JourneyId
            };

            return journeyLog;
        }
    }
}
