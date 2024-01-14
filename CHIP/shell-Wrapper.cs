using System;
using System.Diagnostics;

namespace CHIP
{
    internal class shell_Wrapper
    {
        public string result = null;

        public void Run(string Args)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{Args}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
        }
    }
}
