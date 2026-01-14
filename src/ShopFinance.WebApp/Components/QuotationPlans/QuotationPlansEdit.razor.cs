using Ardalis.Result;
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using ShopFinance.Application.Features.Phases.DTOs;
using ShopFinance.Application.Features.Phases.Queries;
using ShopFinance.Application.Features.QuotationPlans.Commands;
using ShopFinance.Application.Features.QuotationPlans.DTOs;
using ShopFinance.Application.Features.QuotationPlans.Queries;
using ShopFinance.Application.Services.Frequencies.DTOs;
using ShopFinance.Application.Services.InterestRates.DTOs;
using ShopFinance.Application.Services.PaymentTerms.DTOs;
using ShopFinance.Application.Services.TaxRates.DTOs;
using ShopFinance.WebApp.Constants;

namespace ShopFinance.WebApp.Components.QuotationPlans;

public partial class QuotationPlansEdit
{


    #region Fields

    [Parameter]
    public int? QuotationPlanId { get; set; }

    private QuotationPlanEditDto _model = new();
    private FluentValidationValidator? _fluentValidationValidator;
    private List<PhaseListItemDto>? _phasesList;
    private List<PhaseListItemDto>? _phasesListInitial;
    private List<PhaseListItemDto>? _phasesListFinal;
    private List<PhaseListItemDto>? _availablePhases;

    private List<FrequencyListItemDto>? _frequenciesList;
    private List<FrequencyListItemDto>? _availableFrequencies;
    private List<PaymentTermListItemDto>? _paymentTermsList;
    private List<PaymentTermListItemDto>? _availablePaymentTermsList;
    private List<InterestRateListItemDto>? _interestRatesList;
    private List<TaxRateListItemDto>? _taxRatesList;

    private bool _isLoading = true;
    private bool _isSubmitting = false;
    private bool _showAddPhaseDialog = false;
    private bool _showAddPaymentTermDialog = false;
    private bool _showAddFrequencyDialog = false;
    private int? _selectedPaymentTermId = null;
    private int? _selectedFrequencyId = null;

    private int _activePhasesCount => _model.Phases?.Count(p => p.Active) ?? 0;
    private readonly DialogOptions _dialogOptions = new() { MaxWidth = MaxWidth.Medium, FullWidth = true };
    #endregion


    #region Lifecycle Methods
    protected override async Task OnInitializedAsync()
    {
        await LoadData();
    }
    #endregion

    #region Data Methods
    private async Task LoadData()
    {
        try
        {
            _isLoading = true;
            StateHasChanged();

            if (QuotationPlanId != null)
            {
                var result = await _queryMediator.QueryAsync(new GetQuotationEditByIdQuery
                {
                    QuotationPlanId = QuotationPlanId.Value
                });
                if (result.IsSuccess)
                {
                    _model = result.Value;
                }
                else
                {
                    _snackbarService.Add("No es posible Cargar los datos", Severity.Warning);
                }

            }

            // Cargar lista de fases disponibles
            if (_phasesList == null)
            {
                _phasesList = await _queryMediator.QueryAsync(new GetPhasesListQuery { IsInitial = false, IsFinal = false });
                _model.Phases = _phasesList.Select(p => new QuotationPlanPhaseDto
                {
                    PhaseId = p.Id,
                    PhaseName = p.PhaseName,
                    IsFinal = p.IsFinal,
                    IsInitial = p.IsInitial,
                    Order = p.Order,
                    Required = p.Required,
                    Active = p.Required
                }).ToList();
            }
            if (_phasesListInitial == null)
            {
                _phasesListInitial = await _queryMediator.QueryAsync(new GetPhasesListQuery { IsInitial = true });
                if (_phasesListInitial.Count == 1)
                {
                    _model.PhaseIdInitial = _phasesListInitial[0].Id;
                }
            }
            if (_phasesListFinal == null)
            {
                _phasesListFinal = await _queryMediator.QueryAsync(new GetPhasesListQuery { IsFinal = true });
                if (_phasesListFinal.Count == 1)
                {
                    _model.PhaseIdFinal = _phasesListFinal[0].Id;
                }
            }

            var tasks = new List<Task>
{
                LoadFrequenciesAsync(),
                LoadPaymentTermsAsync(),
                LoadInterestRatesAsync(),
                LoadTaxRatesAsync()
            };
            await Task.WhenAll(tasks);

            // Actualizar listas disponibles después de cargar
            UpdateAvailableFrequencies();
            UpdateAvailablePaymentTerms();
        }
        catch (Exception ex)
        {
            _snackbarService.Add($"Error al cargar datos: {ex.Message}", Severity.Error);
        }
        finally
        {
            _isLoading = false;
            StateHasChanged();
        }
    }



    private async Task LoadFrequenciesAsync()
    {
        var filter = new FrequencyFilterDto { IsActive = true, PageSize = 100 };
        var result = await _frequencyService.GetPaginated(filter);
        _frequenciesList = result.Value;
    }

