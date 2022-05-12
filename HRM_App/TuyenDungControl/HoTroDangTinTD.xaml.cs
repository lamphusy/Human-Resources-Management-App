using CefSharp.Wpf;
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

namespace HRM_App.TuyenDungControl
{
    /// <summary>
    /// Interaction logic for HoTroDangTinTD.xaml
    /// </summary>
    public partial class HoTroDangTinTD : Window
    {
        private string sqlstring = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\QLNS.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
        private SqlConnection conn;
        List<string> webURLs = new List<string>();
     
        
        private int indexWeb = -1;
        
        public HoTroDangTinTD()
        {
            InitializeComponent();
            conn = new SqlConnection(sqlstring);


            GoHome();


            conn.Open();

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.CommandText = "select TENTD from TUYENDUNG";
            sqlCommand.Connection = conn;

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                cboChonTD.Items.Add(sqlDataReader.GetString(0));
            }

            conn.Close();
            
        }
       
        private void web_AddressChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (web.IsLoaded)
            {
               
                    LoadWebPages(web.Address);
             
            }
            
        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            web.Address = txtURL.Text;
            

        }

        private void cboChonTD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string tenTD = cboChonTD.SelectedItem.ToString();
            if(tenTD != "Chọn tin tuyển dụng")
            {
                conn.Open();
                try
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandText = "select TENTD,VITRI,HANHS,LUONG,GHICHU,YEUCAU from TUYENDUNG where TENTD =N'" + tenTD + "'";
                    sqlCommand.Connection = conn;

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    if (sqlDataReader.Read())
                    {
                        string text = sqlDataReader.GetString(0) + "\n\n" +
                                  "Tuyển dụng " + sqlDataReader.GetString(1) + "\n" +
                                  "Hạn hồ sơ: " + sqlDataReader.GetDateTime(2).Date + "\n" +
                                  "Lương: " + sqlDataReader.GetString(3) + "\n\n" +
                                  "Mô tả công việc: " + sqlDataReader.GetString(4) + "\n" +
                                  "Yêu cầu công việc: " + sqlDataReader.GetString(5) + "\n" +
                                  "Bạn sẽ thích: ****";
                        txtDescription.Document.Blocks.Clear();
                        txtDescription.Document.Blocks.Add(new Paragraph(new Run(text)));
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Không load được dữ liệu!\n" + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                conn.Close();
            }
            
        }

        private void backWeb_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            ToggleWebPages(btn.Content.ToString());

        }

        private void preWeb_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            ToggleWebPages(btn.Content.ToString());

        }
        private void GoHome()
        {
            
            web.Address = "www.google.com";
           
        }

        private void LoadWebPages(string Link,bool addToList = true)
        {
            if (addToList == true)
            {
                indexWeb++;
                webURLs.Add(Link);
                
            }
            txtURL.Text = Link;
            web.Address = Link;
           

        }
        private void ToggleWebPages(string Option)
        {
            if(Option == "→")
            {
                if ((webURLs.Count - indexWeb - 1) > 0)
                {
                    indexWeb++;
                    LoadWebPages(webURLs[indexWeb], false);
                    indexWeb--;
                    webURLs.RemoveAt(webURLs.Count - 1);

                }
            }
            if (Option == "←")
            {            
                if ( indexWeb >= 1)
                {
                    indexWeb-=1;
                    LoadWebPages(webURLs[indexWeb], false);
                    indexWeb -= 1;
                    webURLs.RemoveAt(webURLs.Count - 1);
                }
            }
        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            LoadWebPages(webURLs[indexWeb],false);
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            LoadWebPages(webURLs[0]);
        }

        private void txtURL_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                web.Address = txtURL.Text;
             

            }
        }

        
    }
}
