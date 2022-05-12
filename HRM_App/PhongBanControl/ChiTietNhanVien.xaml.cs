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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HRM_App.PhongBanControl
{
    /// <summary>
    /// Interaction logic for ChiTietNhanVien.xaml
    /// </summary>
    public partial class ChiTietNhanVien : Window
    {
        private string sqlstring = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\QLNS.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";

        private SqlConnection conn;
        public ChiTietNhanVien(string maNV)
        {
            InitializeComponent();
            conn = new SqlConnection(sqlstring);

            conn.Open();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.Connection = conn;
            sqlCommand.CommandText = "Select ANH,MANV,TENNV,NGAYSINH,GIOITINH,QUOCTICH,TONGIAO,NOISINH,DIACHI,CMND,BOPHAN,CHUCVU,PHG,NGVAOLAM,SDT,EMAIL from NHANVIEN where MANV='"+maNV+"'";

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            sqlDataReader.Read();

            if (!sqlDataReader.IsDBNull(0))
            {
                byte[] pictureIdByte;

                pictureIdByte = (byte[])(sqlDataReader.GetSqlBinary(0));
                if (pictureIdByte != null)
                {
                    using (var stream = new MemoryStream(pictureIdByte))
                    {
                        imgAvatar.Source = BitmapFrame.Create(stream,
                                                BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                    }
                }
            }
            else
            {
                if (sqlDataReader.GetString(4) == "Nam")
                {
                    imgAvatar.Source = new BitmapImage(
                new Uri("../img/user_male.png", UriKind.Relative));
                }
                else
                {
                    imgAvatar.Source = new BitmapImage(
                new Uri("../img/user_female.png", UriKind.Relative));
                }
            }
            txtMa.Text = sqlDataReader.IsDBNull(1) ? "" : sqlDataReader.GetString(1);
            txtTen.Text = sqlDataReader.IsDBNull(2) ? "" : sqlDataReader.GetString(2);
            txtNgaySinh.Text = sqlDataReader.IsDBNull(3) ? "" : sqlDataReader.GetDateTime(3)+"";
            txtGioiTinh.Text = sqlDataReader.IsDBNull(4) ? "" : sqlDataReader.GetString(4);
            txtQuocTich.Text = sqlDataReader.IsDBNull(5) ? "" : sqlDataReader.GetString(5);
            txtTonGiao.Text = sqlDataReader.IsDBNull(6) ? "" : sqlDataReader.GetString(6);
            txtNoiSinh.Text = sqlDataReader.IsDBNull(7) ? "" : sqlDataReader.GetString(7);
            txtDiaChi.Text = sqlDataReader.IsDBNull(8) ? "" : sqlDataReader.GetString(8);
            txtCMND.Text = sqlDataReader.IsDBNull(9) ? "" : sqlDataReader.GetInt32(9)+"";
            txtViTri.Text = sqlDataReader.IsDBNull(10) ? "" : sqlDataReader.GetString(10);
            txtChucDanh.Text = sqlDataReader.IsDBNull(11) ? "" : sqlDataReader.GetString(11);
            txtPhongBan.Text = sqlDataReader.IsDBNull(12) ? "" : sqlDataReader.GetString(12);
            txtNgayVaoLam.Text = sqlDataReader.IsDBNull(13) ? "" : sqlDataReader.GetDateTime(13)+"";
            txtSDT.Text = sqlDataReader.IsDBNull(14) ? "" : sqlDataReader.GetInt32(14)+"";
            txtEmail.Text = sqlDataReader.IsDBNull(15) ? "" : sqlDataReader.GetString(15);

            sqlDataReader.Close();
            conn.Close();

        }

        private void btnExit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
