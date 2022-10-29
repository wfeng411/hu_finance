using System;

namespace hu_app.Shared
{
    public static class HuTool
    {
        public static DateTime GetCleanDate(this DateTime date)
        {
            var year = date.Year;
            var month = date.Month;
            var day = date.Day;
            return new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Unspecified);
        }

        public static DateTime? GetCleanDate(this DateTime? date)
        {
            if (!date.HasValue)
            {
                return null;
            }
            var year = date.Value.Year;
            var month = date.Value.Month;
            var day = date.Value.Day;
            return new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Unspecified);
        }
    }
}
