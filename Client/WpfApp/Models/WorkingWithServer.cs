using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfApp.Models
{
    public class WorkingWithServer
    {
        static byte[] messageBytes = new byte[1024];        //1024 байта
        static byte[] imageBytes = new byte[1024000];       //+- мегабайт
        private static Socket Start()
        {
            int port = 11000;
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);
            Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            sender.Connect(ipEndPoint);
            return sender;
        }
        private static void Stop(Socket sender)
        {
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }
        public static void SendMessageNewUser(User user)
        {
            try
            {
                string message = "NEW USER;" + user.Id + ";" + user.NickName + ";" + user.Discount + ";" + user.LVL;
                byte[] msg = Encoding.UTF8.GetBytes(message);

                Socket sender = Start();
                sender.Send(msg);                               // Отправляем данные через сокет
                int bytesRec = sender.Receive(messageBytes);
                string otvet = Encoding.UTF8.GetString(messageBytes, 0, bytesRec);


                if (otvet == "IMAGE")
                {
                    BitmapImage bitmap = user.AvatarImage;
                    sender.Send(MyConverter.ConverterFromImage(bitmap));
                }
                Stop(sender);
            }
            catch
            {

            }
        }
        public static (User user, string message) SendMessageGetUser(string id)
        {
            string messageForClient = "no connect";
            User user = null;

            try
            {
                string message = "GETUSERID;" + id;
                byte[] msg = Encoding.UTF8.GetBytes(message);

                Socket sender = Start();

                sender.Send(msg);
                int bytesRec = sender.Receive(messageBytes);
                string otvet = Encoding.UTF8.GetString(messageBytes, 0, bytesRec);



                if (otvet.Contains("USERFROMDB"))
                {
                    messageForClient = "success";
                    msg = Encoding.UTF8.GetBytes("GETIMAGE;");
                    sender.Send(msg);
                    sender.Receive(imageBytes);

                    string[] arr = otvet.Split('@');
                    user = new User()
                    {
                        Id = int.Parse(arr[1]),
                        NickName = (arr[2]),
                        LVL = int.Parse(arr[3]),
                        Discount = int.Parse(arr[4]),
                        Avatar = imageBytes
                    };
                }
                else
                {
                    (new WindowVerifi()).Show();
                }
                Stop(sender);
            }
            catch
            {

            }
            return (user, messageForClient);
        }
        public static List<User> SendMessageGetUsersSearch(string columnName, string param, int maxCount = 10, int startIndex = 0)
        {
            List<User> users = new List<User>();
            Socket sender = Start();
            string message = "GETUSER@" + columnName + "@" + param + "@" + maxCount + "@" + startIndex;
            byte[] messageByte = Encoding.UTF8.GetBytes(message);
            sender.Send(messageByte);
            int byteRec = sender.Receive(messageBytes);
            string otvet = Encoding.UTF8.GetString(messageBytes, 0, byteRec);
            if (otvet.ToLower() != "not found")
            {
                string[] arrUsers = otvet.Split('*');
                for (int i = 0; i < arrUsers.Length - 1; i++)
                {
                    string[] arr = arrUsers[i].Split('@');
                    User user = new User()
                    {
                        Id = int.Parse(arr[0].Trim()),
                        NickName = arr[1],
                        LVL = int.Parse(arr[2]),
                        Discount = int.Parse(arr[3])
                    };
                    users.Add(user);
                }
            }
            Stop(sender);
            return users;
        }
    }
}
