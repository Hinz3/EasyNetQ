# EasyNetQ ![Nuget](https://img.shields.io/nuget/v/Hinz3.EasyNetQ)
 EasyNetQ auto subscriber dependency implementation

## How to use

1. Add EasyNetQ and MessageDispatcher to your ServiceCollection using:
``` C#
builder.Services.UseRabbit(builder.Configuration); // Will use appsettings Connection strings looking for Rabbit
builder.Services.UseRabbit("host=localhost;virtualHost=sandbox;username=admin;password=password"); // Put in the connection string directly in the code, instead of using appsettings.
```

2. Add AutoSubscriber to Application builder.
``` C#
app.UseSubscribe(Assembly.GetExecutingAssembly().GetName().Name, Assembly.GetExecutingAssembly());
```

3. Add eventhandler/subscriber to service collection.
``` C#
builder.Services.AddSubscriber<UserUpdatedEventHandler>();
```

## Example
- Event:
``` C#
namespace EasyNetQDI.Events
{
    public class UserUpdatedEvent
    {
        public int UserId { get; set; }
    }
}

```

- Eventhandler:
``` C#
using EasyNetQ.AutoSubscribe;
using EasyNetQDI.Events;

namespace EasyNetQDI.SampleAPI
{
    public class UserUpdatedEventHandler : IConsumeAsync<UserUpdatedEvent>
    {
        private readonly ILogger<UserUpdatedEventHandler> logger;

        public UserUpdatedEventHandler(ILogger<UserUpdatedEventHandler> logger)
        {
            this.logger = logger;
        }

        public Task ConsumeAsync(UserUpdatedEvent message, CancellationToken cancellationToken = default)
        {
            logger.LogInformation($"Received: {message.UserId}");
            return Task.CompletedTask;
        }
    }
}
```

- Publish event:
``` C#
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
            await bus.PubSub.PublishAsync(new UserUpdatedEvent { UserId = userId }); // Pulish UserUpdatedEvent to RabbitMQ.
            return Ok();
        }
    }
}

```
