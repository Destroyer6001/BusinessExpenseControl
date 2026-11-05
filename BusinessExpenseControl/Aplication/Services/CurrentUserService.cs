using BusinessExpenseControl.Aplication.Interfaces;

namespace BusinessExpenseControl.Aplication.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly HttpContextAccessor _contextAccessor;

        public CurrentUserService(HttpContextAccessor contextAccessor)
        {
            this._contextAccessor = contextAccessor;
        }

        public int? userId { 
            get
            {
                var value = _contextAccessor.HttpContext?.User?.FindFirst("userId").Value;
                return int.TryParse(value, out var id) ? id : null;
            } 
        }

        public string? email
        {
            get
            {
                var value = _contextAccessor.HttpContext?.User?.FindFirst("email").Value;
                return string.IsNullOrEmpty(value) ? null : value;
            }
        }

        public int? companyId
        {
            get
            {
                var value = _contextAccessor.HttpContext?.User?.FindFirst("companyId").Value;
                return int.TryParse(value, out var companyId) ? companyId : null;
            }
        }
    }
}
