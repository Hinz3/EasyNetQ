using EasyNetQ.AutoSubscribe;
using EasyNetQDI.Events;

namespace EasyNetQDI.SampleSecondAPI
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
            logger.LogInformation($"Received second api: {message.UserId}");
            return Task.CompletedTask;
        }
    }
}
