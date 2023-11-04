using System.IO;

namespace PrintPDF.ViewModels.File;

public interface IFileViewModel
{
    public FileInfo FileInFolder { get; set; }

    public bool CheckedFile { get; set; }
}