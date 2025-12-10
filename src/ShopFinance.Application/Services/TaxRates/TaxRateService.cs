using ShopFinance.Application.Services.TaxRates.DTOs;
using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Services.TaxRates;


public class TaxRateService : ITaxRateService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<TaxRateEditDto> _validator;
    private readonly IPaginator _paginator;
    private readonly IDynamicSorter _sorter;
    private readonly ILogger<TaxRateService> _logger;

    public TaxRateService(
        IUnitOfWork unitOfWork,
        IValidator<TaxRateEditDto> validator,
        IPaginator paginator,
        IDynamicSorter sorter,
        ILogger<TaxRateService> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _paginator = paginator;
        _sorter = sorter;
        _logger = logger;
    }

    public async Task<Result> ChangeActive(int id, bool isActive)
    {
        _logger.LogInformation("Cambiando estado de tasa de impuesto ID: {TaxRateId} a {IsActive}", id, isActive);

        try
        {
            var taxRate = await _unitOfWork.TaxRates.GetByIdAsync(id);
            if (taxRate is null)
            {
                _logger.LogWarning("Tasa de impuesto no encontrada al cambiar estado. ID: {TaxRateId}", id);
                return Result.Error("Tasa de impuesto no encontrada.");
            }

            // Si se desactiva, verificar si está en uso
            if (!isActive && taxRate.IsActive)
            {
                var isUsed = await IsTaxRateInUse(id);
                if (isUsed)
                {
                    _logger.LogWarning("No se puede desactivar tasa ID: {TaxRateId} - Está en uso", id);
                    return Result.Error("No se puede desactivar la tasa porque está siendo utilizada en planes de cotización");
                }
            }

            taxRate.IsActive = isActive;
            await _unitOfWork.TaxRates.UpdateAsync(taxRate);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Estado de tasa ID: {TaxRateId} cambiado a {IsActive}", id, isActive);
            return Result.SuccessWithMessage($"Tasa de impuesto {(isActive ? "activada" : "desactivada")} correctamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cambiar estado de tasa ID: {TaxRateId}", id);
            return Result.Error($"Error al cambiar estado: {ex.Message}");
        }
    }

    public async Task<Result> Delete(int id)
    {
        _logger.LogInformation("Iniciando eliminación de tasa de impuesto ID: {TaxRateId}", id);

        try
        {
            var taxRate = await _unitOfWork.TaxRates.GetByIdAsync(id);
            if (taxRate is null)
            {
                _logger.LogWarning("Tasa de impuesto no encontrada al eliminar. ID: {TaxRateId}", id);
                return Result.NotFound($"Tasa de impuesto con ID {id} no encontrada");
            }

            // Verificar si está siendo usada
            var isUsed = await IsTaxRateInUse(id);
            if (isUsed)
            {
                _logger.LogWarning("No se puede eliminar tasa ID: {TaxRateId} - Está siendo utilizada", id);
                return Result.Error("No se puede eliminar la tasa porque está siendo utilizada en planes de cotización");
            }

            await _unitOfWork.TaxRates.DeleteAsync(taxRate);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Tasa de impuesto ID: {TaxRateId} eliminada exitosamente", id);
            return Result.SuccessWithMessage("Tasa de impuesto eliminada correctamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar tasa ID: {TaxRateId}", id);
            return Result.Error($"Error al eliminar: {ex.Message}");
        }
    }

    public async Task<Result<TaxRateEditDto>> GetById(int id)
    {
        _logger.LogDebug("Obteniendo tasa de impuesto por ID: {TaxRateId}", id);

        try
        {
            var taxRate = await _unitOfWork.TaxRates.GetByIdAsync(id);
            if (taxRate is null)
            {
                _logger.LogWarning("Tasa de impuesto no encontrada al obtener por ID: {TaxRateId}", id);
                return Result.Error("Tasa de impuesto no encontrada.");
            }

            var result = taxRate.Adapt<TaxRateEditDto>();

            _logger.LogDebug("Tasa de impuesto ID: {TaxRateId} obtenida exitosamente", id);
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener tasa por ID: {TaxRateId}", id);
            return Result<TaxRateEditDto>.Error($"Error al obtener: {ex.Message}");
        }
    }

    public async Task<PagedResult<List<TaxRateListItemDto>>> GetPaginated(TaxRateFilterDto? filter)
    {
        _logger.LogDebug("Obteniendo tasas de impuesto paginadas. Filtro: {@Filter}", filter);

        try
        {
            filter ??= new TaxRateFilterDto();
            var spec = new TaxRateSpec(
                filter.SearchText,
                filter.IsActive,
                filter.EffectiveDateFrom,
                filter.EffectiveDateTo);

            var query = _unitOfWork.TaxRates.ApplySpecification(spec);
            query = _sorter.ApplySort(query, filter.SortColumn, filter.SortDescending);

            var result = await _paginator.PaginateAsync<TaxRate, TaxRateListItemDto>(
                query, filter.Page, filter.PageSize);

            _logger.LogDebug("Tasas de impuesto paginadas obtenidas: {TotalCount} registros", result.PagedInfo.TotalRecords);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener tasas de impuesto paginadas");
            throw;
        }
    }

    public async Task<Result<int>> Save(TaxRateEditDto model)
    {
        _logger.LogInformation("Guardando tasa de impuesto. Modelo: {@Model}", model);

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var validationResult = await _validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validación fallida al guardar tasa. Errores: {@Errors}", validationResult.Errors);
                return Result.Invalid(validationResult.AsErrors());
            }

            // Validar duplicados por código
            var existingByCode = await _unitOfWork.TaxRates
                .AnyAsync(tr => tr.Code == model.Code && tr.Id != model.TaxRateId);
            if (existingByCode)
            {
                _logger.LogWarning("Intento de guardar tasa con código duplicado: {Code}", model.Code);
                return Result.Error("Ya existe una tasa con ese código");
            }

            // Validar solapamiento de fechas para tasas activas del mismo código
            if (model.IsActive)
            {
                var overlappingRates = await _unitOfWork.TaxRates
                    .AnyAsync(tr =>
                        tr.Id != model.TaxRateId &&
                        tr.Code == model.Code &&
                        tr.IsActive &&
                        tr.EffectiveDate <= (model.ExpirationDate ?? DateTime.MaxValue) &&
                        (!tr.ExpirationDate.HasValue || tr.ExpirationDate >= model.EffectiveDate));

                if (overlappingRates)
                {
                    _logger.LogWarning("Solapamiento de fechas detectado para código: {Code}", model.Code);
                    return Result.Error("Existen tasas activas del mismo impuesto que se solapan con estas fechas");
                }
            }

            TaxRate taxRate;

            if (model.TaxRateId == 0)
            {
                // Crear nueva tasa
                taxRate = model.Adapt<TaxRate>();
                taxRate.Id = 0;
                await _unitOfWork.TaxRates.AddAsync(taxRate);
                _logger.LogInformation("Nueva tasa de impuesto creada: {Name} ({Code})", model.Name, model.Code);
            }
            else
            {
                // Actualizar tasa existente
                taxRate = await _unitOfWork.TaxRates.GetByIdAsync(model.TaxRateId);
                if (taxRate is null)
                {
                    _logger.LogWarning("Tasa no encontrada para actualizar. ID: {TaxRateId}", model.TaxRateId);
                    return Result.Error("Tasa de impuesto no encontrada.");
                }

                model.Adapt(taxRate);
                await _unitOfWork.TaxRates.UpdateAsync(taxRate);
                _logger.LogInformation("Tasa de impuesto actualizada. ID: {TaxRateId}, Código: {Code}", model.TaxRateId, model.Code);
            }

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Tasa de impuesto guardada exitosamente. ID: {TaxRateId}", taxRate.Id);
            return Result.Success(taxRate.Id);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error al guardar tasa de impuesto. Modelo: {@Model}", model);
            return Result.Error($"Error al guardar: {ex.Message}");
        }
    }

    private async Task<bool> IsTaxRateInUse(int taxRateId)
    {
        // Verificar si está siendo usada en QuotationPlans
        var inQuotationPlans = await _unitOfWork.QuotationPlans
            .AnyAsync(qp => qp.TaxRateId == taxRateId);

        // También podrías verificar en otras entidades si las tienes
        // var inInvoices = await _unitOfWork.Invoices.AnyAsync(i => i.TaxRateId == taxRateId);

        return inQuotationPlans;
    }
}