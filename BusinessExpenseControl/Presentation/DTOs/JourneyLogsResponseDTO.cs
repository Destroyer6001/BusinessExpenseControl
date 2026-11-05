namespace BusinessExpenseControl.Presentation.DTOs
{
    public class JourneyLogsResponseDTO
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime Date { get; set; }

        public double Price { get; set; }

        public int CategoryId { get; set; }

        public int JourneyId { get; set; }
    }
}
