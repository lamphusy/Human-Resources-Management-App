using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace HRM_App.PhongBanControl
{
    public class CNhanVien
    {
        public string MaNV  {get; set;}
        public string TenNV { get; set; }
        public string ChucVu { get; set; }
        public int SDT { get; set; }
        public string Email { get; set; }
        public ImageSource Avatar { get; set; }
    }
}
