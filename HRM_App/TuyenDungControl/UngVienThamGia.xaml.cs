using ControlzEx.Standard;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
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
    /// Interaction logic for UngVienThamGia.xaml
    /// </summary>
    public partial class UngVienThamGia : Window
    {
        private string sqlstring = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\QLNS.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
        private SqlConnection conn;
        private string _MaTD;
        private List<string> PDFfiles;
      
        public UngVienThamGia(string MaTD)
        {
            InitializeComponent();
            conn = new SqlConnection(sqlstring);

            _MaTD = MaTD;
            render();
            PDFfiles = new List<string>();
        }
        private void render()
        {
            lsvUV.Items.Clear();
            conn.Open();

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.CommandText = "SELECT MAUV,TENUV,GIOITINH,SDT,CMND,NGAYSINH,NOICHON,DATACV from UNGVIEN where MATD ='" + _MaTD + "'";
            sqlCommand.Connection = conn;

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                lsvUV.Items.Add(new UngVien
                {
                    MaUV = sqlDataReader.GetString(0),
                    TenUV = sqlDataReader.IsDBNull(1) ? "" : sqlDataReader.GetString(1),
                    GioiTinh = sqlDataReader.IsDBNull(2) ? "" : sqlDataReader.GetString(2),
                    SDT = sqlDataReader.IsDBNull(3) ? 0 : sqlDataReader.GetInt32(3),
                    CMND = sqlDataReader.IsDBNull(4) ? "" : sqlDataReader.GetString(4),
                    NgaySinh = sqlDataReader.IsDBNull(5) ? "" : sqlDataReader.GetDateTime(5) + "",
                    NoiChon = sqlDataReader.IsDBNull(6) ? "" : sqlDataReader.GetString(6),
                    DataCV = sqlDataReader.IsDBNull(7) ? null : sqlDataReader.GetSqlBinary(7).Value,
                });
            }
            sqlDataReader.Close();
            conn.Close();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "PDF Files(*.pdf) | *.pdf|Word Files(*.docx)|*.docx|JPG Files(*.jpg)|*.jpg|PNG Files(*.png)|*.png|All Files(*.*)|*.*";
            dlg.ShowDialog();
            txtFilePath.Text = dlg.FileName;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if(txtFilePath.Text != "")
            {
                SaveFile(txtFilePath.Text);
            }
          
        }

        private void SaveFile(string filePath)
        {
            using(Stream  stream = File.OpenRead(filePath))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);

                string extension = new FileInfo(filePath).Extension;
                try
                {
                    string query = "INSERT INTO UNGVIEN(MAUV,TENUV,GIOITINH,SDT,CMND,NGAYSINH,NOICHON,DATACV,MATD,MORONG) VALUES ('" + txtMaUV.Text + "',N'" + txtTenUV.Text + "',N'" + (radNam.IsChecked == true ? "Nam" : "Nữ") +
                   "','" + txtSDT.Text + "','" + txtCMND.Text + "','" + dpNgaySinh.Text + "',N'" + txtNoiChon.Text + "',@data,'" + _MaTD + "',@extension)";
                    conn.Open();
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandText = query;
                    sqlCommand.Connection = conn;

                    sqlCommand.Parameters.Add("@data", SqlDbType.VarBinary).Value = buffer;
                    sqlCommand.Parameters.Add("@extension", SqlDbType.Char).Value = extension;

                    int ret = sqlCommand.ExecuteNonQuery();
                    conn.Close();
                    if (ret > 0)
                    {
                        MessageBox.Show("Thêm thành công", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        render();
                    }
                    else
                    {
                        MessageBox.Show("Thêm không thành công", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message,"Lỗi",MessageBoxButton.OK,MessageBoxImage.Error);
                    conn.Close();
                }
               
            }

        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            int index = lsvUV.SelectedIndex;
            if (index >= 0)
            {
                UngVien uvHienThoi = (UngVien)lsvUV.SelectedItem;
                string maUV = uvHienThoi.MaUV;
                if (MessageBox.Show("Bạn có chắc muốn xóa ứng viên "+ maUV,"Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    string query = "delete UNGVIEN where mauv='" + maUV + "'";
                    conn.Open();
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandText = query;
                    sqlCommand.Connection = conn;

                    int ret = sqlCommand.ExecuteNonQuery();
                    conn.Close();
                    if (ret > 0)
                    {
                        MessageBox.Show("Xóa thành công", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        render();
                    }
                    else
                    {
                        MessageBox.Show("Xóa không thành công", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
              
               
            }
        }
        private void btnModify_Click(object sender, RoutedEventArgs e)
        {
            int index = lsvUV.SelectedIndex;
            if (index >= 0)
            {
                UngVien uvHienThoi = (UngVien)lsvUV.SelectedItem;
                string maUV = uvHienThoi.MaUV;
                if (MessageBox.Show("Bạn có chắc muốn sửa thông tin ứng viên " + maUV, "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    
                    string query = "update UNGVIEN set TENUV =N'" + txtTenUV.Text + "',GIOITINH = N'" + (radNam.IsChecked == true ? "Nam" : "Nữ") +
                   "',SDT ='" + txtSDT.Text + "',CMND='" + txtCMND.Text + "',NGAYSINH='" + dpNgaySinh.Text + "',NOICHON=N'" + txtNoiChon.Text +
                   "' where mauv ='"+maUV+"'";

                    conn.Open();
                    try 
                    {
                        SqlCommand sqlCommand = new SqlCommand();
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        sqlCommand.CommandText = query;
                        sqlCommand.Connection = conn;

                        int ret = sqlCommand.ExecuteNonQuery();

                        if (ret > 0)
                        {
                            if (txtFilePath.Text != "")
                            {
                                Stream stream = File.OpenRead(txtFilePath.Text);
                                byte[] buffer = new byte[stream.Length];
                                stream.Read(buffer, 0, buffer.Length);

                                string extension = new FileInfo(txtFilePath.Text).Extension;
                                string update = "update UNGVIEN set DATACV=@data, MORONG = @extension where MAUV ='" + maUV + "'";

                                SqlCommand sqlCommand2 = new SqlCommand(update, conn);
                                sqlCommand2.Parameters.Add("@data", SqlDbType.VarBinary).Value = buffer;
                                sqlCommand2.Parameters.Add("@extension", SqlDbType.Char).Value = extension;

                                int ret2 = sqlCommand2.ExecuteNonQuery();
                                if (ret2 > 0)
                                {
                                    MessageBox.Show("Sửa thành công", "", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                else
                                {
                                    MessageBox.Show("Không update được file", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Sửa thành công", "", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            conn.Close();
                            render();

                        }
                        else
                        {
                            MessageBox.Show("Sửa không thành công", "", MessageBoxButton.OK, MessageBoxImage.Error);
                            conn.Close();
                        }
                    } 
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    
                }
            }
        }

        private void btnCV_Click(object sender, RoutedEventArgs e)
        {
            int index = lsvUV.SelectedIndex;
            
           
            if (index >= 0)
            {
                UngVien uvHienThoi = (UngVien)lsvUV.SelectedItem;
                string maUV = uvHienThoi.MaUV;
                string query = "select DataCV,MORONG from UNGVIEN where mauv='" + maUV+"'";

                conn.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandText = query;
                sqlCommand.Connection = conn;

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.Read())
                {
                    var data = sqlDataReader.GetSqlBinary(0).Value;
                    var extension = sqlDataReader.GetString(1);
                    string newFIleName = "CV" + maUV + "." + extension;
                    
                    File.WriteAllBytes(newFIleName,data);
                    System.Diagnostics.Process process = System.Diagnostics.Process.Start(newFIleName);
                    PDFfiles.Add(newFIleName);


                }
                conn.Close();
            }
        }


        private void lsvUV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lsvUV.SelectedIndex >= 0)
            {
                UngVien uvHienThoi = (UngVien)lsvUV.SelectedItem;
                string maUV = uvHienThoi.MaUV;
  
                conn.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandText = "SELECT MAUV,TENUV,GIOITINH,SDT,CMND,NGAYSINH,NOICHON,DATACV from UNGVIEN where MAUV ='" + maUV + "'";
                sqlCommand.Connection = conn;

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                if (sqlDataReader.Read())
                {
                    txtMaUV.Text = sqlDataReader.IsDBNull(0)?"": sqlDataReader.GetString(0);
                    txtTenUV.Text = sqlDataReader.IsDBNull(1) ? "" : sqlDataReader.GetString(1);
                    if (sqlDataReader.IsDBNull(2)==false && sqlDataReader.GetString(2) == "Nam")
                    {
                        radNam.IsChecked = true;
                    }
                    else
                    {
                        radNu.IsChecked = true;
                    }
                    txtSDT.Text = sqlDataReader.IsDBNull(3) ? "" : sqlDataReader.GetInt32(3)+"";
                    txtCMND.Text = sqlDataReader.IsDBNull(4) ? "" : sqlDataReader.GetString(4);
                    dpNgaySinh.Text = sqlDataReader.IsDBNull(5) ? "" : (sqlDataReader.GetSqlDateTime(5) + "");
                    txtNoiChon.Text = sqlDataReader.IsDBNull(6) ? "" : sqlDataReader.GetString(6);
                    txtFilePath.Text = "";
                }
                conn.Close();
            }
            
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            foreach (string tenFile in PDFfiles)
            {
                if(File.Exists(tenFile))
                    File.Delete(tenFile);
            }
        }
    }
}
