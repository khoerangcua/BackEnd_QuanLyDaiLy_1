using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interface_UI.DAO;
using Interface_UI.BUS.Validators;

namespace Interface_UI.BUS.Controllers
{
    public class ThemPhieuXuatHangController
    {

        #region Entities, MessageFailure and Validators
        public string MessageFailure { get; set; }
        private QuanLyDaiLyEntities db;
        private PhieuXuatHangValidator PhieuXuatHangValidator;
        private ChiTietPhieuXuatValidator ChiTietPhieuXuatValidator;
        #endregion

        #region Temp Objects
        private tb_PhieuXuatHang tempPhieuXuatHang;
        private List<tb_ChiTiet_XuatHang> tempListChiTietPhieuXuatHang;
        private tb_ChiTiet_XuatHang CurrentChiTietPhieuXuatHang { get; set; }
        #endregion

        #region Reference Controls
        public ComboBox DaiLyComboBox { get; set; }
        public ComboBox HangHoaComboBox { get; set; }
        public TextBox SoLuongTextBox { get; set; }
        public TextBox NgayLapTextBox { get; set; }
        public TextBox TienNoHienTai { get; set; }
        public TextBox GhiChuTextBox { get; set; }
        public DataGridView ChiTietPhieuXuatDataGridView { get; set; }
        public Button ThemButton { get; set; }
        public Button XoaButton { get; set; }
        public Button LuuButton { get; set; }
        public Button HuyButton { get; set; }
        #endregion

        #region Contructor
        public ThemPhieuXuatHangController()
        {
            this.MessageFailure = "";
            this.db = new QuanLyDaiLyEntities();
            this.PhieuXuatHangValidator = new PhieuXuatHangValidator();
            this.ChiTietPhieuXuatValidator = new ChiTietPhieuXuatValidator();
            this.tempPhieuXuatHang = new tb_PhieuXuatHang();
            this.tempListChiTietPhieuXuatHang = new List<tb_ChiTiet_XuatHang>();
            this.CurrentChiTietPhieuXuatHang = new tb_ChiTiet_XuatHang();
            this.ChiTietPhieuXuatDataGridView.RowEnter += ChiTietPhieuXuatDataGridView_RowEnter;
            //
            // trạng thái của các điều khiển
            //
            this.ThemButton.Enabled = true;
            this.XoaButton.Enabled = false;
            this.LuuButton.Enabled = false;
            this.HuyButton.Enabled = false;
            this.HangHoaComboBox.Enabled = true;
            this.DaiLyComboBox.Enabled = true;
            this.SoLuongTextBox.Enabled = true;
            this.NgayLapTextBox.Enabled = true;
            this.TienNoHienTai.Enabled = false;
            this.GhiChuTextBox.Enabled = true;
            this.ChiTietPhieuXuatDataGridView.DataSource = null;


        }

        #endregion

        #region Methods
        public void ChayLanDau()
        {
            //
            // Load Đại lý ComboBox
            //
            if (this.DaiLyComboBox != null)
            {
                if (db.tb_DaiLy.Any())
                {
                    var result1 = from dl in db.tb_DaiLy
                                  select new { ID = dl.Ma_DaiLy, Ten = dl.Ten_DaiLy };
                    this.DaiLyComboBox.DataSource = result1.ToList();
                    this.DaiLyComboBox.ValueMember = "ID";
                    this.DaiLyComboBox.DisplayMember = "Ten";
                }
            }
            //
            //Load Hàng hóa ComboBox
            //
            if (this.HangHoaComboBox != null)
            {
                if (db.tb_HangHoa.Any())
                {
                    var result2 = from hh in db.tb_HangHoa
                                  select new { ID = hh.Ma_HangHoa, Ten = hh.Ten_HangHoa };
                    this.HangHoaComboBox.DataSource = result2.ToList();
                    this.HangHoaComboBox.ValueMember = "ID";
                    this.HangHoaComboBox.DisplayMember = "Ten";
                }
            }
            //
            //Load Ngày lập phiếu
            //
            if (this.NgayLapTextBox != null)
            {
                this.NgayLapTextBox.Text = DateTime.Now.ToString();
            }
        }

