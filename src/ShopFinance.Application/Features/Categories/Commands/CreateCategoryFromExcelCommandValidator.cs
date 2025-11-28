using Microsoft.AspNetCore.Http;

namespace ShopFinance.Application.Features.Categories.Commands;

public class CreateCategoryFromExcelCommandValidator : AbstractValidator<CreateCategoryFromExcelCommand>
{
    public CreateCategoryFromExcelCommandValidator()
    {
        RuleFor(x => x.File)
            .NotNull().WithMessage("El archivo es requerido")
            .Must(BeValidExcelFile).WithMessage("El archivo debe ser un Excel válido (.xlsx, .xls)")
            .Must(BeValidSize).WithMessage("El archivo no puede exceder los 5MB");

        RuleFor(x => x.File.FileName)
            .NotEmpty().WithMessage("El nombre del archivo es requerido")
            .Must(BeValidExtension).WithMessage("Solo se permiten archivos Excel (.xlsx, .xls)");
    }

    private bool BeValidExcelFile(IFormFile file)
    {
        if (file == null) return false;

        var allowedExtensions = new[] { ".xlsx", ".xls" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return allowedExtensions.Contains(extension);
    }

    private bool BeValidExtension(string fileName)
    {
        if (string.IsNullOrEmpty(fileName)) return false;

        var allowedExtensions = new[] { ".xlsx", ".xls" };
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return allowedExtensions.Contains(extension);
    }

    private bool BeValidSize(IFormFile file)
    {
        if (file == null) return false;
        return file.Length <= 5 * 1024 * 1024; // 5MB
    }
}