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
using System.Windows.Shapes;

namespace HRM_App.PhongBanControl
{
    /// <summary>
    /// Interaction logic for ChuyenPhongBan.xaml
    /// </summary>
    public partial class ChuyenPhongBan : Window
    {
       

        private string sqlstring = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\QLNS.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
        private SqlConnection conn;
        public ChuyenPhongBan()
        {
            InitializeComponent();
            conn = new SqlConnection(sqlstring);

            conn.Open();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.CommandText = "select MAPHONG, TENPHONG from PHONGBAN";
            sqlCommand.Connection = conn;

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            while (sqlDataReader.Read())
            {
                cboPhongCh.Items.Add(sqlDataReader.GetString(0) + " - " + sqlDataReader.GetString(1));
                cboPhongDen.Items.Add(sqlDataReader.GetString(0) + " - " + sqlDataReader.GetString(1));
            }

            sqlDataReader.Close();
            conn.Close();


        }

        private void btnChuyen_Click(object sender, RoutedEventArgs e)
        {
            conn.Open();

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.CommandText = "update NHANVIEN set PHG='" + cboPhongDen.SelectedItem.ToString().Substring(0, cboPhongDen.SelectedItem.ToString().IndexOf(" ")) + "', " +
                "CHUCVU =N'"+txtViTri.Text+"' where MANV='"+ cboNV.SelectedItem.ToString().Substring(0, cboNV.SelectedItem.ToString().IndexOf(" "))+"'";
            sqlCommand.Connection = conn;

            int ret = sqlCommand.ExecuteNonQuery();
            if (ret > 0)
            {
                MessageBox.Show("Chuyển thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Chuyển không thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            conn.Close();
        }

        private void btnHuy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cboPhongCh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            cboNV.Items.Clear();
 
    
            conn.Open();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.CommandText = "select MANV, TENNV, CHUCVU from NHANVIEN  where PHG='"+ cboPhongCh.SelectedItem.ToString().Substring(0, cboPhongCh.SelectedItem.ToString().IndexOf(" ")) + "'";
            sqlCommand.Connection = conn;

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                cboNV.Items.Add(sqlDataReader.GetString(0) + " - " + sqlDataReader.GetString(1) +" ("+sqlDataReader.GetString(2)+")");
            }

            sqlDataReader.Close();
            conn.Close(); 

         
        }
    }
}
