using ShopFinance.Application.Features.Customers.DTOs;

namespace ShopFinance.Application.Features.Customers.Queries;

internal class GetCustomerByIdQueryHandler : IQueryHandler<GetCustomerByIdQuery, Result<CustomerViewDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCustomerByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<CustomerViewDto>> HandleAsync(GetCustomerByIdQuery query, CancellationToken cancellationToken = default)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(query.CustomerId);
        if (customer == null)
        {
            return Result.NotFound("No existe");
        }
        var result = customer.Adapt<CustomerViewDto>();

        return Result.Success(result);
    }
}