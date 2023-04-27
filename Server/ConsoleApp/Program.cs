﻿using APP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.Remoting.Messaging;

namespace ConsoleApp
{
    public class Program
    {
        static User currentUser = null;
        static void Main(string[] args)
        {
            Connect.connect();

            SocketToo();
        }
        private static void SocketToo()
        {// Устанавливаем для сокета локальную конечную точку

            int port = 11000;
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);


            // Создаем сокет Tcp/Ip
            Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Назначаем сокет локальной конечной точке и слушаем входящие сокеты
            try
            {
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);

                // Начинаем слушать соединения
                //
                while (true)
                {
                    Socket handler = sListener.Accept();
                    Console.WriteLine("Ожидаем соединение через порт {0}", ipEndPoint);

                    // Программа приостанавливается, ожидая входящее соединение
                    string data = null;

                    // Мы дождались клиента, пытающегося с нами соединиться

                    byte[] bytes = new byte[1024];
                    byte[] bytes2 = new byte[262144];

                    int bytesRec = handler.Receive(bytes);


                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);

                    int idClient = -2;

                    // Отправляем ответ клиенту\


                    string reply = "";
                    if (data.Contains("NEW USER;"))
                    {
                        string[] arr = data.Split(';');
                        User user = new User() { Id = int.Parse(arr[2]), Name = arr[1], Discount = int.Parse(arr[3]), LVL = int.Parse(arr[4]) };
                        Connect.WriteInDb(user);


                        handler.Send(Encoding.UTF8.GetBytes("Image"));
                        handler.Receive(bytes2);
                        user.Avatar = bytes2;

                        Connect.UpdateInDb(user);

                        reply = "success";

                    }


                    else if (data.Contains("GETIMAGE;"))
                    {
                        try
                        {
                            int bytesRec2 = handler.Receive(bytes2);
                            data += Encoding.UTF8.GetString(bytes2, 0, bytesRec2);
                            if (data == "GETIMAGE")
                                handler.Send(currentUser.Avatar);

                        }
                        catch { }
                    }
                    else
                    {
                        try
                        {
                            try
                            {
                                Connect.connect();
                                User user = Connect.ReadFromDB(data);
                                currentUser = user;
                                if (user != null)
                                {
                                    reply = "Информация из базы:" + Environment.NewLine + "@" + user.Name + " @" + user.LVL + " @" + user.Discount + " @" + user.Id + " @";
                                }
                                else
                                {
                                    reply = "not found";
                                }
                            }
                            catch
                            {
                                reply = "not found";
                            }
                        }
                        catch
                        {
                            reply = "неверный формат данных!";
                        }
                    }




                    byte[] msg = Encoding.UTF8.GetBytes(reply);
                    handler.Send(msg);



                    if (data.IndexOf("<TheEnd>") > -1)
                    {
                        Console.WriteLine("Сервер завершил соединение с клиентом.");
                    }


                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();

                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}