namespace PrintPDF.ViewModels.Printer;

public interface IPrinterViewModel
{
    public string PrinterName { get; set; }

    public bool CheckedPrinter { get; set; }
}