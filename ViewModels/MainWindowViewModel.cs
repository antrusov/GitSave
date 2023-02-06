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

        NewCommand = ReactiveCommand.Create(OnNew, canExecuteNewCommand);
        RefreshCommand = ReactiveCommand.Create(OnRefresh);
        UpdateCommand = ReactiveCommand.Create(OnUpdate, canExecuteUpdateCommand);
        ResetCommand = ReactiveCommand.Create(OnReset);
        SetWorkFolderCommand = ReactiveCommand.Create(OnSetWorkFolder);

        WorkFolder = Directory.GetCurrentDirectory();
    }

    #region [ Properties ]

    private int _Limit = 25;
    public int Limit
    {
        get => _Limit;
        set => this.RaiseAndSetIfChanged(ref _Limit, value);
    }

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

    public async Task OnNew()
    {
        await Git.New(NewComment, WorkFolder);
        await LoadCommits();
        NewComment = "";
    }
    
    public async Task OnRefresh()
    {
        await LoadCommits();
        NewComment = "";
    }

    public async Task OnUpdate()
    {
        NewComment = "";
        await Git.Update(LastComment, WorkFolder);
        //Commits[0].Comment = LastComment;
        await LoadCommits();
    }

    public async Task OnReset()
    {
        await Git.Reset(WorkFolder);
        NewComment = "";
        LastComment = await Git.LastComment(WorkFolder);
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

        var list = await Git.GetCommits(Limit, WorkFolder);

        foreach(var commit in list)
            Commits.Add(commit);

        LastComment = await Git.LastComment(WorkFolder);
    }

    #endregion

}