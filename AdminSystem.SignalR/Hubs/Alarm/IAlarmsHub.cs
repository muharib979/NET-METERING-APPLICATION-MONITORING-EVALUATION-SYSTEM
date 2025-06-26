namespace CFEMS.SignalR.Hubs.Alarm;

public interface IAlarmsHub
{
    Task RefreshAlarmCount(string message);
}
