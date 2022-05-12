using HRM_App.DaoTaoControl;
using HRM_App.QLNSControl;
using HRM_App.TuyenDungControl;
using HRM_App.CongLuongControl;
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
using HRM_App.PhongBanControl;

namespace HRM_App
{
    /// <summary>
    /// Interaction logic for UIDesign.xaml
    /// </summary>
    public partial class UIDesign : Window
    {
        public UIDesign()
        {
            InitializeComponent();
           
        }
        
       

        private void btnDaoTao_Click(object sender, RoutedEventArgs e)
        {
           
            BG.Children.Clear();
            BG.Children.Add(new DaoTao());

            btnDaoTao.Background = (Brush)new BrushConverter().ConvertFrom("#220185");
            btnNhanSu.Background = (Brush)new BrushConverter().ConvertFrom("#673ab7");
            btnCongLuong.Background = (Brush)new BrushConverter().ConvertFrom("#673ab7"); 
            btnPhongBan.Background = (Brush)new BrushConverter().ConvertFrom("#673ab7"); 
            btnToChuc.Background = (Brush)new BrushConverter().ConvertFrom("#673ab7"); 
            btnTuyenDung.Background = (Brush)new BrushConverter().ConvertFrom("#673ab7"); 
           
        }


        private void btnToChuc_Click(object sender, RoutedEventArgs e)
        {
          
            
            BG.Children.Clear();
            BG.Children.Add(new MainControl());

            btnDaoTao.Background = (Brush)new BrushConverter().ConvertFrom("#673ab7");
            btnNhanSu.Background = (Brush)new BrushConverter().ConvertFrom("#673ab7");
            btnCongLuong.Background = (Brush)new BrushConverter().ConvertFrom("#673ab7");
            btnPhongBan.Background = (Brush)new BrushConverter().ConvertFrom("#673ab7");
            btnToChuc.Background = (Brush)new BrushConverter().ConvertFrom("#220185");
            btnTuyenDung.Background = (Brush)new BrushConverter().ConvertFrom("#673ab7");
          

        }

        private void btnNhanSu_Click(object sender, RoutedEventArgs e)
        {
            
            BG.Children.Clear();
            BG.Children.Add(new NhanSu());

            btnDaoTao.Background = (Brush)new BrushConverter().ConvertFrom("#673ab7");
            btnNhanSu.Background = (Brush)new BrushConverter().ConvertFrom("#220185");
            btnCongLuong.Background = (Brush)new BrushConverter().ConvertFrom("#673ab7");
            btnPhongBan.Background = (Brush)new BrushConverter().ConvertFrom("#673ab7");
            btnToChuc.Background = (Brush)new BrushConverter().ConvertFrom("#673ab7");
            btnTuyenDung.Background = (Brush)new BrushConverter().ConvertFrom("#673ab7");
       
        }

        private void btnTuyenDung_Click(object sender, RoutedEventArgs e)
        {
            
            BG.Children.Clear();
            BG.Children.Add(new TuyenDung());

            btnDaoTao.Background = (Brush)new BrushConverter().ConvertFrom("#673ab7");
            btnNhanSu.Background = (Brush)new BrushConverter().ConvertFrom("#673ab7");
            btnCongLuong.Background = (Brush)new BrushConverter().ConvertFrom("#673ab7");
            btnPhongBan.Background = (Brush)new BrushConverter().ConvertFrom("#673ab7");
            btnToChuc.Background = (Brush)new BrushConverter().ConvertFrom("#673ab7");
            btnTuyenDung.Background = (Brush)new BrushConverter().ConvertFrom("#220185");

        }

        private void btnCongLuong_Click(object sender, RoutedEventArgs e)
        {
          
            BG.Children.Clear();
            BG.Children.Add(new CongLuong());

            btnDaoTao.Background = (Brush)new BrushConverter().ConvertFrom("#673ab7");
            btnNhanSu.Background = (Brush)new BrushConverter().ConvertFrom("#673ab7");
            btnCongLuong.Background = (Brush)new BrushConverter().ConvertFrom("#220185");
            btnPhongBan.Background = (Brush)new BrushConverter().ConvertFrom("#673ab7");
            btnToChuc.Background = (Brush)new BrushConverter().ConvertFrom("#673ab7");
            btnTuyenDung.Background = (Brush)new BrushConverter().ConvertFrom("#673ab7");
           
        }

        private void btnPhongBan_Click(object sender, RoutedEventArgs e)
        {
            BG.Children.Clear();
            BG.Children.Add(new PhongBan());

            btnDaoTao.Background = (Brush)new BrushConverter().ConvertFrom("#673ab7");
            btnNhanSu.Background = (Brush)new BrushConverter().ConvertFrom("#673ab7");
            btnCongLuong.Background = (Brush)new BrushConverter().ConvertFrom("#673ab7");
            btnPhongBan.Background = (Brush)new BrushConverter().ConvertFrom("#220185");
            btnToChuc.Background = (Brush)new BrushConverter().ConvertFrom("#673ab7");
            btnTuyenDung.Background = (Brush)new BrushConverter().ConvertFrom("#673ab7");
        
        }

        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            // Set tooltip visibility

            if (Tg_Btn.IsChecked == true)
            {
                tt_tochuc.Visibility = Visibility.Collapsed;
                tt_nhansu.Visibility = Visibility.Collapsed;
                tt_congluong.Visibility = Visibility.Collapsed;
             
                tt_tuyendung.Visibility = Visibility.Collapsed;
                tt_signout.Visibility = Visibility.Collapsed;
                tt_daotao.Visibility = Visibility.Collapsed;
                tt_pb.Visibility = Visibility.Collapsed;
            }
            else
            {
                tt_tochuc.Visibility = Visibility.Visible;
                tt_nhansu.Visibility = Visibility.Visible;
                tt_congluong.Visibility = Visibility.Visible;

                tt_tuyendung.Visibility = Visibility.Visible;
                tt_signout.Visibility = Visibility.Visible;
                tt_daotao.Visibility = Visibility.Visible;
                tt_pb.Visibility = Visibility.Visible;

            }
        }

        private void Tg_Btn_Unchecked(object sender, RoutedEventArgs e)
        {
            BG.Opacity = 1;
        }

        private void Tg_Btn_Checked(object sender, RoutedEventArgs e)
        {
            BG.Opacity = 0.3;
        }

        private void BG_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Tg_Btn.IsChecked = false;
        }

       

        private void btnDanhGia_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                
            }
        }

        private void BG_Loaded(object sender, RoutedEventArgs e)
        {
           // BG.Children.Add(new MainControl());
        }

        private void btnDangXuat_Click(object sender, RoutedEventArgs e)
        {
            var rep = MessageBox.Show("Bạn muốn đăng xuất?","Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(rep == MessageBoxResult.Yes)
            {
                MainWindow manHinhDangNhap = new MainWindow();
                manHinhDangNhap.Show();
                this.Close();
            }
            
        }

       
    }

}
