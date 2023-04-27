using SuperWebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SuperSocket.SocketEngine.Configuration;
using System.Runtime.InteropServices;
using System.Threading;

namespace APP
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private void SocketToo()
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
                Socket handler = sListener.Accept();
                Console.WriteLine("Ожидаем соединение через порт {0}", ipEndPoint);

                // Программа приостанавливается, ожидая входящее соединение
                string data = null;

                // Мы дождались клиента, пытающегося с нами соединиться

                byte[] bytes = new byte[1024];
                int bytesRec = handler.Receive(bytes);

                data += Encoding.UTF8.GetString(bytes, 0, bytesRec);

                // Показываем данные на консоли
                Console.Write("Полученный текст: " + data + "\n\n");


                int idClient = int.Parse(data);
                Connect.connect();


                // Отправляем ответ клиенту\
                string reply = "Информация из базы:" + Environment.NewLine + Connect.ConnectClasses[idClient].Name;
                byte[] msg = Encoding.UTF8.GetBytes(reply);
                handler.Send(msg);


                if (data.IndexOf("<TheEnd>") > -1)
                {
                    Console.WriteLine("Сервер завершил соединение с клиентом.");
                }


                handler.Shutdown(SocketShutdown.Both);
                handler.Close();



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


        public string text { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Connect.connect();


            SocketToo();
        }


    }
}
