using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp.Models;
using WpfApp.Views;

namespace WpfApp.ViewModels
{
    public class PageLoginSearchViewModel:BaseViewModel
    {
        List<string> _comboBoxItems = new List<string>()
        {
            "id","name","lvl","discount"        //"avatar"
        };
        List<User> _usersSearch;
        int _userSelectedIndex;
        int _comboBoxSelectedIndex;
        string _searchUsersText;

        public string SearchUsersText
        {
            get
            {
                return _searchUsersText;
            }
            set 
            { 
                _searchUsersText = value;
                OnPropertyChanged(nameof(SearchUsersText));
            }
        }
        public int UserSelectedIndex
        {
            get
            {
                return _userSelectedIndex;
            }
            set
            {
                _userSelectedIndex = value;
                OnPropertyChanged(nameof(UserSelectedIndex));
            }
        }
        public User CurrentSelectedUser
        {
            get
            {
                return UsersSearch[UserSelectedIndex];
            }
        }

        public int ComboBoxSelectedIndex
        {
            get
            {
                return _comboBoxSelectedIndex;
            }
            set
            {
                _comboBoxSelectedIndex = value;
                OnPropertyChanged(nameof(ComboBoxSelectedIndex));
            }
        }
       
        public List<User> UsersSearch
        {
            get
            {
                return _usersSearch;
            }
            set
            {
                _usersSearch = value;
                OnPropertyChanged(nameof(UsersSearch));
            }
        }
        public List<string> ComboBoxItems
        {
            get
            {
                return _comboBoxItems;
            }
        }

        ICommand _btnSearchClick;
        ICommand _selectItem;
        ICommand _btnBackClick;
        public ICommand BtnSearchClick
        {
            get
            {
                if (_btnSearchClick == null)
                {
                    _btnSearchClick = new RelayCommand(param => Search());
                }
                return _btnSearchClick;
            }
        }
        public ICommand BtnSelectItemClick
        {
            get
            {
                if (_selectItem == null)
                {
                    _selectItem = new RelayCommand(param => SelectItem());
                }
                return _selectItem;
            }
        }
        public ICommand BtnBackClick
        {
            get
            {
                if (_btnBackClick == null)
                {
                    _btnBackClick = new RelayCommand(param => Back());
                }
                return _btnBackClick;
            }
        }

        void SelectItem()
        {

        }

        void Search()
        {
            UsersSearch = WorkingWithServer.SendMessageGetUsersSearch(ComboBoxItems[ComboBoxSelectedIndex], SearchUsersText);
        }
        void Back()
        {
            MainWindow.mw.NavigateFrame.Navigate(new PageLogin());
        }

    }
}
