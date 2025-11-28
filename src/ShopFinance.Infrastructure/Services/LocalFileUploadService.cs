using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ShopFinance.Application.Common.Interfaces;
using ShopFinance.Application.Common.Models;
using ShopFinance.Domain.Enums;
using System.IO;
namespace ShopFinance.Infrastructure.Services;

public class LocalFileUploadService : IUploadService
{
    private readonly string _baseUploadPath;
    private readonly string _baseUrl;
    private readonly ILogger<LocalFileUploadService> _logger;

    public LocalFileUploadService(
        IConfiguration configuration,
        ILogger<LocalFileUploadService> logger)
    {
        _baseUploadPath = configuration["FileUpload:UploadPath"] ?? "wwwroot/uploads";
        _baseUrl = configuration["FileUpload:BaseUrl"] ?? "/uploads";
        _logger = logger;

        // Crear directorio base si no existe
        EnsureUploadDirectoryExists();
    }

    private void EnsureUploadDirectoryExists()
    {
        try
        {
            if (!Directory.Exists(_baseUploadPath))
            {
                Directory.CreateDirectory(_baseUploadPath);
                _logger.LogInformation("Directorio base de uploads creado: {UploadPath}", _baseUploadPath);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear el directorio base de uploads: {UploadPath}", _baseUploadPath);
            throw;
        }
    }

    public async Task<string> UploadAsync(UploadRequest request)
    {
        try
        {
            // Validaciones
            ValidateUploadRequest(request);

            // Determinar la ruta de upload basada en el tipo
            var uploadPath = GetUploadPath(request.UploadType, request.Folder);

            // Crear directorio específico si no existe
            EnsureSpecificUploadDirectoryExists(uploadPath);

            // Obtener nombre de archivo seguro y único
            var fileName = GetSafeFileName(request);
            var filePath = Path.Combine(uploadPath, fileName);

            // Verificar si ya existe y manejar overwrite
            await HandleExistingFile(filePath, request.Overwrite);

            // Guardar archivo
            await SaveFileAsync(filePath, request.Data);

            // Generar URL de acceso
            var fileUrl = GenerateFileUrl(request.UploadType, request.Folder, fileName);

            _logger.LogInformation("Archivo subido exitosamente: {FileName} -> {FileUrl}", request.FileName, fileUrl);
            return fileUrl;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al subir el archivo: {FileName}", request?.FileName);
            throw;
        }
    }

    public async Task RemoveAsync(string filename)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentException("El nombre del archivo es requerido");

            // Convertir URL a ruta física
            var physicalPath = ConvertUrlToPhysicalPath(filename);

            _logger.LogInformation("Eliminando archivo: {PhysicalPath}", physicalPath);

            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
                _logger.LogInformation("Archivo eliminado: {PhysicalPath}", physicalPath);
            }
            else
            {
                _logger.LogWarning("Archivo no encontrado para eliminar: {PhysicalPath}", physicalPath);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar el archivo: {FileName}", filename);
            throw;
        }
    }

    private void ValidateUploadRequest(UploadRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (string.IsNullOrWhiteSpace(request.FileName))
            throw new ArgumentException("El nombre del archivo es requerido");

        if (request.Data == null || request.Data.Length == 0)
            throw new ArgumentException("Los datos del archivo están vacíos");

        // Validar tamaño máximo (ejemplo: 10MB)
        if (request.Data.Length > 10 * 1024 * 1024)
            throw new ArgumentException("El archivo es demasiado grande. Tamaño máximo: 10MB");
    }

    private string GetUploadPath(UploadType uploadType, string? folder)
    {
        var typeFolder = uploadType.ToString().ToLower();
        var basePath = Path.Combine(_baseUploadPath, typeFolder);

        if (!string.IsNullOrEmpty(folder))
        {
            // Sanitizar el nombre de la carpeta
            var safeFolder = SanitizeFileName(folder);
            basePath = Path.Combine(basePath, safeFolder);
        }

        return basePath;
    }

    private void EnsureSpecificUploadDirectoryExists(string uploadPath)
    {
        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
            _logger.LogInformation("Directorio específico creado: {UploadPath}", uploadPath);
        }
    }

    private string GetSafeFileName(UploadRequest request)
    {
        // Obtener extensión del archivo
        var extension = request.Extension ?? Path.GetExtension(request.FileName) ?? ".bin";
        if (!extension.StartsWith('.'))
            extension = "." + extension;

        // Sanitizar nombre del archivo
        var fileNameWithoutExtension = SanitizeFileName(Path.GetFileNameWithoutExtension(request.FileName));

        // Generar nombre único
        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        var uniqueId = Guid.NewGuid().ToString("N").Substring(0, 8);

        return $"{fileNameWithoutExtension}_{timestamp}_{uniqueId}{extension}";
    }

    private async Task HandleExistingFile(string filePath, bool overwrite)
    {
        if (File.Exists(filePath))
        {
            if (overwrite)
            {
                _logger.LogWarning("Archivo existente será sobrescrito: {FilePath}", filePath);
                File.Delete(filePath);
            }
            else
            {
                throw new InvalidOperationException($"El archivo ya existe y overwrite es false: {Path.GetFileName(filePath)}");
            }
        }
    }

    private async Task SaveFileAsync(string filePath, byte[] data)
    {
        await File.WriteAllBytesAsync(filePath, data);
    }

    private string GenerateFileUrl(UploadType uploadType, string? folder, string fileName)
    {
        var typeFolder = uploadType.ToString().ToLower();
        var urlPath = string.IsNullOrEmpty(folder)
            ? $"{_baseUrl.TrimEnd('/')}/{typeFolder}/{fileName}"
            : $"{_baseUrl.TrimEnd('/')}/{typeFolder}/{folder}/{fileName}";

        return urlPath.Replace("\\", "/");
    }

    private string ConvertUrlToPhysicalPath(string fileUrl)
    {
        // Remover la base URL para obtener la ruta relativa
        var relativePath = fileUrl.Replace(_baseUrl, "").TrimStart('/');

        // Reconstruir la ruta física
        var physicalPath = Path.Combine(_baseUploadPath, relativePath.Replace("/", "\\"));

        return physicalPath;
    }

    private string SanitizeFileName(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return "file";

        var invalidChars = Path.GetInvalidFileNameChars();
        var sanitized = new string(fileName
            .Where(ch => !invalidChars.Contains(ch))
            .ToArray());

        // Limitar longitud
        if (sanitized.Length > 50)
        {
            sanitized = sanitized.Substring(0, 50);
        }

        return string.IsNullOrWhiteSpace(sanitized) ? "file" : sanitized;
    }

    // Método adicional para obtener la ruta física de un archivo
    public string GetPhysicalPath(string fileUrl)
    {
        return ConvertUrlToPhysicalPath(fileUrl);
    }

    // Método adicional para verificar si un archivo existe
    public Task<bool> FileExistsAsync(string fileUrl)
    {
        var physicalPath = ConvertUrlToPhysicalPath(fileUrl);
        return Task.FromResult(File.Exists(physicalPath));
    }
}