        public bool ThemPhieuXuatHang()
        {
            //
            // reset messagefailure
            //
            this.MessageFailure = "";
            //
            //Lấy thông tin phiếu xuất hàng
            //
            int maphieuxuat;
            if (db.tb_PhieuXuatHang.Any())
            {
                var result1 = db.tb_PhieuXuatHang.Max(pxh => pxh.Ma_PhieuXuat);
                maphieuxuat = result1 + 1;
            }
            else
            {
                maphieuxuat = 1;
            }
            int madaily = int.Parse(this.DaiLyComboBox.SelectedValue.ToString());
            DateTime ngaylap = DateTime.Now;
            string ghichu = this.GhiChuTextBox.Text;
            //
            //Lấy thông tin chi tiết phiếu xuất hàng
            //
            int machitietphieuxuat;
            if (db.tb_ChiTiet_XuatHang.Any())
            {
                var result1 = db.tb_ChiTiet_XuatHang.Max(ctpxh => ctpxh.Ma_ChiTiet_XuatHang);
                machitietphieuxuat = result1 + 1;
            }
            else
            {
                machitietphieuxuat = 1;
            }
            int soluong = (this.SoLuongTextBox.Text).All(char.IsDigit) ? int.Parse(this.SoLuongTextBox.Text) : -1;
            int masanpham = int.Parse(this.HangHoaComboBox.SelectedValue.ToString());
            double dongia = (from hh in db.tb_HangHoa
                             where hh.Ma_HangHoa == masanpham
                             select hh.Don_Gia).Single();
            double thanhtien = dongia * soluong;
            //
            // Kiểm tra thông tin phiếu xuất hàng và chi tiết phiếu xuất hàng
            //
            bool checkinput1 = this.PhieuXuatHangValidator.KiemTraThongTinPhieuXuatHang(madaily);
            bool checkinput2 = this.ChiTietPhieuXuatValidator.KiemTraChiTietPhieuXuat(maphieuxuat, masanpham, soluong, dongia, thanhtien);
            if (checkinput1 == false)
            {
                this.MessageFailure = PhieuXuatHangValidator.MessageFailure;
                return false;
            }
            if (checkinput2 == false)
            {
                this.MessageFailure = ChiTietPhieuXuatValidator.MessageFailure;
                return false;
            }
            //
            //Kiểm tra quy định nợ
            //
            double notoida = (from dl in db.tb_DaiLy
                              join ldl in db.tb_LoaiDaiLy on dl.Ma_Loai_DaiLy equals ldl.Ma_Loai_DaiLy
                              where dl.Ma_DaiLy == madaily
                              select ldl.TienNo_ToiDa).Single();

            double tienno = 0;
            if (db.tb_PhieuXuatHang.Any())
            {
                var result3 = from px in db.tb_PhieuXuatHang
                              where px.Ma_DaiLy == madaily
                              group px by px.Ma_DaiLy;
                foreach (var item in result3)
                {
                    if (item.Key == madaily)
                    {
                        tienno = item.Sum(p => p.TongTien);
                    }
                }
            }

            double notamthoi = tempPhieuXuatHang.TongTien;

            if (tienno + notamthoi <= notoida)
            {
                if (this.tempPhieuXuatHang.Ma_DaiLy == 0)
                {
                    //
                    //Thêm lần đầu
                    //
                    this.tempPhieuXuatHang.Ma_PhieuXuat = maphieuxuat;
                    this.tempPhieuXuatHang.Ma_DaiLy = madaily;
                    this.tempPhieuXuatHang.Ngay_Lap = ngaylap;
                    this.tempPhieuXuatHang.TongTien = thanhtien;
                    this.tempPhieuXuatHang.GhiChu = ghichu;

                    this.tempListChiTietPhieuXuatHang.Add(new tb_ChiTiet_XuatHang()
                    {
                        Ma_ChiTiet_XuatHang = machitietphieuxuat,
                        Ma_PhieuXuat = maphieuxuat,
                        Ma_HangHoa = masanpham,
                        So_Luong = soluong,
                        Don_Gia = dongia,
                        Thanh_Tien = thanhtien,

                    });                                                 
                }
                else
                {
                    //
                    //Đã tồn tại tempphieuxuathang và cập nhật lại tông tiền
                    //
                    this.tempListChiTietPhieuXuatHang.Add(new tb_ChiTiet_XuatHang()
                    {
                        Ma_ChiTiet_XuatHang = machitietphieuxuat,
                        Ma_PhieuXuat = maphieuxuat,
                        Ma_HangHoa = masanpham,
                        So_Luong = soluong,
                        Don_Gia = dongia,
                        Thanh_Tien = thanhtien,

                    });

                    this.tempPhieuXuatHang.TongTien = this.tempListChiTietPhieuXuatHang.Sum(p => p.Thanh_Tien);
                    
                }
                //
                //update form
                //
                double tienno1 = 0;
                if (db.tb_PhieuXuatHang.Any())
                {
                    var result3 = from px in db.tb_PhieuXuatHang
                                  where px.Ma_DaiLy == madaily
                                  group px by px.Ma_DaiLy;
                    foreach (var item in result3)
                    {
                        if (item.Key == madaily)
                        {
                            tienno1 = item.Sum(p => p.TongTien);
                        }
                    }
                }
                double notamthoi1 = tempPhieuXuatHang.TongTien;
                this.DaiLyComboBox.Enabled = false;
                this.TienNoHienTai.Text = (tienno1 + notamthoi1).ToString();
                this.NgayLapTextBox.Text = this.tempPhieuXuatHang.Ngay_Lap.ToString();
                this.GhiChuTextBox.Enabled = false;
                this.XoaButton.Enabled = true;
                this.LuuButton.Enabled = true;
                this.HuyButton.Enabled = true;
                this.ChiTietPhieuXuatDataGridView.DataSource = null;
                this.ChiTietPhieuXuatDataGridView.DataSource = tempListChiTietPhieuXuatHang;
                return true;
            }
            else
            {
                this.MessageFailure = "Tiền nợ vượt quá tiền nợ tối đa";
                return false;
            }
        }

