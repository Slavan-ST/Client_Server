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
using System.Text.RegularExpressions;

namespace WpfApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string text { get; set; }
        public string text2 { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            tb_user_name.TextChanged += Tb_TextChanged;
        }

        private void Tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tb_user_name.Text.Trim() == "null")
            {
                (new WindowVerifi(this)).ShowDialog();
            }
        }

        private void Tb_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                (sender as TextBox).Text = (sender as TextBox).Text.Replace(" ", "");
            }
        }

        private void Tb1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[0-9]+");
            if (regex.IsMatch(text)) { return true; }
            return false;
        }

        void SendMessageFromSocket(User user,int port = 11000)
        {
            string message = "NEW USER;" + user.NickName + ";" + user.Id +";" + user.Discount + ";" + user.LVL;
            // Буфер для входящих данных
            byte[] bytes = new byte[1024];

            // Соединяемся с удаленным устройством

            // Устанавливаем удаленную точку для сокета
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);

            Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Соединяем сокет с удаленной точкой
            sender.Connect(ipEndPoint);

            byte[] msg = Encoding.UTF8.GetBytes(message);

            // Отправляем данные через сокет
            int bytesSent = sender.Send(msg);

            // Получаем ответ от сервера
            int bytesRec = sender.Receive(bytes);

            string otvet = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            tb.Text = otvet;
            if (otvet == "Image")
            {
                int bytesSent2 = sender.Send(user.Avatar);
            }
            // Освобождаем сокет
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            User user = new User() { Id = int.Parse(tb1.Text) };
            try
            {
                user = SendMessageFromSocket(user.Id.ToString());
                if (user != null)
                {
                    SendMessageFromSocketGetImage(user);
                    image_avatar_user.Source = user.AvatarImage;
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
        public void AddOrNot(bool otvet)
        {
            if (otvet)
            {
                string id = tb1.Text;
                newUserStackPanel.Visibility = Visibility.Visible;
                tb_new_user_id.Text = id;
                tb_new_user_id.IsEnabled = false;
                mainStackPanel.Visibility = Visibility.Collapsed;
            }
        }

        User userNewCurrent = new User();
        private void addUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                newUserStackPanel.Visibility=Visibility.Collapsed;
                mainStackPanel.Visibility = Visibility.Visible;

                userNewCurrent.AvatarImage = image_avatar_new_user.Source as BitmapImage;
                userNewCurrent.Id = int.Parse(tb_new_user_id.Text);
                userNewCurrent.Discount = int.Parse(tb_new_user_discount.Text);
                userNewCurrent.LVL = int.Parse(tb_new_user_lvl.Text);
                userNewCurrent.NickName = tb_new_user_name.Text;


                SendMessageFromSocket(userNewCurrent);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        User SendMessageFromSocket(string message, int port = 11000)
        {
            // Буфер для входящих данных
            byte[] bytes = new byte[1024];

            // Соединяемся с удаленным устройством

            // Устанавливаем удаленную точку для сокета
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);

            Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Соединяем сокет с удаленной точкой
            sender.Connect(ipEndPoint);

            byte[] msg = Encoding.UTF8.GetBytes("GETUSERID;" + message);

            // Отправляем данные через сокет
            int bytesSent = sender.Send(msg);

            // Получаем ответ от сервера


            int bytesRec = sender.Receive(bytes);

            string otvet = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            tb.Text = otvet;


            User user = new User();

            if (otvet != "not found")
            {
                string[] userInLine = otvet.Split('@');
                try
                {
                    user.NickName = userInLine[1];
                }
                catch { }
                try
                {
                    user.LVL = int.Parse(userInLine[2]);
                }
                catch
                {
                    user.LVL = 0;
                }
                try
                {
                    user.Id = int.Parse(userInLine[4]);
                }
                catch { user.Id = 0; }
                try
                {
                    user.Discount = int.Parse(userInLine[3]);
                }
                catch { user.Discount = 0; }

                tb_user_discount.Text = user.Discount.ToString();
                tb_user_id.Text = user.Id.ToString();
                tb_user_name.Text = user.NickName;
                tb_user_lvl.Text = user.LVL.ToString();

            }
            else
            {
                user = null;
            }

            // Освобождаем сокет
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
            return user;


        }
        void SendMessageFromSocketGetImage(User user,int port = 11000)
        {
            // Буфер для входящих данных
            byte[] bytes = new byte[262144];

            // Устанавливаем удаленную точку для сокета
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);

            Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Соединяем сокет с удаленной точкой
            sender.Connect(ipEndPoint);

            // Отправляем данные через сокет
            byte[] msg = Encoding.UTF8.GetBytes("GETIMAGE");
            int bytesSent = sender.Send(msg);

            // Получаем ответ от сервера
            int bytesRec = sender.Receive(bytes);
            user.Avatar = bytes;
            
            // Освобождаем сокет
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();


        }
        private void addUserAvatar_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                image_avatar_new_user.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                userNewCurrent.AvatarImage = new BitmapImage(new Uri(openFileDialog.FileName));

                userNewCurrent.Avatar = MyConverter.ConverterFromImage(userNewCurrent.AvatarImage);
            }
        }
    }
}
