using AutoMapper;
using PedidoApi.Models.Dtos;
using PedidoApi.Models.Entities;

namespace PedidoApi.Profiles;

public class AppProfile : Profile
{
    public AppProfile()
    {
        CreateMap<UserEntity, UserDto>();
        CreateMap<ProductEntity, ProductDto>();
        CreateMap<ProductCreateDto, ProductEntity>();
        CreateMap<ProductUpdateDto, ProductEntity>();
        CreateMap<OrderItemEntity, OrderItemDto>();
        CreateMap<OrderEntity, OrderDto>();
        CreateMap<BranchEntity, BranchDto>();
    }
}
