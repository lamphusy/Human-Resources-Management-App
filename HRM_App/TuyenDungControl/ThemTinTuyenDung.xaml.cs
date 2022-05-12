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
    /// Interaction logic for ThemTinTuyenDung.xaml
    /// </summary>
    public partial class ThemTinTuyenDung : Window
    {
        public bool isXacNhan;
        public bool isChinhSua;
        private string sqlstring = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\QLNS.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
        private SqlConnection conn;
        public ThemTinTuyenDung()
        {
            InitializeComponent();
            isChinhSua = false;
            isXacNhan = true;
            conn = new SqlConnection(sqlstring);
            conn.Open();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.CommandText = "select MAPHONG from PHONGBAN";
            sqlCommand.Connection = conn;

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            
            while (sqlDataReader.Read())
            {
                txbPhg.Items.Add(sqlDataReader.GetString(0));
            }
            sqlDataReader.Close();
            dpNgayTao.SelectedDate = DateTime.Now.Date;
            
            conn.Close();
        }
        public ThemTinTuyenDung(string maTD)
        {
            InitializeComponent();
            isChinhSua = true;
            isXacNhan = false;
            conn = new SqlConnection(sqlstring);
            conn.Open();
            //THem phong ban
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.CommandText = "select MAPHONG from PHONGBAN";
            sqlCommand.Connection = conn;

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            while (sqlDataReader.Read())
            {
                txbPhg.Items.Add(sqlDataReader.GetString(0));
            }
            sqlDataReader.Close();
            //goi du lieu
            sqlCommand.CommandText = "select MATD,TENTD,VITRI,GHICHU,SOLUONG,YEUCAU" +
                    ",PHONGDX,NGUOIQUANLY,HANHS,NGAYTAO,LUONG from TUYENDUNG where MATD = '" + maTD+"'";
            sqlDataReader = sqlCommand.ExecuteReader();

            //bien chong loi: 
            string phongban = "";
            string nguoiquanly = "";

            while (sqlDataReader.Read())
            {
                if (!sqlDataReader.IsDBNull(0))
                    txtMaTD.Text = sqlDataReader.GetString(0);
                if (!sqlDataReader.IsDBNull(1))
                    txtTenTD.Text = sqlDataReader.GetString(1);
                if (!sqlDataReader.IsDBNull(2))
                    txtViTriTD.Text = sqlDataReader.GetString(2);
                if(!sqlDataReader.IsDBNull(3))
                    txtGhiChu.Text = sqlDataReader.GetString(3);
                if (!sqlDataReader.IsDBNull(4))
                    txtSL.Text = sqlDataReader.GetInt32(4) + "";
                if (!sqlDataReader.IsDBNull(5))
                    txtYeuCau.Text = sqlDataReader.GetString(5);
                if (!sqlDataReader.IsDBNull(6))
                    phongban = sqlDataReader.GetString(6);
                if (!sqlDataReader.IsDBNull(7))
                    nguoiquanly = sqlDataReader.GetString(7);
                if (!sqlDataReader.IsDBNull(8))
                    dpHanNop.SelectedDate = sqlDataReader.GetDateTime(8);
                if (!sqlDataReader.IsDBNull(9))
                    dpNgayTao.SelectedDate = sqlDataReader.GetDateTime(9);
                if (!sqlDataReader.IsDBNull(10))
                    txtLuong.Text = sqlDataReader.GetString(10);
            }
            sqlDataReader.Close();
            conn.Close();
            txbPhg.Text = phongban;
            for(int i=0;i<txbQuanLy.Items.Count;i++)
            {
                if(txbQuanLy.Items.GetItemAt(i).ToString().Substring(0, txbQuanLy.Items.GetItemAt(i).ToString().IndexOf(" ")) == nguoiquanly)
                {
                    txbQuanLy.SelectedIndex = i;
                    break;
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txtMaTD.Text == "")
            {
                txtMaTD.BorderBrush = Brushes.Red;
            }
            else
            {
                if (txbPhg.SelectedIndex < 0)
                {
                    txbPhg.BorderBrush = Brushes.Red;
                }
                else
                {
                    if(dpHanNop.SelectedDate == null)
                    {
                        dpHanNop.BorderBrush = Brushes.Red;
                        return;
                    }
                    if(txtTenTD.Text == "")
                    {
                        txtTenTD.BorderBrush = Brushes.Red;
                        return;
                    }
                    if (txbQuanLy.Text == "")
                    {
                        txbQuanLy.BorderBrush = Brushes.Red;
                        return;
                    }
                    if (txtViTriTD.Text == "")
                    {
                        txtViTriTD.BorderBrush = Brushes.Red;
                        return;
                    }
                    if (isXacNhan == true && isChinhSua == false)
                    {
                        changeConnection("open");
                        try
                        {
                            SqlCommand sqlCommand = new SqlCommand();
                            sqlCommand.CommandType = System.Data.CommandType.Text;
                            sqlCommand.CommandText = "insert into TUYENDUNG(MATD,TENTD,VITRI,GHICHU,SOLUONG,YEUCAU" +
                                ",PHONGDX,NGUOIQUANLY,HANHS,NGAYTAO,LUONG) values ('" + txtMaTD.Text + "',N'" + txtTenTD.Text + "',N'" + txtViTriTD.Text + "',N'"
                                + txtGhiChu.Text + "',N'" + txtSL.Text + "',N'" + txtYeuCau.Text + "',N'" + txbPhg.Text + "',N'" +(txbQuanLy.Text.Length == 0 ? "" :txbQuanLy.Text.Substring(0, txbQuanLy.Text.IndexOf(" "))) + "',N'"
                                + dpHanNop.Text + "',N'" + dpNgayTao.Text + "',N'" + txtLuong.Text + "')";
                            sqlCommand.Connection = conn;

                            int ret = sqlCommand.ExecuteNonQuery();

                            if (ret > 0)
                            {
                                MessageBox.Show("Thêm thành công");
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Thêm không thành công");
                            }
                        }catch(Exception ex)
                        {
                            MessageBox.Show("Thêm không thành công\n" +ex.Message, "Lỗi",MessageBoxButton.OK,MessageBoxImage.Error);
                        }
                     
                        changeConnection("close");

                    }
                    if (isXacNhan == false && isChinhSua == true)
                    {
                        changeConnection("open");
                        try
                        {
                            SqlCommand sqlCommand = new SqlCommand();
                            sqlCommand.CommandType = System.Data.CommandType.Text;
                            sqlCommand.CommandText =
                            "update TUYENDUNG set TENTD=N'" + txtTenTD.Text + "', VITRI=N'" + txtViTriTD.Text + "',GHICHU=N'" + txtGhiChu.Text + "',SOLUONG=N'"
                                + txtSL.Text + "',YEUCAU=N'" + txtYeuCau.Text + "',PHONGDX=N'" + txbPhg.Text + "',NGUOIQUANLY=N'"
                                + (txbQuanLy.Text.Length == 0 ? "" : txbQuanLy.Text.Substring(0, txbQuanLy.Text.IndexOf(" "))) + "',HANHS=N'" + dpHanNop.Text + "',NGAYTAO=N'" + dpNgayTao.Text + "', " +
                                "LUONG=N'" + txtLuong.Text + "'" +
                                " where MATD=N'" + txtMaTD.Text + "'";
                            sqlCommand.Connection = conn;

                            int ret = sqlCommand.ExecuteNonQuery();

                            if (ret > 0)
                            {
                                MessageBox.Show("Sửa thành công");
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Sửa không thành công");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Sửa không thành công\n" + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        changeConnection("close");
                    }
                }

            }
            

        }

        private void txbPhg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeConnection("open");
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.CommandText = "select MANV,TENNV from NHANVIEN where PHG='" + txbPhg.SelectedItem + "'";
            sqlCommand.Connection = conn;

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            txbQuanLy.Items.Clear();
            while (sqlDataReader.Read())
            {
                txbQuanLy.Items.Add(sqlDataReader.GetString(0) +" - "+ sqlDataReader.GetString(1));
            }
            sqlDataReader.Close();
            changeConnection("close");
        }
        private void changeConnection(string request)
        {
            if(request == "open")
            {
                if (conn.State.ToString() == "Closed")
                {
                    conn.Open();
                }
            }
            else if (request == "close")
            {
                if (conn.State.ToString() == "Opened")
                {
                    conn.Close();
                }
            }
            
        } 
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            isXacNhan = false;
            this.Close();
            isChinhSua = false;

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            isXacNhan = false;
            isChinhSua = false;
        }
    }
}
