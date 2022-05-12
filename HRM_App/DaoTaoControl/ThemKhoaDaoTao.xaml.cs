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
    /// Interaction logic for ThemKhoaDaoTao.xaml
    /// </summary>
    public partial class ThemKhoaDaoTao : Window
    {
        public bool check;
        public bool sua;
        private string sqlstring = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\QLNS.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
        private SqlConnection conn;

        public ThemKhoaDaoTao()
        {
            InitializeComponent();
            sua = false;
            conn = new SqlConnection(sqlstring);
            conn.Open();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.CommandText = "select MAPHONG, TENPHONG from PHONGBAN";
            sqlCommand.Connection = conn;

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while(sqlDataReader.Read())
            {
                txbPgDXs.Items.Add(sqlDataReader.GetString(0) +" - "+sqlDataReader.GetString(1));
            }
            sqlDataReader.Close();
            conn.Close();

        }

        private void btnXacNhan_Click(object sender, RoutedEventArgs e)
        {
            if(txbMa.Text == "")
            {
                txbMa.BorderBrush = Brushes.Red;
                return;
            }
            if(txbPgDXs.SelectedIndex < 0)
            {
                txbPgDXs.BorderBrush = Brushes.Red;
                return;
            }
            if(txbTen.Text == "")
            {
                txbTen.BorderBrush = Brushes.Red;
                return;
            }
            if (txbHocPhi.Text == "")
            {
                txbHocPhi.BorderBrush = Brushes.Red;
                return;
            }
            if (txbDiaDiem.Text == "")
            {
                txbDiaDiem.BorderBrush = Brushes.Red;
                return;
            }
            check = true;
            this.Close();
            

            
        }

        private void btnThoat_Click(object sender, RoutedEventArgs e)
        {
            check = false;
            this.Close();
            sua = false;
        }

        private void dpBD_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
           
        }

        private void dpKT_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
           
        }

        private void dpBD_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

            if (dpKT.SelectedDate.ToString() == "")
            {
                txbTgDT.Text = "";
                return;
            }
            else
            {
                TimeSpan t = (TimeSpan)(dpKT.SelectedDate - dpBD.SelectedDate);
                txbTgDT.Text = Math.Round(t.TotalDays / 30,1) + " tháng";
            }
        }

        private void dpKT_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpBD.SelectedDate.ToString() == "")
            {
                txbTgDT.Text = "";
                return;
            }
            else
            {
                TimeSpan t = (TimeSpan)(dpKT.SelectedDate - dpBD.SelectedDate);
                txbTgDT.Text = Math.Round(t.TotalDays / 30, 1) + " tháng";
            }
        }

        private void txbMa_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txbMa.Text != "")
            {
                txbMa.BorderBrush = Brushes.Black;
            }
         
        }
    }
}
