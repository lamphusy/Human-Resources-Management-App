using System;
using System.Collections.Generic;
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
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using System.Data.SqlClient;

namespace HRM_App
{
    /// <summary>
    /// Interaction logic for MainControl.xaml
    /// </summary>
    public partial class MainControl : UserControl
    {
        private string sqlstring = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\QLNS.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
        private SqlConnection conn;
        public MainControl()
        {
            
            InitializeComponent();

            NhanSuChinhThuc = 0;
            TangMoiTrongThang = 0;
            SLDangTuyen = 0;

            conn = new SqlConnection(sqlstring);
            conn.Open();

            //chart 1

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.CommandText = "select distinct tenphong, count(manv) "+
                    "from PHONGBAN left join NHANVIEN "+
                    "on PHONGBAN.MAPHONG = NHANVIEN.PHG "+
                    "group by TENPHONG";
            sqlCommand.Connection = conn;
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            SeriesCollection seriesViews = new SeriesCollection();
            while (sqlDataReader.Read())
            {
                seriesViews.Add(new PieSeries
                {
                    Title = sqlDataReader.GetString(0),
                    Values = new ChartValues<ObservableValue> { new ObservableValue(sqlDataReader.GetInt32(1)) },
                    DataLabels = true
                });
            }
            sqlDataReader.Close();
            NSTheoPB = seriesViews;

            //chart 2

            sqlCommand.CommandText = "select chucvu, count(manv) " +
                    "from Nhanvien " +
                    "group by chucvu";
            sqlDataReader = sqlCommand.ExecuteReader();
            SeriesCollection seriesViews1 = new SeriesCollection();
            while (sqlDataReader.Read())
            {
                seriesViews1.Add(new PieSeries
                {
                    Title = sqlDataReader.GetString(0),
                    Values = new ChartValues<ObservableValue> { new ObservableValue(sqlDataReader.GetInt32(1)) },
                    DataLabels = true
                });
            }
            sqlDataReader.Close();

            NSTheoCV = seriesViews1;

            //chart 3

            sqlCommand.CommandText = "select  month(ngvaolam), count(month(ngvaolam))"+
                       " from Nhanvien where year(ngvaolam) ="+ DateTime.Now.Year+
                       " group by month(ngvaolam)"+
                       " order by month(ngvaolam) asc";
            sqlDataReader = sqlCommand.ExecuteReader();
            
            List<int> listNgayVaoLams = new List<int>();
            for(int i = 0; i < 12; i++)
            {
                listNgayVaoLams.Add(0);
            }
            while (sqlDataReader.Read())
            {
                listNgayVaoLams[sqlDataReader.GetInt32(0)-1] = sqlDataReader.GetInt32(1);
            }
            sqlDataReader.Close();
            ChartValues<int> listNVL_value = new ChartValues<int>();
            for (int i = 0; i <12; i++)
            {
      
                listNVL_value.Add(listNgayVaoLams[i]);
            }

            TangNS = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title="TangNhanSu",
                    Values = listNVL_value,
                    DataLabels= true
                }
            };

            //card
            sqlCommand.CommandText = "select count(*) from nhanvien";
            sqlDataReader = sqlCommand.ExecuteReader();
            if (sqlDataReader.Read())
            {
                NhanSuChinhThuc = sqlDataReader.GetInt32(0);
            }
            sqlDataReader.Close();

            //card
            sqlCommand.CommandText = "select count(*) from nhanvien where month(ngvaolam)="+DateTime.Now.Month+" and year(ngvaolam)="+DateTime.Now.Year;
            sqlDataReader = sqlCommand.ExecuteReader();
            if (sqlDataReader.Read())
            {
                TangMoiTrongThang = sqlDataReader.GetInt32(0);
            }
            sqlDataReader.Close();

            //card
            sqlCommand.CommandText = "select sum(SOLUONG) from TUYENDUNG";
            sqlDataReader = sqlCommand.ExecuteReader();
            if (sqlDataReader.Read())
            {
                SLDangTuyen = sqlDataReader.GetInt32(0);
            }
            sqlDataReader.Close();

            //card
            sqlCommand.CommandText = "select count(distinct manv) from THAMGIADAOTAO";
            sqlDataReader = sqlCommand.ExecuteReader();
            if (sqlDataReader.Read())
            {
                SoLuongThamGiaDaoTao = sqlDataReader.GetInt32(0);
            }
            sqlDataReader.Close();


            conn.Close();

            DataContext = this;
        }

        public SeriesCollection TangNS { get; set; }
        public SeriesCollection NSTheoPB { get; private set; }
        public SeriesCollection NSTheoCV { get; private set; }
        public int NhanSuChinhThuc { get; private set; }
        public int TangMoiTrongThang { get; private set; }
        public int SLDangTuyen { get; private set; }
        public int SoLuongThamGiaDaoTao { get; set; }
    }
}
