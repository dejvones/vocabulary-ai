using CommunityToolkit.Mvvm.ComponentModel;
using VocabularyAI.GUI.Services;

namespace VocabularyAI.GUI.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly NavigationService _navigation;

    public MainViewModel(NavigationService navigation)
    {
        _navigation = navigation;

        _navigation.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(NavigationService.CurrentViewModel))
                OnPropertyChanged(nameof(CurrentViewModel));
        };

        _navigation.Navigate<MenuViewModel>();
    }

    public ObservableObject? CurrentViewModel => _navigation.CurrentViewModel;
}
