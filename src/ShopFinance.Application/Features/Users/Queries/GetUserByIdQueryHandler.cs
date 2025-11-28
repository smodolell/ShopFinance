using ShopFinance.Application.Features.Users.DTOs;

namespace ShopFinance.Application.Features.Users.Queries;

internal class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, Result<UserViewDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<UserViewDto>> HandleAsync(GetUserByIdQuery query, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(query.UserId, cancellationToken);
        if (user == null)
        {
            return Result.NotFound("Usuario no existe");
        }

        var result = user.Adapt<UserViewDto>();

        return Result.Success(result);

    }
}