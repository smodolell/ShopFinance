using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Enums;

namespace ShopFinance.Domain.Specifications;


public class OrderSpec : Specification<Order>
{
    public OrderSpec(
        string? searchText,
        OrderStatus? status,
        Guid? customerId,
        DateTime? orderDateFrom,
        DateTime? orderDateTo,
        DateTime? requiredDateFrom,
        DateTime? requiredDateTo)
    {
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            searchText = searchText.ToLower();
            Query.Where(o => o.OrderNumber.ToLower().Contains(searchText)
                || (o.Customer != null && o.Customer.FirstName.ToLower().Contains(searchText))
                || (o.Customer != null && o.Customer.LastName.ToLower().Contains(searchText))
                || (o.Customer != null && o.Customer.Identifier.ToLower().Contains(searchText)));
        }

        if (status != null)
        {
            Query.Where(o => o.Status == status);
        }

        if (customerId.HasValue)
        {
            Query.Where(o => o.CustomerId == customerId.Value);
        }

        if (orderDateFrom.HasValue)
        {
            Query.Where(o => o.OrderDate >= orderDateFrom.Value);
        }

        if (orderDateTo.HasValue)
        {
            Query.Where(o => o.OrderDate <= orderDateTo.Value.AddDays(1).AddSeconds(-1));
        }

        if (requiredDateFrom.HasValue)
        {
            Query.Where(o => o.RequiredDate >= requiredDateFrom.Value);
        }

        if (requiredDateTo.HasValue)
        {
            Query.Where(o => o.RequiredDate <= requiredDateTo.Value.AddDays(1).AddSeconds(-1));
        }

        // Incluir relaciones
        Query.Include(o => o.Customer);
        Query.Include(o => o.Items);

        // Orden por defecto
        Query.OrderByDescending(o => o.OrderDate);
    }
}
