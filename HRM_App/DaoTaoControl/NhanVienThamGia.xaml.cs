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
    /// Interaction logic for NhanVienThamGia.xaml
    /// </summary>
    public partial class NhanVienThamGia : Window
    {
        class NV_TG
        {
            public string MANV;
            public DateTime NgayThamGia;
        }
        private string sqlstring = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\QLNS.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
        private SqlConnection conn;
        private string _MaDT = "";
        private DateTime _NgayBD;
        private DateTime _NgayKT;
        private List<NV_TG> updateListNv;
        public NhanVienThamGia()
        {
            InitializeComponent();
           
        }
        public NhanVienThamGia(string maDT)
        {
            InitializeComponent();
            updateListNv = new List<NV_TG>();
            conn = new SqlConnection(sqlstring);

            conn.Open();
            SqlCommand sqlCommand =  new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.CommandText = "select NGBD,NGKT" +
                " from KHOADAOTAO where MADT='" + maDT + "'";
            sqlCommand.Connection = conn;

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            sqlDataReader.Read();
            _NgayBD = sqlDataReader.GetDateTime(0);
            _NgayKT = sqlDataReader.GetDateTime(1);
            sqlDataReader.Close();

            sqlCommand.CommandText = "select MANV,NGAYTHAMGIA from THAMGIADAOTAO where MADT='" + maDT + "'";
            sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                updateListNv.Add(new NV_TG()
                {
                    MANV = sqlDataReader.GetString(0),
                    NgayThamGia = sqlDataReader.IsDBNull(1) ? DateTime.Now.Date : sqlDataReader.GetDateTime(1)
                });
            }
            sqlDataReader.Close();
            if(DateTime.Now >= _NgayBD && DateTime.Now <= _NgayKT)
            {
                
                for (int i = 0; i < updateListNv.Count; i++)
                {
                    try
                    {
                        sqlCommand.CommandText = "update THAMGIADAOTAO set TIENDO='" + Math.Ceiling(((DateTime.Now.Date.Subtract(updateListNv[i].NgayThamGia).Days) * 1.0 / (_NgayKT.Subtract(_NgayBD).Days) * 100)) +
                 "' where MANV='" + updateListNv[i].MANV + "' and MADT ='" + maDT + "'";
                        //      sqlCommand.CommandText = "update THAMGIADAOTAO set NGAYTHAMGIA='"+updateListNv[i].NgayThamGia+ "' where MANV='" + updateListNv[i].MANV + "' and MADT ='" + maDT + "'";
                        sqlCommand.ExecuteNonQuery();
                    }
                    catch { }
                }
            }


           
            _MaDT = maDT;
            conn.Close();
            render(maDT);

        }
        private void render(string maDT)
        {
            lsvNV.Items.Clear();
            conn.Open();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.CommandText = "select NHANVIEN.MANV,TENNV,PHG,SDT,TIENDO,DANHGIA" +
                " from THAMGIADAOTAO JOIN NHANVIEN ON THAMGIADAOTAO.MANV = NHANVIEN.MANV where MADT='" + maDT + "'";
            sqlCommand.Connection = conn;

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                lsvNV.Items.Add(new NhanVienThamGiaDT()
                {
                    MANV = sqlDataReader.GetString(0),
                    Ten = sqlDataReader.IsDBNull(1)?"": sqlDataReader.GetString(1),
                    PhongBan = sqlDataReader.IsDBNull(2) ? "" : sqlDataReader.GetString(2),
                    SDT = sqlDataReader.IsDBNull(3) ? 0 : sqlDataReader.GetInt32(3),
                    TienDo = sqlDataReader.IsDBNull(4) ? 0 : sqlDataReader.GetInt32(4),
                    DanhGia = sqlDataReader.IsDBNull(5) ? "" : sqlDataReader.GetString(5)
                });
            }
            sqlDataReader.Close();
            conn.Close();
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if(DateTime.Now > _NgayKT)
            {
                MessageBox.Show("Khóa đào tạo đã kết thúc, dữ liệu sẽ bị xóa sau 15 ngày!");
            }
            else
            {
                NhanVienThamGiaAdd addw = new NhanVienThamGiaAdd(_MaDT, _NgayBD);
                addw.ShowDialog();
                while (addw.isOK == true)
                {
                    addw.isOK = false;
                    render(_MaDT);
                }
            }
           
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (DateTime.Now > _NgayKT)
            {
                MessageBox.Show("Khóa đào tạo đã kết thúc, dữ liệu sẽ bị xóa sau 15 ngày!");
            }
            else
            {
                if (lsvNV.SelectedIndex > -1)
                {
                    string nvDuocChon = updateListNv[lsvNV.SelectedIndex].MANV;
                    conn.Open();
                    try
                    {
                        SqlCommand cmd = new SqlCommand("delete THAMGIADAOTAO where MANV='" + nvDuocChon + "' and MADT ='" + _MaDT + "'", conn);
                        int ret = cmd.ExecuteNonQuery();
                        if (ret > 0)
                        {
                            MessageBox.Show("Xóa thàng công!");
                        }
                        else
                        {
                            MessageBox.Show("Xóa không thành công!", "Lỗi");
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Xóa không thành công!\n" + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    conn.Close();
                    render(_MaDT);

                }
            }
            
        }

        private void btnDanhGia_Click(object sender, RoutedEventArgs e)
        {
            if (lsvNV.SelectedIndex > -1)
            {
                string nvDuocChon = updateListNv[lsvNV.SelectedIndex].MANV;
                DanhGia danhGia = new DanhGia(nvDuocChon,_MaDT);
                danhGia.ShowDialog();
                render(_MaDT);
            }
        }
    }
}
