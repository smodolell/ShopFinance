using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ShopFinance.WebApp.Components.Shared.Dialogs;

public static class CommonDialog
{
    public delegate Task DialogMethodDelegate(CommonDialogEventArgs args);

    public static async Task<bool> ShowDeleteDialog(this IDialogService dialogService,
        string? title = null, string? confirmButtonText = null, DialogMethodDelegate? confirmCallBackMethod = null)
    {
        var confirmCallBack = new EventCallback<CommonDialogEventArgs>(null,
            async (CommonDialogEventArgs e) =>
            {
                if (confirmCallBackMethod != null)
                {
                    await confirmCallBackMethod(e);
                };
            });
        var parameters = new DialogParameters
            {
                {"Title", title},
                {"ConfirmButtonText", confirmButtonText},
                {"ConfirmCallBack", confirmCallBack }
            };
        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraLarge, NoHeader = true, };
        var dialog = await dialogService.ShowAsync<CommonDeleteDialog>(string.Empty, parameters, options);
        var result = await dialog.Result;
        return !result!.Canceled;
    }

    //public static async Task<bool> ShowConfirmUserPasswodDialog(this IDialogService dialogService,
    //    DialogMethodDelegate? confirmCallBackMethod = null)
    //{
    //    var confirmCallBack = new EventCallback<CommonDialogEventArgs>(null,
    //        async (CommonDialogEventArgs e) =>
    //        {
    //            if (confirmCallBackMethod != null)
    //            {
    //                await confirmCallBackMethod(e);
    //            };
    //        });
    //    var parameters = new DialogParameters
    //    {
    //        {"ConfirmCallBack", confirmCallBack }
    //    };
    //    var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraLarge, NoHeader = true, };
    //    var dialog = await dialogService.ShowAsync<ConfirmUserPasswordDialog>(string.Empty, parameters, options);
    //    var result = await dialog.Result;
    //    return !result!.Canceled;
    //}

    public static async Task<bool> ShowConfirmDialog(this IDialogService dialogService,
       string? contentText = null,
       string? buttonText = null,
       DialogMethodDelegate? confirmCallBackMethod = null)
    {
        var confirmCallBack = new EventCallback<CommonDialogEventArgs>(null,
            async (CommonDialogEventArgs e) =>
            {
                if (confirmCallBackMethod != null)
                {
                    await confirmCallBackMethod(e);
                };
            });
        var parameters = new DialogParameters
            {
                {"ContentText", contentText },
                {"ButtonText", buttonText },
                {"ConfirmCallBack", confirmCallBack }
            };
        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, NoHeader = true, };
        var dialog = await dialogService.ShowAsync<ConfirmDialog>(string.Empty, parameters, options);
        var result = await dialog.Result;
        return !result!.Canceled;
    }
}