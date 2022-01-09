using System.Text;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MessageSender.Services;

public class SenderService
{
    private readonly ILogger<SenderService> _logger;

    // ReSharper disable once NotAccessedField.Local
    private readonly IConfiguration _configuration;
    private readonly EventHubProducerClient _eventHubProducerClient;
    private readonly List<Order> _orders;

    public SenderService(ILogger<SenderService> logger, IConfiguration configuration,
        EventHubProducerClient eventHubProducerClient)
    {
        _configuration = configuration;
        _eventHubProducerClient = eventHubProducerClient;
        _logger = logger;
        _orders = new List<Order>
        {
            new Order {Id = 1, Name = "name 1", Qty = 2, Price = 3},
            new Order {Id = 2, Name = "name 2", Qty = 4, Price = 6},
            new Order {Id = 3, Name = "name 2", Qty = 6, Price = 9},
        };
    }

    public async Task Run()
    {
        _logger.LogInformation("Starting...");
        var batch = await _eventHubProducerClient.CreateBatchAsync();
        _orders.ForEach(o => { batch.TryAdd(new EventData(Encoding.UTF8.GetBytes(o.ToString()))); });
        await _eventHubProducerClient.SendAsync(batch);
        _logger.LogInformation("Finished!");
    }
}