using ShopFinance.Application.Features.Users.DTOs;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Application.Features.Users;

public class UsersMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserViewDto>()
            .Map(o => o.UserId, d => d.Id);
        config.NewConfig<User, UserListItemDto>();
    }
}
