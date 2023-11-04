using CommunityToolkit.Mvvm.Input;
using PrintPDF.ViewModels.File;
using PrintPDF.ViewModels.Printer;
using System.Collections.ObjectModel;

namespace PrintPDF.ViewModels.Main;

public interface IMainViewModel
{
    public string FolderFiles { get; set; }

    public bool IsSelectedEnable { get; set; }

    public bool IsPrintEnable { get; set; }

    public ObservableCollection<IPrinterViewModel> Printers { get; set; }

    public ObservableCollection<IFileViewModel> Files { get; set; }

    public IRelayCommand BrowseFolderCommand { get; }

    public IRelayCommand SelectedAllCommand { get; }

    public IRelayCommand UnselectedAllCommand { get; }

    public IRelayCommand PrintFilesCommand { get; }

    public IRelayCommand ExitAppCommand { get; }
}