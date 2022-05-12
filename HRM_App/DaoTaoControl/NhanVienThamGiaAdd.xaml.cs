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

namespace HRM_App.DaoTaoControl
{
    /// <summary>
    /// Interaction logic for NhanVienThamGiaAdd.xaml
    /// </summary>
    public partial class NhanVienThamGiaAdd : Window
    {
        private string sqlstring = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\QLNS.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
        private SqlConnection conn;
        private string _MaDT;
        public bool isOK = false;
        private DateTime _NgayBD;
        public NhanVienThamGiaAdd(string MaDT,DateTime NgayBD)
        {
            InitializeComponent();
            _MaDT = MaDT;
            _NgayBD = NgayBD;
            conn = new SqlConnection(sqlstring);
            conn.Open();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.CommandText = "select TENPHONG from PHONGBAN";
            sqlCommand.Connection = conn;

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            while (sqlDataReader.Read())
            {
                cboPhong.Items.Add(sqlDataReader.GetString(0));
            }
            sqlDataReader.Close();

            conn.Close();

        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if(cboNhanVien.SelectedIndex > -1)
            {
                try
                {
                    conn.Open();
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandText = "insert into THAMGIADAOTAO(MANV,MADT,TIENDO,DANHGIA,NGAYTHAMGIA) VALUES('" +
                        cboNhanVien.SelectedItem.ToString().Substring(0, cboNhanVien.SelectedItem.ToString().IndexOf(" ")) + "','" + _MaDT + "','0','Không','" + (DateTime.Now >= _NgayBD ? DateTime.Now.Date : _NgayBD) + "')";
                    sqlCommand.Connection = conn;

                    int ret = sqlCommand.ExecuteNonQuery();
                    if (ret > 0)
                    {
                        MessageBox.Show("Thêm thành công!");
                        isOK = true;
                    }
                    else
                    {
                        MessageBox.Show("Thêm không thành công");
                    }
                    conn.Close();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Thêm không thành công\n"+ex.Message,"Lỗi",MessageBoxButton.OK,MessageBoxImage.Error);
                }
            }
            else
            {
                cboNhanVien.Background = Brushes.Red;
            }
        }

        private void btnHuy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cboPhong_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cboNhanVien.Items.Clear();
            conn.Open();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.CommandText = "select MANV, TENNV from NHANVIEN JOIN PHONGBAN on NHANVIEN.PHG = PHONGBAN.MAPHONG where TENPHONG=N'"+cboPhong.SelectedItem.ToString()+"'";
            sqlCommand.Connection = conn;

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            while (sqlDataReader.Read())
            {
                cboNhanVien.Items.Add(sqlDataReader.GetString(0)+" - "+ sqlDataReader.GetString(1));
            }
            sqlDataReader.Close();
            conn.Close();
        }
    }
}
