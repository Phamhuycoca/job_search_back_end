using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
namespace job_search_be.Api.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }
    }
}
