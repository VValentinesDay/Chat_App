using Chat_Web_App;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_7_chat_thread
{
    internal class Server
    {
        public static bool ExitVar = true;

        public static void Exit()
        {
            string status = Console.ReadLine();
            if(status == "EXIT") { ExitVar = false; }
            
        }


        public static async Task AcceptMess()
        {
            //IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            //UdpClient udpClient = new UdpClient(16876);

            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            UdpClient udpClient = new UdpClient(16876);
            Console.WriteLine("Waiting for message");
            Console.WriteLine("Для выхода ввести EXIT");

            while (ExitVar) // бесконечный цикл пока сервер ждёт сообщения
            {
                Task exit = Task.Run(() => Exit());


                var data =  udpClient.Receive(ref iPEndPoint);
                //byte[] buffer = data.Buffer;
                string str = Encoding.UTF8.GetString(data);
                //Message? message = Message.FromJson(str);
                //Console.WriteLine("Отправитель: " + message.Name + "Текст: " + message.Text);
                await Task.Run(async () =>
                {
                    Message? message = Message.FromJson(str);
                    Console.WriteLine(message?.ToString());
                    Console.WriteLine("Отправитель: " + message.Name + "Текст: " + message.Text);

                    Message response = new Message () { Name = "Server", Text = "Accepted", DateTime = DateTime.Now };
                    string resp = response.ToJson();
                    byte[] respBytes = Encoding.UTF8.GetBytes(resp);
                    await udpClient.SendAsync(respBytes, iPEndPoint);
                });
               
            }

        }

    }
}
