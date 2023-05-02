using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WpfApp.Models;
using WpfApp.Views;

namespace WpfApp.ViewModels
{
    public class PageLoginViewModel:BaseViewModel
    {
        public string _id;
        public string _userName;
        public int _lvl;
        public int _discount;
        public BitmapImage _avatar;

        public string Id 
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }
        public int LVL
        {
            get { return _lvl; }
            set
            {
                _lvl = value;
                OnPropertyChanged(nameof(LVL));
            }
        }
        public int Discount
        {
            get { return _discount; }
            set
            {
                _discount = value;
                OnPropertyChanged(nameof(Discount));
            }
        }
        public BitmapImage Avatar
        {
            get { return _avatar; }
            set
            {
                _avatar = value;
                OnPropertyChanged(nameof(Avatar));
            }
        }

        public ICommand _btnSendClick;
        public ICommand BtnSendClick
        {
            get
            {
                if (_btnSendClick == null)
                {
                    _btnSendClick = new RelayCommand(param => this.SendClick());
                }
                return _btnSendClick;
            }
        }

        public void SendClick()
        {
            if (Id.Length > 0)
            {
                (User user, string message) = WorkingWithServer.SendMessageGetUser(Id.ToString());
                if (user != null)
                {
                    Id = user.Id.ToString();
                    UserName = user.NickName;
                    LVL = user.LVL;
                    Discount = user.Discount;
                    Avatar = user.AvatarImage;
                }
                if(user == null && message != "no connect")
                {
                    if (WindowVerifi.IsNewUser())
                    {
                        MainWindow.mw.NavigateFrame.Navigate(new PageNewUser(Id));
                    }
                }
            }
        }
    }
}
