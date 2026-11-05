using BusinessExpenseControl.Presentation.DTOs;

namespace BusinessExpenseControl.Aplication.Interfaces
{
    public interface IJourneyService
    {
        Task<ApiResponseDTO<List<JourneysDTO>>> GetAllJourneys();

        Task<ApiResponseDTO<List<JourneysDTO>>> GetJourneysUser(int userId);

        Task<ApiResponseDTO<JourneyResponseDTO>> GetByIdJourney(int id);

        Task<ApiResponseDTO<JourneyResponseDTO>> CreateJourney(JourneyDTO journeyDTO);

        Task<ApiResponseDTO<JourneyResponseDTO>> EditJourney(int id, JourneyDTO journeyDTO);
    }
}
