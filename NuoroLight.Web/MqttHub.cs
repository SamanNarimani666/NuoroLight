using Microsoft.AspNetCore.SignalR;
using NuoroLight.Application.Utils;

namespace NuoroLight.Web.Hubs
{
    public class MqttHub : Hub
    {
        // متد برای ارسال پیام به کلاینت‌ها
        public async Task SendMessage(string message)
        {
            var shamsiDate = $"{DateTime.Now.ToShamsiDate()} - {DateTime.Now:HH:mm:ss}";
            await Clients.All.SendAsync("ReceiveMessage", message, shamsiDate);
        }
    }
}

