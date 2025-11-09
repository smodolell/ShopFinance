namespace ShopFinance.Application.Features.Users.Commands;

public class UpdateUserCommand : ICommand<Result>
{
    public Guid UserId { get; set; }

}


internal class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Result> HandleAsync(UpdateUserCommand message, CancellationToken cancellationToken = default)
    {
         var user = await _unitOfWork.Users.GetByIdAsync(message.UserId);
        throw new NotImplementedException();
    }
}
public class UserUpdateDto
{

}

public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
{
    public UserUpdateDtoValidator()
    {
        
    }
}