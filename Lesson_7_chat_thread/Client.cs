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
        public static void SendMessage(string name)
            
        {
            //IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 16876);
            // any не указывается здесь
            UdpClient udpClient = new UdpClient();
            Message message = new Message { Name = name, Text = "Hi", DateTime = DateTime.Now };
            string response = message.ToJson();
            byte[] responsByte = Encoding.UTF8.GetBytes(response);
            udpClient.Send(responsByte, iPEndPoint);
            Console.WriteLine("Ожидание ответа. Для завершения ввести 0 ");



            byte[] answer = udpClient.Receive(ref iPEndPoint);
            Console.WriteLine("Перевод в байты выполнен");
            string anserStr = Encoding.UTF8.GetString(answer);
            Message answerMes = Message.FromJson(anserStr);

            Console.WriteLine(answerMes.ToString());
        }
       
        
    
    
    }
}
