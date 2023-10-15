using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CHIP
{
    internal class cmatrix
    {
        Logger mylogger;
        bool play = true;
        public cmatrix(Logger mylogger) {
            this.mylogger = mylogger;
            mylogger.Log("start cmatrix");
            using (Process shell = new Process())
            {
                mylogger.Log("Process created");
                shell.StartInfo.FileName = "cmatrix";
                shell.StartInfo.UseShellExecute = false;
                shell.StartInfo.RedirectStandardOutput = true;
                mylogger.Log("Process start");
                shell.Start();
                mylogger.Log("Process started");
                mylogger.Log("build StreamReader");
                StreamReader reader = shell.StandardOutput;
                mylogger.Log("StreamReader built");
                while (play)
                {
                    mylogger.Log("StandardOutput");
                    string output = reader.ReadToEnd();
                    mylogger.Log(output);
                    mylogger.Log("StandardOutput");
                }
            }
        }

    }
}
