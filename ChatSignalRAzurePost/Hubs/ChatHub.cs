using Microsoft.AspNetCore.SignalR;

namespace ChatSignalRAzurePost.Hubs
{
    public class ChatHub : Hub
    {
        public Task BroadcastMessage(string name, string message) =>
            Clients.All.SendAsync("receiveMessage", name, message);

        public Task Echo(string name, string message) =>
            Clients.Client(Context.ConnectionId)
                   .SendAsync("echo", name, $"{message} (echo from server)");
    }
}
