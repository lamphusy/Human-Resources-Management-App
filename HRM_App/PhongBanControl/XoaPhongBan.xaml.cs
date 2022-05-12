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
    /// Interaction logic for XoaPhongBan.xaml
    /// </summary>
    public partial class XoaPhongBan : Window
    {
        public bool isXoa;
        public XoaPhongBan(List<string>danhSachPhongBan)
        {
            InitializeComponent();
            cboPhong.ItemsSource = danhSachPhongBan;
            isXoa = false;
        }

        private void btnHuy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            isXoa = true;
            this.Close();
        }
    }
}
