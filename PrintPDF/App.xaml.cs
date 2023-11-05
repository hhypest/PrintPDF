using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using PrintPDF.Extensions;
using Serilog;
using System;
using System.Windows;

namespace PrintPDF;

public partial class App : Application
{
    public static IHost AppHost { get; private set; } = null!;

    public App()
    {
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(appConfigure =>
            {
                appConfigure.AddJsonFile($@"{AppContext.BaseDirectory}settings\appsettings.json");
                appConfigure.Build();
            })
            .ConfigureServices((_, services) =>
            {
                services.AddViewModels();
                services.AddViews();
            })
            .UseSerilog((hostContext, _, logConfig) => logConfig.ReadFrom.Configuration(hostContext.Configuration)).Build();
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