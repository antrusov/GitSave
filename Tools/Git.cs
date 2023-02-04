using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GitSave.Models;

namespace GitSave.Tools;

public static class Git
{
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

}