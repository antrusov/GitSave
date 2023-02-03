using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace GitSave.Tools;

//see https://stackoverflow.com/a/57376993/5489443
public static class Cmd
{
    public static async Task<string> Run(string command, string workingDirectory = null)
    {
        try
        {
            ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c " + command);

            procStartInfo.RedirectStandardError =
            procStartInfo.RedirectStandardInput =
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.StandardInputEncoding =
            procStartInfo.StandardErrorEncoding =
            procStartInfo.StandardOutputEncoding = Encoding.UTF8;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;
            if (null != workingDirectory)
            {
                procStartInfo.WorkingDirectory = workingDirectory;
            }

            Process proc = new Process();
            proc.StartInfo = procStartInfo;
            proc.Start();

            StringBuilder sb = new StringBuilder();
            proc.OutputDataReceived += delegate (object sender, DataReceivedEventArgs e)
            {
                sb.AppendLine(e.Data);
            };
            proc.ErrorDataReceived += delegate (object sender, DataReceivedEventArgs e)
            {
                sb.AppendLine(e.Data);
            };

            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();
            await proc.WaitForExitAsync();
            return sb.ToString();
        }
        catch (Exception objException)
        {
            return $"Error in command: {command}, {objException.Message}";
        }
    }
}