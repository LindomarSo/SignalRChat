using Microsoft.AspNetCore.SignalR.Client;

HubConnection _connection;

Console.WriteLine("Seja bem vindo!");

Console.WriteLine("Digite o seu nome: ");
string? _userName = Console.ReadLine();

Connect();

while (true)
{
    var message = Console.ReadLine();
    SendMessage(message);
}

async void SendMessage(string? message)
{
    try
    {
        await _connection.SendAsync("NewMessage", _userName, message);
    }
    catch
    {
        Console.WriteLine("Ocorreu um erro ao connectar...");
    }
}

async void Connect()
{
    _connection = new HubConnectionBuilder()
                                          .WithUrl("https://localhost:44362/chat")
                                          .Build();

    _connection.On<string>("NewUser", (user) =>
    {
        var message = user == _userName ? "Você entrou na sala" : $"{user} acabou de entrar na sala";
        Console.WriteLine(message);
    });

    _connection.On<string, string>("NewMessage", (user, message) =>
    {
        if (user != _userName)
            Console.WriteLine($"{user}: {message}");
    });

    _connection.On<List<Message>>("previousMessages", (messages) =>
    {
        foreach(var item in messages)
            Console.WriteLine($"{item.UserName}: {item.Text}");
    });

    try
    {
        await _connection.StartAsync();
        await _connection.SendAsync("NewUser", _userName, _connection.ConnectionId);
    }
    catch
    {
        Console.WriteLine("Ocorreu um erro ao connectar...");
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
