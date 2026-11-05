namespace BusinessExpenseControl.Presentation.DTOs
{
    public class ApiResponseDTO<T>
    {
        public bool Success { get; set; }

        public string? Message { get; set; }

        public T? Data { get; set; }

        public DateTime Timestamp { get; set; }

        public ApiResponseDTO(bool success, string message, T? data)
        {
            this.Success = success;
            this.Message = message;
            this.Data = data;
            this.Timestamp = DateTime.UtcNow;
        }

        public static ApiResponseDTO<T> SuccessResponse(string message, T data)
        {
            return new ApiResponseDTO<T>(true, message, data);
        }

        public static ApiResponseDTO<T> ErrorResponse(string message)
        {
            return new ApiResponseDTO<T>(false, message, default);
        }
    }
}
