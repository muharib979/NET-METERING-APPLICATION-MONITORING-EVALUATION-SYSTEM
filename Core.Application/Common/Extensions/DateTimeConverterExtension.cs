namespace Core.Application.Common.Extensions;

public static class DateTimeConverterExtension
{
    public static DateTime UnixToDateTime(this long unixTimeStamp)
    {
        DateTime  dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        dateTime = dateTime.AddSeconds(Convert.ToDouble(unixTimeStamp)).ToLocalTime();
    
        return dateTime;
    }

    //public static DateTime UnixToDateTimeMs(this long unixTimeStamp)
    //{
    //    System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
    //    dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
    //    return dtDateTime; ;
    //}

    public static DateTime UnixToDateTimeMs(this long unixTimeStamp)
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(unixTimeStamp).DateTime.ToLocalTime();

    }
    public static long DateTimeToUnixMs(this DateTime dateTime)
    {
        return new DateTimeOffset(dateTime.ToUniversalTime(), TimeSpan.Zero).ToUnixTimeMilliseconds();
    }

    public static long DateTimeToUnix(this DateTime dateTime)
    {
        var epoch = dateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToLocalTime();

        return (long)epoch.TotalSeconds;
    }

    public static DateTime DateTimeToSigaporeDateTime(this DateTime dateTime)
    {
        var siTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
        return TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.Local, siTimeZone);
    }

    public static DateTime DateTimeUTCToSigaporeDateTime(this DateTime dateTime)
    {

        var siTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
        return TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.Utc, siTimeZone);
       
    }
}
