namespace BusinessExpenseControl.Aplication.Interfaces
{
    public interface ICurrentUserService
    {
        int? userId { get; }

        string? email { get; }

        int? companyId { get; }
    }
}
