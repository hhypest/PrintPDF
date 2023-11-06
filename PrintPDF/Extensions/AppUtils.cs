using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrintPDF.Services.Dialog;
using PrintPDF.ViewModels.File;
using PrintPDF.ViewModels.Main;
using PrintPDF.ViewModels.Printer;
using PrintPDF.Views.Dialog;
using PrintPDF.Views.Shell;
using Serilog;
using System;

namespace PrintPDF.Extensions;

public static class AppUtils
{
    public static IHost AppBuild(this IHostBuilder hostBuilder)
    {
        return hostBuilder.ConfigureAppConfiguration(configuration =>
        {
            configuration.AddJsonFile($@"{AppContext.BaseDirectory}settings\appsettings.json").Build();
        })
            .ConfigureServices((_, services) =>
            {
                services.AddServices();
                services.AddViewModels();
                services.AddViews();
            })
            .UseSerilog((hostContext, logConfig) => logConfig.ReadFrom.Configuration(hostContext.Configuration)).Build();
    }

    public static void AppStarted(this IHost host)
    {
        var shell = ActivatorUtilities.GetServiceOrCreateInstance<IShellView>(host.Services);
        var dataContext = ActivatorUtilities.GetServiceOrCreateInstance<IMainViewModel>(host.Services) as ObservableRecipient ?? throw new InvalidOperationException();
        dataContext.IsActive = true;
        shell.SetDataContext(dataContext);
        shell.ShowView();
    }

    public static void AppStoped(this IHost host)
    {
        var dataContext = ActivatorUtilities.GetServiceOrCreateInstance<IMainViewModel>(host.Services) as ObservableRecipient ?? throw new InvalidOperationException();
        dataContext.IsActive = false;
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IDialogService, DialogService>();
    }

    private static void AddViewModels(this IServiceCollection services)
    {
        services.AddSingleton<IMainViewModel, MainViewModel>();
        services.AddTransient<IPrinterViewModel, PrinterViewModel>();
        services.AddTransient<IFileViewModel, FileViewModel>();
    }

    private static void AddViews(this IServiceCollection services)
    {
        services.AddSingleton<IShellView, ShellView>();
        services.AddTransient<IDialogView, DialogView>();
    }
}