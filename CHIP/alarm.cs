using System;
using System.Collections.Generic;
using System.Text;

namespace CHIP
{
    internal class alarm
    {
        DateTime alarmTime;
        string alarmMessage;
        public alarm()
        {

        }

        public alarm(DateTime alarmTime, string alarmMessage)
        {
            this.alarmTime = alarmTime;
            this.alarmMessage = alarmMessage;
        }
        public DateTime GetDateTime()
        {
            return alarmTime;
        }
        public void setDateTime(DateTime alarmTime)
        {
            this.alarmTime = alarmTime;
        }
        public string GetAlarmMessage()
        {
            return alarmMessage;
        }

        public void SetAlarmMessage(string alarmMessage)
        {
            this.alarmMessage = alarmMessage;
        }

    }
}

