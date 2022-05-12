using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
using LiveCharts;
using LiveCharts.Wpf;
namespace HRM_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string sqlstring = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\QLNS.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
        private SqlConnection conn;
        string randomCode;
        public static string to;
        private string email="";

        public MainWindow()
        {
            InitializeComponent();
            conn = new SqlConnection(sqlstring);

           
            
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand cmd = new SqlCommand("select * from DANGNHAP where TAIKHOAN=N'" + txtUserName.Text + "' and MATKHAU='" + txtPassword.Password + "'", conn);
            conn.Open();
            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            sqlDataReader.Read();

            if (sqlDataReader.HasRows)
            {
                MessageBox.Show("Đăng nhập thành công", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                UIDesign uIDesign = new UIDesign();
                uIDesign.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Sai username, password! Nếu chưa có tài khoản hãy đăng kí", "Không thành công", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            conn.Close();

            //-----De test---

            //SqlCommand cmd = new SqlCommand("Delete dangnhap", conn);
            //conn.Open();
            //cmd.ExecuteNonQuery();
            //conn.Close();
        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            if(txtUsername2.Text.Length < 8 || txtPassword2.Password.Length < 8)
            {
                MessageBox.Show("Username và password phải có ít nhất 8 kí tự!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
                
            }
            else
            {
                if (txtEmail.Text.ToLower().Contains("@gmail.com"))
                {
                    SqlCommand cmd = new SqlCommand("select * from DANGNHAP", conn);
                    conn.Open();
                    try
                    {
                        SqlDataReader sqlDataReader = cmd.ExecuteReader();
                        sqlDataReader.Read();
                        if (sqlDataReader.HasRows)
                        {
                            MessageBox.Show("Đã có tài khoản! Nếu quên mật khẩu bạn có thể lấy lại thông qua Email", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                            conn.Close();
                            return;
                        }
                        else
                        {
                            sqlDataReader.Close();
                            cmd.CommandText = "insert into DANGNHAP(TAIKHOAN,MATKHAU,EMAIL) values(N'" + txtUsername2.Text +
                                "','" + txtPassword2.Password + "','" + txtEmail.Text + "')";
                            int ret = cmd.ExecuteNonQuery();
                            if (ret > 0)
                            {
                                MessageBox.Show("Đăng kí thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                MessageBox.Show("Đăng kí không thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            conn.Close();
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Có lỗi bất ngờ xảy ra!\n"+ex.Message,"Lỗi",MessageBoxButton.OK,MessageBoxImage.Error);
                        conn.Close();
                    }
                    
                }
                else
                {
                    MessageBox.Show("Sai định dạng Gmail", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                
            }
        }

        private void txbForget_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (email == "")
            {
                MessageBox.Show("Bạn chưa có tài khoản để lấy mật khẩu!");
                return;
            }
                    string from, pass, messageBody;
              
            Random rand = new Random();
                    
            randomCode = (rand.Next(999999)).ToString();
                    
            MailMessage message = new MailMessage();

            to = email;
                    
            from = "viphrmhk2@gmail.com";
                    
            pass = "viphrm123";
                    
            messageBody = "your reset code is " + randomCode;
                    
            message.To.Add(to);
                    
            message.From = new MailAddress(from);
                   
            message.Body = messageBody;
                   
            message.Subject = "password reseting code";
                    
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                    
            smtp.EnableSsl = true;
                    
            smtp.Port = 587;
                    
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    
            smtp.Credentials = new NetworkCredential(from, pass);
                    
            try
        
            {
                       
            smtp.Send(message);
                        
            MessageBox.Show("Code reset password gửi thành công đến " + email);

                ForgotPassword forgotPassword = new ForgotPassword(randomCode);
                forgotPassword.ShowDialog();
                        
            }
                    
            catch (Exception ex)
        
            {
                        
            MessageBox.Show(ex.Message);
                        
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("select Email from dangnhap", conn);
            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            if (sqlDataReader.Read())
            {
                email = sqlDataReader.GetString(0);
            }
            sqlDataReader.Close();
            conn.Close();
        }

        private void grid_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                SqlCommand cmd = new SqlCommand("select * from DANGNHAP where TAIKHOAN=N'" + txtUserName.Text + "' and MATKHAU='" + txtPassword.Password + "'", conn);
                conn.Open();
                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                sqlDataReader.Read();

                if (sqlDataReader.HasRows)
                {
                    MessageBox.Show("Đăng nhập thành công", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    UIDesign uIDesign = new UIDesign();
                    uIDesign.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Sai username hoặc password!", "Không thành công", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                conn.Close();
            }
        }
    }
}