    private async Task LoadPaymentTermsAsync()
    {
        var filter = new PaymentTermFilterDto { IsActive = true, PageSize = 100 };
        var result = await _paymentTermService.GetPaginated(filter);
        _paymentTermsList = result.Value;
    }

    private async Task LoadInterestRatesAsync()
    {
        var filter = new InterestRateFilterDto { IsActive = true, PageSize = 100 };
        var result = await _interestRateService.GetPaginated(filter);
        _interestRatesList = result.Value;
    }

    private async Task LoadTaxRatesAsync()
    {
        var filter = new TaxRateFilterDto { IsActive = true, PageSize = 100 };
        var result = await _taxRateService.GetPaginated(filter);
        _taxRatesList = result.Value;
    }

    private void UpdateAvailableFrequencies()
    {
        if (_frequenciesList == null) return;

        var assignedFrequencyIds = _model.Frequencies.Select(f => f.FrequencyId).ToList();
        _availableFrequencies = _frequenciesList
            .Where(f => !assignedFrequencyIds.Contains(f.Id))
            .ToList();
    }

    private void UpdateAvailablePaymentTerms()
    {
        if (_paymentTermsList == null) return;

        var assignedPaymentTermIds = _model.PaymentTerms.Select(pt => pt.PaymentTermId).ToList();
        _availablePaymentTermsList = _paymentTermsList
            .Where(pt => !assignedPaymentTermIds.Contains(pt.Id))
            .ToList();
    }
    #endregion

    #region UI Handlers para Diálogos
    private void OpenAddFrequencyDialog()
    {
        if (_frequenciesList == null || !_frequenciesList.Any())
        {
            _snackbarService.Add("No hay frecuencias disponibles", Severity.Warning);
            return;
        }

        UpdateAvailableFrequencies();

        if (_availableFrequencies == null || !_availableFrequencies.Any())
        {
            _snackbarService.Add("Todas las frecuencias ya están asignadas", Severity.Info);
            return;
        }

        _selectedFrequencyId = null;
        _showAddFrequencyDialog = true;
    }

    private void OpenAddPaymentTermDialog()
    {
        if (_paymentTermsList == null || !_paymentTermsList.Any())
        {
            _snackbarService.Add("No hay plazos de pago disponibles", Severity.Warning);
            return;
        }

        UpdateAvailablePaymentTerms();

        if (_availablePaymentTermsList == null || !_availablePaymentTermsList.Any())
        {
            _snackbarService.Add("Todos los plazos ya están asignados", Severity.Info);
            return;
        }

        _selectedPaymentTermId = null;
        _showAddPaymentTermDialog = true;
    }

    private void CloseAddFrequencyDialog()
    {
        _showAddFrequencyDialog = false;
        _selectedFrequencyId = null;
    }

    private void CloseAddPaymentTermDialog()
    {
        _showAddPaymentTermDialog = false;
        _selectedPaymentTermId = null;
    }

    private void ConfirmAddFrequency()
    {
        if (_selectedFrequencyId.HasValue && _frequenciesList != null)
        {
            var selectedFrequency = _frequenciesList.FirstOrDefault(f => f.Id == _selectedFrequencyId.Value);
            if (selectedFrequency != null)
            {
                // Verificar si ya existe
                var alreadyExists = _model.Frequencies.Any(f => f.FrequencyId == _selectedFrequencyId.Value);
                if (alreadyExists)
                {
                    _snackbarService.Add("Esta frecuencia ya está asignada al plan", Severity.Warning);
                    return;
                }

                // Agregar nueva frecuencia
                _model.Frequencies.Add(new QuotationPlanFrequencyDto
                {
                    FrequencyId = selectedFrequency.Id,
                    IsDefault = _model.Frequencies.Count == 0, // Primera es default
                    Order = _model.Frequencies.Count + 1,
                    Active = true
                });

                UpdateAvailableFrequencies();
                StateHasChanged();
            }
        }

        CloseAddFrequencyDialog();
    }

    private void ConfirmAddPaymentTerm()
    {
        if (_selectedPaymentTermId.HasValue && _paymentTermsList != null)
        {
            var selectedPaymentTerm = _paymentTermsList.FirstOrDefault(pt => pt.Id == _selectedPaymentTermId.Value);
            if (selectedPaymentTerm != null)
            {
                // Verificar si ya existe
                var alreadyExists = _model.PaymentTerms.Any(pt => pt.PaymentTermId == _selectedPaymentTermId.Value);
                if (alreadyExists)
                {
                    _snackbarService.Add("Este plazo ya está asignado al plan", Severity.Warning);
                    return;
                }

                // Buscar primera tasa de interés activa
                var defaultInterestRate = _interestRatesList?
                    .FirstOrDefault(ir => ir.IsActive);

                // Agregar nuevo plazo
                _model.PaymentTerms.Add(new QuotationPlanPaymentTermDto
                {
                    PaymentTermId = selectedPaymentTerm.Id,
                    InterestRateId = defaultInterestRate?.Id,
                    Order = _model.PaymentTerms.Count + 1,
                    Active = true
                });

                UpdateAvailablePaymentTerms();
                StateHasChanged();
            }
        }

        CloseAddPaymentTermDialog();
    }
    #endregion

