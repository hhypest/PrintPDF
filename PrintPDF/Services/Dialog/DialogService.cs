using Microsoft.Extensions.DependencyInjection;
using PrintPDF.Views.Dialog;
using System;
using System.Linq;
using System.Windows;

namespace PrintPDF.Services.Dialog;

public sealed class DialogService : IDialogService
{
    #region Зависимости
    private readonly IServiceProvider _service;
    #endregion

    #region Определение активного окна
    private static Window? ActiveWindow => Application.Current.Windows.Cast<Window>().FirstOrDefault(w => w.IsActive);

    private static Window? FocusedWindow => Application.Current.Windows.Cast<Window>().FirstOrDefault(w => w.IsFocused);

    private static Window? CurrentWindow => FocusedWindow ?? ActiveWindow;
    #endregion

    public DialogService(IServiceProvider service)
    {
        _service = service;
    }

    public void SendMessage(string message)
    {
        var dialog = ActivatorUtilities.GetServiceOrCreateInstance<IDialogView>(_service);
        dialog.SetOwner(CurrentWindow!);
        dialog.Show(message);
    }
}