using APP;
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
        static byte[] bytes = new byte[1024];           //1024 байт
        static byte[] bytesForImage = new byte[1024000];//+- мегабайт

        static User currentUser = null;
        static void Main(string[] args)
        {
            SocketToo();
        }
        private static void SocketToo()
        {
            int port = 11000;
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);
            Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);
                // Начинаем слушать соединения
                while (true)
                {
                    Socket handler = sListener.Accept();
                    Console.WriteLine("Ожидаем соединение через порт {0}", ipEndPoint);
                    // Программа приостанавливается, ожидая входящее соединение
                    string data = null;
                    // Мы дождались клиента, пытающегося с нами соединиться
                    int bytesRec = handler.Receive(bytes);


                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                    string reply = "";
                    if (data.Contains("NEW USER;"))
                    {
                        Console.WriteLine("NEW User");
                        string[] arr = data.Split(';');

                        handler.Send(Encoding.UTF8.GetBytes("IMAGE"));
                        handler.Receive(bytesForImage);

                        User user = new User()
                        {
                            Id = int.Parse(arr[1]),
                            Name = arr[2],
                            Discount = int.Parse(arr[3]),
                            LVL = int.Parse(arr[4]),
                            Avatar = bytesForImage
                        };
                        Connect.WriteInDb(user);
                        reply = "success";
                    }

                    if (data.Contains("GETUSERID;"))
                    {
                        Console.WriteLine(data);
                        try
                        {
                            string[] arr = data.Split(';');
                            User user = Connect.ReadFromDB(arr[1]);
                            if (user != null)
                            {
                                currentUser = user;
                                reply = "USERFROMDB:" + "@" +
                                    user.Id + " @" +
                                    user.Name + " @" +
                                    user.LVL + " @" +
                                    user.Discount + " @";
                                Console.WriteLine(reply);
                                byte[] msg = Encoding.UTF8.GetBytes(reply);
                                handler.Send(msg);
                                bytesRec = handler.Receive(bytes);
                                data = Encoding.UTF8.GetString(bytes, 0, bytesRec); if (data.Contains("GETIMAGE;"))
                                {
                                    try
                                    {
                                        if (user.Avatar != null)
                                        {
                                            Console.WriteLine(user.Avatar.ToString());
                                            Console.WriteLine("Get image");
                                            handler.Send(user.Avatar);
                                        }
                                        else
                                        {
                                            handler.Send(Encoding.UTF8.GetBytes("not found"));
                                        }
                                    }
                                    catch { }
                                }

                            }
                            else
                            {
                                byte[] msg = Encoding.UTF8.GetBytes("NOT FOUND");
                                handler.Send(msg);
                            }
                        }
                        catch
                        {
                            reply = "not found";
                        }
                    }
                    if (data.Contains("GETUSER@"))
                    {
                        Console.WriteLine(data);
                        try
                        {
                            string[] arr = data.Split('@');
                            string columnName = arr[1];
                            string param = arr[2];
                            int maxCount = int.Parse(arr[3]);
                            int startIndex = int.Parse(arr[4]);
                            List<User> users = Connect.ReadFromDBUsers(columnName, param, maxCount, startIndex);
                            if (users.Count > 0)
                            {
                                reply = "";
                                for (int i = 0; i < users.Count; i++)
                                {
                                    User user = new User();
                                    reply += 
                                        user.Id + " @" +
                                        user.Name + " @" +
                                        user.LVL + " @" +
                                        user.Discount + " @" + "*";
                                    Console.WriteLine(reply);
                                }
                                byte[] msg = Encoding.UTF8.GetBytes(reply);
                                handler.Send(msg);
                            }
                            else
                            {
                                byte[] msg = Encoding.UTF8.GetBytes("NOT FOUND");
                                handler.Send(msg);
                            }
                        }
                        catch
                        {
                            reply = "not found";
                        }
                    }

                    if (!data.Contains("GETIMAGE") && !reply.Contains("USERFROMDB"))
                    {
                        byte[] msg = Encoding.UTF8.GetBytes(reply);
                        handler.Send(msg);
                    }
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
