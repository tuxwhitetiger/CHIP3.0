using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CHIP
{
    public class Logger
    {
        string folder = "/home/tux/logs/";
       // string folder = "./logs/";
        string fileName = "log.log";
        static StreamWriter openLog = null;
        FileStream logfile = null;
        StringBuilder sb = new StringBuilder();
        public Logger() {

            string text = "logger started";
            fileName = DateTime.Now.ToString("dd-MM-yyy-HH:mm:ss");
            fileName += ".log";
            string fullPath = folder + fileName;
            
            /*
            logfile = File.OpenWrite(fullPath);
            openLog = new StreamWriter(logfile);
            */

            openLog = File.CreateText(fullPath);

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
                sb.Append(DateTime.Now.ToString("dd-MM-yyy-HH:mm:ss"));
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
