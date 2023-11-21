using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;

namespace PrintPDF.Shell.Behaviors;

public sealed class CloseBehavior : Behavior<Button>
{
    protected override void OnAttached()
    {
        AssociatedObject.Click += OnCloseClicked;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.Click -= OnCloseClicked;
    }

    private void OnCloseClicked(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
}