//using ShopFinance.Application.Common.Interfaces;

//namespace ShopFinance.Infrastructure.Services;

//public class InMemoryTokenStore : ITokenStore
//{
//    private string? _token;

//    public Task SetAsync(string token)
//    {
//        _token = token;
//        return Task.CompletedTask;
//    }

//    public Task<string?> GetAsync()
//        => Task.FromResult(_token);

//    public Task RemoveAsync()
//    {
//        _token = null;
//        return Task.CompletedTask;
//    }


//}
////public class LocalStorageTokenStore : ITokenStore
////{
////    private readonly ILocalStorageService _localStorage;
////    private const string TokenKey = "authToken";

////    public LocalStorageTokenStore(ILocalStorageService localStorage)
////    {
////        _localStorage = localStorage;
////    }

////    public async Task SetAsync(string token)
////    {
////        await _localStorage.SetItemAsync(TokenKey, token);
////    }

////    public async Task<string?> GetAsync()
////    {
////        return await _localStorage.GetItemAsync<string>(TokenKey);
////    }

////    public async Task RemoveAsync()
////    {
////        await _localStorage.RemoveItemAsync(TokenKey);
////    }
////}