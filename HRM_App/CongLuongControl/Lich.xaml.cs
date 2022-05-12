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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HRM_App.CongLuongControl
{
    /// <summary>
    /// Interaction logic for Lich.xaml
    /// </summary>
    public partial class Lich : UserControl
    {
        private string sqlstring = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\QLNS.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
        private SqlConnection conn;
        List<DateTime> ngayCong = new List<DateTime>();
        List<DayOfWeek> tkb = new List<DayOfWeek>();
        int soNgayPhaiLam = 0;
        int soNgayNghi = 0;
        int soNgayCong = 0;
        private string _maNV;
        public Lich()
        {
            InitializeComponent();


        }
        public Lich(string MaNV)
        {

            InitializeComponent();
            conn = new SqlConnection(sqlstring);
            _maNV = MaNV;
            conn.Open();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.CommandText = "select NGAY from LICHSUCHAMCONG WHERE MANV='" + MaNV + "'";
            sqlCommand.Connection = conn;

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                ngayCong.Add(sqlDataReader.GetDateTime(0));
                calChamCong.SelectedDates.Add(sqlDataReader.GetDateTime(0));
                soNgayCong++;
            }
            sqlDataReader.Close();
            txbSoNgayCong.Text = soNgayCong + "";

            sqlCommand.CommandText = "select LICHTUAN from LICHCONG WHERE MANV='" + MaNV + "'";
            sqlDataReader = sqlCommand.ExecuteReader();
            sqlDataReader.Read();
            if (sqlDataReader.HasRows && !sqlDataReader.IsDBNull(0))
            {
                string[] tkb2 = sqlDataReader.GetString(0).Split(',');
                for (int i = 0; i < tkb2.Length; i++)
                {
                    if (tkb2[i] == "2")
                    {
                        tkb.Add(DayOfWeek.Monday);
                    }
                    else if (tkb2[i] == "3")
                    {
                        tkb.Add(DayOfWeek.Tuesday);
                    }
                    else if (tkb2[i] == "4")
                    {
                        tkb.Add(DayOfWeek.Wednesday);
                    }
                    else if (tkb2[i] == "5")
                    {
                        tkb.Add(DayOfWeek.Thursday);
                    }
                    else if (tkb2[i] == "6")
                    {
                        tkb.Add(DayOfWeek.Friday);
                    }
                    else if (tkb2[i] == "7")
                    {
                        tkb.Add(DayOfWeek.Saturday);
                    }
                }

            }
            sqlDataReader.Close();


            for (int i = 1; i <= DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month); i++)
            {
                DateTime dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, i);

                if (tkb.Contains(dateTime.DayOfWeek) == true)
                {
                    soNgayPhaiLam++;
                }
            }
            for (int i = 1; i <= DateTime.Today.Day; i++)
            {
                DateTime dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, i);

                if (tkb.Contains(dateTime.DayOfWeek) == true && !ngayCong.Contains(dateTime.Date))
                {
                    soNgayNghi++;
                    calChamCong.BlackoutDates.Add(new CalendarDateRange(dateTime));
                }
            }
            txbSoNgayNghi.Text = soNgayNghi + "";

            if(soNgayNghi <= 1)
            {
                txbDanhGia.Text = "Tốt";
            }
            else if(soNgayNghi>=2 && soNgayNghi <= 3)
            {
                txbDanhGia.Text = "Khá";
            }else if (soNgayNghi > 3)
            {
                txbDanhGia.Text = "Không tốt";
            }

            sqlCommand.CommandText = "select LUONGCOBAN,HESOPHUCAP,GHICHU,HESOKHAUTRU from CHAMCONG WHERE MANV='" + MaNV + "'";
            sqlDataReader = sqlCommand.ExecuteReader();

            if (sqlDataReader.Read())
            {
                txtLuongCoBan.Text = sqlDataReader.HasRows == false || sqlDataReader.IsDBNull(0) ? "0" : sqlDataReader.GetSqlMoney(0) + "";
                txtLuongThucTe.Text = sqlDataReader.HasRows == false ? "0" : (sqlDataReader.IsDBNull(1) ? txtLuongCoBan.Text : decimal.Parse(txtLuongCoBan.Text) * (1 + sqlDataReader.GetDecimal(1)) + "");
                txtDanhGiaGhiChu.Text = sqlDataReader.HasRows == false || sqlDataReader.IsDBNull(2) ? "" : sqlDataReader.GetString(2);
                if(DateTime.Now.Day == 27 || DateTime.Now.Day == 28 || DateTime.Now.Day == 29 || DateTime.Now.Day == 30 || DateTime.Now.Day == 31)
                {
                    txtLuongThucTe.Text = decimal.Parse(txtLuongThucTe.Text) - (int.Parse(txbSoNgayNghi.Text) * (sqlDataReader.IsDBNull(2) ? 0 : sqlDataReader.GetDecimal(2))) + "";
                }
            }
            
          
            try
            {
                if (sqlDataReader.HasRows)
                {
                    sqlDataReader.Close();

                    SqlCommand sqlCommand2 = new SqlCommand();
                    sqlCommand2.CommandType = System.Data.CommandType.Text;
                    sqlCommand2.CommandText = "update CHAMCONG set BUOIVANG ='" + txbSoNgayNghi.Text + "', XEPLOAI=N'" + txbDanhGia.Text + "', SONGAYLAM='" + txbSoNgayCong.Text + "'" +
                        ", SONGAYPHAILAM ='" + soNgayPhaiLam + "' where MANV='" + MaNV + "'";
                    sqlCommand2.Connection = conn;
                    sqlCommand2.ExecuteNonQuery();
                }
                else
                {
                    sqlDataReader.Close();

                    SqlCommand sqlCommand2 = new SqlCommand();
                    sqlCommand2.CommandType = System.Data.CommandType.Text;
                    sqlCommand2.CommandText = "insert CHAMCONG(MANV,BUOIVANG,SONGAYLAM,SONGAYPHAILAM,XEPLOAI) values('" + MaNV + "'," +
                        "'" + soNgayNghi + "','" + soNgayCong + "','" + soNgayPhaiLam + "',N'" + txbDanhGia.Text + "')";
                    sqlCommand2.Connection = conn;
                    sqlCommand2.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                sqlDataReader.Close();
                MessageBox.Show("Lỗi!\n"+ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            conn.Close();
            
            
        }
        private void calChamCong_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
           for(int i = 0; i < ngayCong.Count; i++)
            {
                calChamCong.SelectedDates.Add(ngayCong[i]);
            }

        }

 

        private void iconChinhSua_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
            PackIcon ico = (PackIcon)sender;
            if(ico.Kind == PackIconKind.NoteEdit)
            {
                ico.Kind = PackIconKind.FileDocumentBoxTick;
                ico.Foreground = Brushes.DarkRed;
                txtDanhGiaGhiChu.IsReadOnly = false;
            }
            else
            {
                ico.Kind = PackIconKind.NoteEdit;
                ico.Foreground = Brushes.DarkOliveGreen;
                txtDanhGiaGhiChu.IsReadOnly = true;
                if (MessageBox.Show("Bạn có muốn lưu không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {
                    conn.Open();
                    try
                    {
                        SqlCommand sqlCommand = new SqlCommand();
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        sqlCommand.CommandText = "update CHAMCONG set GHICHU = N'" + txtDanhGiaGhiChu.Text + "' WHERE MANV='" + _maNV + "'";
                        sqlCommand.Connection = conn;

                        int ret = sqlCommand.ExecuteNonQuery();
                        if (ret > 0)
                        {
                            MessageBox.Show("Lưu thành công");
                        }
                        else
                        {
                            MessageBox.Show("Lưu không thành công");
                        }
                    }catch(Exception ex)
                    {
                        MessageBox.Show("Lưu thất bại!\n"+ex.Message,"Lỗi",MessageBoxButton.OK,MessageBoxImage.Error);
                    }
                    finally
                    {
                        conn.Close();
                    }
                    
                };
            }

        }
    }
}
