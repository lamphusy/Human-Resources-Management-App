using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HRM_App.PhongBanControl
{
    /// <summary>
    /// Interaction logic for ChiTietPhongBan.xaml
    /// </summary>
    public partial class ChiTietPhongBan : UserControl
    {
        List<CNhanVien> nhanViens = new List<CNhanVien>();
        private string sqlstring = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\QLNS.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";

        private SqlConnection conn;
        public string MaPhong { get; set; }
        public int SoLuong { get; set; }
        public string DiaDiem { get; set; }
        public string TenPhong { get; set; }
        public ChiTietPhongBan(string maPhong)
        {
            InitializeComponent();

            pnHienThi.Children.Clear();
            nhanViens.Clear();

            conn = new SqlConnection(sqlstring);

            conn.Open();
            SqlCommand sqlCommand, sqlCommand1, sqlCommand2;
            sqlCommand = sqlCommand1 = sqlCommand2 = new SqlCommand();
            sqlCommand.Connection = sqlCommand1.Connection = sqlCommand2.Connection = conn;
            sqlCommand.CommandType = sqlCommand1.CommandType = sqlCommand2.CommandType = System.Data.CommandType.Text;

            sqlCommand.CommandText = "select MANV,TENNV,CHUCVU,SDT,EMAIL,ANH,GIOITINH from NHANVIEN where PHG='" + maPhong + "'";


            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            while (sqlDataReader.Read())
            {
                CNhanVien nv = new CNhanVien()
                {
                    MaNV = sqlDataReader.IsDBNull(0) ? "" : sqlDataReader.GetString(0),
                    TenNV = sqlDataReader.IsDBNull(1) ? "" : sqlDataReader.GetString(1),
                    ChucVu = sqlDataReader.IsDBNull(2) ? "" : sqlDataReader.GetString(2),
                    SDT = sqlDataReader.IsDBNull(3) ? 0 : sqlDataReader.GetInt32(3),
                    Email = sqlDataReader.IsDBNull(4) ? "" : sqlDataReader.GetString(4),

                };
                if (!sqlDataReader.IsDBNull(5))
                {
                    byte[] pictureIdByte;

                    pictureIdByte = (byte[])(sqlDataReader.GetSqlBinary(5));
                    if (pictureIdByte != null)
                    {
                        using (var stream = new MemoryStream(pictureIdByte))
                        {
                            nv.Avatar = BitmapFrame.Create(stream,
                                                    BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                        }
                    }
                }
                else
                {
                    if (sqlDataReader.GetString(6) == "Nam")
                    {
                        nv.Avatar = new BitmapImage(
                    new Uri("../img/user_male.png", UriKind.Relative));
                    }
                    else
                    {
                        nv.Avatar = new BitmapImage(
                    new Uri("../img/user_female.png", UriKind.Relative));
                    }


                }
                nhanViens.Add(nv);
            }


            sqlDataReader.Close();



            for (int i = 0; i < nhanViens.Count(); i++)
            {
                string str = "<ContentControl xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" ContentTemplate=\"{DynamicResource NVTemplate}\"></ContentControl>";
                ContentControl theControl = (ContentControl)XamlReader.Parse(str);
                theControl.Content = nhanViens[i];
                pnHienThi.Children.Add(theControl);
            }

            /////////////////////////////////////////////////// THong TIn Phong
            sqlCommand1.CommandText = "select count(*) from NHANVIEN where PHG='" + maPhong + "'";
            sqlDataReader = sqlCommand1.ExecuteReader();
            sqlDataReader.Read();
            SoLuong = sqlDataReader.IsDBNull(0) ? 0 : sqlDataReader.GetInt32(0);
            sqlDataReader.Close();

            sqlCommand2.CommandText = "select DIADIEM,TENPHONG from PHONGBAN where MAPHONG='" + maPhong + "'";
            sqlDataReader = sqlCommand2.ExecuteReader();
            while (sqlDataReader.Read())
            {
                DiaDiem = sqlDataReader.IsDBNull(0) ? "" : sqlDataReader.GetString(0);
                TenPhong = "PHÒNG " + (sqlDataReader.IsDBNull(1) ? "" : sqlDataReader.GetString(1)).ToUpper();
            }

            sqlDataReader.Close();

            MaPhong = maPhong;
            this.DataContext = this;

            conn.Close();
        }

    
        private void Card_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Grid panel = (Grid)sender;
            StackPanel stackPanel = (StackPanel)panel.Children[2];
            TextBlock maNV = (TextBlock)stackPanel.Children[1];

            ChiTietNhanVien chiTietNhanVien = new ChiTietNhanVien(maNV.Text);
            chiTietNhanVien.ShowDialog();
        }
    }
}
