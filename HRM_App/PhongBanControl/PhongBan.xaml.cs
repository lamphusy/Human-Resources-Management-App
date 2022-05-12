using MaterialDesignThemes.Wpf;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HRM_App.PhongBanControl
{
    /// <summary>
    /// Interaction logic for PhongBan.xaml
    /// </summary>
    public partial class PhongBan : UserControl
    {
        List<CPhongBan> phongBans = new List<CPhongBan>();
        private string sqlstring = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\QLNS.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
        private SqlConnection conn;

        public PhongBan()
        {
            InitializeComponent();
            conn = new SqlConnection(sqlstring);
            render();


        }
        private void render()
        {
            pnHienThi.Children.Clear();
            phongBans.Clear();

            conn.Open();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.CommandText = "select MAPHONG,TENPHONG from PHONGBAN";
            sqlCommand.Connection = conn;

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            while (sqlDataReader.Read())
            {
                phongBans.Add(new CPhongBan()
                {
                    MaPhong = sqlDataReader.GetString(0),
                    TenPhong = sqlDataReader.GetString(1).ToUpper(),
                    FontSizeTen = sqlDataReader.GetString(1).Length < 14 ? 60 : 30
                });
            }

            sqlDataReader.Close();
            conn.Close();

            for (int i = 0; i < phongBans.Count(); i++)
            {


                string str = "<ContentControl xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" ContentTemplate=\"{DynamicResource phongTemplate}\"></ContentControl>";
                ContentControl theControl = (ContentControl)XamlReader.Parse(str);
                theControl.Content = phongBans[i];



                pnHienThi.Children.Add(theControl);
            }

        }
        private void cardPhong_MouseEnter(object sender, MouseEventArgs e)
        {
            Card the = (Card)sender;
            the.Opacity = 0.5;
        }

        private void cardPhong_MouseLeave(object sender, MouseEventArgs e)
        {
            Card the = (Card)sender;
            the.Opacity = 1;
        }

        private void cardPhong_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Grid theClick = (Grid)sender;
            TextBlock maPhong = (TextBlock)theClick.Children[0];
            ChiTietPhongBan chiTietPhongBan = new ChiTietPhongBan(maPhong.Text);
            pnHienThiAll.Children.Clear();
            pnHienThiAll.Children.Add(chiTietPhongBan); 
            
        }

        private void btnThemPhong_Click(object sender, RoutedEventArgs e)
        {
            ThemPhongBan themPhongBan = new ThemPhongBan();
            themPhongBan.ShowDialog();

            if (themPhongBan.isThem) {
                conn.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandText = "insert PHONGBAN(MAPHONG,TENPHONG,DIADIEM) values(N'"+themPhongBan.txtMa.Text+"',N'"+themPhongBan.txtTen.Text+"',N'"+themPhongBan.txtDiaDiem.Text+"')";
                sqlCommand.Connection = conn;

                int ret = sqlCommand.ExecuteNonQuery();
                conn.Close();
                if (ret > 0)
                {
                    MessageBox.Show("Thêm thành công!", "Thông báo", MessageBoxButton.OK,MessageBoxImage.Information);
                    render();
                }
                else
                {
                    MessageBox.Show("Thêm không thành công!", "Thông báo", MessageBoxButton.OK,MessageBoxImage.Error);
                }
               
            }
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            List<string> danhSachPhong = new List<string>();

            conn.Open();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.CommandText = "select MAPHONG, TENPHONG from PHONGBAN";
            sqlCommand.Connection = conn;

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            while (sqlDataReader.Read())
            {
                danhSachPhong.Add(sqlDataReader.GetString(0)+" - "+sqlDataReader.GetString(1));
            }
            sqlDataReader.Close();
            conn.Close();
            XoaPhongBan xoaPhongBan = new XoaPhongBan(danhSachPhong);
            xoaPhongBan.ShowDialog();

            if (xoaPhongBan.isXoa)
            {
                conn.Open();
                sqlCommand.CommandText = "select count(*) from NHANVIEN where PHG='" + xoaPhongBan.cboPhong.Text.Substring(0, xoaPhongBan.cboPhong.Text.IndexOf(" ")) + "'";
                sqlDataReader = sqlCommand.ExecuteReader();
                sqlDataReader.Read();

                if (sqlDataReader.GetInt32(0) > 0)
                {
                    MessageBox.Show("Phải chuyển tất cả nhân viên trong phòng ban sang phòng ban khác \n Hoặc sa thải toàn bộ nhân viên trước khi xóa phòng ban", "Không thể xóa", MessageBoxButton.OK, MessageBoxImage.Error);
                    sqlDataReader.Close();
                    conn.Close();
                }
                else
                {
                    sqlDataReader.Close();
                    sqlCommand.CommandText = "delete from PHONGBAN where MAPHONG='" + xoaPhongBan.cboPhong.Text.Substring(0, xoaPhongBan.cboPhong.Text.IndexOf(" ")) + "'";
                    int ret = sqlCommand.ExecuteNonQuery();
                    if (ret > 0) 
                    {
                        MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        conn.Close();
                        render();
                        
                    }
                    else
                    {
                        MessageBox.Show("Xóa không thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                        conn.Close();
                    }
                }
            
            }

        }

        private void btnChuyenPhong_Click(object sender, RoutedEventArgs e)
        {
            ChuyenPhongBan chuyenPhongBan = new ChuyenPhongBan();
            chuyenPhongBan.ShowDialog();
        }
    }
}
