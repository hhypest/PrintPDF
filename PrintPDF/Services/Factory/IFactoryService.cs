using PrintPDF.Shell.ViewModels.File;
using PrintPDF.Shell.ViewModels.Printer;

namespace PrintPDF.Services.Factory;

public interface IFactoryService
{
    public IPrinterViewModel GetPrinter(string printerName);

    public IFileViewModel GetFile(string filePath);
}