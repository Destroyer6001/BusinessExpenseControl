namespace BusinessExpenseControl.Presentation.DTOs
{
    public class JourneysLogsDTO
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime Date { get; set; }

        public double Price { get; set; }

        public string? Category { get; set; }
    }
}
