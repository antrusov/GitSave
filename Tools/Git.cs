using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GitSave.Models;

namespace GitSave.Tools;

public static class Git
{
    #region [ get commits ]

    const char GitLogCommitSeparator = '¶';
    const string GitLogLineSeparator = "\r\n";
    const string GitLog = $"git log --pretty=format:\"%h¶%an¶%ad¶%s\"";

    public static async Task<IEnumerable<Commit>> GetCommits(string root)
    {
        try
        {
            var commits = await Cmd.Run(GitLog, root);

            if (commits.StartsWith("Error"))
                return Enumerable.Empty<Commit>();

            return commits
                .Split(GitLogLineSeparator, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => {
                    var parts = line.Split(GitLogCommitSeparator);
                    return new Commit() {
                        UUID = parts[0],
                        Login = parts[1],
                        Created = parts[2],
                        Comment = parts[3]
                    };
                });
        }
        catch
        {
            return Enumerable.Empty<Commit>();
        }        
    }

    #endregion

    #region [ return to last commit ]

    const string GitReset = "git reset --hard";

    public static async Task Reset(string root)
    {
        await Cmd.Run(GitReset, root);
    }

    #endregion

    #region [ new commit ]

    const string GitAddFiles = "git add .";
    const string GitCommit = "git commit -m '{0}'";

    public static async Task New(string comment, string root)
    {
        await Cmd.Run(GitAddFiles, root);
        var cmd = string.Format(GitCommit, comment);
        var res = await Cmd.Run(cmd, root);
        int i=1;
    }

    #endregion

    #region [ update the last commit ]    

    const string GitAddFilesAmend = "git add .";
    const string GitCommitAmend = "git commit --amend -m\"{0}\"";

    public static async Task Update(string comment, string root)
    {
        await Cmd.Run(GitAddFilesAmend, root);
        await Cmd.Run(string.Format(GitCommitAmend, comment), root);
    }

    #endregion

    #region [ return last full comment ]

    const string GitLastFullComment = "git log --pretty=\"%B\" -1";

    public static async Task<string> LastComment(string root)
    {
        return await Cmd.Run(GitLastFullComment, root);
    }

    #endregion
}