namespace BusinessExpenseControl.Presentation.DTOs
{
    public class JourneyResponseDTO
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime DepartureDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public double Budget { get; set; }

        public string? State { get; set; }

        public string? Destination { get; set; }

        public int  CreatorId { get; set; }

        public int ResponsibleId { get; set; }

        public int CompanyId { get; set; }
    }
}
