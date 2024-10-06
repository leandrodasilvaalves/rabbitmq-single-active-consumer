
using System.Diagnostics;
using System.Text.Json;

using MassTransit;

using Microsoft.Extensions.Options;

namespace Poc.MassTransit.SingleContainer;

public class SingleConsumer : IConsumer<SingleConsumerModel>
{
    private readonly MessageBrokerQueueSettings _settings;
    public SingleConsumer(IOptions<MessageBrokerQueueSettings> options)
    {
        _settings = options.Value;
    }

    public Task Consume(ConsumeContext<SingleConsumerModel> context)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        try
        {
            var message = context.Message;
            message.ConsumerName = _settings.ConsumerName;

            Console.WriteLine("Message: {0}", JsonSerializer.Serialize(message));
            return Task.CompletedTask;
        }
        finally
        {
            Metric.Consumer.Observe(stopwatch.ElapsedMilliseconds);
        }
    }
}

public class SingleConsumerModel
{
    public string ConsumerName { get; set; }
    public string Message { get; set; }
}