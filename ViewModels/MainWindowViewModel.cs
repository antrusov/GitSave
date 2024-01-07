using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using GitSave.Models;
using GitSave.Tools;
using ReactiveUI;
using MessageBox.Avalonia.DTO;
using MessageBoxAvaloniaEnums = MessageBox.Avalonia.Enums;
using System.Reactive.Linq;

namespace GitSave.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    const string notesFileName = "notes.txt";
    const string gitignoreFileName = ".gitignore";
    const string gitignoreContent = $"{notesFileName}";

    public MainWindowViewModel()
    {
        IObservable<bool> canExecuteNewCommand = this.WhenAnyValue(vm => vm.NewComment, (comment) => !string.IsNullOrEmpty(comment));
        IObservable<bool> canExecuteUpdateCommand = this.WhenAnyValue(vm => vm.LastComment, (comment) => !string.IsNullOrEmpty(comment));

        //команды для конопок
        NewCommand = ReactiveCommand.Create(OnNew, canExecuteNewCommand);
        RefreshCommand = ReactiveCommand.Create(OnRefresh);
        UpdateCommand = ReactiveCommand.Create(OnUpdate, canExecuteUpdateCommand);
        ResetCommand = ReactiveCommand.Create(OnReset);
        SetWorkFolderCommand = ReactiveCommand.Create(OnSetWorkFolder);
        ResetToCommit = ReactiveCommand.Create(OnResetToCommit);

        //перезагрузка списка коммитов
        this
            .WhenAnyValue(
                vm => vm.ShowAllCommits,
                vm => vm.Limit,
                (all, limit) => (all, limit))
            .Throttle(TimeSpan.FromSeconds(0.8))
            .DistinctUntilChanged()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(async _ => await LoadCommits());

        //смена рабочей папки (для подготовки "notes.txt" и ".gitignore"
        this
            .WhenAnyValue(vm => vm.WorkFolder, (folder) => Directory.Exists(folder))
            .Throttle(TimeSpan.FromSeconds(0.8))
            .DistinctUntilChanged()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(async _ => await OnWorkFolderChanged());

        //обновление текстового поля и его автоматическое сохранение в notes.txt
        this
            .WhenAnyValue(vm => vm.Notes)
            .Throttle(TimeSpan.FromSeconds(0.8))
            .DistinctUntilChanged()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(async _ => await OnNotesChanged());

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

    private Commit? _SelectedCommit;
    public Commit? SelectedCommit
    {
        get => _SelectedCommit;
        set => this.RaiseAndSetIfChanged(ref _SelectedCommit, value);
    }

    private string? _WorkFolder;
    public string? WorkFolder
    {
        get => _WorkFolder;
        set => this.RaiseAndSetIfChanged(ref _WorkFolder, value);
    }

    private string? _Notes;
    public string? Notes
    {
        get => _Notes;
        set => this.RaiseAndSetIfChanged(ref _Notes, value);
    }

    //private readonly ObservableAsPropertyHelper<bool> _ShowAllCommits;
    //public bool ShowAllCommits => _ShowAllCommits.Value;

    private bool _ShowAllCommits;
    public bool ShowAllCommits
    {
        get => _ShowAllCommits;
        set => this.RaiseAndSetIfChanged(ref _ShowAllCommits, value);
    }

    public ObservableCollection<Commit> Commits { get; } = new ObservableCollection<Commit>();

    #endregion

    #region [ Commands ]

    public ICommand NewCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand UpdateCommand { get; }
    public ICommand ResetCommand { get; }
    public ICommand SetWorkFolderCommand { get; }
    public ICommand ResetToCommit { get; }

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

    public async Task OnResetToCommit()
    {        

        if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            //dialog
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBoxAvaloniaEnums.ButtonEnum.OkAbort,
                    ContentHeader = $"Reset to commit {SelectedCommit.UUID}",
                    ContentMessage = $"{SelectedCommit.Comment}"
                });

            //confirmation
            var res = await messageBoxStandardWindow.ShowDialog(desktop.MainWindow);

            //reset to commit
            if (res == MessageBoxAvaloniaEnums.ButtonResult.Ok)
            {
                await Git.ResetToCommit(SelectedCommit.UUID, WorkFolder);
                await LoadCommits();
                NewComment = "";
            }
        }
    }

    async Task LoadCommits ()
    {
        Commits.Clear();

        var list = await Git.GetCommits(Limit, ShowAllCommits, WorkFolder);

        foreach(var commit in list)
            Commits.Add(commit);

        LastComment = await Git.LastComment(WorkFolder);
    }

    public async Task OnWorkFolderChanged()
    {
        var pathToGitignore = Path.Combine(WorkFolder, gitignoreFileName);

        if (!File.Exists(pathToGitignore))
            await File.WriteAllTextAsync(pathToGitignore, gitignoreContent);

        var pathToNotes = Path.Combine(WorkFolder, notesFileName);
        if (!File.Exists(pathToNotes))
            await File.WriteAllTextAsync(pathToNotes, "write notes here!");

        Notes = await File.ReadAllTextAsync(pathToNotes);
    }

    public async Task OnNotesChanged()
    {
        var pathToNotes = Path.Combine(WorkFolder, notesFileName);

        if (File.Exists(pathToNotes))
            await File.WriteAllTextAsync(pathToNotes, Notes);
    }

    #endregion

}