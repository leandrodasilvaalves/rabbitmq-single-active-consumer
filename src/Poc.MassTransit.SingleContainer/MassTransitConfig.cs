using MassTransit;

namespace Poc.MassTransit.SingleContainer;

public static class MassTransitConfig
{
    public static void AddMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = ConfigureSettings(services, configuration);
        services.AddMassTransit(x =>
        {
            x.AddConsumer<SingleConsumer>();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(settings.HostName, settings.VirtualHost, h =>
                {
                    h.Username(settings.UserName);
                    h.Password(settings.Password);
                });

                var queueName = "single.consumer.queue";
                cfg.Message<SingleConsumerModel>(c => c.SetEntityName(queueName));
                cfg.ReceiveEndpoint(queueName, x =>
                {
                    x.PrefetchCount = 3;
                    x.SingleActiveConsumer = true;
                    x.Consumer<SingleConsumer>(context, consumer =>
                    {
                        consumer.UseRateLimit(5, TimeSpan.FromSeconds(10));
                        consumer.UseConcurrencyLimit(2);
                    });
                });
            });
        });
    }

    private static MessageBrokerQueueSettings ConfigureSettings(IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("MessageBroker:QueueSettings");
        services.Configure<MessageBrokerQueueSettings>(section);
        return section.Get<MessageBrokerQueueSettings>();
    }
}

public class MessageBrokerQueueSettings
{
    public string HostName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public int Port { get; set; }
    public string VirtualHost { get; set; }

    public string ConsumerName { get; set; }
}