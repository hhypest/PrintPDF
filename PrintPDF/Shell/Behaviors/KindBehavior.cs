using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace PrintPDF.Shell.Behaviors;

public sealed class KindBehavior : Behavior<PackIcon>
{
    protected override void OnAttached()
    {
        AssociatedObject.Loaded += OnLoaded;
        AssociatedObject.MouseMove += OnMouseMove;
        AssociatedObject.MouseLeave += OnMouseLeave;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.Loaded -= OnLoaded;
        AssociatedObject.MouseMove -= OnMouseMove;
        AssociatedObject.MouseLeave -= OnMouseLeave;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (sender is not PackIcon kind)
            return;

        var brush = new SolidColorBrush(Colors.White);
        kind.Foreground = brush;
    }

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
        if (sender is not PackIcon kind)
            return;

        var red = SwatchHelper.Lookup[MaterialDesignColor.Red];
        var brush = new SolidColorBrush(red);
        kind.Foreground = brush;
    }

    private void OnMouseLeave(object sender, MouseEventArgs e)
    {
        if (sender is not PackIcon kind)
            return;

        var brush = new SolidColorBrush(Colors.White);
        kind.Foreground = brush;
    }
}