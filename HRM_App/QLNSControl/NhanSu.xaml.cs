using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Win32;
using System.IO;
using OfficeOpenXml;

namespace HRM_App.QLNSControl
{
    /// <summary>
    /// Interaction logic for NhanSu.xaml
    /// </summary>
    public partial class NhanSu : UserControl
    {
        private SqlConnection con;
        DataTable dta;
        private string imagelocation = "";
        public NhanSu()
        {
            InitializeComponent();
            con = new SqlConnection("Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\QLNS.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True");
            LoadGrid();

            con.Open();
            
            SqlCommand cmd = new SqlCommand(" select TENPHONG from PHONGBAN", con);
            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            while (sqlDataReader.Read())
            {
                cboPhong.Items.Add(sqlDataReader.GetString(0));
            }
            sqlDataReader.Close();
            con.Close();
            this.DataContext = this;
        }


        public void LoadGrid()
        {

            con.Open();
            SqlCommand cmd = new SqlCommand("select MANV,TENNV,NGAYSINH,GIOITINH,QUOCTICH,TONGIAO,NOISINH,DIACHI,CMND,BOPHAN,CHUCVU,PHG,NGVAOLAM,SDT,EMAIL from NHANVIEN", con);
            dta = new DataTable();
            SqlDataReader sdr = cmd.ExecuteReader();
            dta.Load(sdr);


            con.Close();
            datagird.SelectedItem = -1;
           
            datagird.ItemsSource = dta.DefaultView;
            
           

        }
        public bool isValid()
        {
            if (MANVTextBox.Text == string.Empty)
            {
                MessageBox.Show("Yêu cầu điền mã ", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;

            }
            if (TENTextBox.Text == string.Empty)
            {
                MessageBox.Show("Yêu cầu điền tên", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;

            }
            if (GTTextbox.Text == string.Empty)
            {
                MessageBox.Show("Yêu cầu điền giới tính ", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;

            }
            if (CMNDTextbox.Text == string.Empty)
            {
                MessageBox.Show("Yêu cầu điền CMND ", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;

            }
            if (SDTTextbox.Text == string.Empty)
            {
                MessageBox.Show("Yêu cầu điền số điện thoại ", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;

            }
            if(cboPhong.Text == string.Empty)
            {
                MessageBox.Show("Yêu cầu điền phòng ban ", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;

        }

        private void btnThemNV_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            try
            {
                if (isValid())
                {
                    //byte[] b = ImageToByteArray(anh );

                    SqlCommand cmd = new SqlCommand();

                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO NHANVIEN(MANV , TENNV, GIOITINH,NGAYSINH,NOISINH,CMND,SDT,CHUCVU,NGVAOLAM,EMAIL,DIACHI,QUOCTICH,BOPHAN,TONGIAO)" + "VALUES(N'" + MANVTextBox.Text + "',N'" +
                        TENTextBox.Text + "',N'" + GTTextbox.Text + "',N'" + NSTextbox.Text + "',N'" + NOISTextbox.Text + "',N'" + CMNDTextbox.Text + "',N'" + SDTTextbox.Text + "',N'" +
                        txtChucVu.Text + "','" + dpNgayVaoLam.Text + "',N'" + txtEmail.Text + "',N'" + txtDiaChi.Text + "',N'" + txtQuocTich.Text + "',N'" + txtBoPhan.Text + "',N'" + txtTonGiao.Text + "')";
                    cmd.Connection = con;
                    //cmd.Parameters.AddWithValue("@MANV", MANVTextBox.Text);
                    // //cmd.Parameters.AddWithValue("@ANH", b);
                    // cmd.Parameters.AddWithValue("@TENNV", TENTextBox.Text);  
                    // cmd.Parameters.AddWithValue("@GIOITINH", GTTextbox.SelectedValue.ToString());
                    // cmd.Parameters.AddWithValue("@NGAYSINH", NSTextbox.SelectedDate.ToString());
                    // cmd.Parameters.AddWithValue("@NOISINH", NOISTextbox.Text);
                    // cmd.Parameters.AddWithValue("@CMND", CMNDTextbox.Text);
                    // cmd.Parameters.AddWithValue("@SDT", SDTTextbox.Text);

                    int ret = cmd.ExecuteNonQuery();
                    if (ret > 0)
                    {
                        cmd.CommandText = "update NHANVIEN set PHG = (select MAPHONG from PHONGBAN where TENPHONG=N'" + (cboPhong.SelectedIndex >= 0 ? cboPhong.SelectedItem.ToString() : "") + "') where MANV ='" + MANVTextBox.Text + "'";
                        cmd.ExecuteNonQuery();
                        if (imagelocation != "")
                        {
                            using (Stream stream = File.OpenRead(imagelocation))
                            {
                                byte[] buffer = new byte[stream.Length];
                                stream.Read(buffer, 0, buffer.Length);

                                cmd.CommandText = "update NHANVIEN set ANH = @data where MANV ='" + MANVTextBox.Text + "'";
                                cmd.Parameters.Add("@data", SqlDbType.VarBinary).Value = buffer;
                                cmd.ExecuteNonQuery();
                            }

                        }
                    }
                    con.Close();
                    LoadGrid();
                    MessageBox.Show("Thành công ");
                }
                else
                {
                    con.Close();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Thất bại! " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                con.Close();

            }
         

        }
        //byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(anh));
        //public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        //{
        //    using (var ms = new MemoryStream())
        //    {
        //        imageIn.Save(ms, imageIn.RawFormat);
        //        return ms.ToArray();
        //    }
        //}


        private void btnXoaNV_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Bạn có chắc muốn xóa nhân viên không?","Xác Nhận",MessageBoxButton.YesNo,MessageBoxImage.Question)== MessageBoxResult.Yes)
            {
                con.Open();
                try
                {
                    
                    SqlCommand cmd = new SqlCommand("delete from NHANVIEN where MANV =  '" + MANVTextBox.Text + "' ", con);
                    SqlCommand cmd1 = new SqlCommand("delete from THAMGIADAOTAO where MANV =  '" + MANVTextBox.Text + "' ", con);
                    SqlCommand cmd2 = new SqlCommand("delete from CHAMCONG where MANV =  '" + MANVTextBox.Text + "' ", con);
                    SqlCommand cmd3 = new SqlCommand("delete from LICHCONG where MANV =  '" + MANVTextBox.Text + "' ", con);
                    SqlCommand cmd4 = new SqlCommand("delete from LICHSUCHAMCONG where MANV =  '" + MANVTextBox.Text + "' ", con);
                    cmd1.ExecuteNonQuery();
                    cmd2.ExecuteNonQuery();
                    cmd3.ExecuteNonQuery();
                    cmd4.ExecuteNonQuery();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Xóa thành công");
                    con.Close();
                    
                    LoadGrid();

                    MANVTextBox.Text = "";
                    TENTextBox.Text = "";
                    CMNDTextbox.Text = "";
                    GTTextbox.Text = "";
                    SDTTextbox.Text = "";
                    NOISTextbox.Text = "";
                    NSTextbox.Text = "";
                    txtBoPhan.Text = "";
                    txtChucVu.Text = "";
                    txtDiaChi.Text = "";
                    txtEmail.Text = "";
                    txtQuocTich.Text = "";
                    txtTonGiao.Text = "";
                    dpNgayVaoLam.Text = "";
                    cboPhong.Text = "";
                    datagird.SelectedIndex = -1;
                    imagelocation = "";
                    imgAvatar.Source = null;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Lỗi không thể xóa! " + ex.Message,"Lỗi",MessageBoxButton.OK,MessageBoxImage.Error);
                    con.Close();

                }

            }
           
        }

        private void btnSuaNV_Click(object sender, RoutedEventArgs e)
        {
           
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "update  NHANVIEN  set  TENNV = N'" + TENTextBox.Text + "',GIOITINH= N'" + GTTextbox.Text + "' ,NGAYSINH= N'" + 
                NSTextbox.Text + "', NOISINH= N'" + NOISTextbox.Text + "',CMND= N'" + CMNDTextbox.Text + "',SDT= '" + SDTTextbox.Text +
                "',CHUCVU= N'"+txtChucVu.Text+"', PHG= (select MAPHONG from PHONGBAN where TENPHONG=N'"+ (cboPhong.SelectedIndex >= 0 ? cboPhong.SelectedItem.ToString() : "") + "'),NGVAOLAM='"+dpNgayVaoLam.Text+
                "', EMAIL=N'"+txtEmail.Text+"',DIACHI=N'"+txtDiaChi.Text+"',QUOCTICH=N'"+txtQuocTich.Text+"',TONGIAO=N'"+txtTonGiao.Text+"',BOPHAN=N'"+txtBoPhan.Text+
                "' WHERE  MANV ='" +MANVTextBox.Text + "'";
            cmd.Connection = con;
            con.Open();
            try
            {
                
                int ret1= cmd.ExecuteNonQuery();
                if (imagelocation != "")
                {
                    using (Stream stream = File.OpenRead(imagelocation))
                    {
                        byte[] buffer = new byte[stream.Length];
                        stream.Read(buffer, 0, buffer.Length);

                        cmd.CommandText = "update NHANVIEN set ANH = @data where MANV ='" + MANVTextBox.Text + "'";
                        cmd.Parameters.Add("@data", SqlDbType.VarBinary).Value = buffer;
                        cmd.ExecuteNonQuery();
                    }

                }
                if(ret1> 0)
                {
                    MessageBox.Show("Cập nhật thành công!");

                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại!");
                }
                con.Close();
                LoadGrid();

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
           
        }

        private void btnUpNV_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                bool? kq = dialog.ShowDialog();
                if (kq ?? true)
                {
                    imagelocation = dialog.FileName;
                    imgAvatar.Source = new BitmapImage(new Uri(imagelocation));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void mydatagrid(object sender, SelectionChangedEventArgs e)
        {
            
            if (datagird.SelectedIndex >= 0 && datagird.SelectedIndex < datagird.Items.Count)
            {
                DataRowView drv = (DataRowView)datagird.SelectedItem;
                if (drv != null)
                {
                    MANVTextBox.Text = drv["MANV"].ToString();
                    TENTextBox.Text = drv["TENNV"].ToString();
                    GTTextbox.Text = drv["GIOITINH"].ToString();
                    NSTextbox.Text = drv["NGAYSINH"].ToString();
                    NOISTextbox.Text = drv["NOISINH"].ToString();
                    CMNDTextbox.Text = drv["CMND"].ToString();
                    SDTTextbox.Text = drv["SDT"].ToString();
                    txtChucVu.Text = drv["CHUCVU"].ToString();
                    txtDiaChi.Text = drv["DIACHI"].ToString();
                    txtEmail.Text = drv["EMAIL"].ToString();
                    txtQuocTich.Text = drv["QUOCTICH"].ToString();
                    txtTonGiao.Text = drv["TONGIAO"].ToString();
                    txtBoPhan.Text = drv["BOPHAN"].ToString();
                    dpNgayVaoLam.Text = drv["NGVAOLAM"].ToString();
                    string phong= drv["PHG"].ToString();

                    SqlCommand sqlCommand = new SqlCommand("select tenphong from Phongban where maphong='" + phong + "'", con);
                    con.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    if (sqlDataReader.Read())
                        cboPhong.Text = sqlDataReader.GetString(0);
                    else cboPhong.Text = "";
                    sqlDataReader.Close();

                    sqlCommand = new SqlCommand("select ANH,GIOITINH from NHANVIEN where MANV='" + drv["MANV"].ToString() + "'", con);
                    sqlDataReader = sqlCommand.ExecuteReader();
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
                        if (sqlDataReader.GetString(1) == "Nam")
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
                    sqlDataReader.Close();
                    con.Close();
                    imagelocation = "";
                }
            }
        }



        private void excel_click(object sender, RoutedEventArgs e)
        {
            string filepath = "";
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Excel | *.xlsx |Excel 2003 | *.xls";
            if(dialog.ShowDialog()==true)
                filepath = dialog.FileName;


            if (string.IsNullOrEmpty(filepath))
            {
                return;
            }
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            try
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    p.Workbook.Properties.Author = "Quản lý nhân sự";
                    p.Workbook.Properties.Title = " Danh sách nhân viên";
                    p.Workbook.Worksheets.Add("Test sheet");
                    ExcelWorksheet ws = p.Workbook.Worksheets[0];
                    ws.Name = "Danh sach nhan vien";
                    ws.Cells.Style.Font.Size = 13;
                    ws.Cells.Style.Font.Name = "Times New Roman";
                    
                    string[] arrColumnHeader = { "Mã nhân viên", "Tên nhân viên", "Giới tính", "Ngày sinh", " Nơi sinh", "CMND", "Số điện thoại","Email","Quốc tịch", "Chức vụ","Bộ phận","Phòng","Ngày vào làm" };
                    ws.Columns.AutoFit();
                    ws.Columns.Width = 20;
                    ws.Cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    var countHeader = arrColumnHeader.Length;
                    ws.Cells[1, 1].Value = "Danh sách nhân viên";
                    ws.Cells[1, 1, 1, countHeader].Merge = true;
                    ws.Cells[1, 1, 1, countHeader].Style.Font.Bold = true;
                    ws.Cells[1, 1, 1, countHeader].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    int colIndex = 1;
                    int rowIndex = 2;
                    foreach (var item in arrColumnHeader)
                    {
                        var cell = ws.Cells[rowIndex, colIndex];
                        var fill = cell.Style.Fill;
                        fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                        var boder = cell.Style.Border;
                        boder.Bottom.Style = boder.Top.Style = boder.Left.Style = boder.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Value = item;
                        colIndex++;
                    }
                    DataTable dt = new DataTable();
                    dt = dta;
                    foreach (DataRow dr in dt.Rows)
                    {
                        colIndex = 1;
                        rowIndex++;
                        ws.Cells[rowIndex, colIndex++].Value = dr["MANV"].ToString();
                        ws.Cells[rowIndex, colIndex++].Value = dr["TENNV"].ToString();
                        ws.Cells[rowIndex, colIndex++].Value = dr["GIOITINH"].ToString();
                        ws.Cells[rowIndex, colIndex++].Value = Convert.ToDateTime(dr["NGAYSINH"].ToString()).ToShortDateString();
                        ws.Cells[rowIndex, colIndex++].Value = dr["NOISINH"].ToString();
                        ws.Cells[rowIndex, colIndex++].Value = dr["CMND"].ToString();
                        ws.Cells[rowIndex, colIndex++].Value = dr["SDT"].ToString();
                        ws.Cells[rowIndex, colIndex++].Value = dr["EMAIL"].ToString();
                        ws.Cells[rowIndex, colIndex++].Value = dr["QUOCTICH"].ToString();
                        ws.Cells[rowIndex, colIndex++].Value = dr["CHUCVU"].ToString();
                        ws.Cells[rowIndex, colIndex++].Value = dr["BOPHAN"].ToString();
                        ws.Cells[rowIndex, colIndex++].Value = dr["PHG"].ToString();
                        ws.Cells[rowIndex, colIndex++].Value = Convert.ToDateTime(dr["NGVAOLAM"].ToString()).ToShortDateString();

                    }
                    Byte[] bin = p.GetAsByteArray();
                    File.WriteAllBytes(filepath, bin);
                }
                MessageBox.Show("Xuất file thành công !");

            }
            catch(Exception ex)
            {
                MessageBox.Show("Có lỗi khi lưu file! "+ex.Message );
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            MANVTextBox.Text = "";
            TENTextBox.Text = "";
            CMNDTextbox.Text = "";
            GTTextbox.Text = "";
            SDTTextbox.Text = "";
            NOISTextbox.Text = "";
            NSTextbox.Text = "";
            txtBoPhan.Text = "";
            txtChucVu.Text = "";
            txtDiaChi.Text = "";
            txtEmail.Text = "";
            txtQuocTich.Text = "";
            txtTonGiao.Text = "";
            dpNgayVaoLam.Text = "";
            cboPhong.Text = "";
            datagird.SelectedIndex = -1;
            imagelocation = "";
            imgAvatar.Source = null;
        }
    }
}
