using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrintPDF.ViewModels.File;
using PrintPDF.ViewModels.Main;
using PrintPDF.ViewModels.Printer;
using PrintPDF.Views.Shell;
using System;

namespace PrintPDF.Extensions;

public static class AppUtils
{
    public static void AddViewModels(this IServiceCollection services)
    {
        services.AddSingleton<IMainViewModel, MainViewModel>();
        services.AddTransient<IPrinterViewModel, PrinterViewModel>();
        services.AddTransient<IFileViewModel, FileViewModel>();
    }

    public static void AddViews(this IServiceCollection services)
    {
        services.AddSingleton<IShellView, ShellView>();
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
}