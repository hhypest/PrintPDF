using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PrintPDF.Extensions;
using System.Windows;

namespace PrintPDF;

public partial class App : Application
{
    public static IHost AppHost { get; private set; } = null!;

    public App()
    {
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((_, services) =>
            {
                services.AddViewModels();
                services.AddViews();
            })
            .ConfigureLogging(logging =>
            {
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Debug);
            }).Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost.StartAsync();
        AppHost.AppStarted();
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        AppHost.AppStoped();
        await AppHost.StopAsync();
        base.OnExit(e);
    }
}