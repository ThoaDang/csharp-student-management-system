using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BaiTap.Models;

namespace BaiTap
{
    public partial class Form1 : Form
    {
        private List<SinhVien> DanhSachSinhVien;
        private int SinhVienDuocChon = -1; // -1 khong co sinh vien nao duoc chon
        private ErrorProvider DiemErrorProvider;
        private string pathToFile = @"C:\Users\THO\Desktop\BaiTapEmThoaHoanThienLan1\SinhVien.txt";
        public Form1()
        {
            InitializeComponent();

            DiemErrorProvider = new ErrorProvider();
            DiemErrorProvider.BlinkStyle = ErrorBlinkStyle.AlwaysBlink;
            DiemErrorProvider.SetIconAlignment(this.txtdiem, ErrorIconAlignment.MiddleRight);
            DiemErrorProvider.SetIconPadding(this.txtdiem, 2);

            DanhSachSinhVien = new List<SinhVien>();
            this.DocFile();
        }
        public bool CheckControl()
        {
            if (string.IsNullOrWhiteSpace(txtten.Text))
            {
                MessageBox.Show("Bạn chưa nhập họ và tên sinh viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtten.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtchuyennganh.Text))
            {
                MessageBox.Show("Bạn chưa nhập chuyên ngành", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtchuyennganh.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtdiem.Text))
            {
                MessageBox.Show("Bạn chưa nhập điểm sinh viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtdiem.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(cboChucVu.Text))
            {
                MessageBox.Show("Bạn chưa nhập chức vụ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cboChucVu.Focus();
                return false;
            }

            return true;
        }
        public string TinhDiemXepLoai(double d)
        {
            if (d >= 9 && d <= 10)
            {
                return "Xuất sắc";
            }
            else if (d >= 8)
            {
                return "Giỏi";
            }
            else if (d >= 7)
            {
                return "Khá";
            }
            else if (d >= 5)
            {
                return "Trung bình";
            }
            else if (d > 0 && d < 5)
            {
                return "Yếu";
            }
            else
            {
                return "Điểm không hợp lệ";
            }

        }

        private void btthem_Click(object sender, EventArgs e)
        {
            if (CheckControl())
            {
                string HoTen = txtten.Text;
                string ChuyenNganh = txtchuyennganh.Text;
                string GioiTinh = cobgioitinh.Text;
                string ChucVu = cboChucVu.Text;

                double Diem = double.Parse(txtdiem.Text);
                string XepLoai = TinhDiemXepLoai(Diem);
                double DiemChuan = TinhDiem(ChucVu, Diem);
                SinhVien sv = new SinhVien(HoTen, ChuyenNganh, DiemChuan, ChucVu, XepLoai, GioiTinh);
                if (SinhVienDuocChon == -1)
                {
                    // Nếu không có sinh viên nào được chọn thì Thêm mới Sinh viên vào Danh sách
                    DanhSachSinhVien.Add(sv);
                    MessageBox.Show("Thêm Sinh viên '" + sv.HoVaTen + "' thành công!");
                    ClearForm(); // Làm mới lại dữ liệu hiển thị trong Form
                } else
                {
                    // Ngược lại thì cập nhật lại dữ liệu của Sinh Viên vào Danh sách
                    DanhSachSinhVien[SinhVienDuocChon] = sv;
                    MessageBox.Show("Cập nhật Sinh viên '" + sv.HoVaTen + "' thành công!");
                    ClearForm();
                    SinhVienDuocChon = -1; // Set lại về giá trị ban đầu
                    btthem.Text = "Thêm mới"; // Đổi lại nút Update thành Thêm mới
                }
            }
        }

        public void ClearForm()
        {
            txtdiem.Text = string.Empty;
            txtchuyennganh.Text = string.Empty;
            txtten.Text = string.Empty;
            txtxeploai.Text = string.Empty;
            cboChucVu.Text = string.Empty;
            cobgioitinh.Text = string.Empty;
        }

        public double TinhDiem(string ChucVu, double Diem)
        {
            if (ChucVu.Equals("Lớp trưởng"))
            {
                Diem += 0.5;
            }
            else if (ChucVu.Equals("Lớp phó"))
            {
                Diem += 0.3;
            }
            else if (ChucVu.Equals("Bí thư"))
            {
                Diem += 0.4;
            }
            else if (ChucVu.Equals("Phó bí thư"))
            {
                Diem += 0.3;
            }
            return ((Diem > 10) ? 10 : Diem); // Điểm > 10 thì trả về 10; Không thì trả về Điểm
        }
        private void btxoa_Click(object sender, EventArgs e)
        {
            try
            {
                SinhVien sv = DanhSachSinhVien[SinhVienDuocChon];
                DialogResult result = MessageBox.Show("Bạn có thực sự muốn xóa sinh viên '" + sv.HoVaTen + "'?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    DanhSachSinhVien.RemoveAt(SinhVienDuocChon);
                    MessageBox.Show("Xóa sinh viên '" + sv.HoVaTen + "' thành công!");
                    ClearForm();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi. " + ex.Message);
            }
        }


        private void btquaylai_Click(object sender, EventArgs e)
        {
            if (DanhSachSinhVien.Count > 0)
            {
                btxoa.Enabled = true;
                if (SinhVienDuocChon <= 0)
                {
                    SinhVienDuocChon = DanhSachSinhVien.Count - 1;
                }
                else
                {
                    SinhVienDuocChon--;
                }
                SinhVien sv = DanhSachSinhVien[SinhVienDuocChon];
                txtten.Text = sv.HoVaTen;
                txtchuyennganh.Text = sv.ChuyenNganh;
                txtdiem.Text = sv.Diem.ToString();
                txtxeploai.Text = sv.XepLoaiHocLuc;
                cboChucVu.Text = sv.ChucVu;
                cobgioitinh.Text = sv.GioiTinh;

                btthem.Text = "Cập nhật"; // Nếu có sinh viên được chọn thì đổi chữ Thêm mới > Cập nhật
            }
            else
            {
                MessageBox.Show("Chưa có Sinh viên nào trong Danh sách");
            }
        }

        private void cboChucVu_TextChanged(object sender, EventArgs e)
        {
            if (IsDiemValid() && IsChucVuValid())
            {
                double Diem = TinhDiem(cboChucVu.Text, double.Parse(txtdiem.Text.ToString()));
                txtxeploai.Text = TinhDiemXepLoai(Diem);
            } else
            {
                txtxeploai.Text = string.Empty;
            }
        }

        private void txtdiem_TextChanged(object sender, EventArgs e)
        {
            if (IsDiemValid() && IsChucVuValid())
            {
                double Diem = TinhDiem(cboChucVu.Text, double.Parse(txtdiem.Text.ToString()));
                txtxeploai.Text = TinhDiemXepLoai(Diem);
            } else
            {
                txtxeploai.Text = string.Empty;
            }
        }

        public bool IsDiemValid()
        {
            double Diem = 0;
            try
            {
                Diem = double.Parse(txtdiem.Text.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return (Diem > 0 && Diem <= 10);
        }

        public bool IsChucVuValid()
        {
            return (cboChucVu.SelectedIndex > -1);
        }

        private void txtdiem_Validated(object sender, EventArgs e)
        {
            if (IsDiemValid())
            {
                DiemErrorProvider.SetError(this.txtdiem, string.Empty);
            } else
            {
                DiemErrorProvider.SetError(this.txtdiem, "Điểm phải lớn hơn 0 và nhỏ hơn 10");
            }
        }

        public void DocFile()
        {
            try
            {
                // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(pathToFile))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] SinhVienInLine = line.Split(',');

                        SinhVien sv = new SinhVien();
                        sv.HoVaTen = SinhVienInLine[0];
                        sv.ChuyenNganh = SinhVienInLine[1];
                        sv.Diem = Convert.ToInt32(SinhVienInLine[2]);
                        sv.GioiTinh = SinhVienInLine[3];
                        sv.ChucVu = SinhVienInLine[4];
                        sv.XepLoaiHocLuc = SinhVienInLine[5];
                        DanhSachSinhVien.Add(sv);
                    }
                    if (DanhSachSinhVien.Count > 0)
                    {
                        MessageBox.Show("Lấy dữ liệu từ file thành công!");
                    }
                }
            }
            catch (IOException e)
            {
                MessageBox.Show("Không đọc được dữ liệu từ File vì: " + e.Message);
            }
        }

        public void GhiFile()
        {
            try
            {
                // Create a file to write to.
                if (!System.IO.File.Exists(pathToFile))
                {
                    System.IO.File.Create(pathToFile).Dispose();
                }
                using (StreamWriter sw = new StreamWriter(pathToFile))
                {
                    foreach (var item in DanhSachSinhVien)
                    {
                        sw.WriteLine(item.HoVaTen + "," + item.ChuyenNganh + "," + item.Diem + "," + item.GioiTinh + "," + item.ChucVu + "," + item.XepLoaiHocLuc);
                    }
                    MessageBox.Show("Ghi xuống file thành công!");
                }
            }
            catch (IOException e)
            {
                MessageBox.Show("Không ghi được xuống file vì: " + e.Message);
            }
        }

        private void btnSaveToFile_Click(object sender, EventArgs e)
        {
            this.GhiFile();
        }
    }

}
