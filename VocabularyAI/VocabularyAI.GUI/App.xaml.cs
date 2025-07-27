using Microsoft.Extensions.DependencyInjection;
using System.Windows;
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

        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IOpenAIService, OpenAIService>();
        services.AddSingleton<IWordsService, WordsService>();
        services.AddSingleton<MainWindow>();
        services.AddSingleton<MainWindowViewModel>();
    }
}
