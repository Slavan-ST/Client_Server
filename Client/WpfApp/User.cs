using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfApp
{
    public class User2
    {
        public int Id { get; set; } = 0;
        public string NickName { get; set; } = "null";
        public int LVL { get; set; } = 0;
        public int Discount { get; set; } = 0;

        public byte[] Avatar { 
            get
            {
                return _avatar;
            }
            set { 
                _avatar = value;
                AvatarImage = new BitmapImage();
                AvatarImage.BeginInit();
                AvatarImage.StreamSource = new System.IO.MemoryStream(_avatar);
                AvatarImage.EndInit();
            }
        }
        public byte[] _avatar = new byte[2621440];

        public BitmapImage AvatarImage { get; set; }
    }
}
