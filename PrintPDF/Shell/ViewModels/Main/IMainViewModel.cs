using CommunityToolkit.Mvvm.Input;
using PrintPDF.Shell.ViewModels.File;
using PrintPDF.Shell.ViewModels.Printer;
using System.Collections.ObjectModel;

namespace PrintPDF.Shell.ViewModels.Main;

public interface IMainViewModel
{
    public string FolderPath { get; set; }

    public ObservableCollection<IPrinterViewModel> PrinterList { get; set; }

    public ObservableCollection<IFileViewModel> FileList { get; set; }

    public IRelayCommand BrowseFolderCommand { get; }

    public IRelayCommand SelectAllFileCommand { get; }

    public IRelayCommand UnselectAllFileCommand { get; }

    public IAsyncRelayCommand PrintFilesCommand { get; }

    public void RegisterMessage(bool state);
}