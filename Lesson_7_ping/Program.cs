using System.Net;
using System.Net.NetworkInformation;

class Program
{
    static void Main(string[] args)
    {
        string res = "yandex.ru";

        // получение ай пи связанных с данным хосто

        var IPAdresses = Dns.GetHostAddresses(res, System.Net.Sockets.AddressFamily.InterNetwork);

        foreach (var ip in IPAdresses)
        {
            Console.WriteLine(ip);
        }
        Console.WriteLine(IPAdresses.GetType());

        Dictionary<IPAddress, long> pings = new Dictionary<IPAddress, long>();

        List<Thread> listThread = new List<Thread>();

        foreach (var ip in IPAdresses)
        // создали потоки 
        {
            var t = new Thread(() =>
            {
                Ping ping = new Ping();
                PingReply pingReply = ping.Send(ip);

                pings.Add(ip, pingReply.RoundtripTime);
            });

            listThread.Add(t);
            t.Start();
        }

        foreach (var t in listThread)
        {
            t.Join();
        }

        long min = long.MaxValue;

        // вывод данных со словаря
        foreach (var item in pings)
        {
            if (item.Value < min)
            {
                min = item.Value;
            }

            Console.WriteLine($"IP: {item.Key},  ping: {item.Value}");
        }
        Console.WriteLine($"Минимальный пинг: {min}");

    }
}