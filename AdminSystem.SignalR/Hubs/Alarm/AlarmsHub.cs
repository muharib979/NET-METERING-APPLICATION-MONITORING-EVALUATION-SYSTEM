using Microsoft.AspNetCore.SignalR;

namespace CFEMS.SignalR.Hubs.Alarm;

public class AlarmsHub : Hub<IAlarmsHub>
{
    public AlarmsHub()
    {

    }
    public async Task SendRefreshAlarmCount()
    {
        await Clients.All.RefreshAlarmCount("RefreshAlarmCount");
    }
}
