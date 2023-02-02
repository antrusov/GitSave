using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using GitSave.Models;
using ReactiveUI;

namespace GitSave.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    public MainWindowViewModel()
    {
        IObservable<bool> canExecuteNewCommand =
            this.WhenAnyValue(vm => vm.NewComment, (comment) => !string.IsNullOrEmpty(comment));

        IObservable<bool> canExecuteUpdateCommand =
            this.WhenAnyValue(vm => vm.LastComment, (comment) => !string.IsNullOrEmpty(comment));

        NewCommand =
            ReactiveCommand.Create<string?>(comment => OnNew(comment), canExecuteNewCommand);

        UpdateCommand =
            ReactiveCommand.Create<string?>(comment => OnUpdate(comment), canExecuteNewCommand);

        ResetCommand = ReactiveCommand.Create(OnReset);
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

    public ObservableCollection<Commit> Commits { get; } = new ObservableCollection<Commit>(GenerateMockCommitTable());

    #endregion

    #region [ Commands ]

    public ICommand NewCommand { get; }
    public ICommand UpdateCommand { get; }
    public ICommand ResetCommand { get; }

    #endregion

    #region [ Helpers ]

    public async Task OnNew(string? comment) => NewComment = "1";

    public async Task OnUpdate(string? comment) => NewComment = "2";

    public async Task OnReset() => NewComment = "3";

    static IEnumerable<Commit> GenerateMockCommitTable()
    {
            var defaultCommit = new List<Commit>()
            {
                new Commit()
                {
                    UUID = "123",
                    Comment = "Comment1",
                    Login = "Login1",
                    Email = "Email1",
                    Created = DateTime.Now,
                },
                new Commit()
                {
                    UUID = "456",
                    Comment = "Comment2",
                    Login = "Login2",
                    Email = "Email2",
                    Created = DateTime.Now.AddMinutes(-5),
                },
                new Commit()
                {
                    UUID = "123",
                    Comment = "Comment1",
                    Login = "Login1",
                    Email = "Email1",
                    Created = DateTime.Now,
                },
                new Commit()
                {
                    UUID = "456",
                    Comment = "Comment2",
                    Login = "Login2",
                    Email = "Email2",
                    Created = DateTime.Now.AddMinutes(-5),
                },
                new Commit()
                {
                    UUID = "123",
                    Comment = "Comment1",
                    Login = "Login1",
                    Email = "Email1",
                    Created = DateTime.Now,
                },
                new Commit()
                {
                    UUID = "456",
                    Comment = "Comment2",
                    Login = "Login2",
                    Email = "Email2",
                    Created = DateTime.Now.AddMinutes(-5),
                },
                new Commit()
                {
                    UUID = "123",
                    Comment = "Comment1",
                    Login = "Login1",
                    Email = "Email1",
                    Created = DateTime.Now,
                },
                new Commit()
                {
                    UUID = "456",
                    Comment = "Comment2",
                    Login = "Login2",
                    Email = "Email2",
                    Created = DateTime.Now.AddMinutes(-5),
                },
                new Commit()
                {
                    UUID = "123",
                    Comment = "Comment1",
                    Login = "Login1",
                    Email = "Email1",
                    Created = DateTime.Now,
                },
                new Commit()
                {
                    UUID = "456",
                    Comment = "Comment2",
                    Login = "Login2",
                    Email = "Email2",
                    Created = DateTime.Now.AddMinutes(-5),
                },
                new Commit()
                {
                    UUID = "123",
                    Comment = "Comment1",
                    Login = "Login1",
                    Email = "Email1",
                    Created = DateTime.Now,
                },
                new Commit()
                {
                    UUID = "456",
                    Comment = "Comment2",
                    Login = "Login2",
                    Email = "Email2",
                    Created = DateTime.Now.AddMinutes(-5),
                },
                new Commit()
                {
                    UUID = "123",
                    Comment = "Comment1",
                    Login = "Login1",
                    Email = "Email1",
                    Created = DateTime.Now,
                },
                new Commit()
                {
                    UUID = "456",
                    Comment = "Comment2",
                    Login = "Login2",
                    Email = "Email2",
                    Created = DateTime.Now.AddMinutes(-5),
                },
                new Commit()
                {
                    UUID = "123",
                    Comment = "Comment1",
                    Login = "Login1",
                    Email = "Email1",
                    Created = DateTime.Now,
                },
                new Commit()
                {
                    UUID = "456",
                    Comment = "Comment2",
                    Login = "Login2",
                    Email = "Email2",
                    Created = DateTime.Now.AddMinutes(-5),
                },
                new Commit()
                {
                    UUID = "123",
                    Comment = "Comment1",
                    Login = "Login1",
                    Email = "Email1",
                    Created = DateTime.Now,
                },
                new Commit()
                {
                    UUID = "456",
                    Comment = "Comment2",
                    Login = "Login2",
                    Email = "Email2",
                    Created = DateTime.Now.AddMinutes(-5),
                },
                new Commit()
                {
                    UUID = "123",
                    Comment = "Comment1",
                    Login = "Login1",
                    Email = "Email1",
                    Created = DateTime.Now,
                },
                new Commit()
                {
                    UUID = "456",
                    Comment = "Comment2",
                    Login = "Login2",
                    Email = "Email2",
                    Created = DateTime.Now.AddMinutes(-5),
                },
                new Commit()
                {
                    UUID = "123",
                    Comment = "Comment1",
                    Login = "Login1",
                    Email = "Email1",
                    Created = DateTime.Now,
                },
                new Commit()
                {
                    UUID = "456",
                    Comment = "Comment2",
                    Login = "Login2",
                    Email = "Email2",
                    Created = DateTime.Now.AddMinutes(-5),
                },
                new Commit()
                {
                    UUID = "123",
                    Comment = "Comment1",
                    Login = "Login1",
                    Email = "Email1",
                    Created = DateTime.Now,
                },
                new Commit()
                {
                    UUID = "456",
                    Comment = "Comment2",
                    Login = "Login2",
                    Email = "Email2",
                    Created = DateTime.Now.AddMinutes(-5),
                },
                new Commit()
                {
                    UUID = "123",
                    Comment = "Comment1",
                    Login = "Login1",
                    Email = "Email1",
                    Created = DateTime.Now,
                },
                new Commit()
                {
                    UUID = "456",
                    Comment = "Comment2",
                    Login = "Login2",
                    Email = "Email2",
                    Created = DateTime.Now.AddMinutes(-5),
                },
                new Commit()
                {
                    UUID = "123",
                    Comment = "Comment1",
                    Login = "Login1",
                    Email = "Email1",
                    Created = DateTime.Now,
                },
                new Commit()
                {
                    UUID = "456",
                    Comment = "Comment2",
                    Login = "Login2",
                    Email = "Email2",
                    Created = DateTime.Now.AddMinutes(-5),
                },
                new Commit()
                {
                    UUID = "123",
                    Comment = "Comment1",
                    Login = "Login1",
                    Email = "Email1",
                    Created = DateTime.Now,
                },
                new Commit()
                {
                    UUID = "456",
                    Comment = "Comment2",
                    Login = "Login2",
                    Email = "Email2",
                    Created = DateTime.Now.AddMinutes(-5),
                },
                new Commit()
                {
                    UUID = "123",
                    Comment = "Comment1",
                    Login = "Login1",
                    Email = "Email1",
                    Created = DateTime.Now,
                },
                new Commit()
                {
                    UUID = "456",
                    Comment = "Comment2",
                    Login = "Login2",
                    Email = "Email2",
                    Created = DateTime.Now.AddMinutes(-5),
                },
                new Commit()
                {
                    UUID = "123",
                    Comment = "Comment1",
                    Login = "Login1",
                    Email = "Email1",
                    Created = DateTime.Now,
                },
                new Commit()
                {
                    UUID = "456",
                    Comment = "Comment2",
                    Login = "Login2",
                    Email = "Email2",
                    Created = DateTime.Now.AddMinutes(-5),
                },
                new Commit()
                {
                    UUID = "123",
                    Comment = "Comment1",
                    Login = "Login1",
                    Email = "Email1",
                    Created = DateTime.Now,
                },
                new Commit()
                {
                    UUID = "456",
                    Comment = "Comment2",
                    Login = "Login2",
                    Email = "Email2",
                    Created = DateTime.Now.AddMinutes(-5),
                },
                new Commit()
                {
                    UUID = "123",
                    Comment = "Comment1",
                    Login = "Login1",
                    Email = "Email1",
                    Created = DateTime.Now,
                },
                new Commit()
                {
                    UUID = "456",
                    Comment = "Comment2",
                    Login = "Login2",
                    Email = "Email2",
                    Created = DateTime.Now.AddMinutes(-5),
                },
            };

            return defaultCommit;
    }

    #endregion

}