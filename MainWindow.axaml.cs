using Avalonia.Controls;
using GitSave.ViewModels;

namespace GitSave;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
}