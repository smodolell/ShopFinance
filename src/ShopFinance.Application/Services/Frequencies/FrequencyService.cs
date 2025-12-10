using ShopFinance.Application.Services.Frequencies.DTOs;
using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Services.Frequencies;

public class FrequencyService : IFrequencyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<FrequencyEditDto> _validator;
    private readonly IPaginator _paginator;
    private readonly IDynamicSorter _sorter;
    private readonly ILogger<FrequencyService> _logger;

    public FrequencyService(
        IUnitOfWork unitOfWork,
        IValidator<FrequencyEditDto> validator,
        IPaginator paginator,
        IDynamicSorter sorter,
        ILogger<FrequencyService> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _paginator = paginator;
        _sorter = sorter;
        _logger = logger;
    }

    public async Task<Result> ChangeActive(int id, bool isActive)
    {
        _logger.LogInformation("Cambiando estado de frecuencia ID: {FrequencyId} a {IsActive}", id, isActive);

        try
        {
            var frequency = await _unitOfWork.Frequencies.GetByIdAsync(id);
            if (frequency is null)
            {
                _logger.LogWarning("Frecuencia no encontrada al cambiar estado. ID: {FrequencyId}", id);
                return Result.Error("Frecuencia no encontrada.");
            }

            // Si se desactiva, verificar si está en uso
            if (!isActive && frequency.IsActive)
            {
                var isUsed = await IsFrequencyInUse(id);
                if (isUsed)
                {
                    _logger.LogWarning("No se puede desactivar frecuencia ID: {FrequencyId} - Está en uso", id);
                    return Result.Error("No se puede desactivar la frecuencia porque está siendo utilizada");
                }
            }

            frequency.IsActive = isActive;
            await _unitOfWork.Frequencies.UpdateAsync(frequency);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Estado de frecuencia ID: {FrequencyId} cambiado a {IsActive}", id, isActive);
            return Result.SuccessWithMessage($"Frecuencia {(isActive ? "activada" : "desactivada")} correctamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cambiar estado de frecuencia ID: {FrequencyId}", id);
            return Result.Error($"Error al cambiar estado: {ex.Message}");
        }
    }

    public async Task<Result> Delete(int id)
    {
        _logger.LogInformation("Iniciando eliminación de frecuencia ID: {FrequencyId}", id);

        try
        {
            var frequency = await _unitOfWork.Frequencies.GetByIdAsync(id);
            if (frequency is null)
            {
                _logger.LogWarning("Frecuencia no encontrada al eliminar. ID: {FrequencyId}", id);
                return Result.NotFound($"Frecuencia con ID {id} no encontrada");
            }

            // Verificar si está siendo usada
            var isUsed = await IsFrequencyInUse(id);
            if (isUsed)
            {
                _logger.LogWarning("No se puede eliminar frecuencia ID: {FrequencyId} - Está siendo utilizada", id);
                return Result.Error("No se puede eliminar la frecuencia porque está siendo utilizada");
            }

            await _unitOfWork.Frequencies.DeleteAsync(frequency);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Frecuencia ID: {FrequencyId} eliminada exitosamente", id);
            return Result.SuccessWithMessage("Frecuencia eliminada correctamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar frecuencia ID: {FrequencyId}", id);
            return Result.Error($"Error al eliminar: {ex.Message}");
        }
    }

    public async Task<Result<FrequencyEditDto>> GetById(int id)
    {
        _logger.LogDebug("Obteniendo frecuencia por ID: {FrequencyId}", id);

        try
        {
            var frequency = await _unitOfWork.Frequencies.GetByIdAsync(id);
            if (frequency is null)
            {
                _logger.LogWarning("Frecuencia no encontrada al obtener por ID: {FrequencyId}", id);
                return Result.Error("Frecuencia no encontrada.");
            }

            var result = frequency.Adapt<FrequencyEditDto>();

            _logger.LogDebug("Frecuencia ID: {FrequencyId} obtenida exitosamente", id);
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener frecuencia por ID: {FrequencyId}", id);
            return Result<FrequencyEditDto>.Error($"Error al obtener: {ex.Message}");
        }
    }

    public async Task<PagedResult<List<FrequencyListItemDto>>> GetPaginated(FrequencyFilterDto? filter)
    {
        _logger.LogDebug("Obteniendo frecuencias paginadas. Filtro: {@Filter}", filter);

        try
        {
            filter ??= new FrequencyFilterDto();
            var spec = new FrequencySpec(filter.SearchText, filter.IsActive);

            var query = _unitOfWork.Frequencies.ApplySpecification(spec);
            query = _sorter.ApplySort(query, filter.SortColumn, filter.SortDescending);

            var result = await _paginator.PaginateAsync<Frequency, FrequencyListItemDto>(
                query, filter.Page, filter.PageSize);

            _logger.LogDebug("Frecuencias paginadas obtenidas: {TotalCount} registros", result.PagedInfo.TotalRecords);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener frecuencias paginadas");
            throw;
        }
    }

    public async Task<Result<int>> Save(FrequencyEditDto model)
    {
        _logger.LogInformation("Guardando frecuencia. Modelo: {@Model}", model);

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var validationResult = await _validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validación fallida al guardar frecuencia. Errores: {@Errors}", validationResult.Errors);
                return Result.Invalid(validationResult.AsErrors());
            }

            // Validar duplicados por nombre
            var existingByName = await _unitOfWork.Frequencies
                .AnyAsync(f => f.Name == model.Name && f.Id != model.FrequencyId);
            if (existingByName)
            {
                _logger.LogWarning("Intento de guardar frecuencia con nombre duplicado: {Name}", model.Name);
                return Result.Error("Ya existe una frecuencia con ese nombre");
            }

            // Validar duplicados por código
            var existingByCode = await _unitOfWork.Frequencies
                .AnyAsync(f => f.Code == model.Code && f.Id != model.FrequencyId);
            if (existingByCode)
            {
                _logger.LogWarning("Intento de guardar frecuencia con código duplicado: {Code}", model.Code);
                return Result.Error("Ya existe una frecuencia con ese código");
            }

            Frequency? frequency;

            if (model.FrequencyId == 0)
            {
                // Crear nueva frecuencia
                frequency = model.Adapt<Frequency>();
                frequency.Id = 0;
                await _unitOfWork.Frequencies.AddAsync(frequency);
                _logger.LogInformation("Nueva frecuencia creada: {Name} ({Code})", model.Name, model.Code);
            }
            else
            {
                // Actualizar frecuencia existente
                frequency = await _unitOfWork.Frequencies.GetByIdAsync(model.FrequencyId);
                if (frequency is null)
                {
                    _logger.LogWarning("Frecuencia no encontrada para actualizar. ID: {FrequencyId}", model.FrequencyId);
                    return Result.Error("Frecuencia no encontrada.");
                }

                model.Adapt(frequency);
                await _unitOfWork.Frequencies.UpdateAsync(frequency);
                _logger.LogInformation("Frecuencia actualizada. ID: {FrequencyId}, Nombre: {Name}", model.FrequencyId, model.Name);
            }

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Frecuencia guardada exitosamente. ID: {FrequencyId}", frequency.Id);
            return Result.Success(frequency.Id);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error al guardar frecuencia. Modelo: {@Model}", model);
            return Result.Error($"Error al guardar: {ex.Message}");
        }
    }

    private async Task<bool> IsFrequencyInUse(int frequencyId)
    {
        // Verificar si está siendo usada en QuotationPlanFrequency
        var inQuotationPlan = await _unitOfWork.QuotationPlanFrequencies
            .AnyAsync(qpf => qpf.FrequencyId == frequencyId);

        // Verificar si está siendo usada en Credits
        var inCredits = await _unitOfWork.Credits
            .AnyAsync(c => c.FrequencyId == frequencyId);

        // Verificar si está siendo usada en Quotations
        var inQuotations = await _unitOfWork.Quotations
            .AnyAsync(q => q.FrequencyId == frequencyId);

        return inQuotationPlan || inCredits || inQuotations;
    }
}