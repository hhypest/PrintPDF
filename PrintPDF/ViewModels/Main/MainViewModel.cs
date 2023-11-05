using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using Ookii.Dialogs.Wpf;
using PrintPDF.Messages;
using PrintPDF.ViewModels.File;
using PrintPDF.ViewModels.Printer;
using RawNet.Printer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows;

namespace PrintPDF.ViewModels.Main;

public partial class MainViewModel : ObservableRecipient, IMainViewModel, IRecipient<EnablePrintMessage>, IRecipient<EnableSelectedMessage>
{
    #region Зависимости

    private readonly ILogger<MainViewModel> _logger;

    #endregion Зависимости

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

    public MainViewModel(ILoggerFactory logger)
    {
        _folderFiles = string.Empty;
        _printers = new(GetPrinterList().Select(name => new PrinterViewModel(name)));
        _files = new();
        _logger = logger.CreateLogger<MainViewModel>();
        _logger.LogInformation("Инициализация {MainViewModel}", nameof(MainViewModel));
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
        _logger.LogInformation("Подписка на сообщения {MainViewModel}", nameof(MainViewModel));
    }

    protected override void OnDeactivated()
    {
        StrongReferenceMessenger.Default.UnregisterAll(this);
        _logger.LogInformation("Отписка от сообщений {MainViewModel}", nameof(MainViewModel));
    }

    #endregion Подписка на сообщения

    #region Обработка сообщений

    public void Receive(EnablePrintMessage message)
    {
        IsPrintEnable = Printers.Any(printer => printer.CheckedPrinter);
        _logger.LogInformation("Получено сообщение от {PrinterViewModel}", typeof(PrinterViewModel));
    }

    public void Receive(EnableSelectedMessage message)
    {
        IsSelectedEnable = Files.Any(file => file.CheckedFile);
        _logger.LogInformation("Получено сообщение от {FileViewModel}", typeof(FileViewModel));
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
        _logger.LogInformation("Каталог выбран - {Folder}", FolderFiles);
    }

    [RelayCommand(CanExecute = nameof(OnCanExecuteSelectedAll))]
    private void OnSelectedAll()
    {
        var list = Files.Select(file => new FileViewModel(file.FileInFolder) { CheckedFile = true });
        Files = new(list);
        IsSelectedEnable = true;
        _logger.LogInformation("Файлы выбраны {OnSelectedAll}", nameof(OnSelectedAll));
    }

    [RelayCommand(CanExecute = nameof(OnCanExecuteUnselectedAll))]
    private void OnUnselectedAll()
    {
        var list = Files.Select(file => new FileViewModel(file.FileInFolder) { CheckedFile = false });
        Files = new(list);
        IsSelectedEnable = false;
        _logger.LogInformation("Файлы не выбраны {OnUnselectedAll}", nameof(OnUnselectedAll));
    }

    [RelayCommand(CanExecute = nameof(OnCanExecutePrintFiles))]
    private void OnPrintFiles()
    {
        var printerName = Printers.FirstOrDefault(printer => printer.CheckedPrinter)!.PrinterName;
        var printer = new PrinterAdapter();

        try
        {
            foreach (var file in Files.Where(pdf => pdf.CheckedFile).Select(f => f.FileInFolder))
            {
                file.Refresh();
                if (!file.Exists)
                {
                    _logger.LogWarning("Файла не существует");
                    continue;
                }

                printer.PrintRawFile(printerName, file);
            }

            _logger.LogInformation("Печать файлов завершена на принтере {Name}", printerName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка печати!");
        }
    }

    [RelayCommand]
    private static void OnExitApp()
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