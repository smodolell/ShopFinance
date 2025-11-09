using ShopFinance.Application.Features.Roles.DTOs;

namespace ShopFinance.Application.Features.Roles.Queries;

public class GetRoleByIdQuery : IQuery<Result<RoleDto>>
{
    public Guid RoleId { get; set; }
}

internal class GetRoleByIdQueryHandler : IQueryHandler<GetRoleByIdQuery, Result<RoleDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetRoleByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<RoleDto>> HandleAsync(GetRoleByIdQuery message, CancellationToken cancellationToken = default)
    {
        var role = await _unitOfWork.Roles.GetByIdAsync(message.RoleId);

        if (role == null)
        {
            return Result.NotFound("Np Exi");
        }

        var result = role.Adapt<RoleDto>();

        return Result.Success(result);
    }
}
