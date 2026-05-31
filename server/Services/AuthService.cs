using AutoMapper;
using FluentValidation;
using Google.Apis.Auth;
using PedidoApi.Helpers;
using PedidoApi.Models.Dtos;
using PedidoApi.Models.Entities;
using PedidoApi.Repositories;

namespace PedidoApi.Services;

public class AuthService
{
    private readonly GenericRepository<UserEntity> _users;
    private readonly GenericRepository<BranchEntity> _branches;
    private readonly IValidator<LoginDto> _loginValidator;
    private readonly IValidator<RegisterDto> _registerValidator;
    private readonly TokenHelper _tokenHelper;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public AuthService(GenericRepository<UserEntity> users, GenericRepository<BranchEntity> branches, IValidator<LoginDto> loginValidator, IValidator<RegisterDto> registerValidator, TokenHelper tokenHelper, IMapper mapper, IConfiguration configuration)
    {
        _users = users;
        _branches = branches;
        _loginValidator = loginValidator;
        _registerValidator = registerValidator;
        _tokenHelper = tokenHelper;
        _mapper = mapper;
        _configuration = configuration;
    }

    public AuthResponseDto Login(LoginDto dto)
    {
        _loginValidator.ValidateAndThrow(dto);
        var hash = HashHelper.Sha256(dto.Password);
        var user = _users.GetAll().FirstOrDefault(x => x.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase) && x.PasswordHash == hash && x.Role == dto.Role);

        if (user is null && dto.Role == "admin")
        {
            var branch = _branches.GetAll().FirstOrDefault(x => x.Username.Equals(dto.Email, StringComparison.OrdinalIgnoreCase) && x.PasswordHash == hash);
            if (branch is not null)
            {
                user = new UserEntity
                {
                    Id = branch.Id * -1,
                    Name = branch.Name,
                    Email = branch.Username,
                    Role = "sucursal",
                    AuthProvider = "local"
                };

                return new AuthResponseDto
                {
                    Token = _tokenHelper.CreateToken(user.Id),
                    User = new UserDto
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        Role = user.Role,
                        AuthProvider = user.AuthProvider,
                        BranchId = branch.Id
                    }
                };
            }
        }

        if (user is null)
        {
            throw new UnauthorizedAccessException("Credenciales incorrectas.");
        }

        return new AuthResponseDto
        {
            Token = _tokenHelper.CreateToken(user.Id),
            User = _mapper.Map<UserDto>(user)
        };
    }

    public AuthResponseDto Register(RegisterDto dto)
    {
        _registerValidator.ValidateAndThrow(dto);

        if (_users.GetAll().Any(x => x.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException("El correo ya esta registrado.");
        }

        if (dto.Role == "admin" && _users.GetAll().Any(x => x.Role == "admin"))
        {
            throw new InvalidOperationException("Ya existe una cuenta de negocio registrada.");
        }

        var user = new UserEntity
        {
            Name = SecurityHelper.Clean(dto.Name),
            Email = dto.Email.Trim().ToLowerInvariant(),
            PasswordHash = HashHelper.Sha256(dto.Password),
            Role = dto.Role,
            AuthProvider = dto.AuthProvider
        };

        _users.Add(user);
        return new AuthResponseDto
        {
            Token = _tokenHelper.CreateToken(user.Id),
            User = _mapper.Map<UserDto>(user)
        };
    }

    public async Task<AuthResponseDto> GoogleLogin(GoogleLoginDto dto)
    {
        if (dto.Role is not ("cliente" or "admin"))
        {
            throw new InvalidOperationException("Tipo de cuenta no valido.");
        }

        var clientId = _configuration["GoogleAuth:ClientId"];
        if (string.IsNullOrWhiteSpace(clientId) || clientId.Contains("TU_CLIENT_ID"))
        {
            throw new InvalidOperationException("Falta configurar GoogleAuth:ClientId en appsettings.json.");
        }

        var settings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = new[] { clientId }
        };

        var payload = await GoogleJsonWebSignature.ValidateAsync(dto.Credential, settings);
        var email = payload.Email.Trim().ToLowerInvariant();
        var user = _users.GetAll().FirstOrDefault(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        if (user is null)
        {
            user = new UserEntity
            {
                Name = dto.Role == "admin" ? "Admin" : SecurityHelper.Clean(payload.Name ?? email),
                Email = email,
                PasswordHash = HashHelper.Sha256(Guid.NewGuid().ToString()),
                Role = dto.Role,
                AuthProvider = "google"
            };

            _users.Add(user);
        }

        if (user.Role != dto.Role)
        {
            throw new UnauthorizedAccessException("Esta cuenta de Google ya existe con otro tipo de usuario.");
        }

        return new AuthResponseDto
        {
            Token = _tokenHelper.CreateToken(user.Id),
            User = _mapper.Map<UserDto>(user)
        };
    }
}
