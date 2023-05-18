using Microsoft.AspNetCore.SignalR;
using SignalRChatServer.Api.Models.Enums;

namespace SignalRChatServer.Api.Services;

public class Chat : Hub
{
    private static List<Message>? _messages;

    public Chat()
    {
        if (_messages is null)
            _messages = new List<Message>();
    }

    public async Task NewMessage(string userName, string message)
    {
        await Clients.All.SendAsync(nameof(NewMessage), userName, message);
        _messages?.Add(new Message(userName, message));
    }

    public async Task NewUser(string userName, string connectionId)
    {
        await Clients.Client(connectionId).SendAsync(HubMethod.PreviousMessages.ToString(), _messages);
        await Clients.All.SendAsync(nameof(NewUser), userName);
    }
}

public class Message
{
    public Message(string userName, string text)
    {
        UserName = userName;
        Text = text;
    }

    public string UserName { get; set; }
    public string Text { get; set; }
}
