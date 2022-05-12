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

namespace HRM_App.CongLuongControl
{
    /// <summary>
    /// Interaction logic for BangDieuChinhHeSo.xaml
    /// </summary>
    public partial class BangDieuChinhHeSo : Window
    {
        private bool isEdit = false;
        private string sqlstring = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\QLNS.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
        private SqlConnection conn;
        private string manV;
        public BangDieuChinhHeSo()
        {
            InitializeComponent();
        }
        public BangDieuChinhHeSo( string MaNV)
        {
            InitializeComponent();
            isEdit = false;
            manV = MaNV;
            conn = new SqlConnection(sqlstring);
            conn.Open();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.CommandText = "select HESOKHAUTRU, HESOPHUCAP, LUONGCOBAN from CHAMCONG where MaNV='" + MaNV + "'";
            sqlCommand.Connection = conn;

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                txtHeSoTru.Text = sqlDataReader.IsDBNull(0)?"": sqlDataReader.GetDecimal(0) + "";
                txtHeSoPhuCap.Text = sqlDataReader.IsDBNull(1) ? "" : sqlDataReader.GetDecimal(1) + "";
                txtLuongCoBan.Text = sqlDataReader.IsDBNull(2) ? "" : sqlDataReader.GetSqlMoney(2) + "";
            }
            sqlDataReader.Close();
            conn.Close();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isEdit == false)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("Hãy thoát khỏi chế độ chỉnh sửa!");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           if(isEdit == false)
            {
                textChoPhep.Visibility = Visibility.Visible;
                isEdit = true;
                txtHeSoTru.IsReadOnly = false;
                txtHeSoPhuCap.IsReadOnly = false;
                txtLuongCoBan.IsReadOnly = false;
            }
            else
            {
                if (MessageBox.Show("Bạn có muốn lưu không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {
                    conn.Open();
                    try
                    {
                        SqlCommand sqlCommand, sqlCommand1;
                        sqlCommand = sqlCommand1 = new SqlCommand();
                        sqlCommand.CommandType = sqlCommand1.CommandType = System.Data.CommandType.Text;
                        sqlCommand1.CommandText = "select * from CHAMCONG where MANV='" + manV + "'";

                        sqlCommand.Connection = sqlCommand1.Connection = conn;

                        SqlDataReader sqlDataReader = sqlCommand1.ExecuteReader();
                        sqlDataReader.Read();
                        if (sqlDataReader.HasRows == false)
                        {
                            SqlCommand sqlCommand2 = new SqlCommand();
                            sqlCommand2.CommandType = System.Data.CommandType.Text;
                            sqlCommand2.CommandText = "insert CHAMCONG(MANV,LUONGCOBAN,HESOPHUCAP,HESOKHAUTRU) values('" + manV + "','" + (txtLuongCoBan.Text == "" ? "0" : txtLuongCoBan.Text) + "','" + 
                                (txtHeSoPhuCap.Text == ""? "0" : txtHeSoPhuCap.Text) + "','" + (txtHeSoTru.Text == "" ? "0" : txtHeSoTru.Text) + "')";
                            sqlCommand2.Connection = conn;
                            sqlDataReader.Close();

                            int ret = sqlCommand2.ExecuteNonQuery();
                            if (ret > 0)
                            {
                                MessageBox.Show("Thành công!");
                            }
                            else
                            {
                                MessageBox.Show("Không thành công!");
                            }
                        }
                        else
                        {
                            sqlDataReader.Close();

                            sqlCommand.CommandText = "update CHAMCONG set LUONGCOBAN='" + (txtLuongCoBan.Text == "" ? "0" : txtLuongCoBan.Text) + "'," +
                                "HESOPHUCAP ='" + (txtHeSoPhuCap.Text == "" ? "0" : txtHeSoPhuCap.Text) + "', HESOKHAUTRU='" + (txtHeSoTru.Text == "" ? "0" : txtHeSoTru.Text) + "' where MaNV='" + manV + "'";
                            int ret = sqlCommand.ExecuteNonQuery();
                            if (ret > 0)
                            {
                                MessageBox.Show("Thành công!");
                            }
                            else
                            {
                                MessageBox.Show("Không thành công!");
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Lưu thất bại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    conn.Close();
                }
                textChoPhep.Visibility = Visibility.Hidden;
                isEdit = false;
                txtHeSoTru.IsReadOnly = true;
                txtHeSoPhuCap.IsReadOnly = true;
                txtLuongCoBan.IsReadOnly = true;
            }
        }
    }
}
