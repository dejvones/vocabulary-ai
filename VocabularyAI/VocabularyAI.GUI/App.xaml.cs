using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using VocabularyAI.GUI.Services;
using VocabularyAI.GUI.ViewModels;
using VocabularyAI.Services;

namespace VocabularyAI.GUI;

public partial class App : Application
{
    public static ServiceProvider? ServiceProvider { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();
        ConfigureServices(services);

        ServiceProvider = services.BuildServiceProvider();

        var mainWindow = new MainWindow
        {
            DataContext = ServiceProvider.GetRequiredService<MainViewModel>()
        };
        mainWindow.Show();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<NavigationService>();

        services.AddSingleton<MainViewModel>();
        services.AddSingleton<MenuViewModel>();
        services.AddSingleton<WordsViewModel>();
        services.AddSingleton<HistoryViewModel>();

        services.AddSingleton<IOpenAIService, OpenAIService>();
        services.AddSingleton<IWordsService, WordsService>();
    }
}
