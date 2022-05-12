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

namespace HRM_App.DaoTaoControl
{
    /// <summary>
    /// Interaction logic for DanhGia.xaml
    /// </summary>
    public partial class DanhGia : Window
    {
        private string sqlstring = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\QLNS.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
        private SqlConnection conn;
        string _MaNV;
        string _MaDT;
        public DanhGia()
        {
            InitializeComponent();
        }
        public DanhGia(string manv,string madt)
        {
            InitializeComponent();
            conn = new SqlConnection(sqlstring);
            _MaNV = manv;
            _MaDT = madt;

            conn.Open();
            SqlCommand cmd = new SqlCommand("select danhgia from THAMGIADAOTAO where MANV='" + _MaNV + "' and MADT='" + _MaDT + "'",conn);
            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            if (sqlDataReader.Read())
            {
                cboDanhGia.Text = sqlDataReader.GetString(0);
            }
            sqlDataReader.Close();
            conn.Close();
        }


        private void btnXacNhan_Click(object sender, RoutedEventArgs e)
        {
            if (cboDanhGia.SelectedIndex > -1)
            {
                conn.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("update THAMGIADAOTAO set DANHGIA=N'" + cboDanhGia.Text + "' where MANV='" + _MaNV + "' and MADT='" + _MaDT + "'",conn);
                    int ret =cmd.ExecuteNonQuery();
                    if (ret > 0)
                    {
                        MessageBox.Show("Cập nhật đánh giá thành công!");
                        conn.Close();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật đánh giá thất bại!","Lỗi");
                    }

                }
                catch(Exception ex)
                {
                    MessageBox.Show("Cập nhật đánh giá thất bại!\n" + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                conn.Close();
            }
            else
            {
                cboDanhGia.BorderBrush = Brushes.Red;
            }
            
        }
    }
}
