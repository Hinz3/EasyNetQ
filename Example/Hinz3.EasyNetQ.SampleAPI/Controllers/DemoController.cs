using EasyNetQ;
using EasyNetQDI.Events;
using Microsoft.AspNetCore.Mvc;

namespace EasyNetQDI.SampleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly IBus bus;

        public DemoController(IBus bus)
        {
            this.bus = bus;
        }

        [HttpGet]
        public async Task<IActionResult> SendMessage(int userId)
        {
            await bus.PubSub.PublishAsync(new UserUpdatedEvent { UserId = userId });
            return Ok();
        }
    }
}
