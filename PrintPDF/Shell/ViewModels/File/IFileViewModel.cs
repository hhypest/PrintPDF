using System.IO;

namespace PrintPDF.Shell.ViewModels.File;

public interface IFileViewModel
{
    public FileInfo FileInFolder { get; set; }

    public bool IsSelectedFile { get; set; }
}