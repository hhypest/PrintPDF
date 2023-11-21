using Microsoft.Xaml.Behaviors;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;

namespace PrintPDF.Shell.Behaviors;

public partial class HeadBehavior : Behavior<Border>
{
    private static Window? ActiveWindow => Application.Current.Windows.Cast<Window>().FirstOrDefault(w => w.IsActive);
    private static Window? FocusedWindow => Application.Current.Windows.Cast<Window>().FirstOrDefault(w => w.IsFocused);
    private static Window? CurrentWindow => FocusedWindow ?? ActiveWindow;

    private const int _wMsg = 161;
    private const int _wParam = 2;
    private const int _lParam = 0;

    protected override void OnAttached()
    {
        AssociatedObject.MouseLeftButtonDown += OnDragMove;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.MouseLeftButtonDown -= OnDragMove;
    }

    [LibraryImport("user32.dll", EntryPoint = "SendMessageA")]
    private static partial IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

    private void OnDragMove(object sender, MouseButtonEventArgs e)
    {
        var helper = new WindowInteropHelper(CurrentWindow);
        SendMessage(helper.Handle, _wMsg, _wParam, _lParam);
    }
}