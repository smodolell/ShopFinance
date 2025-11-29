using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Enums;

namespace ShopFinance.Domain.Specifications;

public class SaleSpec : Specification<Sale>
{
    public SaleSpec(
        string? searchText,
        SaleStatus? status,
        PaymentMethod? paymentMethod,
        Guid? customerId,
        DateTime? saleDateFrom,
        DateTime? saleDateTo)
    {
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            searchText = searchText.ToLower();
            Query.Where(s => s.SaleNumber.ToLower().Contains(searchText)
                || (s.InvoiceNumber != null && s.InvoiceNumber.ToLower().Contains(searchText))
                || (s.Order != null && s.Order.OrderNumber.ToLower().Contains(searchText))
                || (s.Order != null && s.Order.Customer != null &&
                   (s.Order.Customer.FirstName.ToLower().Contains(searchText)
                    || s.Order.Customer.LastName.ToLower().Contains(searchText)
                    || s.Order.Customer.Identifier.ToLower().Contains(searchText))));
        }

        if (status != null)
        {
            Query.Where(s => s.Status == status);
        }

        if (paymentMethod != null)
        {
            Query.Where(s => s.PaymentMethod == paymentMethod.Value);
        }

        if (customerId.HasValue)
        {
            Query.Where(s => s.Order != null && s.Order.CustomerId == customerId.Value);
        }

        if (saleDateFrom.HasValue)
        {
            Query.Where(s => s.SaleDate >= saleDateFrom.Value);
        }

        if (saleDateTo.HasValue)
        {
            Query.Where(s => s.SaleDate <= saleDateTo.Value.AddDays(1).AddSeconds(-1));
        }

        // Incluir relaciones necesarias
        Query.Include(s => s.Order)
             .ThenInclude(o => o.Customer);
        Query.Include(s => s.Items)
             .ThenInclude(i => i.Product);

        // Orden por defecto
        Query.OrderByDescending(s => s.SaleDate);
    }
}