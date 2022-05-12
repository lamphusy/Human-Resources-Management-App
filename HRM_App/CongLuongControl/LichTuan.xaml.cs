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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HRM_App.CongLuongControl
{
    /// <summary>
    /// Interaction logic for LichTuan.xaml
    /// </summary>
    public partial class LichTuan : Window
    {
        private bool isEdit;
        private string sqlstring = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\QLNS.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
        private SqlConnection conn;
        private string manV;
        public LichTuan()
        {
            InitializeComponent();
            isEdit = false;
        }
        public LichTuan(string MaNV)
        {
            InitializeComponent();
            isEdit = false;
            manV = MaNV;
            conn = new SqlConnection(sqlstring);
            conn.Open();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.CommandText = "select LICHTUAN from LICHCONG where MaNV='"+MaNV+"'";
            sqlCommand.Connection = conn;

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            sqlDataReader.Read();
            if(sqlDataReader.HasRows == true)
            {
                if (!sqlDataReader.IsDBNull(0))
                {
                    string[] arr = sqlDataReader.GetString(0).Split(','); 
                    for(int i = 0; i < arr.Length; i++)
                    {
                        if (arr[i] == "2") {
                            PackIcon icon = new PackIcon();
                            icon.Kind = PackIconKind.Tick;
                            thu2.Children.Add(icon);
                        }
                        else if (arr[i] == "3")
                        {
                            PackIcon icon = new PackIcon();
                            icon.Kind = PackIconKind.Tick;
                            thu3.Children.Add(icon);
                        }
                        else if (arr[i] == "4")
                        {
                            PackIcon icon = new PackIcon();
                            icon.Kind = PackIconKind.Tick;
                            thu4.Children.Add(icon);
                        }
                        else if (arr[i] == "5")
                        {
                            PackIcon icon = new PackIcon();
                            icon.Kind = PackIconKind.Tick;
                            thu5.Children.Add(icon);
                        }
                        else if (arr[i] == "6")
                        {
                            PackIcon icon = new PackIcon();
                            icon.Kind = PackIconKind.Tick;
                            thu6.Children.Add(icon);
                        }
                        else if (arr[i] == "7")
                        {
                            PackIcon icon = new PackIcon();
                            icon.Kind = PackIconKind.Tick;
                            thu7.Children.Add(icon);
                        }
                    }
                }
            }
            sqlDataReader.Close();
            conn.Close();
        }

        private void btnEdit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(txbChinhSua.Visibility == Visibility.Hidden)
            {
                txbChinhSua.Visibility = Visibility.Visible;
                bg.Background = (Brush)new BrushConverter().ConvertFrom("#E1F7E2");
                isEdit = true;
            }
            else
            {
                if( MessageBox.Show("Bạn có muốn lưu không?","Xác nhận",MessageBoxButton.YesNo,MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {
                    conn.Open();

                    string kq = "";
                    for(int i = 2; i <= 7; i++)
                    {
                        if(thu2.Children.Count > 0)
                        {
                            kq = "2";
                        }
                        if (thu3.Children.Count > 0)
                        {
                            kq += ",3";
                        }
                        if (thu4.Children.Count > 0)
                        {
                            kq += ",4";
                        }
                        if (thu5.Children.Count > 0)
                        {
                            kq += ",5";
                        }
                        if (thu6.Children.Count > 0)
                        {
                            kq += ",6";
                        }
                        if (thu7.Children.Count > 0)
                        {
                            kq += ",7";
                        }
                    }
                    try
                    {
                        SqlCommand sqlCommand, sqlCommand1;
                        sqlCommand = sqlCommand1 = new SqlCommand();
                        sqlCommand.CommandType = sqlCommand1.CommandType = System.Data.CommandType.Text;
                        sqlCommand1.CommandText = "select LICHTUAN from LICHCONG where MANV='" + manV + "'";

                        sqlCommand.Connection = sqlCommand1.Connection = conn;

                        SqlDataReader sqlDataReader = sqlCommand1.ExecuteReader();
                        sqlDataReader.Read();
                        if (sqlDataReader.HasRows == false)
                        {
                            SqlCommand sqlCommand2 = new SqlCommand();
                            sqlCommand2.CommandType = System.Data.CommandType.Text;
                            sqlCommand2.CommandText = "insert LICHCONG(MANV,LICHTUAN) values('" + manV + "','" + kq + "')";
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

                            sqlCommand.CommandText = "update LICHCONG set LICHTUAN='" + kq + "' where MaNV='" + manV + "'";
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
                        MessageBox.Show("Lỗi\n" + ex.Message,"", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                txbChinhSua.Visibility = Visibility.Hidden;
                bg.Background = Brushes.White;
                isEdit = false;
            }

        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if(isEdit == false)
                this.Close();
            else
            {
                MessageBox.Show("Hãy thoát chế độ Edit");
            }
        }

        private void thu2_MouseDown(object sender, MouseButtonEventArgs e)
        {

            Grid grid = (Grid)sender;
            if(isEdit == true)
            {
                if(grid.Children.Count == 0)
                {
                    PackIcon icon = new PackIcon();
                    icon.Kind = PackIconKind.Tick;
                    grid.Children.Add(icon);
                 
                }
                else
                { 
                    grid.Children.Clear();
                }
            }
        }
    }
}
