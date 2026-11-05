using BusinessExpenseControl.Presentation.DTOs;

namespace BusinessExpenseControl.Aplication.Interfaces
{
    public interface IJourneyLogsService
    {
        Task<ApiResponseDTO<List<JourneysLogsDTO>>> GetAllJourneysLogs(int journeyId);

        Task<ApiResponseDTO<JourneyLogsResponseDTO>> GetById(int id);

        Task<ApiResponseDTO<JourneyLogsResponseDTO>> CreateJourneyLog(JourneyLogsDTO journeyLogsDTO);

        Task<ApiResponseDTO<JourneyLogsResponseDTO>> UpdateJourneyLog(JourneyLogsDTO journeyLogsDTO, int id);
    }
}
