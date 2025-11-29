
using ShopFinance.Application.Features.Sales.Commands;
using ShopFinance.Application.Features.Sales.DTOs;
using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Enums;

namespace ShopFinance.Application.Features.Sales;
public class SalesMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Mapeo para listado de órdenes
        config.NewConfig<Order, OrderListItemDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.OrderNumber, src => src.OrderNumber)
            .Map(dest => dest.OrderDate, src => src.OrderDate)
            .Map(dest => dest.RequiredDate, src => src.RequiredDate)
            .Map(dest => dest.Status, src => src.Status)
            .Map(dest => dest.TotalAmount, src => src.TotalAmount)
            .Map(dest => dest.CustomerName, src =>
                src.Customer != null ? $"{src.Customer.FirstName} {src.Customer.LastName}" : "Cliente no especificado")
            .Map(dest => dest.CustomerIdentifier, src =>
                src.Customer != null ? src.Customer.Identifier : string.Empty)
            .Map(dest => dest.ItemsCount, src => src.Items.Count)
            .Map(dest => dest.IsOverdue, src =>
                src.RequiredDate.HasValue && src.RequiredDate.Value < DateTime.Today && src.Status != OrderStatus.Cancelled);

        // Mapeo para detalles de orden
        //config.NewConfig<Order, OrderDetailDto>()
        //    .Map(dest => dest.OrderId, src => src.Id)
        //    .Map(dest => dest.OrderNumber, src => src.OrderNumber)
        //    .Map(dest => dest.OrderDate, src => src.OrderDate)
        //    .Map(dest => dest.RequiredDate, src => src.RequiredDate)
        //    .Map(dest => dest.Status, src => src.Status)
        //    .Map(dest => dest.TotalAmount, src => src.TotalAmount)
        //    .Map(dest => dest.Notes, src => src.Notes)
        //    .Map(dest => dest.CustomerName, src =>
        //        src.Customer != null ? $"{src.Customer.FirstName} {src.Customer.LastName}" : null)
        //    .Map(dest => dest.CustomerIdentifier, src =>
        //        src.Customer != null ? src.Customer.Identifier : null)
        //    .Map(dest => dest.CustomerId, src => src.CustomerId);

        // Mapeo para items de orden
        config.NewConfig<OrderItem, OrderItemDto>()
            .Map(dest => dest.ProductId, src => src.ProductId)
            .Map(dest => dest.ProductName, src => src.Product.Name)
            .Map(dest => dest.Code, src => src.Product.Code)
            .Map(dest => dest.Quantity, src => src.Quantity)
            .Map(dest => dest.UnitPrice, src => src.UnitPrice)
            .Map(dest => dest.Stock, src => src.Product.Stock);


        config.NewConfig<OrderItemDto, OrderItem>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.Order)
            .Ignore(dest => dest.Product);



        config.NewConfig<Order, OrderViewDto>()
            .Map(dest => dest.OrderId, src => src.Id)
            .Map(dest => dest.CustomerName, src =>
                src.Customer != null ? $"{src.Customer.FirstName} {src.Customer.LastName}" : "Cliente no especificado")
            .Map(dest => dest.CustomerIdentifier, src => src.Customer != null ? src.Customer.Identifier : "")
            .Map(dest => dest.TotalItems, src => src.Items.Sum(i => i.Quantity))
            .Map(dest => dest.IsOverdue, src =>
                src.RequiredDate.HasValue && src.RequiredDate.Value < DateTime.Today && src.Status != OrderStatus.Cancelled);

        config.NewConfig<OrderItem, OrderItemViewDto>()
            .Map(dest => dest.ProductName, src => src.Product != null ? src.Product.Name : "Producto no encontrado")
            .Map(dest => dest.Code, src => src.Product.Code ?? string.Empty)
            .Map(dest => dest.Stock, src => src.Product.Stock)
            .Map(dest => dest.UnitPrice, src => src.UnitPrice)
            .Map(dest => dest.SubTotal, src => src.Quantity * src.UnitPrice);



        config.NewConfig<Sale, SaleListItemDto>()
    .Map(dest => dest.Id, src => src.Id)
    .Map(dest => dest.CustomerName, src =>
        src.Order != null && src.Order.Customer != null
            ? $"{src.Order.Customer.FirstName} {src.Order.Customer.LastName}"
            : "Cliente no especificado")
    .Map(dest => dest.CustomerIdentifier, src => src.Order.Customer != null ? src.Order.Customer.Identifier : string.Empty)
    .Map(dest => dest.OrderNumber, src => src.Order.OrderNumber ?? string.Empty)
    .Map(dest => dest.ItemsCount, src => src.Items.Count)
    .Map(dest => dest.PaymentMethodDisplay, src => src.PaymentMethod.ToString())
    .Map(dest => dest.StatusDisplay, src => src.Status.ToString());

        // Mapeo para detalle de venta
        config.NewConfig<Sale, SaleDetailDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.CustomerName, src =>
                src.Order != null && src.Order.Customer != null
                    ? $"{src.Order.Customer.FirstName} {src.Order.Customer.LastName}"
                    : null)
            .Map(dest => dest.CustomerIdentifier, src => src.Order.Customer != null ? src.Order.Customer.Identifier : "")
            .Map(dest => dest.CustomerId, src => src.Order.CustomerId)
            .Map(dest => dest.OrderNumber, src => src.Order.OrderNumber ?? string.Empty);


        config.NewConfig<Sale, SaleDetailDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.CustomerName, src =>
                src.Order != null && src.Order.Customer != null
                    ? $"{src.Order.Customer.FirstName} {src.Order.Customer.LastName}"
                    : null)
            .Map(dest => dest.CustomerIdentifier, src => src.Order.Customer != null ? src.Order.Customer.Identifier : "")
            .Map(dest => dest.CustomerId, src => src.Order.CustomerId)
            .Map(dest => dest.OrderNumber, src => src.Order.OrderNumber ?? string.Empty)
            .Map(dest => dest.TotalProfit, src =>
                src.Items.Sum(i => (i.UnitPrice - i.CostPrice) * i.Quantity))
            .Map(dest => dest.TotalItems, src => src.Items.Sum(i => i.Quantity))
            .Map(dest => dest.PaymentMethodDisplay, src => src.PaymentMethod.ToString())
            .Map(dest => dest.StatusDisplay, src => src.Status.ToString());

        config.NewConfig<SaleItem, SaleItemDto>()
            .Map(dest => dest.ProductName, src => src.Product.Name ?? "Producto no encontrado")
            .Map(dest => dest.Code, src => src.Product.Code ?? string.Empty)
            .Map(dest => dest.CategoryName, src => src.Product.Category.Name ?? "Sin categoría")
            .Map(dest => dest.SubTotal, src => src.Quantity * src.UnitPrice)
            .Map(dest => dest.Profit, src => (src.UnitPrice - src.CostPrice) * src.Quantity)
            .Map(dest => dest.ProfitMargin, src => src.UnitPrice > 0 ? ((src.UnitPrice - src.CostPrice) / src.CostPrice) * 100 : 0);
    }
}