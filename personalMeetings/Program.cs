using personalMeetings;
using System.Globalization;

class Program
{
    public static void getDash()
    {
        Console.WriteLine("\n--------------------------------\n");
    }

    public static List<DateTime> createDate(DateTime? MeetingDay = null, DateTime? timeStart = null, DateTime? timeEnd = null, DateTime? reminder = null)
    {
        bool isDay = false, isStartDate = false, isEndDate = false, isReminderTime = false;
        string createOrEdit = "";

        DateTime day = MeetingDay == null ? DateTime.Today : (DateTime)MeetingDay;
        DateTime startTime = timeStart == null ? DateTime.Today : (DateTime)timeStart;
        DateTime endTime = timeEnd == null ? DateTime.Today : (DateTime)timeEnd;
        DateTime reminderTime = reminder == null ? DateTime.Today : (DateTime)reminder;

        if (MeetingDay != null || timeStart != null || timeEnd != null)
        {
            createOrEdit = "изменена";
        }
        else
        {
            createOrEdit = "создана";
        }


        while (!isDay | !isStartDate | !isEndDate | !isReminderTime)
        {
            while (!isDay)
            {
                Console.WriteLine("Введите дату формате dd/MM/yyyy");
                Console.Write("Введите день встречи: ");
                string inputDay = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(inputDay))
                {
                    isDay = DateTime.TryParseExact(inputDay, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.NoCurrentDateDefault, out day);

                    if (!isDay)
                    {
                        Console.WriteLine("Некорректный формат даты.");
                        getDash();
                    }
                    else
                    {
                        if (day < DateTime.Today)
                        {
                            Console.WriteLine("Встречу нельзя назначить на прошедшее время.");
                            isDay = false;
                            getDash();
                        }
                    }
                }
                else
                {
                    if (createOrEdit == "изменена")
                    {
                        isDay = true;
                    }
                }
            }

            while (!isStartDate)
            {
                Console.WriteLine("Введите время формате HH:mm");
                Console.Write("Введите начала встречи: ");
                string inputStartDate = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(inputStartDate))
                {
                    isStartDate = DateTime.TryParseExact(inputStartDate, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.NoCurrentDateDefault, out startTime);

                    if (!isStartDate)
                    {
                        Console.WriteLine("Некорректный формат даты.");
                        getDash();
                    }
                }
                else
                {
                    if (createOrEdit == "изменена")
                    {
                        isStartDate = true;
                    }
                }
            }


            DateTime startDateTime = day + startTime.TimeOfDay;
            if (startDateTime <= DateTime.Now && isDay)
            {
                Console.WriteLine("Встречу нельзя назначить на прошедшее время.");
                isStartDate = false;
                startTime = DateTime.MinValue;
                getDash();
                continue;
            }


