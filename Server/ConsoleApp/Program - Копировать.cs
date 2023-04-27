using APP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main2(string[] args)
        {
            Connect.connect();

            SocketToo();
        }

        private static void SocketToo()
        {// Устанавливаем для сокета локальную конечную точку


            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = new IPAddress(new byte[] { 127, 0, 0, 1 });
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);


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
                    int bytesRec = handler.Receive(bytes);

                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);

                    int idClient = -2;



                    // Отправляем ответ клиенту\
                    string reply = "";
                    if (data.Contains("NEW USER;"))
                    {
                        string[] arr = data.Split(';');
                        Connect.WriteInDb(new User() { Id = 0, Name = arr[1]});
                        reply = "succes";
                    }


                    else
                    {
                        try
                        {
                            try
                            {
                                Connect.connect();
                                idClient = int.Parse(data) - 1;
                                reply = "Информация из базы:" + Environment.NewLine + Connect.Users[idClient].Name;
                            }
                            catch
                            {
                                reply = "not found";
                            }
                        }
                        catch
                        {
                            reply = "неверный ввод данных!";
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
