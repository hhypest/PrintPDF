using Microsoft.Extensions.Hosting;
using PrintPDF.Extensions;
using System.Windows;

namespace PrintPDF;

public partial class App : Application
{
    public static IHost AppHost { get; private set; } = null!;

    public App()
    {
        AppHost = Host.CreateDefaultBuilder().HostBuild();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        await AppHost.AppStarted();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost.AppStoped();
        base.OnExit(e);
    }
}