    #region UI Handlers para Formulario
    private async Task HandleValidSubmit()
    {
        if (_isSubmitting) return;

        if (!await ValidateFormAsync())
            return;

        await UpdateQuotationPlanAsync();
    }

    private async Task<bool> ValidateFormAsync()
    {
        if (_fluentValidationValidator != null)
        {
            var isValid = await _fluentValidationValidator.ValidateAsync(options =>
            {
                options.IncludeAllRuleSets();
            });

            if (!isValid)
            {
                _snackbarService.Add("Por favor, corrige los errores del formulario", Severity.Warning);
                return false;
            }
        }

        // Validación adicional: al menos una fase activa
        if (_model.Phases.Count == 0)
        {
            _snackbarService.Add("Debe agregar al menos una fase al plan", Severity.Warning);
            return false;
        }

        if (!_model.Phases.Any(p => p.Active))
        {
            _snackbarService.Add("Debe tener al menos una fase activa", Severity.Warning);
            return false;
        }

        // Validación adicional: al menos una frecuencia activa
        if (_model.Frequencies.Count == 0)
        {
            _snackbarService.Add("Debe agregar al menos una frecuencia al plan", Severity.Warning);
            return false;
        }

        // Validación adicional: al menos un plazo de pago activo
        if (_model.PaymentTerms.Count == 0)
        {
            _snackbarService.Add("Debe agregar al menos un plazo de pago al plan", Severity.Warning);
            return false;
        }

        return true;
    }

    private void OnChangeInitialEffectiveDate(DateTime? dateTime)
    {
        _model.InitialEffectiveDate = dateTime;
        _model.Active = IsPlanActive();
        StateHasChanged();
    }

    private void OnChangeFinalEffectiveDate(DateTime? dateTime)
    {
        _model.FinalEffectiveDate = dateTime;
        _model.Active = IsPlanActive();
        StateHasChanged();
    }

    private bool IsPlanActive()
    {
        if (!_model.InitialEffectiveDate.HasValue ||
            !_model.FinalEffectiveDate.HasValue)
            return false;

        var today = DateTime.Today;
        return today >= _model.InitialEffectiveDate.Value.Date &&
               today <= _model.FinalEffectiveDate.Value.Date;
    }

    #endregion

    #region UI Handlers para Eliminación
    private void RemoveFrequency(QuotationPlanFrequencyDto frequency)
    {
        if (frequency.IsDefault && _model.Frequencies.Count > 1)
        {
            _snackbarService.Add("No puede eliminar la frecuencia predeterminada", Severity.Warning);
            return;
        }

        _model.Frequencies.Remove(frequency);

        // Si eliminamos la default y hay otras, marcar la primera como default
        if (frequency.IsDefault && _model.Frequencies.Any())
        {
            _model.Frequencies[0].IsDefault = true;
        }

        UpdateAvailableFrequencies();
    }

    private void RemovePaymentTerm(QuotationPlanPaymentTermDto paymentTerm)
    {
        _model.PaymentTerms.Remove(paymentTerm);
        UpdateAvailablePaymentTerms();
    }
    #endregion

    #region CRUD Operations
    private async Task UpdateQuotationPlanAsync()
    {
        _isSubmitting = true;
        StateHasChanged();

        try
        {
            Result<int> result;
            if (QuotationPlanId != null)
            {

                result = await _commandMediator.SendAsync(new UpdateQuotationPlanCommand
                {
                    QuotationPlanId = QuotationPlanId.Value,
                    Model = _model
                });


            }
            else
            {
                result = await _commandMediator.SendAsync(new CreateQuotationPlanCommand
                {
                    Model = _model
                });
            }



            if (result.IsSuccess)
            {
                _snackbarService.Add(
                    result.SuccessMessage ?? "Plan de cotización actualizado exitosamente",
                    Severity.Success);

                await Task.Delay(1000); // Pequeño delay para mostrar el mensaje
                NavigateToList();
            }
            else
            {
                var errorMessage = result.Errors?.FirstOrDefault() ?? "Error al actualizar el plan de cotización";
                _snackbarService.Add(errorMessage, Severity.Error);
            }
        }
        catch (Exception ex)
        {
            _snackbarService.Add($"Error inesperado: {ex.Message}", Severity.Error);
        }
        finally
        {
            _isSubmitting = false;
            StateHasChanged();
        }
    }

    private void NavigateToList()
    {
        _navManager.NavigateTo("." + PageRoute.QuotationPlans);
    }
    #endregion

}
