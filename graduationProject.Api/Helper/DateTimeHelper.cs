using System;

namespace graduationProject.Helper
{
    public static class DateTimeHelper
    {
        public static string FormatDate(DateTime dateTime)
        {
            // Convert the given DateTime value to Egypt time zone
            DateTime dateTimeInEgyptTimeZone = ConvertToEgyptTimeZone(dateTime);

            // Format the date in "yyyy/MM/dd" format
            return dateTimeInEgyptTimeZone.ToString("yyyy/MM/dd");
        }

        public static string FormatTime(DateTime dateTime)
        {
            // Convert the given DateTime value to Egypt time zone
            DateTime dateTimeInEgyptTimeZone = ConvertToEgyptTimeZone(dateTime);

            // Format the time in "HH:mm:ss" format
            return dateTimeInEgyptTimeZone.ToString("HH:mm:ss");
        }

        // Method to convert a DateTime value to Egypt time zone
        private static DateTime ConvertToEgyptTimeZone(DateTime dateTime)
        {
            // Find Egypt time zone
            TimeZoneInfo egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");

            // Convert the given DateTime value to Egypt time zone
            DateTime dateTimeInEgyptTimeZone = TimeZoneInfo.ConvertTime(dateTime, egyptTimeZone);

            return dateTimeInEgyptTimeZone;
        }
    }
}
