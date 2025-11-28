
using ShopFinance.Application.Features.Users.DTOs;

namespace ShopFinance.Application.Features.Users.Queries;

public class GetUserByIdQuery : IQuery<Result<UserViewDto>>
{
    public Guid UserId { get; set; }
}
