using Microsoft.JSInterop;

namespace ShopFinance.WebApp.Services.JsInterop;

public class HistoryGo
{
    private readonly IJSRuntime _jsRuntime;

    public HistoryGo(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<ValueTask> GoBack(int  value=-1)
    {
        var jsmodule = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "/js/historygo.js").ConfigureAwait(false);
        return jsmodule.InvokeVoidAsync(JSInteropConstants.HistoryGo, value);
    }
}
public static class JSInteropConstants
{
    public const string ExternalLogin = "externalLogin";
    public const string ExternalLogout = "externalLogout";
    public const string ShowOpenSeadragon = "showOpenSeadragon";
    public const string ClearInput = "clearInput";
    public const string PreviewImage = "previewImage";
    public const string CreateOrgChart = "createOrgChart";
    public const string GetTimezoneOffset = "getTimezoneOffset";
    public const string GetTimezoneOffsetByTimeZone = "getTimezoneOffsetByTimeZone";
    public const string HistoryGo = "historyGo";
}
