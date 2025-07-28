using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using VocabularyAI.GUI.Services;

namespace VocabularyAI.GUI.ViewModels;

public partial class MenuViewModel(NavigationService navigationService) : ObservableObject
{
    private readonly NavigationService _navigationService = navigationService;

    [RelayCommand]
    public void NavigateToWords()
    {
        _navigationService.Navigate<WordsViewModel>();
    }

    [RelayCommand]
    public void NavigateToHistory()
    {
        _navigationService.Navigate<HistoryViewModel>();
    }

    [RelayCommand]
    public static void End()
    {
        Application.Current.Shutdown();
    }
}
