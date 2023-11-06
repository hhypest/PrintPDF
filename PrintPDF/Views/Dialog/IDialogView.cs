using System.Windows;

namespace PrintPDF.Views.Dialog;

public interface IDialogView
{
    public void Show(string message);

    public void SetOwner(Window owner);
}