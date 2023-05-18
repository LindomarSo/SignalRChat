namespace SignalRChatServer.Api.Services;

public interface IHubConnectionService
{
    Task SendAsync(string user, string message);
}