using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CHIP
{
    public class Logger
    {
        string folder = "./";
        string fileName = "log.log";
        static StreamWriter openLog = null;
        FileStream logfile = null;
        public Logger() {

            string text = "logger started";
            fileName = DateTime.Now.ToString("dd-MM-yyy-HH:mm:ss");
            fileName += ".log";
            string fullPath = folder + fileName;

            logfile = File.OpenWrite(fullPath);
            openLog = new StreamWriter(logfile);
            openLog.AutoFlush = true;
            Log(text);
            openLog.Flush();
            logfile.Flush();
        }

        public void Log(string message)
        {
            if (openLog == null)
            {
                //panic
            }
            else
            {
                DateTime now = DateTime.Now;
                StringBuilder sb = new StringBuilder();
                sb.Append(now.Day);
                sb.Append("/");
                sb.Append(now.Month);
                sb.Append("/");
                sb.Append(now.Year);
                sb.Append(" ");
                sb.Append(now.Hour);
                sb.Append(":");
                sb.Append(now.Minute);
                sb.Append(":");
                sb.Append(now.Second);
                sb.Append(" ");
                sb.Append("Message: ");
                sb.Append(message);

                openLog.WriteLine(sb.ToString());
                openLog.Flush();
                logfile.Flush();
            }
        }

    }
}
