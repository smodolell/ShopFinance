using ShopFinance.Application.Common.Models;

namespace ShopFinance.Application.Common.Interfaces;

public interface IUploadService
{
    Task<string> UploadAsync(UploadRequest request);
    Task RemoveAsync(string filename);
}