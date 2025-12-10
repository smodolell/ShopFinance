using ShopFinance.Application.Common.Interfaces;
using ShopFinance.Application.Common.Models;
using ShopFinance.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ShopFinance.Application.Common.Services;

class SelectListService : ISelectListService
{
    private readonly IUnitOfWork _unitOfWork;

    public SelectListService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<SelectListItemDto>> Categories()
    {
        var categories = await _unitOfWork.Categories.GetListAsync();

        return categories.Select(s => new SelectListItemDto
        {
            Text = s.Name,
            Value = s.Id.ToString()
        }).ToList();
    }



    public async Task<List<SelectListItemDto>> Warehouses()
    {
        var categories = await _unitOfWork.Warehouses.GetListAsync();

        return categories.Select(s => new SelectListItemDto
        {
            Text = s.Name,
            Value = s.Id.ToString()
        }).ToList();
    }

    public Task<List<SelectListItemDto>> ProductState()
    {
        return GetSelectListsByEnum<ProductState>();
    }

    public async Task<List<SelectListItemDto>> Phases()
    {
        var data = await _unitOfWork.Phases.GetListAsync();

        return data.Select(s => new SelectListItemDto
        {
            Text = s.PhaseName,
            Value = s.Id.ToString()
        }).ToList();
    }

    #region Private

    private Task<List<SelectListItemDto>> GetSelectListsByEnum<T>() where T : Enum
    {
        var result = Enum.GetValues(typeof(T))
            .Cast<T>()
            .Select(e => new SelectListItemDto
            {
                Text = GetEnumDescription(e),
                Value = Convert.ToInt32(e).ToString(),
                ValueDecimal = Convert.ToDecimal(e),
                Selected = false
            })
            .ToList();

        return Task.FromResult(result);
    }

    private string GetEnumDescription<T>(T enumValue) where T : Enum
    {
        var field = enumValue.GetType().GetField(enumValue.ToString());
        var attribute = field?.GetCustomAttribute<DisplayAttribute>();

        return attribute?.Name ?? enumValue.ToString();
    }

  





    #endregion



}