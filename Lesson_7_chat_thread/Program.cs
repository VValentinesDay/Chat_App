using Chat_Web_App;
using Lesson_7_chat_thread;
using System.Net;
using System.Net.NetworkInformation;

class Program
    // Версия с выходом с консольного приложения чере ReadLine
{
    static async Task Main(string[] args)
    {
        if (args.Length == 0)
        {

            await Server.AcceptMess();
        }
        else
        {
            for (int i = 0; i < 1; i++)
            {

            await Client.SendMessage($"{args[0]}  {i} - число с Main");
            }

        }
    }
}