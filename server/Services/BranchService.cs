using AutoMapper;
using FluentValidation;
using PedidoApi.Helpers;
using PedidoApi.Models.Dtos;
using PedidoApi.Models.Entities;
using PedidoApi.Repositories;

namespace PedidoApi.Services;

public class BranchService
{
    private readonly GenericRepository<BranchEntity> _branches;
    private readonly IValidator<BranchCreateDto> _createValidator;
    private readonly IValidator<BranchUpdateDto> _updateValidator;
    private readonly IMapper _mapper;

    public BranchService(GenericRepository<BranchEntity> branches, IValidator<BranchCreateDto> createValidator, IValidator<BranchUpdateDto> updateValidator, IMapper mapper)
    {
        _branches = branches;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _mapper = mapper;
    }

    public List<BranchDto> GetAll()
    {
        return _branches.GetAll().Select(_mapper.Map<BranchDto>).ToList();
    }

    public BranchDto Create(BranchCreateDto dto)
    {
        _createValidator.ValidateAndThrow(dto);
        if (_branches.GetAll().Any(x => x.Username.Equals(dto.Username, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException("Ese usuario de sucursal ya existe.");
        }

        var branch = new BranchEntity
        {
            Name = SecurityHelper.Clean(dto.Name),
            Address = SecurityHelper.Clean(dto.Address),
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            Username = SecurityHelper.Clean(dto.Username).ToLowerInvariant(),
            PasswordHash = HashHelper.Sha256(dto.Password)
        };

        return _mapper.Map<BranchDto>(_branches.Add(branch));
    }

    public BranchDto Update(int id, BranchUpdateDto dto)
    {
        _updateValidator.ValidateAndThrow(dto);
        var branch = _branches.GetById(id) ?? throw new KeyNotFoundException("Sucursal no encontrada.");

        branch.Name = SecurityHelper.Clean(dto.Name);
        branch.Address = SecurityHelper.Clean(dto.Address);
        branch.Latitude = dto.Latitude;
        branch.Longitude = dto.Longitude;
        branch.Username = SecurityHelper.Clean(dto.Username).ToLowerInvariant();

        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            branch.PasswordHash = HashHelper.Sha256(dto.Password);
        }

        return _mapper.Map<BranchDto>(_branches.Update(branch));
    }

    public void Delete(int id)
    {
        if (!_branches.Delete(id))
        {
            throw new KeyNotFoundException("Sucursal no encontrada.");
        }
    }
}