        public bool XoaPhieuXuatHang()
        {

            //reset messagefailure
            this.MessageFailure = "";
            // xóa chi tiết phiếu xuất đã chọn
            
            bool result = this.tempListChiTietPhieuXuatHang.Remove(CurrentChiTietPhieuXuatHang);
            if (result == false)
            {
                this.MessageFailure = "Xoa không thành công";
                return false;
            }
            //
            //reset form
            //
            if (this.tempListChiTietPhieuXuatHang.Count()==0)
            {
                this.XoaButton.Enabled = false;
                this.LuuButton.Enabled = false;
            }
            this.ChiTietPhieuXuatDataGridView.DataSource = null;
            this.ChiTietPhieuXuatDataGridView.DataSource = tempListChiTietPhieuXuatHang;

            return true;
        }

        public bool LuuPhieuXuatHang()
        {
            db.tb_PhieuXuatHang.Add(this.tempPhieuXuatHang);
            foreach (var item in this.tempListChiTietPhieuXuatHang)
            {
                db.tb_ChiTiet_XuatHang.Add(item);
            }

            if (db.SaveChanges()==0)
            {
                this.MessageFailure = "Lưu không thành công";
                return false;
            }
            else
            {
                this.ChiTietPhieuXuatDataGridView = null;
                this.tempPhieuXuatHang = new tb_PhieuXuatHang();
                this.tempListChiTietPhieuXuatHang.Clear();
                this.DaiLyComboBox.Enabled = true;
                this.GhiChuTextBox.Text = "";
                this.GhiChuTextBox.Enabled = true;
                this.SoLuongTextBox.Text = "";
                this.NgayLapTextBox.Text = DateTime.Now.ToString();
                this.TienNoHienTai.Text = "";
                this.XoaButton.Enabled = false;
                this.LuuButton.Enabled = false;
                this.HuyButton.Enabled = false;
                return true;
            }
        }

        public bool HuyPhieuXuatHang()
        {
            this.tempPhieuXuatHang = new tb_PhieuXuatHang();
            this.tempListChiTietPhieuXuatHang.Clear();
            //
            //reset form
            //
            this.ChiTietPhieuXuatDataGridView = null;
            this.tempPhieuXuatHang = new tb_PhieuXuatHang();
            this.tempListChiTietPhieuXuatHang.Clear();
            this.DaiLyComboBox.Enabled = true;
            this.GhiChuTextBox.Text = "";
            this.GhiChuTextBox.Enabled = true;
            this.SoLuongTextBox.Text = "";
            this.NgayLapTextBox.Text = DateTime.Now.ToString();
            this.TienNoHienTai.Text = "";
            this.XoaButton.Enabled = false;
            this.LuuButton.Enabled = false;
            this.HuyButton.Enabled = false;
            return true;
        }
        #endregion

        #region Events
        private void ChiTietPhieuXuatDataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            this.CurrentChiTietPhieuXuatHang.Ma_ChiTiet_XuatHang = int.Parse(this.ChiTietPhieuXuatDataGridView.Rows[e.RowIndex].Cells[0].ToString());
            this.CurrentChiTietPhieuXuatHang.Ma_PhieuXuat = int.Parse(this.ChiTietPhieuXuatDataGridView.Rows[e.RowIndex].Cells[1].ToString());
            this.CurrentChiTietPhieuXuatHang.Ma_HangHoa = int.Parse(this.ChiTietPhieuXuatDataGridView.Rows[e.RowIndex].Cells[2].ToString());
            this.CurrentChiTietPhieuXuatHang.So_Luong = int.Parse(this.ChiTietPhieuXuatDataGridView.Rows[e.RowIndex].Cells[0].ToString());
            this.CurrentChiTietPhieuXuatHang.Don_Gia = double.Parse(this.ChiTietPhieuXuatDataGridView.Rows[e.RowIndex].Cells[0].ToString());
            this.CurrentChiTietPhieuXuatHang.Thanh_Tien = double.Parse(this.ChiTietPhieuXuatDataGridView.Rows[e.RowIndex].Cells[0].ToString());
        }
        #endregion
    }
}
