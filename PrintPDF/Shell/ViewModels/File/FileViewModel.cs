using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using PrintPDF.Shell.Messages;
using System.IO;

namespace PrintPDF.Shell.ViewModels.File;

public partial class FileViewModel(IMessenger messenger) : ObservableObject, IFileViewModel
{
    #region Зависимости

    private readonly IMessenger _messenger = messenger;

    #endregion Зависимости

    #region Свойства модели представления

    [ObservableProperty]
    private FileInfo _fileInFolder = null!;

    private bool _isSelectedFile;

    public bool IsSelectedFile
    {
        get => _isSelectedFile;
        set
        {
            if (SetProperty(ref _isSelectedFile, value))
                _messenger.Send(new SelectFileMessage(value));
        }
    }

    #endregion Свойства модели представления
}