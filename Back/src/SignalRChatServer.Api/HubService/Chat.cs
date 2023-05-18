using Microsoft.AspNetCore.SignalR;

namespace SignalRChatServer.Api.HubService;

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
        await Clients.All.SendAsync("NewMessage", userName, message);
        _messages?.Add(new Message(userName, message));
    }

    public async Task NewUser(string userName, string connectionId)
    {
        await Clients.Client(connectionId).SendAsync("previousMessages", _messages);
        await Clients.All.SendAsync("NewUser", userName);
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
