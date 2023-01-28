using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace CHIP
{
    internal class CalanderClock
    {
        List<alarm> Clocks = new List<alarm>();
        public CalanderClock() { }

        public string getTime()
        {

            return DateTime.Now.ToString();
        }

        public void setTimer(string time)
        {
            alarm timer = convertTimerString(time);
            Clocks.Add(timer);
        }

        public void checkTimers()
        {
            foreach (alarm a in Clocks)
            {
                if (a.GetDateTime() < DateTime.Now)
                {
                    //trigger alarm

                    Clocks.Remove(a);
                }
            }
        }

        //incoming "set alarm for three thirty"
        //incoming "set alarm for three thirty called something something something"
        //incomming "set timer for one hour and thirty minutes"
        //incomming "set timer for one hour and thirty minutes called something something something"
        //set timer will be trimmed
        private alarm convertTimerString(string time)
        {

            List<string> words = time.Split(' ').ToList<String>();

            if (words.Contains("alarm"))
            {
                DateTime dt = Convert.ToDateTime(NumberToInt(words[3]).ToString() + ":" + NumberToInt(words[4]).ToString() + ":00");
                //assume alarm is for the future
                while (dt < DateTime.Now)
                {
                    dt = dt.AddHours(12);
                }
                int calledIndex = words.IndexOf("called");
                string message = "unknow alarm";
                if (calledIndex != -1)
                {
                    //need magic to put spaces back in here
                    List<string> args = words.GetRange(calledIndex + 1, words.Count - 1 - calledIndex);
                    //need magic to put spaces back in here
                    int argsStartCount = args.Count;
                    for (int i = 0; i < argsStartCount - 1; i++)
                    {
                        args.Insert((i * 2) + 1, " ");
                    }

                    message = String.Concat(args);
                }
                alarm newA = new alarm(dt, message);
                return newA;
            }
            else if (words.Contains("timer"))
            {
                int hoursIndex = -1;
                hoursIndex = words.IndexOf("hour");
                if (hoursIndex == -1)
                {
                    hoursIndex = words.IndexOf("hours");
                }

                int minuteIndex = -1;
                minuteIndex = words.IndexOf("minute");
                if (minuteIndex == -1)
                {
                    minuteIndex = words.IndexOf("minutes");
                }

                int hour = 0;
                int minute = 0;

                if (hoursIndex != -1)
                {
                    hour = NumberToInt(words[hoursIndex - 1]);
                }
                if (minuteIndex != -1)
                {
                    minute = NumberToInt(words[minuteIndex - 1]);
                }
                TimeSpan ts = new TimeSpan(hour, minute, 0);
                DateTime dt = DateTime.Now + ts;
                string message = "unknow timmer";
                int calledIndex = words.IndexOf("called");
                if (calledIndex != -1)
                {
                    //need magic to put spaces back in here
                    List<string> args = words.GetRange(calledIndex + 1, words.Count - 1 - calledIndex);
                    //need magic to put spaces back in here
                    int argsStartCount = args.Count;
                    for (int i = 0; i < argsStartCount - 1; i++)
                    {
                        args.Insert((i * 2) + 1, " ");
                    }

                    message = String.Concat(args);
                }
                alarm newA = new alarm(dt, message);
                return newA;
            }
            else
            {
                //report back needs to be formatted correctly
                return new alarm();
            }

        }

        private int NumberToInt(string str)
        {
            switch (str)
            {
                case "one": return 1;
                case "two": return 2;
                case "three": return 3;
                case "four": return 4;
                case "five": return 5;
                case "six": return 6;
                case "seven": return 7;
                case "eight": return 8;
                case "nine": return 9;
                case "ten": return 10;
                case "eleven": return 11;
                case "twelve": return 12;
                case "thirteen": return 13;
                case "fourteen": return 14;
                case "fifteen": return 15;
                case "sixteen": return 16;
                case "seventeen": return 17;
                case "eighteen": return 18;
                case "nineteen": return 19;
                case "twenty": return 20;
                case "thirty": return 30;
                case "forty": return 40;
                case "fifty": return 50;
                case "sixty": return 60;
                case "seventy": return 70;
                case "eighty": return 80;
                case "ninety": return 90;
                default: return 0;
            }

        }
    }
}
