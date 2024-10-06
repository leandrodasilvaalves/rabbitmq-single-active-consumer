using Bogus;

using MassTransit;

using Poc.MassTransit.SingleContainer;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMassTransit(builder.Configuration);

var app = builder.Build();
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