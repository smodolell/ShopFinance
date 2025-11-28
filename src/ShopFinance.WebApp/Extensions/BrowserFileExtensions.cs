using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace ShopFinance.WebApp.Extensions;


public static class BrowserFileExtensions
{
    public static async Task<IFormFile> ToFormFileAsync(this IBrowserFile browserFile)
    {
        // Crear un stream temporal
        var memoryStream = new MemoryStream();
        await browserFile.OpenReadStream(maxAllowedSize: 1024 * 1024 * 15) // hasta 15 MB por ejemplo
                         .CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        // Crear el FormFile (emula un archivo subido vía HTTP)
        var formFile = new FormFile(memoryStream, 0, memoryStream.Length, browserFile.Name, browserFile.Name)
        {
            Headers = new HeaderDictionary(),
            ContentType = browserFile.ContentType
        };

        return formFile;
    }
}

