using ClosedXML.Excel;
using ShopFinance.Application.Features.Categories.Commands;
using ShopFinance.Domain.Entities;

internal class CreateCategoryFromExcelCommandHandler
    : ICommandHandler<CreateCategoryFromExcelCommand, Result<CreateCategoryFromExcelCommandResult>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateCategoryFromExcelCommand> _validator;

    public CreateCategoryFromExcelCommandHandler(
        IUnitOfWork unitOfWork,
        IValidator<CreateCategoryFromExcelCommand> validator)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<CreateCategoryFromExcelCommandResult>> HandleAsync(
        CreateCategoryFromExcelCommand message,
        CancellationToken cancellationToken = default)
    {
        var response = new CreateCategoryFromExcelCommandResult();

        try
        {
            // Validar el comando primero
            var validationResult = await _validator.ValidateAsync(message, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Invalid(validationResult.AsErrors());
            }

            using var stream = new MemoryStream();
            await message.File.CopyToAsync(stream, cancellationToken);

            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheet(1); // Primera hoja
            var rows = worksheet.RangeUsed()!.RowsUsed().Skip(1); // Saltar encabezados

            var categoriesToAdd = new List<Category>();
            var rowNumber = 2;
            var processedCodes = new HashSet<string>(); // Para detectar duplicados en el mismo archivo

            foreach (var row in rows)
            {
                try
                {
                    var code = row.Cell(1).GetValue<string>()?.Trim();
                    var name = row.Cell(2).GetValue<string>()?.Trim();
                    var description = row.Cell(3).GetValue<string>()?.Trim();

                    // Validaciones básicas
                    if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(name))
                    {
                        response.Errors.Add($"Fila {rowNumber}: Código y Nombre son obligatorios");
                        response.ErrorCount++;
                        rowNumber++;
                        continue;
                    }

                    // Validar longitud
                    if (code.Length > 50)
                    {
                        response.Errors.Add($"Fila {rowNumber}: El código no puede tener más de 50 caracteres");
                        response.ErrorCount++;
                        rowNumber++;
                        continue;
                    }

                    if (name.Length > 200)
                    {
                        response.Errors.Add($"Fila {rowNumber}: El nombre no puede tener más de 200 caracteres");
                        response.ErrorCount++;
                        rowNumber++;
                        continue;
                    }

                    // Verificar duplicados en el mismo archivo
                    if (processedCodes.Contains(code))
                    {
                        response.Errors.Add($"Fila {rowNumber}: El código '{code}' está duplicado en el archivo");
                        response.ErrorCount++;
                        rowNumber++;
                        continue;
                    }

                    // Verificar si ya existe en la base de datos
                    var exists = await _unitOfWork.Categories
                        .AnyAsync(c => c.Code == code, cancellationToken);

                    if (exists)
                    {
                        response.Errors.Add($"Fila {rowNumber}: El código '{code}' ya existe en el sistema");
                        response.ErrorCount++;
                        rowNumber++;
                        continue;
                    }

                    var category = new Category()
                    {
                        Id = 0,
                        Code = code,
                        Name = name
                    };

                    categoriesToAdd.Add(category);
                    processedCodes.Add(code);
                    response.SuccessCount++;
                }
                catch (Exception ex)
                {
                    response.Errors.Add($"Fila {rowNumber}: Error - {ex.Message}");
                    response.ErrorCount++;
                }

                rowNumber++;
            }

            response.TotalRecords = rows.Count();

            if (categoriesToAdd.Any())
            {
                foreach (var category in categoriesToAdd)
                {
                    await _unitOfWork.Categories.AddAsync(category, cancellationToken);
                }
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            // Log the exception here if you have logging
            return Result.Error($"Error al procesar el archivo: {ex.Message}");
        }
    }
}