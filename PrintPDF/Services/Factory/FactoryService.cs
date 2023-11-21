using Microsoft.Extensions.DependencyInjection;
using PrintPDF.Shell.ViewModels.File;
using PrintPDF.Shell.ViewModels.Printer;
using System.IO;

namespace PrintPDF.Services.Factory;

public sealed class FactoryService(IServiceProvider services) : IFactoryService
{
    #region Зависимости

    private readonly IServiceProvider _services = services;

    #endregion Зависимости

    #region Реализация интерфейса

    public IPrinterViewModel GetPrinter(string printerName)
    {
        var printer = ActivatorUtilities.GetServiceOrCreateInstance<IPrinterViewModel>(_services);
        printer.PrinterName = printerName;
        return printer;
    }

    public IFileViewModel GetFile(string filePath)
    {
        var file = ActivatorUtilities.GetServiceOrCreateInstance<IFileViewModel>(_services);
        file.FileInFolder = new FileInfo(filePath);
        return file;
    }

    #endregion Реализация интерфейса
}