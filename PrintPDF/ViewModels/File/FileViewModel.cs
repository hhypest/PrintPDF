using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using PrintPDF.Messages;
using System.IO;

namespace PrintPDF.ViewModels.File;

public partial class FileViewModel : ObservableObject, IFileViewModel
{
    #region Свойства модели представления

    [ObservableProperty]
    private FileInfo _fileInFolder;

    private bool _checkedFile;

    public bool CheckedFile
    {
        get => _checkedFile; set
        {
            if (SetProperty(ref _checkedFile, value))
                StrongReferenceMessenger.Default.Send(new EnableSelectedMessage(value));
        }
    }

    #endregion Свойства модели представления

    #region Конструктор

    public FileViewModel(FileInfo file)
    {
        _fileInFolder = file;
    }

    #endregion Конструктор
}