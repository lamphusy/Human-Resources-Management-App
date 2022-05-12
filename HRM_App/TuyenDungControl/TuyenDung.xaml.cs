using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HRM_App.TuyenDungControl
{
    /// <summary>
    /// Interaction logic for TuyenDung.xaml
    /// </summary>
    public partial class TuyenDung : UserControl
    {
        private string sqlstring = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\QLNS.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
        private SqlConnection conn;

        public TuyenDung()
        {
            InitializeComponent();
            pnHienThi.Children.Add(new TinTuyenDungControl());
            conn = new SqlConnection(sqlstring);
            btnChiTiet.IsEnabled = false;
            btnChiTiet.Opacity = 0;
            btnDSUV.IsEnabled = false;
            btnDSUV.Opacity = 0;
            btnXoa.IsEnabled = false;
            btnXoa.Opacity = 0;
            btnWeb.IsEnabled = false;
            btnWeb.Opacity = 0;
        }

        private void btnTaoMoi_Click(object sender, RoutedEventArgs e)
        {
            ThemTinTuyenDung them = new ThemTinTuyenDung();
            them.ShowDialog();

        
                pnHienThi.Children.Clear();
                pnHienThi.Children.Add(new TinTuyenDungControl());
  
           
        }

        private void txtTimKiem_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtTimKiem.Text == "")
            {
                pnHienThi.Children.Clear();
                pnHienThi.Children.Add(new TinTuyenDungControl());
            }
            else
            {
                TinTuyenDungControl TimKiem = new TinTuyenDungControl(txtTimKiem.Text);
                pnHienThi.Children.Clear();
                pnHienThi.Children.Add(TimKiem);
            }
        }

        private void tbtnCauHinh_Checked(object sender, RoutedEventArgs e)
        {
            btnChiTiet.IsEnabled= true;
            btnChiTiet.Opacity = 1;
            btnDSUV.IsEnabled = true;
            btnDSUV.Opacity = 1;
            btnXoa.IsEnabled = true;
            btnXoa.Opacity = 1;
            btnWeb.IsEnabled = true;
            btnWeb.Opacity = 1;
            tbtnCauHinh.Background = Brushes.Red;
            tbtnCauHinh.BorderBrush = Brushes.Red;
        }

        private void tbtnCauHinh_Unchecked(object sender, RoutedEventArgs e)
        {
            btnChiTiet.IsEnabled = false;
            btnChiTiet.Opacity = 0;
            btnDSUV.IsEnabled = false;
            btnDSUV.Opacity = 0;
            btnXoa.IsEnabled = false;
            btnXoa.Opacity = 0;
            btnWeb.IsEnabled = false;
            btnWeb.Opacity = 0;
            tbtnCauHinh.Background = (Brush)(new BrushConverter().ConvertFrom("#FF0D0B5D"));
            tbtnCauHinh.BorderBrush = Brushes.Black;
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            TinTuyenDungControl tinTuyenDungHienThoi =(TinTuyenDungControl)VisualTreeHelper.GetChild(pnHienThi,0);
            if (tinTuyenDungHienThoi.GetItemSelected() == null) return;
            TinTuyenDung tinTuyenDungDuocChon = (TinTuyenDung)tinTuyenDungHienThoi.GetItemSelected();
            string maXoa = (tinTuyenDungDuocChon.MATD);


            if (MessageBox.Show("Bạn có chắc xóa tin tuyển dụng có mã "+ maXoa+" không?\n(Tất cả ứng viên của tuyển dụng này sẽ tự động xóa)","Xác nhận",MessageBoxButton.YesNo,MessageBoxImage.Warning)==MessageBoxResult.Yes)
            {
                conn.Open();
                try
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandText = "delete UNGVIEN where MATD='" + maXoa + "'";
                    sqlCommand.Connection = conn;
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.CommandText = "delete TUYENDUNG where MATD = '" + maXoa + "'";


                    int ret = sqlCommand.ExecuteNonQuery();

                    if (ret > 0)
                    {
                        MessageBox.Show("Xóa thành công", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        pnHienThi.Children.Clear();
                        pnHienThi.Children.Add(new TinTuyenDungControl());


                    }
                    else
                    {
                        MessageBox.Show("Xóa không thành công", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch(Exception ex)
                {
                        MessageBox.Show("Xóa không thành công", "", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                conn.Close();
                
            }    
            
        }

        private void btnChiTiet_Click(object sender, RoutedEventArgs e)
        {
            TinTuyenDungControl tinTuyenDungHienThoi = (TinTuyenDungControl)VisualTreeHelper.GetChild(pnHienThi, 0);
            if(tinTuyenDungHienThoi.GetItemSelected() != null)
            {
                TinTuyenDung tinTuyenDungDuocChon = (TinTuyenDung)tinTuyenDungHienThoi.GetItemSelected();
                string maChon = (tinTuyenDungDuocChon.MATD);

                ThemTinTuyenDung chiTietTuyenDung = new ThemTinTuyenDung(maChon);

                chiTietTuyenDung.ShowDialog();

                    pnHienThi.Children.Clear();
                    pnHienThi.Children.Add(new TinTuyenDungControl());
                

            }
        }

        private void btnDSUV_Click(object sender, RoutedEventArgs e)
        {
            TinTuyenDungControl tinTuyenDungHienThoi = (TinTuyenDungControl)VisualTreeHelper.GetChild(pnHienThi, 0);
            if (tinTuyenDungHienThoi.GetItemSelected() == null) 
                  return;
            TinTuyenDung tinTuyenDungDuocChon = (TinTuyenDung)tinTuyenDungHienThoi.GetItemSelected();
            UngVienThamGia ungVienThamGias = new UngVienThamGia(tinTuyenDungDuocChon.MATD);
            ungVienThamGias.ShowDialog();
            
           
        }

        private void btnWeb_Click(object sender, RoutedEventArgs e)
        {
            HoTroDangTinTD hoTroDangTinTD = new HoTroDangTinTD();
            hoTroDangTinTD.ShowDialog();
        }
    }
}
