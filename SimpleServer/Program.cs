// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Net.Sockets;
using System.Text;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

var hostName = Dns.GetHostName();

Console.WriteLine($"Мій хост: {hostName}");

var localHost = await Dns.GetHostEntryAsync( hostName );
int i = 0;
foreach (var item in localHost.AddressList)
{
    Console.WriteLine($"{++i}. {item}");
}

Console.WriteLine("->");
int numberIP = int.Parse(Console.ReadLine());
int serverPort = 2009;
var serverIP = localHost.AddressList[numberIP-1];
Console.Title = $"{serverIP}: {serverPort}";

var ipEndPoint = new IPEndPoint(serverIP, serverPort);

Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
try
{
    server.Bind(ipEndPoint);
    server.Listen(10);

    while (true)
    {
        Socket client = server.Accept();
        Console.WriteLine($"Нам постукав наступний носоріг {client.RemoteEndPoint}");
        int bytes = 0;
        byte[] buffer = new byte[1024];

        do
        {
            bytes = await client.ReceiveAsync(buffer);
            Console.WriteLine($"Повідомлення {Encoding.Unicode.GetString(buffer)}");
        }
        while (client.Available > 0);

        string message = $"Дякую {DateTime.Now}";
        buffer = Encoding.Unicode.GetBytes(message);
        client.Send(buffer);
        client.Shutdown(SocketShutdown.Both);
        client.Close();

    }
    
}
catch (Exception e)
{
    Console.WriteLine($"Помилка { e.Message}");
}

Console.ReadKey();
