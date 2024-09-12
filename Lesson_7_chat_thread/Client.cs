using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Chat_Web_App;

namespace Lesson_7_chat_thread
{
    internal class Client
    {
        public async static Task SendMessage(string name)

        {

            //IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 16876);
            // any не указывается здесь
            UdpClient udpClient = new UdpClient();

            while (true)
            {

                Console.WriteLine("Введите имя получателя");
                string toName = Console.ReadLine();

                if (String.IsNullOrEmpty(toName))
                {
                    Console.WriteLine("Вы не ввели имя пользователя");
                    continue;
                }

                Console.WriteLine("Введите сообщение");
                string text = Console.ReadLine();

                if (text.ToLower() == "ex") {
                    break;
                }



                Message message = new Message { Name = name, FromName = name, ToName = toName, Text = text };



                string response = message.ToJson();
                byte[] responsByte = Encoding.UTF8.GetBytes(response);
                await udpClient.SendAsync(responsByte, iPEndPoint);
                Console.WriteLine("Ожидание ответа.");


                //var data = await udpClient.ReceiveAsync();

                byte[] answer = udpClient.Receive(ref iPEndPoint);
                Console.WriteLine("Ответ получен. Перевод в строку...");
                string anserStr = Encoding.UTF8.GetString(answer);
                Message answerMes = Message.FromJson(anserStr);

                Console.WriteLine(answerMes.ToString());
            }
        }
    }
}
