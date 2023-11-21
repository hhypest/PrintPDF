namespace PrintPDF.Shell.ViewModels.Printer;

public interface IPrinterViewModel
{
    public string PrinterName { get; set; }

    public bool IsSelectedPrinter { get; set; }
}