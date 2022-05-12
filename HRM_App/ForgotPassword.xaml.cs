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

namespace HRM_App
{
    /// <summary>
    /// Interaction logic for ForgotPassword.xaml
    /// </summary>
    public partial class ForgotPassword : Window
    {
        string _RandomCode = "";
        private string sqlstring = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\QLNS.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
        private SqlConnection conn;
        public ForgotPassword()
        {
            InitializeComponent();
            conn = new SqlConnection(sqlstring);
        }
        public ForgotPassword(string randomCode)
        {
            InitializeComponent();
            _RandomCode = randomCode;
            conn = new SqlConnection(sqlstring);
        }

        private void btnVerify_Click(object sender, RoutedEventArgs e)
        {
            if (_RandomCode == "") return;
            if(txtVerify.Text == _RandomCode)
            {
                frame1.Visibility = Visibility.Hidden;
                frame2.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Mã xác thực không chính xác");
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if(txtNewPas.Password.Length < 8)
            {
                MessageBox.Show("Mật khẩu phải có độ dài ít nhất 8 kí tự");
                return;
            }
            if(txtNewPas.Password != txtNewPasConf.Password)
            {

                MessageBox.Show("Mật khẩu không trùng khớp");
                return;
            }
            conn.Open();
            try
            {

                SqlCommand cmd = new SqlCommand("update DANGNHAP set MATKHAU=N'" + txtNewPas.Password + "'",conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Mật khẩu được cập nhật thành công!");
                conn.Close();
                this.Close();
                
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                conn.Close();

            }
        }
    }
}
