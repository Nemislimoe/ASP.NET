using System;

namespace Lab13.Services
{
    public class DateTimeService : IDateTimeService
    {
        public string GetNowFormatted() => DateTime.Now.ToString("dd.MM.yyyy HH:mm");
    }
}
