using MQTTnet;
using MQTTnet.Adapter;
using MQTTnet.Client;
using System.Text;

namespace nuorolight.web;
public class MqttService
{
    private readonly IMqttClient _mqttClient;
    public bool IsConnected => _mqttClient.IsConnected;

    public MqttService()
    {
        // ساخت یک کلاینت MQTT
        var mqttFactory = new MqttFactory();
        _mqttClient = mqttFactory.CreateMqttClient();

    }

    // متد اتصال به سرور MQTT با احراز هویت
    public async Task ConnectAsync()
    {

        try
        {
            var mqttOptions = new MqttClientOptionsBuilder()
                .WithClientId("AspNetCoreClient")  // شناسه کلاینت
                .WithTcpServer("services.irn9.chabokan.net", 38042)  // آدرس سرور MQTT
                .WithCredentials("james", "sgBsCDIb0vTMwdxC")  // نام کاربری و کلمه عبور
                .WithCleanSession()  // استفاده از نشست جدید
                .Build();

            await _mqttClient.ConnectAsync(mqttOptions);
            Console.WriteLine("Connected to MQTT Broker with authentication.");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while connecting: {ex.Message}");
        }

    }

    // متد برای ارسال پیام به یک تاپیک خاص
    public async Task PublishAsync(string topic, string message)
    {
        var mqttMessage = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(Encoding.UTF8.GetBytes(message))
            .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce)  // تنظیم QoS
            .WithRetainFlag(false)
            .Build();

        if (_mqttClient.IsConnected)
        {
            await _mqttClient.PublishAsync(mqttMessage);
            Console.WriteLine($"Message '{message}' sent to topic '{topic}'.");
        }
        else
        {
            Console.WriteLine("MQTT client is not connected.");
        }
    }

    // متد قطع اتصال
    public async Task DisconnectAsync()
    {
        await _mqttClient.DisconnectAsync();
        Console.WriteLine("Disconnected from MQTT Broker.");
    }
}