using Microsoft.AspNetCore.SignalR;

namespace SignalRChatServer.Api.HubService;

public class Chat : Hub
{
    private static List<Message>? _messages;

    public Chat()
    {
        if(_messages is null)
            _messages = new List<Message>();    
    }

    public void NewMessage(string userName, string message)
    {
        Clients.All.SendAsync("NewMessage", userName, message).Wait();
        _messages?.Add(new Message(userName, message));
    }

    public void NewUser(string userName, string connectionId)
    {
        Clients.Client(connectionId).SendAsync("previousMessages", _messages).Wait();
        Clients.All.SendAsync("NewUser", userName).Wait();
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
