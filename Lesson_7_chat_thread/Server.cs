using Chat_Web_App;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_7_chat_thread
{
    internal class Server


    {   // Словарь с клиентами
        public static Dictionary<string, IPEndPoint> clients = new Dictionary<string, IPEndPoint>();


        public static bool ExitVar = true;

        //public static void Exit()
        //{
        //    string status = Console.ReadLine();
        //    if (status == "EXIT") { ExitVar = false; }

        //}


        public static async Task AcceptMess()
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            UdpClient udpClient = new UdpClient(16876);
            Console.WriteLine("Сервер ожилает сообщение...");
            //Console.WriteLine("Для выхода ввести EXIT");

            while (ExitVar) // бесконечный цикл пока сервер ждёт сообщения
            {
                //Task exit = Task.Run(() => Exit());

                var data = udpClient.Receive(ref iPEndPoint);
                string str = Encoding.UTF8.GetString(data);
                Message? message = Message.FromJson(str);
                Console.WriteLine(message?.ToString());

                await Task.Run(async () =>
                {
                    Message? responseFromServer = new Message() { };

                    if (message.ToName.ToUpper() == "SERVER")
                    {

                        if (message.Text.ToLower() == "registred")
                        {
                            if (clients.ContainsKey(message.Name))
                            {
                                responseFromServer = new Message() { Name = "СЕРВЕР", Text = $"Пользователь {message.FromName} уже добавлен." };

                            }
                            else
                            {
                                if (clients.TryAdd(message.FromName, iPEndPoint))
                                {
                                    responseFromServer = new Message() { Name = "СЕРВЕР", Text = $"Пользователь {message.FromName} добавлен." };
                                }
                                else
                                {
                                    responseFromServer = new Message() { Name = "СЕРВЕР", Text = $"Пользователя {message.FromName} НЕ УДАЛОСЬ добавить." };

                                }
                            }

                        }
                        else if (message.Text.ToLower() == "delet")
                        {
                            clients.Remove(message.FromName);
                            responseFromServer = new Message() { Name = "СЕРВЕР", Text = $"Клиент удалён: {message.FromName}" };
                        }
                        else if(message.Text.ToLower() == "list")
                        {
                            responseFromServer = new Message() { Name = "СЕРВЕР", Text = $"Количесвто зарегистрированных пользователей: {clients.Count.ToString()}" };
                        }


                    }
                    else if (message.ToName.ToUpper() == "All")
                    {
                        foreach (var client in clients)
                        {
                            message.ToName = client.Key;
                            var stringAll = message.ToJson();


                            byte[] buffer = Encoding.UTF8.GetBytes(stringAll);
                            await udpClient.SendAsync(buffer, client.Value);
                        }
                        // сообщение просто создано и не используется
                        responseFromServer = new Message() { Name = "СЕРВЕР", Text = "Сообщение отправлено всем." };
                     
                    }
                    else if (clients.TryGetValue(message.ToName, out IPEndPoint iP)) 
                    { 
                        var messageToName = message.ToJson();
                        byte[] buffer = Encoding.UTF8.GetBytes(messageToName);
                        udpClient.Send(buffer, iP);

                        responseFromServer = new Message() { Name = "СЕРВЕР", Text = $"Сообщение отправлено {message.ToName}." };

                    }

                    else
                    {
                        responseFromServer = new Message() { Name = "СЕРВЕР", Text = $"Пользователя {message.ToName} - не существует..." };
                    }


                    Console.WriteLine("Отправитель: " + message.Name + "Текст: " + message.Text);

                    
                    string resp = responseFromServer.ToJson();
                    byte[] respBytes = Encoding.UTF8.GetBytes(resp);
                    await udpClient.SendAsync(respBytes, iPEndPoint);
                });

            }

        }

    }
}
