using System.Windows;
using VocabularyAI.GUI.ViewModels;

namespace VocabularyAI.GUI;

public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}