using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VocabularyAI.GUI.Services;

namespace VocabularyAI.GUI.ViewModels;

public partial class HistoryViewModel(NavigationService navigationService) : ObservableObject
{
    private readonly NavigationService _navigationService = navigationService;

    [RelayCommand]
    public void NavigateToMenu()
    {
        _navigationService.Navigate<MenuViewModel>();
    }
}
