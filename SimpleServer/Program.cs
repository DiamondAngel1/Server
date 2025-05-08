// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Net.Sockets;
using System.Text;
using SimpleServer;

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
Console.WriteLine("Вкажіть порт:");
int serverPort = int.Parse(Console.ReadLine());
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
        string userIP = client.RemoteEndPoint.ToString();
        Console.WriteLine($"Нам постукав наступний носоріг {userIP}");
        
        byte[] buffer = new byte[1024];
        int bytes = await client.ReceiveAsync(buffer);
        string messageText = Encoding.Unicode.GetString(buffer,0,bytes);

            Console.WriteLine($"Повідомлення {messageText}");
        using (var context = new SerContext())
        {
            try
            {
                var message = new Message { Text = messageText, UserIP = userIP };
                context.Messages.Add(message);
                await context.SaveChangesAsync();
                Console.WriteLine("Повідомлення успішно збережено в БД.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Детальна помилка: {ex.InnerException.Message}");
                }
            }
        }
        
        string response = $"Дякую {DateTime.Now}";
        buffer = Encoding.Unicode.GetBytes(response);
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
