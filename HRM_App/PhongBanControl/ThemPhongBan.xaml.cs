using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HRM_App.PhongBanControl
{
    /// <summary>
    /// Interaction logic for ThemPhongBan.xaml
    /// </summary>
    public partial class ThemPhongBan : Window
    {
        public bool isThem;
        public ThemPhongBan()
        {
            InitializeComponent();
            isThem = false;
        }

        private void btnHuy_Click(object sender, RoutedEventArgs e)
        {
            if(txtMa.Text!="" || txtTen.Text!=""|| txtDiaDiem.Text != "")
            {
                MessageBoxResult ret = MessageBox.Show("Bạn đang nhập, có chắc chắn muốn thoát?", "Hỏi thoát", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if(ret == MessageBoxResult.Yes)
                {
                    this.Close();
                }
            }
                 
            else this.Close();
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            isThem = true;
            this.Close();
        }

    }
}
