using System.Windows;

namespace PrintPDF.Shell.Views;

public partial class ShellView : Window, IShellView
{
    #region Конструктор

    public ShellView()
    {
        InitializeComponent();
    }

    #endregion Конструктор

    #region Реализация интерфейса

    public void SetDataContext<T>(T dataContext)
    {
        DataContext = dataContext;
    }

    public void ShowView()
    {
        Show();
    }

    #endregion Реализация интерфейса
}