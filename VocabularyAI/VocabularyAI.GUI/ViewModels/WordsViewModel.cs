using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using VocabularyAI.GUI.Models;
using VocabularyAI.GUI.Services;
using VocabularyAI.Services;

namespace VocabularyAI.GUI.ViewModels;

public partial class WordsViewModel : ObservableObject
{
    private readonly IWordsService _wordsService;
    private readonly NavigationService _navigationService;

    private string _czechSelected = string.Empty;
    private string _englishSelected = string.Empty;
    private int _correct = 0;
    private int _all = 0;

    [ObservableProperty]
    private ObservableCollection<LevelModel> _levels = new([
        new(Level.A1, "A1"),
        new(Level.A2, "A2"),
        new(Level.B1, "B1"),
        new(Level.B2, "B2"),
        new(Level.C1, "C1"),
        new(Level.C2, "C2"),
    ]);
    [ObservableProperty]
    private LevelModel _selectedLevel;
    [ObservableProperty]
    private string _topic = "Vyber téma";
    [ObservableProperty]
    private Visibility _showLoading = Visibility.Hidden;
    [ObservableProperty]
    private Brush _lastResultBrush = new SolidColorBrush(Color.FromRgb(33, 150, 243));
    [ObservableProperty]
    private string _status = "Správný počet: čekám...";
    [ObservableProperty]
    private string _lastResultMessage = "Vítej!";
    [ObservableProperty]
    private string _lastResultIcon = "ℹ️";
    [ObservableProperty]
    private ObservableCollection<WordModel> _czechs = [];
    [ObservableProperty]
    private ObservableCollection<WordModel> _englishs = [];

    public WordsViewModel(IWordsService wordsService, NavigationService navigationService)
    {
        _wordsService = wordsService;
        _navigationService = navigationService;
        _selectedLevel = Levels.First();
    }

    [RelayCommand]
    public async Task GenerateWordsAsync()
    {
        ShowLoading = Visibility.Visible;
        try
        {
            var words = await _wordsService.GenerateWordsAsync(SelectedLevel.Level, Topic);
            Czechs = new ObservableCollection<WordModel>(words.Select(w => new WordModel(w.Czech, false)));
            Englishs = new ObservableCollection<WordModel>(words.Select(w => new WordModel(w.English, false)));
            _czechSelected = string.Empty;
            _englishSelected = string.Empty;
        }
        finally
        {
            ShowLoading = Visibility.Hidden;
        }
    }

    [RelayCommand]
    public void SelectCzechWord(string text)
    {
        if (text == _czechSelected)
        {
            Czechs = new ObservableCollection<WordModel>(Czechs.Select(w => new WordModel(w.Value, false)));
            _czechSelected = string.Empty;
            return;
        }
        _czechSelected = text;
        Czechs = new ObservableCollection<WordModel>(Czechs.Select(w => new WordModel(w.Value, w.Value == text)));
        if (_englishSelected != string.Empty)
        {
            CompareSelection(text, _englishSelected);
        }
    }

    [RelayCommand]
    public void SelectEnglishWord(string text)
    {
        if (text == _englishSelected)
        {
            Englishs = new ObservableCollection<WordModel>(Englishs.Select(w => new WordModel(w.Value, false)));
            _englishSelected = string.Empty;
            return;
        }
        _englishSelected = text;
        Englishs = new ObservableCollection<WordModel>(Englishs.Select(w => new WordModel(w.Value, w.Value == text)));
        if (_czechSelected != string.Empty)
        {
            CompareSelection(_czechSelected, text);
        }
    }

    [RelayCommand]
    public void NavigateToMenu()
    {
        _navigationService.Navigate<MenuViewModel>();
    }

    private void CompareSelection(string czech, string english)
    {
        _all++;
        var isCorrect = _wordsService.IsCorrect(czech, english);
        LastResultBrush = isCorrect ? Brushes.LimeGreen : Brushes.OrangeRed;
        LastResultMessage = isCorrect ? "Gratuluji, správně!" : "Smůla, chyba!";
        LastResultIcon = isCorrect ? "✅" : "❌";

        if (isCorrect)
        {
            Czechs.Remove(Czechs.First(w => w.Value == czech));
            Englishs.Remove(Englishs.First(w => w.Value == english));
            _correct++;
        }
        Czechs = new ObservableCollection<WordModel>(Czechs.Select(w => new WordModel(w.Value, false)));
        Englishs = new ObservableCollection<WordModel>(Englishs.Select(w => new WordModel(w.Value, false)));
        _czechSelected = string.Empty;
        _englishSelected = string.Empty;
        Status = $"Skóre: {_correct} správně / {_all} celkem";
    }
}
