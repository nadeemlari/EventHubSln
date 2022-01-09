using System.Text;
using Azure.Messaging.EventHubs.Consumer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MessageReceiver.Services;

public class ReceiverService
{
    private readonly ILogger<ReceiverService> _logger;

    // ReSharper disable once NotAccessedField.Local
    private readonly IConfiguration _configuration;
    private readonly EventHubConsumerClient _hubConsumerClient;


    public ReceiverService(ILogger<ReceiverService> logger, IConfiguration configuration,
        EventHubConsumerClient hubConsumerClient)
    {
        _configuration = configuration;
        _hubConsumerClient = hubConsumerClient;
        _logger = logger;
    }

    public async Task Run()
    {
        _logger.LogInformation("Starting...");
        await foreach (var e in  _hubConsumerClient.ReadEventsAsync())
        {
            Console.WriteLine(Encoding.UTF8.GetString(e.Data.EventBody));
        }
        _logger.LogInformation("Finished!");
    }
}