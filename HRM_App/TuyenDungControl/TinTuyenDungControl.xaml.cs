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

namespace HRM_App.TuyenDungControl
{
    /// <summary>
    /// Interaction logic for TinTuyenDung.xaml
    /// </summary>
    public partial class TinTuyenDungControl : UserControl
    {
        private string sqlstring = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\QLNS.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
        private SqlConnection conn;
        List<TinTuyenDung> listTD = new List<TinTuyenDung>();
        public TinTuyenDungControl()
        {
            InitializeComponent();
           

            conn = new SqlConnection(sqlstring);
            conn.Open();
           
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.CommandText = "select TUYENDUNG.MATD,TENTD,VITRI,COUNT(MAUV) AS SOLUONGUV,HANHS" +
                " from TUYENDUNG LEFT JOIN UNGVIEN ON TUYENDUNG.MATD = UNGVIEN.MATD " +
                "GROUP BY TUYENDUNG.MATD,TENTD,VITRI,HANHS";
            sqlCommand.Connection = conn;

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                listTD.Add(new TinTuyenDung()
                {
                    MATD = sqlDataReader.GetString(0),
                    TenTD = sqlDataReader.GetString(1),
                    ViTriTD = sqlDataReader.GetString(2),
                    SoLuongUngVien = sqlDataReader.GetInt32(3),
                    HanNopHoSo = sqlDataReader.GetDateTime(4)
                });
            }
            sqlDataReader.Close();
            conn.Close();

            lsvTinTD.ItemsSource = listTD;
        }
        public TinTuyenDungControl(string text)
        {
            InitializeComponent();
            conn = new SqlConnection(sqlstring);
            conn.Open();

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.CommandText = "select TUYENDUNG.MATD,TENTD,VITRI,COUNT(MAUV) AS SOLUONGUV,HANHS" +
                " from TUYENDUNG LEFT JOIN UNGVIEN ON TUYENDUNG.MATD = UNGVIEN.MATD " +
                "where TENTD like N'%"+text+"%' " + 
                "GROUP BY TUYENDUNG.MATD,TENTD,VITRI,HANHS";
            sqlCommand.Connection = conn;

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                listTD.Add(new TinTuyenDung()
                {
                    MATD = sqlDataReader.GetString(0),
                    TenTD = sqlDataReader.GetString(1),
                    ViTriTD = sqlDataReader.GetString(2),
                    SoLuongUngVien = sqlDataReader.GetInt32(3),
                    HanNopHoSo = sqlDataReader.GetDateTime(4)
                });
            }
            sqlDataReader.Close();
            conn.Close();

            lsvTinTD.ItemsSource = listTD;
        }
        public object GetItemSelected()
        {
            return lsvTinTD.SelectedItem;
        }
    }
}
