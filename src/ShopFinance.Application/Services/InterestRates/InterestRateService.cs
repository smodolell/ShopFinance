using ShopFinance.Application.Services.InterestRates.DTOs;
using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Services.InterestRates;

public class InterestRateService : IInterestRateService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<InterestRateEditDto> _validator;
    private readonly IPaginator _paginator;
    private readonly IDynamicSorter _sorter;
    private readonly ILogger<InterestRateService> _logger;

    public InterestRateService(
        IUnitOfWork unitOfWork,
        IValidator<InterestRateEditDto> validator,
        IPaginator paginator,
        IDynamicSorter sorter,
        ILogger<InterestRateService> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _paginator = paginator;
        _sorter = sorter;
        _logger = logger;
    }

    public async Task<Result> ChangeActive(int id, bool isActive)
    {
        _logger.LogInformation("Cambiando estado de tasa ID: {InterestRateId} a {IsActive}", id, isActive);

        try
        {
            var interestRate = await _unitOfWork.InterestRates.GetByIdAsync(id);
            if (interestRate is null)
            {
                _logger.LogWarning("Tasa no encontrada al cambiar estado. ID: {InterestRateId}", id);
                return Result.Error("Interest rate not found.");
            }

            interestRate.IsActive = isActive;
            await _unitOfWork.InterestRates.UpdateAsync(interestRate);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Estado de tasa ID: {InterestRateId} cambiado a {IsActive} exitosamente", id, isActive);
            return Result.SuccessWithMessage("Estado cambiado");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cambiar estado de tasa ID: {InterestRateId}", id);
            return Result.Error($"Error al cambiar estado: {ex.Message}");
        }
    }

    public async Task<Result> Delete(int id)
    {
        _logger.LogInformation("Iniciando eliminación de tasa ID: {InterestRateId}", id);

        try
        {
            var interestRate = await _unitOfWork.InterestRates.GetByIdAsync(id);
            if (interestRate is null)
            {
                _logger.LogWarning("Tasa no encontrada al eliminar. ID: {InterestRateId}", id);
                return Result.NotFound($"Tasa con ID {id} no encontrada");
            }

            // Verificar si está siendo usada
            var isUsed = await _unitOfWork.QuotationPlanPaymentTerms
                .AnyAsync(qppt => qppt.InterestRateId == id);

            if (isUsed)
            {
                _logger.LogWarning("No se puede eliminar tasa ID: {InterestRateId} - Está siendo utilizada en planes", id);
                return Result.Error("No se puede eliminar la tasa porque está siendo utilizada en planes de cotización");
            }

            await _unitOfWork.InterestRates.DeleteAsync(interestRate);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Tasa ID: {InterestRateId} eliminada exitosamente", id);
            return Result.SuccessWithMessage("Tasa eliminada correctamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar tasa ID: {InterestRateId}", id);
            return Result.Error($"Error al eliminar la tasa: {ex.Message}");
        }
    }

    public async Task<Result<InterestRateEditDto>> GetById(int id)
    {
        _logger.LogDebug("Obteniendo tasa por ID: {InterestRateId}", id);

        try
        {
            var interestRate = await _unitOfWork.InterestRates.GetByIdAsync(id);
            if (interestRate is null)
            {
                _logger.LogWarning("Tasa no encontrada al obtener por ID: {InterestRateId}", id);
                return Result.Error("Interest rate not found.");
            }

            var result = interestRate.Adapt<InterestRateEditDto>();

            _logger.LogDebug("Tasa ID: {InterestRateId} obtenida exitosamente", id);
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener tasa por ID: {InterestRateId}", id);
            return Result<InterestRateEditDto>.Error($"Error al obtener la tasa: {ex.Message}");
        }
    }

    public async Task<PagedResult<List<InterestRateListItemDto>>> GetPaginated(InterestRateFilterDto? filter)
    {
        _logger.LogDebug("Obteniendo tasas paginadas. Filtro: {@Filter}", filter);

        try
        {
            filter = filter ?? new InterestRateFilterDto();
            var spec = new InterestRateSpec(filter.SearchText, filter.IsActive);

            var query = _unitOfWork.InterestRates.ApplySpecification(spec);
            query = _sorter.ApplySort(query, filter.SortColumn, filter.SortDescending);

            var result = await _paginator.PaginateAsync<InterestRate, InterestRateListItemDto>(
                query,
                filter.Page,
                filter.PageSize
            );

            _logger.LogDebug("Tasas paginadas obtenidas: {TotalCount} registros", result.PagedInfo.TotalRecords);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener tasas paginadas");
            throw;
        }
    }

    public async Task<Result<int>> Save(InterestRateEditDto model)
    {
        _logger.LogInformation("Guardando tasa. Modelo: {@Model}", model);

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var validationResult = await _validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validación fallida al guardar tasa. Errores: {@Errors}", validationResult.Errors);
                return Result.Invalid(validationResult.AsErrors());
            }

            // Validar duplicados
            var existingByName = await _unitOfWork.InterestRates
                .AnyAsync(r => r.RateName == model.RateName && r.Id != model.InterestRateId);

            if (existingByName)
            {
                _logger.LogWarning("Intento de guardar tasa con nombre duplicado: {RateName}", model.RateName);
                return Result.Error("Ya existe una tasa con ese nombre");
            }

            // Validar solapamiento de fechas
            if (model.ExpirationDate.HasValue)
            {
                var overlappingRates = await _unitOfWork.InterestRates
                    .AnyAsync(r =>
                        r.Id != model.InterestRateId &&
                        r.IsActive &&
                        r.EffectiveDate <= model.ExpirationDate.Value &&
                        (!r.ExpirationDate.HasValue || r.ExpirationDate >= model.EffectiveDate));

                if (overlappingRates)
                {
                    _logger.LogWarning("Solapamiento de fechas detectado al guardar tasa: {RateName}", model.RateName);
                    return Result.Error("Existen tasas activas que se solapan con estas fechas");
                }
            }

            var interestRate = await _unitOfWork.InterestRates.GetByIdAsync(model.InterestRateId);

            if (interestRate is null && model.InterestRateId != 0)
            {
                _logger.LogWarning("Tasa no encontrada para actualizar. ID: {InterestRateId}", model.InterestRateId);
                return Result.Error("Interest rate not found.");
            }

            if (interestRate is null)
            {
                // Crear nueva tasa
                interestRate = new InterestRate { Id = 0 };
                model.Adapt(interestRate);
                await _unitOfWork.InterestRates.AddAsync(interestRate);
                _logger.LogInformation("Nueva tasa creada: {RateName}", model.RateName);
            }
            else
            {
                // Actualizar tasa existente
                model.Adapt(interestRate);
                await _unitOfWork.InterestRates.UpdateAsync(interestRate);
                _logger.LogInformation("Tasa actualizada. ID: {InterestRateId}, Nombre: {RateName}", model.InterestRateId, model.RateName);
            }

            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Tasa guardada exitosamente. ID: {InterestRateId}", interestRate.Id);
            return Result.Success(interestRate.Id);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error al guardar tasa. Modelo: {@Model}", model);
            return Result.Error($"Error al guardar la tasa: {ex.Message}");
        }
    }
}