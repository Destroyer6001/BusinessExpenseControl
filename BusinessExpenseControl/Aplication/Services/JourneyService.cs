using BusinessExpenseControl.Aplication.Interfaces;
using BusinessExpenseControl.Domain.Entities;
using BusinessExpenseControl.Domain.Enums;
using BusinessExpenseControl.Infraestructure.Data;
using BusinessExpenseControl.Presentation.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BusinessExpenseControl.Aplication.Services
{
    public class JourneyService : IJourneyService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public JourneyService(ApplicationDBContext context, ICurrentUserService currentUserService)
        {
            this._dbContext = context;
            this._currentUserService = currentUserService;
        }

        public async Task<ApiResponseDTO<JourneyResponseDTO>> CreateJourney(JourneyDTO journeyDTO)
        {
            var responsibleUser = await _dbContext.User.Include(p => p.CompanyUsers).Where(p => p.Id == journeyDTO.ResponsibleId).FirstOrDefaultAsync();
            var companyId = _currentUserService.companyId;
            var creatorId = _currentUserService.userId;

            if (responsibleUser == null)
            {
                return ApiResponseDTO<JourneyResponseDTO>.ErrorResponse("El usuario seleccionado no se encuentra registrado en el sistema");
            }

            if (!responsibleUser.CompanyUsers.Any(p => p.CompanyId == companyId && p.State == true))
            {
                return ApiResponseDTO<JourneyResponseDTO>.ErrorResponse("El usuario seleccionado no se encuentra actualmente vinculado a la empresa");
            }

            var existJourneyDate = await _dbContext.Journey.AnyAsync(p =>
                                        p.CompanyId == companyId && p.ResponsibleId == journeyDTO.ResponsibleId
                                        && journeyDTO.DepertureDate < p.ReturnDate && journeyDTO.ReturnDate > p.DepartureDate);

            if (existJourneyDate)
            {
                return ApiResponseDTO<JourneyResponseDTO>.ErrorResponse("Parece ser ya hay otro viaje registrado para este empleado en este rango de fechas");
            }

            var Journey = new JourneyEntity();

            Journey.Title = journeyDTO.Title;
            Journey.Description = journeyDTO.Description;
            Journey.Destination = journeyDTO.Destination;
            Journey.Budget = journeyDTO.Budget;
            Journey.DepartureDate = journeyDTO.ReturnDate;
            Journey.ReturnDate = journeyDTO.ReturnDate;
            Journey.State = JourneyState.Active;
            Journey.ResponsibleId = journeyDTO.ResponsibleId;
            Journey.CompanyId = companyId.Value;
            Journey.CreatorId = creatorId.Value;

            _dbContext.Add(Journey);
            await _dbContext.SaveChangesAsync();
            var responseJourney = this.GetJourneyResponseDTO(Journey);

            return ApiResponseDTO<JourneyResponseDTO>.SuccessResponse("Se ha creado con exito el viaje y se ha asignado correctamente al empleado", responseJourney);
        }

        public async Task<ApiResponseDTO<JourneyResponseDTO>> EditJourney(int id, JourneyDTO journeyDTO)
        {
            var companyId = _currentUserService.companyId;
            var journey = await _dbContext.Journey.FirstOrDefaultAsync(p => p.Id == id);

            if (journey == null)
            {
                return ApiResponseDTO<JourneyResponseDTO>.ErrorResponse("El viaje seleccionado no ha sido registrado en el sistema");
            }

            if (journey.State == JourneyState.Cancel || journey.State == JourneyState.Completed)
            {
                return ApiResponseDTO<JourneyResponseDTO>.ErrorResponse("No se puede modificar un viaje que ya ha sido compleado o cancelado");
            }

            var existJourneyDate = await _dbContext.Journey.AnyAsync(p =>
                                    p.Id != id && p.ResponsibleId == journeyDTO.ResponsibleId && p.CompanyId == companyId
                                    && journeyDTO.DepertureDate < p.ReturnDate && journeyDTO.ReturnDate > p.DepartureDate);

            if (existJourneyDate)
            {
                return ApiResponseDTO<JourneyResponseDTO>.ErrorResponse("Parece ser ya hay otro viaje registrado para este empleado en este rango de fechas");
            }

            var journeyLogs = await _dbContext.JourneyLog.Where(p => p.JourneyId == id).ToListAsync();

            if (journeyLogs.Count > 0)
            {
                var maxDate = journeyLogs.Max(p => p.Date);
                var minDate = journeyLogs.Min(p => p.Date);

                if (journeyDTO.DepertureDate > minDate || journeyDTO.ReturnDate < maxDate)
                {
                    return ApiResponseDTO<JourneyResponseDTO>.ErrorResponse("No puedes modificar la fecha del viaje si ya hay gastos registrados para este y se supera las fechas minimas y maximas");
                }
            }

            if (journey.ResponsibleId != journeyDTO.ResponsibleId)
            {
                var logsmovements = await _dbContext.JourneyLog.Where(p => p.JourneyId == id).ToListAsync();

                if (logsmovements.Count > 0)
                {
                    return ApiResponseDTO<JourneyResponseDTO>.ErrorResponse("No puedes cambiar el responsable de este viaje ya que se han registados gastos");
                }

                var User = await _dbContext.User.Include(p => p.CompanyUsers).Where(p => p.Id == journeyDTO.ResponsibleId).FirstOrDefaultAsync();

                if (User == null)
                {
                    return ApiResponseDTO<JourneyResponseDTO>.ErrorResponse("El usuario seleccionado no se encuentra registrado en el sistema");
                }

                if (!User.CompanyUsers.Any(p => p.CompanyId == companyId && p.State == true))
                {
                    return ApiResponseDTO<JourneyResponseDTO>.ErrorResponse("El usuario seleccionado no se encuentra actualmente vinculado a la empresa");
                }

                journey.ResponsibleId = journeyDTO.ResponsibleId;
            }

            journey.Title = journeyDTO.Title;
            journey.Description = journeyDTO.Description;
            journey.Destination = journeyDTO.Destination;
            journey.DepartureDate = journeyDTO.DepertureDate;
            journey.ReturnDate = journey.ReturnDate;
            journey.Budget = journey.Budget;

            await _dbContext.SaveChangesAsync();
            var responseDTO = this.GetJourneyResponseDTO(journey);
            return ApiResponseDTO<JourneyResponseDTO>.SuccessResponse("Se ha actualizado con exito la informacion del viaje", responseDTO);
        }

        public async Task<ApiResponseDTO<List<JourneysDTO>>> GetAllJourneys()
        {
            var companyId = _currentUserService.companyId;

            var Journeys = await _dbContext.Journey.Where(p => p.CompanyId == companyId)
                                    .Select(p => new JourneysDTO
                                    {
                                        Id = p.Id,
                                        Title = p.Title,
                                        Destination = p.Destination,
                                        DepartureDate = p.DepartureDate,
                                        ReturnDate = p.ReturnDate,
                                        Budget = p.Budget,
                                        Responsible = p.Responsible.FirstName + ' ' + p.Responsible.LastName,
                                        Creator = p.Creator.FirstName + ' ' + p.Creator.LastName,
                                        TotalExpenses = p.JourneyLogs.Sum(p => p.Price),
                                        Spare = p.Budget - p.JourneyLogs.Sum(p => p.Price),
                                        State = p.State.ToString()
                                    }).ToListAsync();

            return ApiResponseDTO<List<JourneysDTO>>.SuccessResponse("Se ha obtenido con exito la lista de todos los viajes", Journeys);
        }

        public async Task<ApiResponseDTO<JourneyResponseDTO>> GetByIdJourney(int id)
        {
            var Journey = await _dbContext.Journey.Where(p => p.Id == id)
                                .Select(p => new JourneyResponseDTO
                                {
                                    Id = p.Id,
                                    CompanyId = p.CompanyId,
                                    Title = p.Title,
                                    Budget = p.Budget,
                                    State = p.State.ToString(),
                                    Description = p.Description,
                                    Destination = p.Destination,
                                    ResponsibleId = p.ResponsibleId,
                                    CreatorId = p.CreatorId,
                                    DepartureDate = p.DepartureDate,
                                    ReturnDate = p.ReturnDate
                                }).FirstOrDefaultAsync();

            if (Journey == null)
            {
                return ApiResponseDTO<JourneyResponseDTO>.ErrorResponse("No hay un viaje registrado con ese id");
            }

            return ApiResponseDTO<JourneyResponseDTO>.SuccessResponse("Se ha obtenido con exito la informacion del viaje", Journey);
        }

        public async Task<ApiResponseDTO<List<JourneysDTO>>> GetJourneysUser(int userId)
        {
            var companyId = _currentUserService.companyId;

            var Journeys = await _dbContext.Journey.Where(p => p.CompanyId == companyId && p.ResponsibleId == userId)
                                            .Select(p => new JourneysDTO
                                            {
                                                Id = p.Id,
                                                Title = p.Title,
                                                DepartureDate = p.DepartureDate,
                                                ReturnDate = p.ReturnDate,
                                                Budget = p.Budget,
                                                Destination = p.Destination,
                                                Creator = p.Creator.FirstName + ' ' + p.Creator.LastName,
                                                Responsible = p.Responsible.FirstName + ' ' + p.Responsible.LastName,
                                                TotalExpenses = p.JourneyLogs.Sum(p => p.Price),
                                                Spare = p.Budget - p.JourneyLogs.Sum(p => p.Price),
                                                State = p.State.ToString()
                                            }).ToListAsync();

            return ApiResponseDTO<List<JourneysDTO>>.SuccessResponse("Se ha obtenido con exito la lista de viajes por empleados", Journeys);
        }

        private JourneyResponseDTO GetJourneyResponseDTO(JourneyEntity journey)
        {
            var journeyDTO = new JourneyResponseDTO();

            journeyDTO.Id = journey.Id;
            journeyDTO.Title = journey.Title;
            journeyDTO.Destination = journey.Destination;
            journeyDTO.Description = journey.Description;
            journeyDTO.State = journey.State.ToString();
            journeyDTO.DepartureDate = journey.DepartureDate;
            journeyDTO.ReturnDate = journey.ReturnDate;
            journeyDTO.ResponsibleId = journey.ResponsibleId;
            journeyDTO.CreatorId = journey.CreatorId;
            journeyDTO.CompanyId = journey.CompanyId;
            journeyDTO.Budget = journey.Budget;

            return journeyDTO;

        }
    }
}
