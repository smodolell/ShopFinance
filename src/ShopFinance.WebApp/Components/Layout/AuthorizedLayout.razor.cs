using Microsoft.AspNetCore.Components;

namespace ShopFinance.WebApp.Components.Layout;

public partial class AuthorizedLayout
{
    [Parameter] public RenderFragment? Child { get; set; }
}
