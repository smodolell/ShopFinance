// PaymentTermSpec.cs
using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

public class PaymentTermSpec : Specification<PaymentTerm>
{
    public PaymentTermSpec(
        string? searchText,
        bool? isActive,
        int? minMonths,
        int? maxMonths)
    {
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            Query.Where(pt =>
                pt.Name.Contains(searchText) ||
                pt.Code.Contains(searchText) ||
                pt.NumberOfPayments.ToString().Contains(searchText));
        }

        if (isActive.HasValue)
        {
            Query.Where(pt => pt.IsActive == isActive.Value);
        }

        if (minMonths.HasValue)
        {
            Query.Where(pt => pt.NumberOfPayments>= minMonths.Value);
        }

        if (maxMonths.HasValue)
        {
            Query.Where(pt => pt.NumberOfPayments<= maxMonths.Value);
        }

        // Orden por defecto: por número de meses ascendente
        Query.OrderBy(pt => pt.NumberOfPayments);
    }
}