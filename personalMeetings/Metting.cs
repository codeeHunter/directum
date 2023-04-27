using personalMeetings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace personalMeetings
{
    internal class Meeting
    {
        public Guid userId;
        public DateTime date;
        public DateTime startTime;
        public DateTime endTime;
        public DateTime reminderTime;
        public Guid meetingId = Guid.NewGuid();

        public Meeting(DateTime day, DateTime start, DateTime end, DateTime reminder)
        {
            date = day;
            startTime = start;
            endTime = end;
            reminderTime = reminder;
        }

        public void printMeeting()
        {
            string dateFormatted = date.ToString("dd/MM/yyyy");
            string startTimeFormatted = startTime.ToString("HH:mm");
            string endTimeFormatted = endTime.ToString("HH:mm");
            string reminderFormatted = reminderTime.ToString("dd/MM/yyyy HH:mm");

            Console.WriteLine($"Встреча назначена на {dateFormatted}.\nВремя встречи {startTimeFormatted} - {endTimeFormatted} \nНапомнить: {reminderFormatted} \n");
        }

        public void editMeeting(DateTime? day = null, DateTime? start = null, DateTime? end = null, DateTime? reminder = null)
        {
            if (day != null)
            {
                date = (DateTime)day;
            }
            if (start != null)
            {
                startTime = (DateTime)start;
            }
            if (end != null)
            {
                endTime = (DateTime)end;
            }

            if (reminder != null)
            {
                reminderTime = (DateTime)reminder;
            }


            Console.WriteLine($"Данные у {meetingId} успешно изменены.");
        }


        public void checkReminder()
        {
            if (DateTime.Now >= reminderTime)
            {
                Console.WriteLine($"Напоминание:\n");
                printMeeting();
            }
        }
    }
}

