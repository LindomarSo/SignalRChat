using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using SignalRChatServer.Api.Models;

namespace SignalRChatServer.Api.Services;

public class HubConnectionService : IHubConnectionService
{
    private HubConnectionModel _hubConnection;
    private readonly HubConnection _connection;

    public HubConnectionService(IOptions<HubConnectionModel> options)
    {
        _hubConnection = options.Value;
        _connection = new HubConnectionBuilder().WithUrl(_hubConnection.Url).Build();
    }

    public async Task SendAsync(string user, string message)
    {
        await Connect();
        await _connection.SendAsync("NewMessage", user, message);
    }

    private async Task Connect()
    {
        await _connection.StartAsync();
    }
}
