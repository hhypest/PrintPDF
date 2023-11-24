using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Win32;
using PrintPDF.Services.Factory;
using PrintPDF.Shell.Messages;
using PrintPDF.Shell.ViewModels.File;
using PrintPDF.Shell.ViewModels.Printer;
using RawNet.Printer;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
using System.IO;

namespace PrintPDF.Shell.ViewModels.Main;

public partial class MainViewModel : ObservableRecipient, IMainViewModel, IRecipient<SelectPrinterMessage>, IRecipient<SelectFileMessage>
{
    #region Зависимости

    private readonly IFactoryService _factoryService;

    #endregion Зависимости

    #region Свойства модели представления

    [ObservableProperty]
    private string _folderPath;

    [ObservableProperty]
    private ObservableCollection<IPrinterViewModel> _printerList;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SelectAllFileCommand), nameof(UnselectAllFileCommand), nameof(PrintFilesCommand))]
    private ObservableCollection<IFileViewModel> _fileList;

    #endregion Свойства модели представления

    #region Конструктор

    public MainViewModel(IMessenger messenger, IFactoryService factoryService) : base(messenger)
    {
        _factoryService = factoryService;
        _folderPath = string.Empty;
        _printerList = new(GetPrinterList().Select(p => _factoryService.GetPrinter(p)));
        _fileList = [];
    }

    #endregion Конструктор

    #region Регистрация сообщений

    public void RegisterMessage(bool state)
    {
        switch (state)
        {
            case true:
                Messenger.RegisterAll(this);
                break;

            case false:
                Messenger.UnregisterAll(this);
                break;
        }
    }

    #endregion Регистрация сообщений

    #region Обработка сообщений

    public void Receive(SelectPrinterMessage message)
    {
        PrintFilesCommand.NotifyCanExecuteChanged();
    }

    public void Receive(SelectFileMessage message)
    {
        PrintFilesCommand.NotifyCanExecuteChanged();
        SelectAllFileCommand.NotifyCanExecuteChanged();
        UnselectAllFileCommand.NotifyCanExecuteChanged();
    }

    #endregion Обработка сообщений

    #region Инициализация списка принтеров

    private static IEnumerable<string> GetPrinterList()
    {
        return PrinterSettings.InstalledPrinters.Cast<string>();
    }

    #endregion Инициализация списка принтеров

    #region Методы

    private static IFileViewModel SetIsSelect(IFileViewModel file, bool isSelect)
    {
        file.IsSelectedFile = isSelect;
        return file;
    }

    #endregion Методы

    #region Команды

    [RelayCommand]
    private void BrowseFolder()
    {
        var dialog = new OpenFolderDialog()
        {
            Title = "Выберите каталог с pdf-файлами",
            Multiselect = false
        };

        if (dialog.ShowDialog() == false)
            return;

        FolderPath = dialog.FolderName;
        var files = Directory.EnumerateFiles(FolderPath, "*.pdf");
        FileList = new(files.Select(file => _factoryService.GetFile(file)));
    }

    [RelayCommand(CanExecute = nameof(OnSelectAllFileChanged))]
    private void SelectAllFile()
    {
        FileList = new(FileList.Select(file => SetIsSelect(file, true)));
    }

    [RelayCommand(CanExecute = nameof(OnUnselectAllFileChanged))]
    private void UnselectAllFile()
    {
        FileList = new(FileList.Select(file => SetIsSelect(file, false)));
    }

    [RelayCommand(CanExecute = nameof(OnPrintFilesChanged))]
    private async Task PrintFiles()
    {
        var printerName = PrinterList.FirstOrDefault(p => p.IsSelectedPrinter)!.PrinterName;
        var files = FileList.Where(f => f.IsSelectedFile).Select(f => f.FileInFolder);

        await Task.Run(() =>
        {
            Parallel.ForEach(files, file =>
            {
                file.Refresh();
                if (!file.Exists)
                    return;

                var printer = new PrinterAdapter();
                printer.PrintRawFile(printerName, file);
            });
        });
    }

    #endregion Команды

    #region Предикаты

    private bool OnSelectAllFileChanged()
    {
        return FileList.Count > 0 && !FileList.Any(file => file.IsSelectedFile);
    }

    private bool OnUnselectAllFileChanged()
    {
        return FileList.Count > 0 && FileList.Any(file => file.IsSelectedFile);
    }

    private bool OnPrintFilesChanged()
    {
        return FileList.Count >= 1 && PrinterList.Any(printer => printer.IsSelectedPrinter) && FileList.Any(file => file.IsSelectedFile);
    }

    #endregion Предикаты
}