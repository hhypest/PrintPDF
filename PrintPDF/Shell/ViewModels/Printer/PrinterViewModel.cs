using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using PrintPDF.Shell.Messages;

namespace PrintPDF.Shell.ViewModels.Printer;

public partial class PrinterViewModel(IMessenger messenger) : ObservableObject, IPrinterViewModel
{
    #region Зависимости

    private readonly IMessenger _messenger = messenger;

    #endregion Зависимости

    #region Свойства модели представления

    [ObservableProperty]
    private string _printerName = string.Empty;

    private bool _isSelectedPrinter;

    public bool IsSelectedPrinter
    {
        get => _isSelectedPrinter;
        set
        {
            if (SetProperty(ref _isSelectedPrinter, value))
                _messenger.Send(new SelectPrinterMessage(value));
        }
    }

    #endregion Свойства модели представления
}