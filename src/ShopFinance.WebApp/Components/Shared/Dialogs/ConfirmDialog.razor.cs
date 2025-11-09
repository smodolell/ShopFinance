using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ShopFinance.WebApp.Components.Shared.Dialogs;

public partial class ConfirmDialog
{
    [CascadingParameter]
    IMudDialogInstance? MudDialog { get; set; }

    [Parameter]
    public string ContentText { get; set; } = "Confirmar?";
    [Parameter] 
    public string ButtonText { get; set; } = "Confirmar";

    private bool _isLoading = false;

    [Parameter] public EventCallback<CommonDialogEventArgs> ConfirmCallBack { get; set; }
    void Submit() => MudDialog?.Close(DialogResult.Ok(true));

    private async Task Confirm()
    {
        _isLoading = true;
        await ConfirmCallBack.InvokeAsync(new CommonDialogEventArgs());
        _isLoading = false;
        MudDialog?.Close(DialogResult.Ok(true));
    }
    void Cancel() => MudDialog?.Cancel();
}
