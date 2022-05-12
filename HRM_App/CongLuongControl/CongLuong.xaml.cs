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

namespace HRM_App.CongLuongControl
{
    /// <summary>
    /// Interaction logic for CongLuong.xaml
    /// </summary>
    public partial class CongLuong : UserControl
    {
        private string sqlstring = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\QLNS.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
        private SqlConnection conn;
        List<ChamCongCaNhan> chamCongCaNhans = new List<ChamCongCaNhan>();
        public CongLuong()
        {
            InitializeComponent();
            conn = new SqlConnection(sqlstring);
            
            
        }

        private void btnDiemDanh_Click(object sender, RoutedEventArgs e)
        {
            if (cboChonNV.Text != "")
            {
                string MaNV = cboChonNV.SelectedItem.ToString().Substring(0, cboChonNV.SelectedItem.ToString().IndexOf(" "));
                conn.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandText = "select NGAY from LICHSUCHAMCONG where MANV ='" + MaNV + "' and NGAY ='" + DateTime.Today + "'";
                sqlCommand.Connection = conn;


                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlDataReader.Read();
                try
                {
                    if (sqlDataReader.HasRows == false)
                    {

                        SqlCommand sqlCommand2 = new SqlCommand();
                        sqlCommand2.CommandType = System.Data.CommandType.Text;
                        sqlCommand2.CommandText = "insert into LiCHSUCHAMCONG(manv,ngay) values ('" + MaNV + "','" + DateTime.Today + "')";
                        sqlCommand2.Connection = conn;

                        sqlDataReader.Close();
                        int ret = sqlCommand2.ExecuteNonQuery();
                        if (ret > 0)
                        {
                            MessageBox.Show("Điểm danh thành công!");
                            pnHienThi.Children.Clear();
                            Lich newLich = new Lich(MaNV);
                            pnHienThi.Children.Add(newLich);

                        }
                        else
                        {
                            MessageBox.Show("Điểm danh không thành công!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Hôm nay bạn đã điểm danh rồi!");
                        sqlDataReader.Close();
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Lỗi!\n" + ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
               
                conn.Close();
            }
            else
            {
                cboChonNV.BorderBrush = Brushes.Red;
            }
        }

        private void cboChonNV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            pnHienThi.Children.Clear();
            string MaNV = cboChonNV.SelectedItem.ToString().Substring(0, cboChonNV.SelectedItem.ToString().IndexOf(" "));
            Lich newLich = new Lich(MaNV);
            pnHienThi.Children.Add(newLich);
        }

        private void btnLichNhanVien_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(cboChonNV.SelectedIndex > -1)
            {
                string MaNV = cboChonNV.SelectedItem.ToString().Substring(0, cboChonNV.SelectedItem.ToString().IndexOf(" "));
                LichTuan lichTuan = new LichTuan(MaNV);
                lichTuan.ShowDialog();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn nhân viên!");
            }
          
        }

        private void btnCacHeSo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (cboChonNV.SelectedIndex > -1)
            {
                string MaNV = cboChonNV.SelectedItem.ToString().Substring(0, cboChonNV.SelectedItem.ToString().IndexOf(" "));
                BangDieuChinhHeSo bangDieuChinhHeSo = new BangDieuChinhHeSo(MaNV);
                bangDieuChinhHeSo.ShowDialog();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn nhân viên!");
            }
        }

        private void btnRefresh_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pnHienThi.Children.Clear();
            string MaNV = cboChonNV.SelectedItem.ToString().Substring(0, cboChonNV.SelectedItem.ToString().IndexOf(" "));
            Lich newLich = new Lich(MaNV);
            pnHienThi.Children.Add(newLich);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            pnHienThi.Children.Add(new Lich());


            cboChonNV.Items.Clear();
            conn.Open();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.CommandText = "select MANV, TENnV from NHANVIEN";
            sqlCommand.Connection = conn;

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                cboChonNV.Items.Add(sqlDataReader.GetString(0) + " - " + sqlDataReader.GetString(1));
                chamCongCaNhans.Add(new ChamCongCaNhan()
                {
                    TenNV = sqlDataReader.GetString(1),
                    MANV = sqlDataReader.GetString(0),
                    NgayChamCong = DateTime.Today,
                    isChamCong = false,
                });
            }
            sqlDataReader.Close();
            try
            {
                sqlCommand.CommandText = "delete LICHSUCHAMCONG where DATEDIFF(month,ngay,getdate()) > 2";
                sqlCommand.ExecuteNonQuery();
            }
            catch
            {

            }
            conn.Close();
        }
    }
}
