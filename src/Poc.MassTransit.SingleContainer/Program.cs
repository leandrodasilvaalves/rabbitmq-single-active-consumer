using Bogus;

using MassTransit;

using Poc.MassTransit.SingleContainer;

using Prometheus;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMassTransit(builder.Configuration);
builder.Services.UseHttpClientMetrics();

var app = builder.Build();

app.UseHttpMetrics(o =>
{
    o.AddCustomLabel("host", ctx => ctx.Request.Host.Value);
});
app.MapMetrics();

app.UseSwagger();
app.UseSwaggerUI();

var faker = new Faker();
app.MapPost("/send-messages", async (IBus bus, RequestModel request) =>
{
    var endpoint = await bus.GetPublishSendEndpoint<SingleConsumerModel>();
    for (var i = 1; i <= request.Quantity; i++)
    {
        var message = new SingleConsumerModel { Message = faker.Lorem.Lines(1) };
        await endpoint.Send(message);
    }

    return Results.Ok();
})
.WithName("SendMessages")
.WithOpenApi();

app.Run();

public record RequestModel(int Quantity);