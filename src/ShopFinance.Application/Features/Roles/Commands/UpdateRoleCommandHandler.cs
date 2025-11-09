namespace ShopFinance.Application.Features.Roles.Commands;

internal class UpdateRoleCommandHandler : ICommandHandler<UpdateRoleCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateRoleCommand> _validator;

    public UpdateRoleCommandHandler(IUnitOfWork unitOfWork, IValidator<UpdateRoleCommand> validator)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    public async Task<Result> HandleAsync(UpdateRoleCommand command, CancellationToken cancellationToken = default)
    {

        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result.Invalid(validationResult.AsErrors());
        }



        var role = await _unitOfWork.Roles.GetByIdAsync(command.RoleId);

        if (role == null)
        {
            return Result.NotFound("Np Exi");
        }


        role.Update(command.Name, command.Description);

        await _unitOfWork.Roles.UpdateAsync(role);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();

    }
}



