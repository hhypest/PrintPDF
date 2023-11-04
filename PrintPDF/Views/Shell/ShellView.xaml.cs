using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace PrintPDF.Views.Shell;

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

    #region Поведение окна

    [LibraryImport("user32.dll", EntryPoint = "SendMessageA")]
    private static partial IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

    private void OnMoveView(object sender, MouseButtonEventArgs e)
    {
        var helper = new WindowInteropHelper(this);
        SendMessage(helper.Handle, 161, 2, 0);
    }

    #endregion Поведение окна
}