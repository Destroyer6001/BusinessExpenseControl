namespace BusinessExpenseControl.Presentation.DTOs
{
    public class JourneysDTO
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public DateTime DepartureDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public string? State { get; set; }

        public double Budget { get; set; }

        public string? Destination { get; set; }

        public string? Responsible { get; set; }

        public string? Creator { get; set; }

        public double TotalExpenses { get; set; }

        public double Spare { get; set; }
    }
}
