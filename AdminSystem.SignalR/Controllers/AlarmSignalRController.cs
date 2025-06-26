using CFEMS.SignalR.Hubs.Alarm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CFEMS.SignalR.Controllers
{
    [ApiController]
    public class AlarmSignalRController : ControllerBase
    {
        private readonly IHubContext<AlarmsHub, IAlarmsHub> _hub;
        private readonly ILogger<AlarmSignalRController> _logger;

        public AlarmSignalRController(IHubContext<AlarmsHub, IAlarmsHub> hub, ILogger<AlarmSignalRController> logger)
        {
            _hub = hub;
            _logger = logger;
        }

        [HttpGet("/api/alarm-trigger")]
        public async Task<IActionResult> AlarmTrigger(string apiKey)
        {
            if (apiKey == "1c852fac-4271-42b7-b0x91-48d7579ecdee")
            {
                try
                {
                    await _hub.Clients.All.RefreshAlarmCount("RefreshAlarmCount");
                    _logger.LogInformation("SignalR successfully triggerd for alarms");
                    return Ok(new { Message = "SignalR successfully triggerd for alarms" });
                }
                catch (Exception ex)
                {

                    _logger.LogError(ex.Message);
                    return BadRequest(ex.Message);                    
                }                
            }
            else
            {
                _logger.LogWarning("Unauthorized");
                return Unauthorized();
            }

        }
    }
}
