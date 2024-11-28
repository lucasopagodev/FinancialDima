using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Web.Pages.Orders;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Components.Orders;

public partial class OrderActionComponent : ComponentBase
{
  #region Parameters

  [CascadingParameter]
  public DetailsPage Parent { get; set; } = null!;

  [Parameter, EditorRequired]
  public Order Order { get; set; } = null!;

  #endregion

  #region Services

  [Inject] public IDialogService DialogService { get; set; } = null!;
  [Inject] public IOrderHandler OrderHandler { get; set; } = null!;
  [Inject] public ISnackbar Snackbar { get; set; } = null!;

  #endregion

  #region Public Methods

  public async void OnCancelButtonClicked()
  {
    bool? result = await DialogService.ShowMessageBox("ATENÇÃO", "Deseja realmente cancelar o pedido?", yesText: "SIM", cancelText: "NÃO");

    if (result is not null && result == true)
      await CancelOrderAsync();
  }

  public async void OnPayButtonClickedAsync()
  {
    await PayOrderAsync();
  }

  public async void OnRefundButtonClicked()
  {
    bool? result = await DialogService.ShowMessageBox("ATENÇÃO", "Deseja realmente estornar este pedido?", yesText: "SIM", cancelText: "NÃO");

    if (result is not null && result == true)
      await RefundOrderAsync();
  }

  #endregion

  #region Private Methods

  private async Task CancelOrderAsync()
  {
    var request = new CancelOrderRequest
    {
      Id = Order.Id
    };

    var result = await OrderHandler.CancelAsync(request);
    if (result.IsSuccess)
      Parent.RefreshState(result.Data!);
    else
      Snackbar.Add(result.Message, Severity.Error);
  }

  private async Task PayOrderAsync()
  {
    await Task.Delay(1);
    Snackbar.Add("Pagamento não implementado", Severity.Error);
  }

  private async Task RefundOrderAsync()
  {
    var request = new RefundOrderRequest
    {
      Id = Order.Id
    };

    var result = await OrderHandler.RefundAsync(request);
    if (result.IsSuccess)
      Parent.RefreshState(result.Data!);
    else
      Snackbar.Add(result.Message, Severity.Error);
  }

  #endregion
}
