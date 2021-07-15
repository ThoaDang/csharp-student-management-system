using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiTap.Models
{
    class SinhVien
    {
        public string HoVaTen { set; get; }
        public string ChuyenNganh { set; get; }
        public double Diem { set; get; }
        public string ChucVu { set; get; }
        public string XepLoaiHocLuc { set; get; }
        public string GioiTinh { get; set; }
        //   public string Gioitinh { set; get; }
        public SinhVien()
        {
            HoVaTen = "Khong co ten";
            ChuyenNganh = "Khong co chuyen nganh";
            Diem = 0;
            ChucVu = "";
            XepLoaiHocLuc = "";
        }
        public SinhVien(string _HoVaTen, string _ChuyenNganh, double _Diem, string _ChucVu, string _XepLoaiHocLuc, string _GioiTinh)
        {
            HoVaTen = _HoVaTen;
            ChuyenNganh = _ChuyenNganh;
            Diem = _Diem;
            ChucVu = _ChucVu;
            XepLoaiHocLuc = _XepLoaiHocLuc;
            GioiTinh = _GioiTinh;
        }
    }
}
