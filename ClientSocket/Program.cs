// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.Sockets;
using System.Text;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("Вкажіть IP сервер:");
var ip = IPAddress.Parse(Console.ReadLine());
Console.WriteLine("Вкажіть порт:");
var port = int.Parse(Console.ReadLine());

try
{
    var serverEndPoint = new IPEndPoint(ip, port);
    Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    sender.Connect(serverEndPoint);
    Console.WriteLine("Вкажіть повідомлення");
    string text = Console.ReadLine();
    var data = Encoding.Unicode.GetBytes(text);
    sender.Send(data);
    data = new byte[1024];
    int bytes = 0;
    do
    {
        bytes = sender.Receive(data);
        Console.WriteLine($"Сервер відповів {Encoding.Unicode.GetString(data)}");
    }
    while (sender.Available > 0);
}
catch(Exception e)
{
    Console.WriteLine("Щось пішло не так " + e.Message);
}
