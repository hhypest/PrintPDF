using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrintPDF.Services.Factory;
using PrintPDF.Shell.ViewModels.File;
using PrintPDF.Shell.ViewModels.Main;
using PrintPDF.Shell.ViewModels.Printer;
using PrintPDF.Shell.Views;

namespace PrintPDF.Extensions;

public static class AppBuild
{
    public static IHost HostBuild(this IHostBuilder host)
    {
        return host.ConfigureServices((hostContext, services) =>
        {
            services.AddServices();
            services.AddViewModels();
            services.AddViews();
        }).Build();
    }

    public static async Task AppStarted(this IHost host)
    {
        await host.StartAsync();
        var shell = ActivatorUtilities.GetServiceOrCreateInstance<IShellView>(host.Services);
        var viewModel = ActivatorUtilities.GetServiceOrCreateInstance<IMainViewModel>(host.Services);
        viewModel.RegisterMessage(true);
        shell.SetDataContext(viewModel);
        shell.ShowView();
    }

    public static async Task AppStoped(this IHost host)
    {
        var viewModel = ActivatorUtilities.GetServiceOrCreateInstance<IMainViewModel>(host.Services);
        viewModel.RegisterMessage(false);
        await host.StopAsync();
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IMessenger, StrongReferenceMessenger>();
        services.AddSingleton<IFactoryService, FactoryService>();
        return services;
    }

    private static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddSingleton<IMainViewModel, MainViewModel>();
        services.AddTransient<IPrinterViewModel, PrinterViewModel>();
        services.AddTransient<IFileViewModel, FileViewModel>();
        return services;
    }

    private static IServiceCollection AddViews(this IServiceCollection services)
    {
        services.AddSingleton<IShellView, ShellView>();
        return services;
    }
}