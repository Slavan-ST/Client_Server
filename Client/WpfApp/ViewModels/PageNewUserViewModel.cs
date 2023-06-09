﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using WpfApp.Models;
using WpfApp.Views;

namespace WpfApp.ViewModels
{
    public class PageNewUserViewModel:BaseViewModel
    {
        public PageNewUserViewModel(string Id)
        {
            this.Id = Id;
            Avatar = new BitmapImage(new Uri("/Resources/no-image.png", UriKind.Relative));
        }
        public string _id;
        public string _userName;
        public int _lvl;
        public int _discount;
        public BitmapImage _avatar;
        public bool _isEnableAddBtn;

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
            get 
            {
                if (_userName != null)
                {
                    if (_userName != "")
                    {
                        IsEnableAddBtn = true;
                    }
                }
                return _userName; 
            }
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
        public bool IsEnableAddBtn
        {
            get
            {
                return _isEnableAddBtn;
            }
            set
            {
                _isEnableAddBtn = value;
                OnPropertyChanged(nameof(IsEnableAddBtn));
            }
        }

        public ICommand _btnAddAvatarClick;
        public ICommand _btnAddNewUserClick;
        public ICommand _btnBackClick;

        public ICommand BtnAddAvatarClick
        {
            get
            {
                if (_btnAddAvatarClick == null)
                {
                    _btnAddAvatarClick = new RelayCommand(
                        param => this.AddAvatarClick());
                }
                return _btnAddAvatarClick;
            }
        }
        public ICommand BtnAddNewUserClick
        {
            get
            {
                if (_btnAddNewUserClick == null)
                {
                    _btnAddNewUserClick = new RelayCommand(
                        param => this.AddNewUserClick());
                }
                return _btnAddNewUserClick;
            }
        }
        public ICommand BtnBackClick
        {
            get
            {
                if (_btnBackClick == null)
                {
                    _btnBackClick = new RelayCommand(param => this.BackClick());
                }
                return _btnBackClick;
            }
        }


        public void AddAvatarClick()
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Avatar = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }
        public void AddNewUserClick()
        {
            User user = new User()
            {
                Id = int.Parse(Id),
                NickName = UserName,
                LVL = (LVL.ToString().Length > 0) ? LVL : 0,
                Discount = (Discount.ToString().Length > 0) ? Discount : 0,
                AvatarImage = Avatar
            };
            WorkingWithServer.SendMessageNewUser(user);            
            MainWindow.mw.NavigateFrame.GoBack();
        }
        public void BackClick()
        {
            MainWindow.mw.NavigateFrame.GoBack();
        }
    }
}
