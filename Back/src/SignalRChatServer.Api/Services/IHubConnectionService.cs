using SignalRChatServer.Api.Models.Enums;

namespace SignalRChatServer.Api.Services;

public interface IHubConnectionService
{
    Task SendAsync(HubMethod method, string user, string message);
}