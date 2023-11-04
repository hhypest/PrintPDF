using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using PrintPDF.Messages;

namespace PrintPDF.ViewModels.Printer;

public partial class PrinterViewModel : ObservableObject, IPrinterViewModel
{
    #region Свойства модели представления

    [ObservableProperty]
    private string _printerName;

    private bool _checkedPrinter;

    public bool CheckedPrinter
    {
        get => _checkedPrinter; set
        {
            if (SetProperty(ref _checkedPrinter, value))
                StrongReferenceMessenger.Default.Send(new EnablePrintMessage(value));
        }
    }

    #endregion Свойства модели представления

    #region Конструктор

    public PrinterViewModel(string printerName)
    {
        _printerName = printerName;
    }

    #endregion Конструктор
}