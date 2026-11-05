using BusinessExpenseControl.Aplication.Interfaces;
using BusinessExpenseControl.Domain.Entities;
using BusinessExpenseControl.Domain.Enums;
using BusinessExpenseControl.Infraestructure.Data;
using BusinessExpenseControl.Presentation.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BusinessExpenseControl.Aplication.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IJwtService _jwtService;


        public UserService(ApplicationDBContext context, ICurrentUserService currentUserService, IJwtService jwtService)
        {
            this._currentUserService = currentUserService;
            this._dbContext = context;
            this._jwtService = jwtService;
        }

        public async Task<ApiResponseDTO<UserDTO>> ChangeRole(int id)
        {
            var companyId = _currentUserService.companyId;
            var User = await _dbContext.User.Include(p => p.CompanyUsers).FirstOrDefaultAsync(p => p.Id == id);

            if (User == null)
            {
                return ApiResponseDTO<UserDTO>.ErrorResponse("El usuario no se encuentra registrado en el sistema");
            }

            var CompanyUser = User.CompanyUsers.FirstOrDefault(p => p.CompanyId == companyId);

            if (CompanyUser == null)
            {
                return ApiResponseDTO<UserDTO>.ErrorResponse("El usuario actual no esta asociado a su empresa");
            }

            if (CompanyUser.State == false)
            {
                return ApiResponseDTO<UserDTO>.ErrorResponse("El usuario se encuentra inactivo para esta empresa por favor reactivarlo para poder cambiar el rol");
            }

            CompanyUser.Role = CompanyUser.Role == RoleEnum.Administrator ? RoleEnum.Employee : RoleEnum.Administrator;
            await _dbContext.SaveChangesAsync();
            var UserResponse = this.GenerateDTO(User);

            return ApiResponseDTO<UserDTO>.SuccessResponse("Se ha actualizado el rol del usuario", UserResponse);   
        }

        public async Task<ApiResponseDTO<UserDTO>> ChangeStateUser(int id)
        {
            var User = await _dbContext.User.Include(p => p.CompanyUsers).FirstOrDefaultAsync(p => p.Id == id);
            var companyId = _currentUserService.companyId;

            if (User == null)
            {
                return ApiResponseDTO<UserDTO>.ErrorResponse("El usuario no se encuentra registrado en el sistema");
            }

            if (!User.CompanyUsers.Any(p => p.CompanyId == companyId))
            {
                return ApiResponseDTO<UserDTO>.ErrorResponse("El usuario no se cuenta con una vinculacion registrada a esta empresa");
            }

            if (User.CompanyUsers.Any(p => p.State && p.CompanyId != companyId))
            {
                return ApiResponseDTO<UserDTO>.ErrorResponse("No puedes activar de nuevo este usuario ya que se encuentra vinculado con otra empresa");
            }

            var CompanyUser = User.CompanyUsers.FirstOrDefault(p => p.CompanyId == companyId);

            if (CompanyUser.State)
            {
                var existsJourneys = await _dbContext.Journey.AnyAsync(p => p.ResponsibleId == User.Id && p.CompanyId == companyId && p.State == JourneyState.Active);

                if (existsJourneys)
                {
                    return ApiResponseDTO<UserDTO>.ErrorResponse("No puedes inhabilitar un usuario que tiene viajes pendientes por realizar");
                }
            }

            CompanyUser.State = !CompanyUser.State;
            await _dbContext.SaveChangesAsync();

            var UserResponse = this.GenerateDTO(User);
            return ApiResponseDTO<UserDTO>.SuccessResponse("Se ha cambiado con exito el estado del usuario", UserResponse);
        }

        public async Task<ApiResponseDTO<UserDTO>> EditUser(EditUserDTO editUser, int id)
        {
            var User = await _dbContext.User.FirstOrDefaultAsync(x => x.Id == id);

            if (User == null)
            {
                return ApiResponseDTO<UserDTO>.ErrorResponse("El usuario no se encuentra registrado en el sistema");
            }

            User.FirstName = editUser.FirstName;
            User.LastName = editUser.LastName;
            User.Email = editUser.Email;
            User.Password = editUser.Password != "" ? BCrypt.Net.BCrypt.HashPassword(editUser.Password) : User.Password;

            await _dbContext.SaveChangesAsync();
            var UserResponse = this.GenerateDTO(User);
            return ApiResponseDTO<UserDTO>.SuccessResponse("Se ha actualizado con exito la informacion del cliente", UserResponse);
        }

        public async Task<ApiResponseDTO<List<UserDTO>>> GetAdminUsers()
        {
            var companyId = _currentUserService.companyId;
            var userId = _currentUserService.userId;
            var admins = await this._dbContext.User.Where(p => p.Id != userId && p.CompanyUsers.Any(p => p.CompanyId == companyId && p.Role == RoleEnum.Administrator)).
                           Select(p => new UserDTO
                           {
                               Email = p.Email,
                               Id = p.Id,
                               FirstName = p.FirstName,
                               LastName = p.LastName,
                               State = p.CompanyUsers.Where(p => p.CompanyId == companyId).Select(p => p.State).FirstOrDefault()
                           }).ToListAsync();

            return ApiResponseDTO<List<UserDTO>>.SuccessResponse("Se ha obtenido con exito la lista de los administradores", admins);
        }

        public async Task<ApiResponseDTO<List<UserDTO>>> GetEmployeesActive()
        {
            var companyId = _currentUserService.companyId;
            var employees = await this._dbContext.User.Where(p => p.CompanyUsers.Any(p => p.CompanyId == companyId && p.State == true && p.Role == RoleEnum.Employee))
                                        .Select(p => new UserDTO
                                        {
                                            Email = p.Email,
                                            Id = p.Id,
                                            FirstName = p.FirstName,
                                            LastName = p.LastName,
                                        }).ToListAsync();

            return ApiResponseDTO<List<UserDTO>>.SuccessResponse("Se ha obtenido con exito la lista de los empleados vinculados", employees);
        }

        public async Task<ApiResponseDTO<List<UserDTO>>> GetEmployeeUsers()
        {
            var companyId = _currentUserService.companyId;
            var employees = await this._dbContext.User.Where(p => p.CompanyUsers.Any(p => p.CompanyId == companyId && p.Role == RoleEnum.Employee))
                            .Select(p => new UserDTO
                            {
                                Email = p.Email,
                                Id = p.Id,
                                LastName = p.LastName,
                                FirstName = p.FirstName,
                                State = p.CompanyUsers.Where(p => p.CompanyId == companyId).Select(p => p.State).FirstOrDefault()
                            }).ToListAsync();

            return ApiResponseDTO<List<UserDTO>>.SuccessResponse("Se ha obtenido con exito la lista de los empleados", employees);
        }

        public async Task<ApiResponseDTO<UserDTO>> GetUserById(int id)
        {
            var user = await this._dbContext.User.Where(p => p.Id == id)
                        .Select(p => new UserDTO
                        {
                            Email = p.Email,
                            Id = p.Id,
                            FirstName = p.FirstName,
                            LastName = p.LastName,
                        }).FirstOrDefaultAsync();

            if (user == null)
            {
                return ApiResponseDTO<UserDTO>.ErrorResponse("No hay un usuario registrado en el sistema con ese id");
            }

            return ApiResponseDTO<UserDTO>.SuccessResponse("Se ha obtenido con exito la informacion del usuario", user);
        }

        public async Task<ApiResponseDTO<string>> Login(LoginDTO login)
        {
            var User = await _dbContext.User.Include(p => p.CompanyUsers).FirstOrDefaultAsync(p => p.Email.ToLower() == login.Email.ToLower());

            if (User == null)
            {
                return ApiResponseDTO<string>.ErrorResponse("El usuario no se encuentra registrado en el sistema");
            }

            if (User.CompanyUsers.Any(p => p.State == false))
            {
                return ApiResponseDTO<string>.ErrorResponse("Parece ser que no tienes ninguna vinculacion activa en alguna empresa");
            }

            var validPassword = BCrypt.Net.BCrypt.Verify(User.Password, login.Password);

            if (!validPassword)
            {
                return ApiResponseDTO<string>.ErrorResponse("Contraseña incorrecta");
            }

            string token = _jwtService.GenerateToken(User);

            return ApiResponseDTO<string>.SuccessResponse("Inicio de sesion realizado correctamente", token);
        }

        public async Task<ApiResponseDTO<UserDTO>> Register(RegisterDTO register)
        {
            var existsCompany = await this._dbContext.Company.AnyAsync(p => p.Nit == register.Nit || p.CompanyName.Equals(register.CompanyName, StringComparison.CurrentCultureIgnoreCase));
            var userResponse = new UserDTO();

            if (existsCompany)
            {
                return ApiResponseDTO<UserDTO>.ErrorResponse("Ya hay una empresa registrada con esa razon social o nit");
            }
            
            var user = await _dbContext.User.Include(p => p.CompanyUsers).FirstAsync(p => p.Email.ToLower() == register.Email.ToLower());

            if (user != null && user.CompanyUsers.Any(p => p.State))
            {
                return ApiResponseDTO<UserDTO>.ErrorResponse("Parece que el usuario que intenta registrar se encuentra todavia activo en otra empresa");
            }

            CompanyEntity company = new CompanyEntity();
            company.CompanyName = register.CompanyName;
            company.TradeName = register.TradeName;
            company.Address = register.Address;
            company.Phone = register.Phone;
            company.DocumentNumberLegalRepresentative = register.DocumentNumberLegalRepresentative;
            company.LegalRepresentative = register.LegalRepresentative;
            company.Nit = register.Nit;

            this._dbContext.Add(company);
            await _dbContext.SaveChangesAsync();

            if (user != null)
            {
                CompanyUserEntity companyUser = new CompanyUserEntity();
                companyUser.UserId = user.Id;
                companyUser.Role = RoleEnum.Administrator;
                companyUser.CompanyId = company.Id;
                companyUser.State = true;

                this._dbContext.Add(companyUser);
                await _dbContext.SaveChangesAsync();
                userResponse = this.GenerateDTO(user);
            }
            else
            {
                UserEntity userCreate = new UserEntity();
                userCreate.FirstName = register.FirstName;
                userCreate.LastName = register.LastName;
                userCreate.Email = register.Email;
                userCreate.Password = BCrypt.Net.BCrypt.HashPassword(register.Password);

                this._dbContext.Add(userCreate);
                await _dbContext.SaveChangesAsync();

                CompanyUserEntity companyUser = new CompanyUserEntity();
                companyUser.UserId = userCreate.Id;
                companyUser.CompanyId = company.Id;
                companyUser.Role = RoleEnum.Administrator;
                companyUser.State = true;

                this._dbContext.Add(companyUser);
                await _dbContext.SaveChangesAsync();
                userResponse = this.GenerateDTO(userCreate);
            }

            return ApiResponseDTO<UserDTO>.SuccessResponse("Se ha registrado con el exito el usuario y la empresa en el sistema", userResponse);

        }

        public async Task<ApiResponseDTO<UserDTO>> RegisterUser(RegisterUserDTO registerUser)
        {
            var companyId = _currentUserService.companyId;
            string message = "";
            var userResponse = new UserDTO();

            var user = await _dbContext.User.Include(p => p.CompanyUsers).FirstOrDefaultAsync(p => p.Email.ToLower() == registerUser.Email.ToLower());

            if (user != null && user.CompanyUsers.Any(p => p.State))
            {
                return ApiResponseDTO<UserDTO>.ErrorResponse("Parece que el usuario que intenta registrar se encuentra todavia activo en otra empresa");
            }

            if (user != null)
            {
                if (user.CompanyUsers.Any(p => p.CompanyId == companyId))
                {
                    return ApiResponseDTO<UserDTO>.ErrorResponse("El usuario que intentas registrar ya esta asociado a tu empresa");
                }

                CompanyUserEntity companyUser = new CompanyUserEntity();
                companyUser.UserId = user.Id;
                companyUser.CompanyId = companyId.Value;
                companyUser.State = true;
                companyUser.Role = registerUser.role == 1 ? RoleEnum.Administrator : RoleEnum.Employee;

                _dbContext.Add(companyUser);
                await _dbContext.SaveChangesAsync();

                message = "El usuario ya se encontraba registrado en el sistema pero se ha vinculado con exito a su empresa";
                userResponse = this.GenerateDTO(user);
            }
            else
            {
                UserEntity userEntity = new UserEntity();
                userEntity.FirstName = registerUser.FirstName;
                userEntity.LastName = registerUser.LastName;
                userEntity.Email = registerUser.Email;
                userEntity.Password = BCrypt.Net.BCrypt.HashPassword(registerUser.Password);

                _dbContext.Add(userEntity);
                await _dbContext.SaveChangesAsync();

                CompanyUserEntity companyUser = new CompanyUserEntity();
                companyUser.UserId = userEntity.Id;
                companyUser.CompanyId = companyId.Value;
                companyUser.Role = registerUser.role == 1 ? RoleEnum.Administrator : RoleEnum.Employee;
                companyUser.State = true;

                _dbContext.Add(companyUser);
                await _dbContext.SaveChangesAsync();

                message = "Se ha registrado con exito el usuario";
                userResponse = this.GenerateDTO(userEntity);
            }

            return ApiResponseDTO<UserDTO>.SuccessResponse(message, userResponse);
        }

        private UserDTO GenerateDTO(UserEntity user)
        {
            UserDTO userDTO = new UserDTO();
            userDTO.Id = user.Id;
            userDTO.FirstName = user.FirstName;
            userDTO.LastName = user.LastName;
            userDTO.Email = user.Email; 

            return userDTO;
        }
    }
}