            if (isStartDate)
            {

                while (!isEndDate)
                {
                    Console.Write("Введите примерное время окончания встречи: ");
                    string inputEndDate = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(inputEndDate))
                    {
                        isEndDate = DateTime.TryParseExact(inputEndDate, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.NoCurrentDateDefault, out endTime);

                        if (!isEndDate)
                        {
                            Console.WriteLine("Некорректный формат даты.");
                            getDash();
                        }
                    }
                    else
                    {
                        if (createOrEdit == "изменена")
                        {
                            isEndDate = true;
                        }
                    }
                }
            }

            while (!isReminderTime)
            {
                Console.WriteLine("Введите время формате dd/MM/yyyy HH:mm");
                Console.Write("Введите время для уведомления: ");
                string inputReminderTime = Console.ReadLine();

                DateTime startDate = day.AddTicks(startTime.TimeOfDay.Ticks);

                if (!string.IsNullOrWhiteSpace(inputReminderTime))
                {
                    isReminderTime = DateTime.TryParseExact(inputReminderTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.NoCurrentDateDefault, out reminderTime);

                    if (!isReminderTime)
                    {
                        Console.WriteLine("Некорректный формат даты.");
                        getDash();
                    }
                    else if (isReminderTime && reminderTime > startDate)
                    {
                        Console.WriteLine("Время уведомления не может быть позже времени начала встречи.");
                        isReminderTime = false;
                    }
                }
                else
                {
                    if (createOrEdit == "изменена")
                    {
                        isReminderTime = true;
                    }
                }
            }




            if (startTime < endTime)
            {
                Console.WriteLine($"Встреча {day:dd/MM/yyyy} {createOrEdit}: {startTime:HH\\:mm} - {endTime:HH\\:mm}");
            }
            else
            {
                if (createOrEdit != "изменена")
                {
                    isStartDate = false;
                    startTime = DateTime.MinValue;
                    isEndDate = false;
                    endTime = DateTime.MinValue;
                    Console.WriteLine("Начало встречи не может быть больше окончания.");
                    getDash();
                }

            }
        }

        getDash();

        List<DateTime> timeMeeting = new List<DateTime>() { day, startTime, endTime, reminderTime };

        return timeMeeting;
    }


    public static Meeting createMeeting()
    {
        List<DateTime> times = createDate();

        DateTime day = times[0];
        DateTime startTime = times[1];
        DateTime endTime = times[2];
        DateTime reminder = times[3];

        Meeting meet = new(day, startTime, endTime, reminder);

        return meet;
    }


    public static void editMeeting(List<Meeting> meetings, Meeting meeting)
    {
        Console.WriteLine("Чтобы пропустить изменения поля, просто нажмите Enter");

        getDash();


        List<DateTime> times = createDate(meeting.date, meeting.startTime, meeting.endTime);

        DateTime day = times[0];
        DateTime startTime = times[1];
        DateTime endTime = times[2];
        DateTime reminder = times[3];

        Meeting editMeeting;
        if (meeting.startTime != startTime | meeting.endTime != endTime | meeting.date != day | meeting.reminderTime == reminder)
        {
            editMeeting = new(day, startTime, endTime, reminder);
            if (CheckIntersect(meetings, editMeeting))
            {
                meeting.editMeeting(day, startTime, endTime);
            }
        }
    }


    public static void printMeetings(List<Meeting> meetings)
    {
        for (int i = 0; i < meetings.Count; i++)
        {
            Console.Write($"{i + 1}.");
            meetings[i].printMeeting();
        }
    }


    public static bool isIntersect(Meeting m1, Meeting m2)
    {
        // Если дата встреч не совпадает, значит они не могут пересекаться
        if (m1.date != m2.date)
        {
            return false;
        }

        // Если время начала одной встречи находится в середине другой встречи,
        // значит они пересекаются
        if (m1.startTime >= m2.startTime && m1.startTime < m2.endTime ||
            m2.startTime >= m1.startTime && m2.startTime < m1.endTime)
        {
            return true;
        }

        // Если время окончания одной встречи находится в середине другой встречи,
        // значит они пересекаются
        if (m1.endTime > m2.startTime && m1.endTime <= m2.endTime ||
            m2.endTime > m1.startTime && m2.endTime <= m1.endTime)
        {
            return true;
        }

        return false;
    }

    public static bool CheckIntersect(List<Meeting> meetings, Meeting newMeeting)
    {
        foreach (Meeting meeting in meetings)
        {

            if (isIntersect(newMeeting, meeting))
            {
                Console.WriteLine("Ошибка: Встреча пересекается с существующей.");
                getDash();
                return false;
            }
        }
        return true;
    }


    public static void Main()
    {

        List<Meeting> meetings = new List<Meeting>();

        while (true)
        {
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1. Посмотреть список встреч.");
            Console.WriteLine("2. Добавить новую встречу.");
            Console.WriteLine("3. Изменить существующую встречу.");
            Console.WriteLine("4. Удалить существующую встречу.");
            Console.WriteLine("5. Экспорт встреч за выбранный день в файл.");
            Console.WriteLine("6. Выход.");

            string choice = Console.ReadLine();

            foreach (Meeting meeting in meetings)
            {
                meeting.checkReminder();
            }


            // Сама программа.
            switch (choice)
            {
                case "1":
                    getDash();
                    Console.WriteLine("1. Посмотреть список встреч.\n");
                    printMeetings(meetings);
                    break;

                case "2":
                    getDash();
                    Console.WriteLine("2. Добавить новую встречу.\n");
                    Meeting newMeeting = createMeeting();
                    if (CheckIntersect(meetings, newMeeting))
                    {
                        meetings.Add(newMeeting);
                    }

                    break;

                case "3":
                    getDash();
                    Console.WriteLine("3. Изменить существующую встречу.\n");
                    while (true)
                    {
                        if (meetings.Count > 0)
                        {
                            Console.WriteLine("Выберите встречу: \n");
                            printMeetings(meetings);
                            int editChoice = int.Parse(Console.ReadLine());

                            if (meetings.Count >= editChoice && editChoice > 0)
                            {
                                editMeeting(meetings, meetings[editChoice - 1]);
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Вы не выбрали из списка.");
                                break;
                            }
                        }

                    }
                    break;

                case "4":
                    getDash();
                    Console.WriteLine("4. Удалить существующую встречу.\n");
                    Console.WriteLine("Выберите встречу: ");
                    printMeetings(meetings);
                    int deleteChoice = int.Parse(Console.ReadLine());

                    if (meetings.Count >= deleteChoice && deleteChoice > 0)
                    {
                        Console.WriteLine($"Встреча {deleteChoice} успешно удалена");
                        meetings.RemoveAt(deleteChoice - 1);
                        getDash();
                    }
                    else
                    {
                        Console.WriteLine("Вы не выбрали из списка.");
                    }

                    break;

                case "5":
                    getDash();
                    Console.WriteLine("Введите день, за который нужно экспортировать расписание встреч dd/MM/гг:");
                    string exportDateStr = Console.ReadLine();
                    DateTime exportDate = DateTime.ParseExact(exportDateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    string fileName = $"meetings_{exportDate.ToString("yyyyMMdd")}.txt";
                    bool flag = false;
                    using (StreamWriter sw = new StreamWriter(fileName))
                    {
                        foreach (Meeting meeting in meetings)
                        {
                            if (meeting.date == exportDate.Date)
                            {
                                sw.WriteLine($"Встреча '{meeting.date.ToString("dd/MM/yyyy")}' состоится: {meeting.startTime.ToString("HH:mm")} - {meeting.endTime.ToString("HH:mm")}");
                                flag = true;
                            }
                        }
                    }

                    if(flag)
                    {
                        Console.WriteLine($"Расписание встреч за {exportDateStr} успешно экспортировано в файл '{fileName}'.");
                    }
                    break;

                case "6":
                    getDash();

                    Console.WriteLine("\n5. Выход из личного кабинета.");
                    return;
                default:
                    Console.WriteLine("\n Некорректный выбор.");
                    break;
            }


        }
    }
}

