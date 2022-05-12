using MaterialDesignThemes.Wpf;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaterialDesignThemes;
using System.Data.SqlClient;

namespace HRM_App.DaoTaoControl
{
    
    /// <summary>
    /// Interaction logic for DaoTao.xaml
    /// </summary>
    public partial class DaoTao : UserControl
    {
        List<KhoaDaoTao> DsKhoa = new List<KhoaDaoTao>();
        private string sqlstring = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\QLNS.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
        private SqlConnection conn;
        void render()
        {
            pnHienThi.Children.Clear();
            DsKhoa.Clear();
            conn.Open();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.CommandText = "select KHOADAOTAO.MADT,TENDT,MOTA,COUNT(MANV),HOCPHI,TGDT from KHOADAOTAO left join  THAMGIADAOTAO on KHOADAOTAO.MADT = THAMGIADAOTAO.MADT group by KHOADAOTAO.MADT,TENDT,MOTA,HOCPHI,TGDT";
            sqlCommand.Connection = conn;

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            while (sqlDataReader.Read())
            {
                DsKhoa.Add(new KhoaDaoTao()
                {
                    MaDaoTao = sqlDataReader.GetString(0),
                    TenKhoaDaoTao = sqlDataReader.GetString(1),
                    GioiThieuKhoaHoc = sqlDataReader.GetString(2),
                    SoLuongDaoTao = sqlDataReader.GetInt32(3),
                    ChiPhiKhoaHoc = double.Parse(sqlDataReader.GetSqlMoney(4) + ""),
                    ThoiGianDaoTao = sqlDataReader.GetString(5)
                }) ;
            }
            sqlDataReader.Close();
            conn.Close();
            for (int i = 0; i < DsKhoa.Count(); i++)
            {
                Card the = new Card();

                string str = "<ContentControl xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" ContentTemplate=\"{DynamicResource trainTemplate}\"></ContentControl>";
                ContentControl theControl = (ContentControl)XamlReader.Parse(str);
                theControl.Content = DsKhoa[i];

                the.Content = theControl;

                pnHienThi.Children.Add(the);
            }

        }
        void render(string timkiem,int selectedIndex)
        {
            if(selectedIndex == 1)
            {
                pnHienThi.Children.Clear();
                DsKhoa.Clear();
                conn.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandText = "select KHOADAOTAO.MADT,TENDT,MOTA,COUNT(MANV),HOCPHI,TGDT from KHOADAOTAO left join THAMGIADAOTAO on KHOADAOTAO.MADT = THAMGIADAOTAO.MADT"
                  + " WHERE TENDT LIKE N'%" + timkiem + "%' group by KHOADAOTAO.MADT,TENDT,MOTA,HOCPHI,TGDT"; 
                sqlCommand.Connection = conn;

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    DsKhoa.Add(new KhoaDaoTao()
                    {
                        MaDaoTao = sqlDataReader.GetString(0),
                        TenKhoaDaoTao = sqlDataReader.GetString(1),
                        GioiThieuKhoaHoc = sqlDataReader.GetString(2),
                        SoLuongDaoTao = sqlDataReader.GetInt32(3),
                        ChiPhiKhoaHoc = double.Parse(sqlDataReader.GetSqlMoney(4) + ""),
                        ThoiGianDaoTao = sqlDataReader.GetString(5)

                    }) ; 
                }
                sqlDataReader.Close();
                conn.Close();
                for (int i = 0; i < DsKhoa.Count(); i++)
                {

                    Card the = new Card();

                    string str = "<ContentControl xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" ContentTemplate=\"{DynamicResource trainTemplate}\"></ContentControl>";
                    ContentControl theControl = (ContentControl)XamlReader.Parse(str);
                    theControl.Content = DsKhoa[i];

                    the.Content = theControl;

                    pnHienThi.Children.Add(the);
                }
            }
            if (selectedIndex == 2)
            {
                pnHienThi.Children.Clear();
                DsKhoa.Clear();
                conn.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandText = "select KHOADAOTAO.MADT,TENDT,MOTA,SL,HOCPHI,TGDT " +
                                            "from (select MADT, count(*) as SL " +
                                            "from THAMGIADAOTAO a " +
                                            "where exists(select *from THAMGIADAOTAO b where MANV IN (select MANV from NHANVIEN where TENNV LIKE N'%" + timkiem + "%') and a.MADT = b.MADT) " +
                                            "group by MADT) c " +
                                            "join KHOADAOTAO on KHOADAOTAO.MADT = c.MADT";
                sqlCommand.Connection = conn;

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    DsKhoa.Add(new KhoaDaoTao()
                    {
                        MaDaoTao = sqlDataReader.GetString(0),
                        TenKhoaDaoTao = sqlDataReader.GetString(1),
                        GioiThieuKhoaHoc = sqlDataReader.GetString(2),
                        SoLuongDaoTao = sqlDataReader.GetInt32(3),
                        ChiPhiKhoaHoc = double.Parse(sqlDataReader.GetSqlMoney(4) + ""),
                        ThoiGianDaoTao = sqlDataReader.GetString(5)

                    });
                }
                sqlDataReader.Close();
                conn.Close();
                for (int i = 0; i < DsKhoa.Count(); i++)
                {

                    Card the = new Card();

                    string str = "<ContentControl xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" ContentTemplate=\"{DynamicResource trainTemplate}\"></ContentControl>";
                    ContentControl theControl = (ContentControl)XamlReader.Parse(str);
                    theControl.Content = DsKhoa[i];

                    the.Content = theControl;

                    pnHienThi.Children.Add(the);
                }
            }
        }
        public DaoTao()
        {
            InitializeComponent();

            conn = new SqlConnection(sqlstring);
        
            
            this.DataContext = this;
        }

        private void btnThemKhoa_Click(object sender, RoutedEventArgs e)
        {
            ThemKhoaDaoTao newKhoa = new ThemKhoaDaoTao();
                    newKhoa.ShowDialog();
            if (newKhoa.check == true)
            {
                conn.Open();
                try
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandText = "insert into KHOADAOTAO(MADT,TENDT,MOTA,MUCTIEU,PHG,NGUOIDT,TGDT,NGBD,NGKT,HOCPHI,DIADIEM,LOAIHINHDT,DVTAITRO,GHICHU)" +
                        "values (N'" + newKhoa.txbMa.Text + "',N'" + newKhoa.txbTen.Text + "',N'" + newKhoa.txbMoTa.Text + "',N'" + newKhoa.txbMucTieu.Text + "',N'" +
                        newKhoa.txbPgDXs.Text.Substring(0, newKhoa.txbPgDXs.Text.IndexOf(' ')) + "',N'" + newKhoa.txbNgDT.Text + "',N'" + newKhoa.txbTgDT.Text + "',N'" + newKhoa.dpBD.Text + "',N'" + newKhoa.dpKT.Text + "',N'" +
                        newKhoa.txbHocPhi.Text + "',N'" + newKhoa.txbDiaDiem.Text + "',N'" + newKhoa.txbLoaiDT.Text + "',N'" + newKhoa.txbTaiTro.Text + "',N'" + newKhoa.txbGhiChu.Text + "')";
                    sqlCommand.Connection = conn;

                    int ret = sqlCommand.ExecuteNonQuery();

                    if (ret > 0)
                    {
                        MessageBox.Show("Thêm thành công");
                    }
                    else
                    {
                        MessageBox.Show("Thêm không thành công");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
               
                conn.Close();
                render();
            }
        }

        private void btnChiTiet_Click(object sender, RoutedEventArgs e)
        {
            Button a = (Button)sender;
            Grid c = (Grid)a.Parent;
            StackPanel d = (StackPanel)c.Parent;
            TextBlock z = (TextBlock)d.Children[0];

            string maDT = z.Text;
            NhanVienThamGia listNV = new NhanVienThamGia(maDT);

            listNV.ShowDialog();
        }
        private void DelData(string MaDT)
        {
            conn.Open();
            try
            {
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = conn;
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandText = "delete from THAMGIADAOTAO where MADT='" + MaDT + "'";

                sqlCommand.ExecuteNonQuery();




                sqlCommand.CommandText = "delete from KHOADAOTAO where MADT='" + MaDT + "'";

                sqlCommand.ExecuteNonQuery();
              
                conn.Close();
                render();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xóa không thành công \n" + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                conn.Close();
            }
        }
        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            Button a = (Button)sender;
            StackPanel b = (StackPanel)a.Parent;
            Grid c = (Grid)b.Parent;
            StackPanel d = (StackPanel)c.Parent;
            TextBlock z = (TextBlock) d.Children[0];

            string delText = z.Text;

            MessageBoxResult result =    MessageBox.Show("Bạn có chắc muốn xóa khóa đào tạo không?", "Xác nhận",
            MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {

                DelData(delText);
            }

        }

        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            
            ThemKhoaDaoTao newKhoa = new ThemKhoaDaoTao();

            Button a = (Button)sender;
            StackPanel b = (StackPanel)a.Parent;
            Grid c = (Grid)b.Parent;
            StackPanel d = (StackPanel)c.Parent;
            TextBlock z = (TextBlock)d.Children[0];

            conn.Open();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.CommandText = "select MADT,TENDT,MOTA,MUCTIEU,KHOADAOTAO.PHG,TENPHONG,NGUOIDT,TGDT,NGBD" +
                ",NGKT,HOCPHI,KHOADAOTAO.DIADIEM,LOAIHINHDT,DVTAITRO,GHICHU from KHOADAOTAO join PHONGBAN on KHOADAOTAO.PHG = PHONGBAN.MAPHONG where MADT=N'"+z.Text+"'";
            sqlCommand.Connection = conn;
           

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                newKhoa.txbMa.Text = sqlDataReader.GetString(0);
                newKhoa.txbTen.Text = sqlDataReader.GetString(1);
                newKhoa.txbMoTa.Text = sqlDataReader.GetString(2);
                newKhoa.txbMucTieu.Text = sqlDataReader.GetString(3);
                newKhoa.txbPgDXs.Text = sqlDataReader.GetString(4)+" - "+sqlDataReader.GetString(5);
                newKhoa.txbNgDT.Text = sqlDataReader.GetString(6);
                newKhoa.txbTgDT.Text = sqlDataReader.GetString(7);
                newKhoa.dpBD.SelectedDate = sqlDataReader.GetDateTime(8);
                newKhoa.dpKT.SelectedDate = sqlDataReader.GetDateTime(9);
                newKhoa.txbHocPhi.Text = sqlDataReader.GetSqlMoney(10) + "";
                newKhoa.txbDiaDiem.Text = sqlDataReader.GetString(11);
                newKhoa.txbLoaiDT.Text = sqlDataReader.GetString(12);
                newKhoa.txbTaiTro.Text = sqlDataReader.GetString(13);
                newKhoa.txbGhiChu.Text = sqlDataReader.GetString(14);
            }
            sqlDataReader.Close();
            conn.Close();
            newKhoa.sua = true;
            newKhoa.ShowDialog();
            

            if (newKhoa.sua == true)
            {
                conn.Open();
                try
                {
                   
                    SqlCommand sqlCommand1 = new SqlCommand();
                    sqlCommand1.CommandType = System.Data.CommandType.Text;
                    sqlCommand1.CommandText = "update KHOADAOTAO " +
                        "set TENDT=N'" + newKhoa.txbTen.Text + "', MOTA=N'" + newKhoa.txbMoTa.Text + "', MUCTIEU=N'" + newKhoa.txbMucTieu.Text + "', PHG=N'" +
                       newKhoa.txbPgDXs.Text.Substring(0, newKhoa.txbPgDXs.Text.IndexOf(' ')) + "', NGUOIDT=N'" + newKhoa.txbNgDT.Text + "', TGDT=N'" + newKhoa.txbTgDT.Text + "', HOCPHI=N'" +
                        newKhoa.txbHocPhi.Text + "', DIADIEM=N'" + newKhoa.txbDiaDiem.Text + "', LOAIHINHDT=N'" + newKhoa.txbLoaiDT.Text + "', DVTAITRO=N'" +
                        newKhoa.txbTaiTro.Text + "', GHICHU=N'" + newKhoa.txbGhiChu.Text + "',NGBD ='"+newKhoa.dpBD.Text+"', NGKT='"+newKhoa.dpKT.Text+"' where MADT=N'" + newKhoa.txbMa.Text + "'";
                    sqlCommand1.Connection = conn;

                    int ret = sqlCommand1.ExecuteNonQuery();

                    if (ret > 0)
                    {
                        MessageBox.Show("Sửa thành công");
                    }
                    else
                    {
                        MessageBox.Show("Sửa không thành công");
                    }
                    conn.Close();
                    render();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Sửa không thành công!\n" +ex.Message,"Lỗi",MessageBoxButton.OK,MessageBoxImage.Error);
                    conn.Close();
                }
            }

        }

        private void btnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            if (cbxLoaiTimKiem.SelectedIndex == 1)
            {
                if (txbTimKiem.Text == "") 
                    return;
                else
                {
                    render(txbTimKiem.Text, 1);
                }
            }
            if (cbxLoaiTimKiem.SelectedIndex == 2)
            {
                if (txbTimKiem.Text == "")
                    return;
                else
                {
                    render(txbTimKiem.Text, 2);
                }
            }

        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            render();
            txbTimKiem.Text = "";
        }

        private void txbTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if (cbxLoaiTimKiem.SelectedIndex == 1)
                {
                    if (txbTimKiem.Text == "")
                        return;
                    else
                    {
                        render(txbTimKiem.Text, 1);
                    }
                }
                if (cbxLoaiTimKiem.SelectedIndex == 2)
                {
                    if (txbTimKiem.Text == "")
                        return;
                    else
                    {
                        render(txbTimKiem.Text, 2);
                    }
                }
            }
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> ListDel = new List<string>();
            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("select MADT from khoadaotao where DATEDIFF(day,NGKT,getdate()) > 15", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ListDel.Add(reader.GetString(0));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            conn.Close();
            foreach (string maXoa in ListDel)
            {
                DelData(maXoa);
            }
            
            render();
        }
    }
}
