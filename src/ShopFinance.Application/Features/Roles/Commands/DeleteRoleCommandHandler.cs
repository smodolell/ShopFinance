namespace ShopFinance.Application.Features.Roles.Commands;

internal class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRoleCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(DeleteRoleCommand message, CancellationToken cancellationToken = default)
    {
        var role = await _unitOfWork.Roles.GetByIdAsync(message.RoleId);
        if (role == null)
        {
            return Result.NotFound("no existe");
        }
        await _unitOfWork.Roles.DeleteAsync(role);

        await _unitOfWork.SaveChangesAsync();

        return Result.SuccessWithMessage("Eliminado");


    }
}