using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using GitSave.Models;
using GitSave.Tools;
using ReactiveUI;

namespace GitSave.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    public MainWindowViewModel()
    {
        IObservable<bool> canExecuteNewCommand = this.WhenAnyValue(vm => vm.NewComment, (comment) => !string.IsNullOrEmpty(comment));
        IObservable<bool> canExecuteUpdateCommand = this.WhenAnyValue(vm => vm.LastComment, (comment) => !string.IsNullOrEmpty(comment));

        NewCommand = ReactiveCommand.Create<string?>(comment => OnNew(comment), canExecuteNewCommand);
        RefreshCommand = ReactiveCommand.Create(OnRefresh);
        UpdateCommand = ReactiveCommand.Create<string?>(comment => OnUpdate(comment), canExecuteNewCommand);
        ResetCommand = ReactiveCommand.Create(OnReset);
        SetWorkFolderCommand = ReactiveCommand.Create(OnSetWorkFolder);

        WorkFolder = Directory.GetCurrentDirectory();
    }

    #region [ Properties ]

    private string? _NewComment;
    public string? NewComment
    {
        get => _NewComment;
        set => this.RaiseAndSetIfChanged(ref _NewComment, value);
    }

    private string? _LastComment;
    public string? LastComment
    {
        get => _LastComment;
        set => this.RaiseAndSetIfChanged(ref _LastComment, value);
    }

    private string? _WorkFolder;
    public string? WorkFolder
    {
        get => _WorkFolder;
        set => this.RaiseAndSetIfChanged(ref _WorkFolder, value);
    }

    public ObservableCollection<Commit> Commits { get; } = new ObservableCollection<Commit>();

    #endregion

    #region [ Commands ]

    public ICommand NewCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand UpdateCommand { get; }
    public ICommand ResetCommand { get; }
    public ICommand SetWorkFolderCommand { get; }

    #endregion

    #region [ Helpers ]

    public async Task OnNew(string? comment) => NewComment = "1";
    public async Task OnRefresh() => await LoadCommits();

    public async Task OnUpdate(string? comment) => NewComment = "2";

    public async Task OnReset()
    {
        //LastComment = await Cmd.Run("git status", WorkFolder);
        LastComment = await Cmd.Run("git log --pretty=format:\"%h, %an, %ad : %s\"", WorkFolder);
    }

    public async Task OnSetWorkFolder()
    {
        OpenFolderDialog dialog = new OpenFolderDialog
        {
            Title = "Select work folder"
        };

        if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            WorkFolder = await dialog.ShowAsync(desktop.MainWindow);
            await LoadCommits();
        }
    }

    async Task LoadCommits ()
    {
        Commits.Clear();

        var list = await Git.GetCommits(WorkFolder);

        foreach(var commit in list)
            Commits.Add(commit);
    }

    #endregion

}