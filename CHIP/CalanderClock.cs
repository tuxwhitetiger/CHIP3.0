using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using DateTime;

namespace CHIP
{
    internal class CalanderClock
    {
        List<DateTime> Clocks = new List<DateTime>();
        public CalanderClock() { }

        public string getTime() {

            return DateTime.Now.ToString();
        }

        public void setTimer(string time)
        {
            DateTime timer = DateTime.Now + convertTimerString(time);
            Clocks.Add(timer);
        }

        public void checkTimers() {
            foreach( DateTime dt in Clocks) {
                if (dt < DateTime.Now) { 
                    //trigger alarm

                    Clocks.Remove(dt);
                }
            }
        }

        //incomming "set timer for one hour and 30 minutes"
        //set timer will be trimmed
        private TimeSpan convertTimerString(string time) {

            List<string> words = time.Split(' ').ToList<String>();
            int hoursIndex = -1;
            hoursIndex = words.IndexOf("hour");
            hoursIndex = words.IndexOf("hours");

            int minuteIndex = -1;
            minuteIndex = words.IndexOf("minute");
            minuteIndex = words.IndexOf("minutes");

            int hour = 0;
            int minute = 0;

            if (hoursIndex != -1) { 
                hour = NumberToInt(words[hoursIndex-1]);
            }
            if (minuteIndex != -1) {
                minute = NumberToInt(words[minuteIndex-1]); 
            }
            TimeSpan ts = new TimeSpan(hour,minute,0);
            return ts;
        }

        private int NumberToInt(string str)
        {
            switch(str)
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
