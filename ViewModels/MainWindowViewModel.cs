using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GitSave.Models;
using ReactiveUI;

namespace GitSave.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    //Comment
    private string? _Comment;

    public string? Comment
    {
        get => _Comment;
        set => this.RaiseAndSetIfChanged(ref _Comment, value);
    }

    //commits
    public ObservableCollection<Commit> Commits { get; } = new ObservableCollection<Commit>(GenerateMockCommitTable());

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
            };

            return defaultCommit;
    }
}