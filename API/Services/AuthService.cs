using API.Helpers;
using API.Models.DTOs;
using API.Models.Entities;
using API.Repositories;
using AutoMapper;
using FluentValidation;
using Google.Apis.Auth;

namespace API.Services
{
    public class AuthService
    {
        public Repository<Usuario> RepoUsuario { get; set; }
        public Repository<Sucursal> RepoSucursal { get; set; }
        public IValidator<LoginDTO> LoginValidator { get; set; }
        public IValidator<RegistroUsuarioDTO> RegistroValidator { get; set; }
        public IMapper Mapper { get; set; }
        public IConfiguration Configuration { get; set; }
        public TokenHelper TokenHelper { get; set; }

        public AuthService(Repository<Usuario> repoUsuario, Repository<Sucursal> repoSucursal, IValidator<LoginDTO> loginValidator, IValidator<RegistroUsuarioDTO> registroValidator, IMapper mapper, IConfiguration configuration, TokenHelper tokenHelper)
        {
            RepoUsuario = repoUsuario;
            RepoSucursal = repoSucursal;
            LoginValidator = loginValidator;
            RegistroValidator = registroValidator;
            Mapper = mapper;
            Configuration = configuration;
            TokenHelper = tokenHelper;
        }

        public LoginResponseDTO? Login(LoginDTO dto)
        {
            LoginValidator.ValidateAndThrow(dto);
            var correo = dto.Email?.Trim().ToLowerInvariant() ?? "";
            var hash = HashHelper.Sha256(dto.Password ?? "");
            var usuario = RepoUsuario.GetAll().FirstOrDefault(x => x.Email.ToLower() == correo && x.PasswordHash == hash && x.Role == dto.Role);

            if (usuario == null && dto.Role == "admin")
            {
                var sucursal = RepoSucursal.GetAll().FirstOrDefault(x => x.Usuario.ToLower() == correo && x.ContrasenaHash == hash);
                if (sucursal != null)
                {
                    return new LoginResponseDTO
                    {
                        Token = TokenHelper.CreateToken(sucursal.Id * -1),
                        User = new UsuarioDTO
                        {
                            Id = sucursal.Id * -1,
                            Name = sucursal.Nombre,
                            Email = sucursal.Usuario,
                            Role = "sucursal",
                            AuthProvider = "local",
                            BranchId = sucursal.Id
                        }
                    };
                }
            }

            if (usuario == null)
                return null;

            return new LoginResponseDTO
            {
                Token = TokenHelper.CreateToken(usuario.Id),
                User = Mapper.Map<UsuarioDTO>(usuario)
            };
        }

        public LoginResponseDTO? Registrar(RegistroUsuarioDTO dto)
        {
            RegistroValidator.ValidateAndThrow(dto);
            var correo = dto.Email?.Trim().ToLowerInvariant() ?? "";
            if (RepoUsuario.GetAll().Any(x => x.Email.ToLower() == correo))
                return null;

            if (dto.Role == "admin" && RepoUsuario.GetAll().Any(x => x.Role == "admin"))
                throw new InvalidOperationException("Ya existe una cuenta de administrador.");

            var usuario = new Usuario
            {
                Nombre = SecurityHelper.Clean(dto.Name),
                Email = correo,
                PasswordHash = HashHelper.Sha256(dto.Password ?? ""),
                Role = dto.Role ?? "cliente",
                AuthProvider = dto.AuthProvider
            };

            RepoUsuario.Insert(usuario);

            return new LoginResponseDTO
            {
                Token = TokenHelper.CreateToken(usuario.Id),
                User = Mapper.Map<UsuarioDTO>(usuario)
            };
        }

        public async Task<LoginResponseDTO> GoogleLogin(GoogleLoginDTO dto)
        {
            var clientId = Configuration["GoogleAuth:ClientId"];
            if (string.IsNullOrWhiteSpace(clientId))
                throw new InvalidOperationException("Falta configurar GoogleAuth:ClientId.");

            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { clientId }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(dto.Credential, settings);
            var correo = payload.Email.Trim().ToLowerInvariant();
            var usuario = RepoUsuario.GetAll().FirstOrDefault(x => x.Email.ToLower() == correo);

            if (usuario == null)
            {
                usuario = new Usuario
                {
                    Nombre = SecurityHelper.Clean(payload.Name ?? correo),
                    Email = correo,
                    PasswordHash = HashHelper.Sha256(Guid.NewGuid().ToString()),
                    Role = dto.Role ?? "cliente",
                    AuthProvider = "google"
                };
                RepoUsuario.Insert(usuario);
            }

            if (usuario.Role != dto.Role)
                throw new UnauthorizedAccessException("Esta cuenta ya existe con otro tipo de usuario.");

            return new LoginResponseDTO
            {
                Token = TokenHelper.CreateToken(usuario.Id),
                User = Mapper.Map<UsuarioDTO>(usuario)
            };
        }
    }
}
