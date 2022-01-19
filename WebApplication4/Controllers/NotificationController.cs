using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using WebApplication4.Hubs;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly NotificationHub _notificationHub;

        public NotificationController(IHubContext<NotificationHub> hubContext , NotificationHub notificationHub)
        {
            _hubContext = hubContext;
            _notificationHub = notificationHub;
        }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage(NotificationModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", model.User, model.Message);

            return Ok(model);
        }
    }
}
