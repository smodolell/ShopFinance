// IPaymentTermService.cs
using Microsoft.EntityFrameworkCore;
using ShopFinance.Application.Services.PaymentTerms.DTOs;
using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Services.PaymentTerms;


public class PaymentTermService : IPaymentTermService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<PaymentTermEditDto> _validator;
    private readonly IPaginator _paginator;
    private readonly IDynamicSorter _sorter;
    private readonly ILogger<PaymentTermService> _logger;

    public PaymentTermService(
        IUnitOfWork unitOfWork,
        IValidator<PaymentTermEditDto> validator,
        IPaginator paginator,
        IDynamicSorter sorter,
        ILogger<PaymentTermService> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _paginator = paginator;
        _sorter = sorter;
        _logger = logger;
    }

    public async Task<Result> ChangeActive(int id, bool isActive)
    {
        _logger.LogInformation("Cambiando estado de plazo ID: {PaymentTermId} a {IsActive}", id, isActive);

        try
        {
            var paymentTerm = await _unitOfWork.PaymentTerms.GetByIdAsync(id);
            if (paymentTerm is null)
            {
                _logger.LogWarning("Plazo no encontrado al cambiar estado. ID: {PaymentTermId}", id);
                return Result.Error("Plazo de pago no encontrado.");
            }

            // Si se desactiva, verificar si está en uso
            if (!isActive && paymentTerm.IsActive)
            {
                var isUsed = await IsPaymentTermInUse(id);
                if (isUsed)
                {
                    _logger.LogWarning("No se puede desactivar plazo ID: {PaymentTermId} - Está en uso", id);
                    return Result.Error("No se puede desactivar el plazo porque está siendo utilizado en planes de cotización");
                }
            }

            paymentTerm.IsActive = isActive;
            await _unitOfWork.PaymentTerms.UpdateAsync(paymentTerm);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Estado de plazo ID: {PaymentTermId} cambiado a {IsActive}", id, isActive);
            return Result.SuccessWithMessage($"Plazo {(isActive ? "activado" : "desactivado")} correctamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cambiar estado de plazo ID: {PaymentTermId}", id);
            return Result.Error($"Error al cambiar estado: {ex.Message}");
        }
    }

    public async Task<Result> Delete(int id)
    {
        _logger.LogInformation("Iniciando eliminación de plazo ID: {PaymentTermId}", id);

        try
        {
            var paymentTerm = await _unitOfWork.PaymentTerms.GetByIdAsync(id);
            if (paymentTerm is null)
            {
                _logger.LogWarning("Plazo no encontrado al eliminar. ID: {PaymentTermId}", id);
                return Result.NotFound($"Plazo con ID {id} no encontrado");
            }

            // Verificar si está siendo usada
            var isUsed = await IsPaymentTermInUse(id);
            if (isUsed)
            {
                _logger.LogWarning("No se puede eliminar plazo ID: {PaymentTermId} - Está siendo utilizado", id);
                return Result.Error("No se puede eliminar el plazo porque está siendo utilizado en planes de cotización");
            }

            await _unitOfWork.PaymentTerms.DeleteAsync(paymentTerm);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Plazo ID: {PaymentTermId} eliminado exitosamente", id);
            return Result.SuccessWithMessage("Plazo eliminado correctamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar plazo ID: {PaymentTermId}", id);
            return Result.Error($"Error al eliminar: {ex.Message}");
        }
    }

    public async Task<Result<PaymentTermEditDto>> GetById(int id)
    {
        _logger.LogDebug("Obteniendo plazo por ID: {PaymentTermId}", id);

        try
        {
            var paymentTerm = await _unitOfWork.PaymentTerms.GetByIdAsync(id);
            if (paymentTerm is null)
            {
                _logger.LogWarning("Plazo no encontrado al obtener por ID: {PaymentTermId}", id);
                return Result.Error("Plazo de pago no encontrado.");
            }

            var result = paymentTerm.Adapt<PaymentTermEditDto>();

            _logger.LogDebug("Plazo ID: {PaymentTermId} obtenido exitosamente", id);
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener plazo por ID: {PaymentTermId}", id);
            return Result<PaymentTermEditDto>.Error($"Error al obtener: {ex.Message}");
        }
    }

    public async Task<PagedResult<List<PaymentTermListItemDto>>> GetPaginated(PaymentTermFilterDto? filter)
    {
        _logger.LogDebug("Obteniendo plazos paginados. Filtro: {@Filter}", filter);

        try
        {
            filter ??= new PaymentTermFilterDto();
            var spec = new PaymentTermSpec(
                filter.SearchText,
                filter.IsActive,
                filter.MinMonths,
                filter.MaxMonths);

            var query = _unitOfWork.PaymentTerms.ApplySpecification(spec);
            query = _sorter.ApplySort(query, filter.SortColumn, filter.SortDescending);

            var result = await _paginator.PaginateAsync<PaymentTerm, PaymentTermListItemDto>(
                query, filter.Page, filter.PageSize);

            _logger.LogDebug("Plazos paginados obtenidos: {TotalCount} registros", result.PagedInfo.TotalRecords);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener plazos paginados");
            throw;
        }
    }

    public async Task<Result<List<PaymentTermListItemDto>>> GetActivePaymentTerms()
    {
        _logger.LogDebug("Obteniendo plazos activos");

        try
        {
            var spec = new PaymentTermSpec(null,true, null, null);
            var query = _unitOfWork.PaymentTerms.ApplySpecification(spec);
            var paymentTerms = await query
                .ProjectToType<PaymentTermListItemDto>().ToListAsync();

            _logger.LogDebug("Se obtuvieron {Count} plazos activos", paymentTerms.Count);
            return Result.Success(paymentTerms);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener plazos activos");
            return Result.Error($"Error al obtener plazos activos: {ex.Message}");
        }
    }

    public async Task<Result<int>> Save(PaymentTermEditDto model)
    {
        _logger.LogInformation("Guardando plazo. Modelo: {@Model}", model);

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var validationResult = await _validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validación fallida al guardar plazo. Errores: {@Errors}", validationResult.Errors);
                return Result.Invalid(validationResult.AsErrors());
            }

            // Validar duplicados por nombre
            var existingByName = await _unitOfWork.PaymentTerms
                .AnyAsync(pt => pt.Name == model.Name && pt.Id != model.PaymentTermId);
            if (existingByName)
            {
                _logger.LogWarning("Intento de guardar plazo con nombre duplicado: {Name}", model.Name);
                return Result.Error("Ya existe un plazo con ese nombre");
            }

            // Validar duplicados por código
            var existingByCode = await _unitOfWork.PaymentTerms
                .AnyAsync(pt => pt.Code == model.Code && pt.Id != model.PaymentTermId);
            if (existingByCode)
            {
                _logger.LogWarning("Intento de guardar plazo con código duplicado: {Code}", model.Code);
                return Result.Error("Ya existe un plazo con ese código");
            }

            // Validar duplicados por número de meses (opcional, si quieres que sea único)
            var existingByMonths = await _unitOfWork.PaymentTerms
                .AnyAsync(pt => pt.NumberOfPayments== model.NumberOfPayments&& pt.Id != model.PaymentTermId);
            if (existingByMonths)
            {
                _logger.LogWarning("Intento de guardar plazo con meses duplicados: {NumberOfMonths}", model.NumberOfPayments);
                return Result.Error("Ya existe un plazo con esa cantidad de meses");
            }

            PaymentTerm? paymentTerm;

            if (model.PaymentTermId == 0)
            {
                // Crear nuevo plazo
                paymentTerm = model.Adapt<PaymentTerm>();
                paymentTerm.Id = 0;
                await _unitOfWork.PaymentTerms.AddAsync(paymentTerm);
                _logger.LogInformation("Nuevo plazo creado: {Name} ({NumberOfMonths} meses)", model.Name, model.NumberOfPayments);
            }
            else
            {
                // Actualizar plazo existente
                paymentTerm = await _unitOfWork.PaymentTerms.GetByIdAsync(model.PaymentTermId);
                if (paymentTerm is null)
                {
                    _logger.LogWarning("Plazo no encontrado para actualizar. ID: {PaymentTermId}", model.PaymentTermId);
                    return Result.Error("Plazo de pago no encontrado.");
                }

                model.Adapt(paymentTerm);
                await _unitOfWork.PaymentTerms.UpdateAsync(paymentTerm);
                _logger.LogInformation("Plazo actualizado. ID: {PaymentTermId}, Nombre: {Name}", model.PaymentTermId, model.Name);
            }

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Plazo guardado exitosamente. ID: {PaymentTermId}", paymentTerm.Id);
            return Result.Success(paymentTerm.Id);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error al guardar plazo. Modelo: {@Model}", model);
            return Result.Error($"Error al guardar: {ex.Message}");
        }
    }

    private async Task<bool> IsPaymentTermInUse(int paymentTermId)
    {
        // Verificar si está siendo usada en QuotationPlanPaymentTerms
        var inQuotationPlanPaymentTerms = await _unitOfWork.QuotationPlanPaymentTerms
            .AnyAsync(qppt => qppt.PaymentTermId == paymentTermId);

        // También podrías verificar en otras entidades si las tienes
        // var inCredits = await _unitOfWork.Credits.AnyAsync(c => c.PaymentTermId == paymentTermId);

        return inQuotationPlanPaymentTerms;
    }
}