using Microsoft.AspNetCore.SignalR;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Packets;
using MQTTnet.Protocol;
using NuoroLight.Application.Services.Interfaces;
using NuoroLight.Infrastructure.Context;
using NuoroLight.Web.Hubs;
using System.Text;

namespace NuoroLight.Web.BackgroundServices
{
    public class MqttWorker : BackgroundService
    {
        private readonly ILogger<MqttWorker> _logger;
        private readonly IHubContext<MqttHub> _hubContext;
        private IMqttClient _client;
        private readonly IServiceProvider _serviceProvider;

        public MqttWorker(ILogger<MqttWorker> logger, IHubContext<MqttHub> hubContext, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _hubContext = hubContext;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var mqttFactory = new MqttFactory();


            _client = mqttFactory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId(Guid.NewGuid().ToString())
                .WithTcpServer("services.irn9.chabokan.net", 38042)
                .WithCredentials("james", "sgBsCDIb0vTMwdxC")
                .WithCleanSession()
                .Build();

            var topicFilters = new List<MqttTopicFilter>
            {
                new MqttTopicFilter { Topic = "lamp1", QualityOfServiceLevel = MqttQualityOfServiceLevel.AtLeastOnce },
                new MqttTopicFilter { Topic = "lamp2", QualityOfServiceLevel = MqttQualityOfServiceLevel.AtLeastOnce },
                new MqttTopicFilter { Topic = "lamp3", QualityOfServiceLevel = MqttQualityOfServiceLevel.AtLeastOnce },
                new MqttTopicFilter { Topic = "rgb", QualityOfServiceLevel = MqttQualityOfServiceLevel.AtLeastOnce }
            };

            _client.ConnectedAsync += async e =>
            {
                _logger.LogInformation("Connected to MQTT broker.");

                foreach (var topicFilter in topicFilters)
                {
                    await _client.SubscribeAsync(topicFilter);
                    _logger.LogInformation($"Subscribed to topic: {topicFilter.Topic}");
                }
            };

            _client.DisconnectedAsync += e =>
            {
                _logger.LogWarning($"Disconnected from MQTT broker. Reason: {e.Reason}");
                return Task.CompletedTask;
            };

            _client.ApplicationMessageReceivedAsync += async e =>
            {
                var message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                _logger.LogInformation($"Received Message - {message}");
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", message); // ارسال پیام به کلاینت‌ها

                using (var scope = _serviceProvider.CreateScope())
                {
                    var deviceService = scope.ServiceProvider.GetRequiredService<IDeviceService>();

                    await deviceService.AddLogs(message);
                }

            };

            await _client.ConnectAsync(options, stoppingToken);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_client.IsConnected)
            {
                await _client.DisconnectAsync();
            }

            await base.StopAsync(cancellationToken);
        }
    }
}
