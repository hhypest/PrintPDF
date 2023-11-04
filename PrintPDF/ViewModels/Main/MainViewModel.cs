using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Ookii.Dialogs.Wpf;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Controls.Wpf;
using PrintPDF.Messages;
using PrintPDF.ViewModels.File;
using PrintPDF.ViewModels.Printer;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows;

namespace PrintPDF.ViewModels.Main;

public partial class MainViewModel : ObservableRecipient, IMainViewModel, IRecipient<EnablePrintMessage>, IRecipient<EnableSelectedMessage>
{
    #region Свойства модели представления

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(PrintFilesCommand))]
    private string _folderFiles;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SelectedAllCommand), nameof(UnselectedAllCommand), nameof(PrintFilesCommand))]
    private bool _isSelectedEnable;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(PrintFilesCommand))]
    private bool _isPrintEnable;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(PrintFilesCommand))]
    private ObservableCollection<IPrinterViewModel> _printers;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SelectedAllCommand), nameof(UnselectedAllCommand), nameof(PrintFilesCommand))]
    private ObservableCollection<IFileViewModel> _files;

    #endregion Свойства модели представления

    #region Конструктор

    public MainViewModel()
    {
        _folderFiles = string.Empty;
        _printers = new(GetPrinterList().Select(name => new PrinterViewModel(name)));
        _files = new();
    }

    #endregion Конструктор

    #region Функции

    private static IEnumerable<string> GetPrinterList()
    {
        return PrinterSettings.InstalledPrinters.Cast<string>();
    }

    private static IEnumerable<FileInfo> GetFilesList(string pathFolder)
    {
        return Directory.GetFiles(pathFolder, "*.pdf").Select(file => new FileInfo(file));
    }

    #endregion Функции

    #region Подписка на сообщения

    protected override void OnActivated()
    {
        StrongReferenceMessenger.Default.RegisterAll(this);
    }

    protected override void OnDeactivated()
    {
        StrongReferenceMessenger.Default.UnregisterAll(this);
    }

    #endregion Подписка на сообщения

    #region Обработка сообщений

    public void Receive(EnablePrintMessage message)
    {
        IsPrintEnable = Printers.Any(printer => printer.CheckedPrinter);
    }

    public void Receive(EnableSelectedMessage message)
    {
        IsSelectedEnable = Files.Any(file => file.CheckedFile);
    }

    #endregion Обработка сообщений

    #region Команды

    [RelayCommand]
    private void OnBrowseFolder()
    {
        var folderDialog = new VistaFolderBrowserDialog()
        {
            Description = "Выберите каталог",
            UseDescriptionForTitle = true,
            Multiselect = false,
            ShowNewFolderButton = false
        };

        if (folderDialog.ShowDialog() == false)
            return;

        FolderFiles = folderDialog.SelectedPath;
        Files = new(GetFilesList(FolderFiles).Select(file => new FileViewModel(file)));
        IsSelectedEnable = false;
    }

    [RelayCommand(CanExecute = nameof(OnCanExecuteSelectedAll))]
    private void OnSelectedAll()
    {
        var list = Files.Select(file => new FileViewModel(file.FileInFolder) { CheckedFile = true });
        Files = new(list);
        IsSelectedEnable = true;
    }

    [RelayCommand(CanExecute = nameof(OnCanExecuteUnselectedAll))]
    private void OnUnselectedAll()
    {
        var list = Files.Select(file => new FileViewModel(file.FileInFolder) { CheckedFile = false });
        Files = new(list);
        IsSelectedEnable = false;
    }

    [RelayCommand(CanExecute = nameof(OnCanExecutePrintFiles))]
    private void OnPrintFiles()
    {
        var printerName = Printers.FirstOrDefault(printer => printer.CheckedPrinter)!.PrinterName;
        var printerSettings = new PrinterSettings() { PrinterName = printerName };

        foreach (var file in Files.Where(pdfFile => pdfFile.CheckedFile))
        {
            using var document = PdfDocument.Load(file.FileInFolder.FullName);
            using var printDocument = new PdfPrintDocument(document)
            {
                PrinterSettings = printerSettings
            };

            printDocument.Print();
        }
    }

    [RelayCommand]
    private void OnExitApp()
    {
        Application.Current.Shutdown();
    }

    #endregion Команды

    #region Предикаты

    private bool OnCanExecuteSelectedAll()
    {
        if (Files.Count < 1)
            return false;

        return !IsSelectedEnable;
    }

    private bool OnCanExecuteUnselectedAll()
    {
        return IsSelectedEnable;
    }

    private bool OnCanExecutePrintFiles()
    {
        if (!Files.Any(file => file.CheckedFile))
            return false;

        return IsPrintEnable && IsSelectedEnable;
    }

    #endregion Предикаты
}