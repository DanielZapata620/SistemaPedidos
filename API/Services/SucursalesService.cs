using API.Helpers;
using API.Models.DTOs;
using API.Models.Entities;
using API.Repositories;
using AutoMapper;
using FluentValidation;

namespace API.Services
{
    public class SucursalesService
    {
        public Repository<Sucursal> RepoSucursal { get; set; }
        public IValidator<CrearSucursalDTO> CrearValidator { get; set; }
        public IValidator<EditarSucursalDTO> EditarValidator { get; set; }
        public IMapper Mapper { get; set; }

        public SucursalesService(Repository<Sucursal> repoSucursal, IValidator<CrearSucursalDTO> crearValidator, IValidator<EditarSucursalDTO> editarValidator, IMapper mapper)
        {
            RepoSucursal = repoSucursal;
            CrearValidator = crearValidator;
            EditarValidator = editarValidator;
            Mapper = mapper;
        }

        public List<SucursalDTO> Obtener()
        {
            return RepoSucursal.GetAll().Select(x => Mapper.Map<SucursalDTO>(x)).ToList();
        }

        public SucursalDTO Crear(CrearSucursalDTO dto)
        {
            CrearValidator.ValidateAndThrow(dto);
            var sucursal = new Sucursal
            {
                Nombre = SecurityHelper.Clean(dto.Name),
                Direccion = SecurityHelper.Clean(dto.Address),
                Usuario = SecurityHelper.Clean(dto.Username).ToLowerInvariant(),
                ContrasenaHash = HashHelper.Sha256(dto.Password ?? ""),
                Latitud = dto.Latitude,
                Longitud = dto.Longitude
            };
            RepoSucursal.Insert(sucursal);
            return Mapper.Map<SucursalDTO>(sucursal);
        }

        public SucursalDTO? Editar(int id, EditarSucursalDTO dto)
        {
            EditarValidator.ValidateAndThrow(dto);
            var sucursal = RepoSucursal.Get(id);
            if (sucursal == null) return null;
            sucursal.Nombre = SecurityHelper.Clean(dto.Name);
            sucursal.Direccion = SecurityHelper.Clean(dto.Address);
            sucursal.Usuario = SecurityHelper.Clean(dto.Username).ToLowerInvariant();
            sucursal.Latitud = dto.Latitude;
            sucursal.Longitud = dto.Longitude;
            if (!string.IsNullOrWhiteSpace(dto.Password))
                sucursal.ContrasenaHash = HashHelper.Sha256(dto.Password);
            RepoSucursal.Update(sucursal);
            return Mapper.Map<SucursalDTO>(sucursal);
        }

        public bool Eliminar(int id)
        {
            var sucursal = RepoSucursal.Get(id);
            if (sucursal == null) return false;
            RepoSucursal.Delete(id);
            return true;
        }
    }
}
