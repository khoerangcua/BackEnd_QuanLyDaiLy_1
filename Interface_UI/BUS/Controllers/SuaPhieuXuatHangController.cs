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
    class SuaPhieuXuatHangController
    {
        public string MessageFailure { get; set; }
        private QuanLyDaiLyEntities db;
        private ChiTietPhieuXuatValidator chiTietPhieuXuatValidator;

        private int currentIDPhieuXuatHang { get; set; }
        private int currentIDChiTietPhieuXuatHang { get; set; }
        private List<tb_ChiTiet_XuatHang> tempChiTiet_XuatHangs { get; set; }

        public ComboBox DaiLyComboBox { get; set; }
        public ComboBox ThangComboBox { get; set; }
        public ComboBox NamComboBox { get; set; }
        public ComboBox HangHoaComboBox { get; set; }
        public TextBox SoLuongTextBox { get; set; }
        public TextBox NoHienTaiTextBox { get; set; }
        public TextBox GhiChuTexBox { get; set; }
        public DataGridView PhieuXuatHangData { get; set; }
        public DataGridView ChiTietPhieuXuatHangData { get; set; }
        public Button TimButton { get; set; }
        public Button XoaButton { get; set; }
        public Button ThemButton { get; set; }
        public Button LuuButton { get; set; }
        public Button HuyButton { get; set; }

        public SuaPhieuXuatHangController()
        {
            this.MessageFailure = "";
            this.db = new QuanLyDaiLyEntities();
            this.chiTietPhieuXuatValidator = new ChiTietPhieuXuatValidator();
            this.PhieuXuatHangData.RowEnter += PhieuXuatHangData_RowEnter;
            this.ChiTietPhieuXuatHangData.RowEnter += ChiTietPhieuXuatHangData_RowEnter;
            this.tempChiTiet_XuatHangs = new List<tb_ChiTiet_XuatHang>();
            //
            // Thiet lap trang thai cua cac control
            //
            HangHoaComboBox.Enabled = false;
            SoLuongTextBox.Enabled = false;
            NoHienTaiTextBox.Enabled = false;
            GhiChuTexBox.Enabled = false;
            PhieuXuatHangData.Enabled = false;
            ChiTietPhieuXuatHangData.Enabled = false;
            XoaButton.Enabled = false;
            ThemButton.Enabled = false;
            LuuButton.Enabled = false;
            HuyButton.Enabled = false;

        }

        private void ChiTietPhieuXuatHangData_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void PhieuXuatHangData_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            this.currentIDPhieuXuatHang = int.Parse(this.PhieuXuatHangData.Rows[e.RowIndex].Cells[0].Value.ToString());
            LoadChiTietPhieuXuatHang();
            this.ThemButton.Enabled = true;
            this.SoLuongTextBox.Enabled = true;
            this.HangHoaComboBox.Enabled = true;
        }

        public void LoadLanDau()
        {
            //
            //Load dai ly ComboBox
            //
            if (db.tb_DaiLy.Any())
            {
                var result = from dl in db.tb_DaiLy.ToList()
                             select new { ID = dl.Ma_DaiLy, Name = dl.Ten_DaiLy };
                this.DaiLyComboBox.DataSource = result;
            }
            //
            //Load Thang ComboBox
            //
            if (db.tb_PhieuXuatHang.Any())
            {
                var result = (from px in db.tb_PhieuXuatHang.ToList()
                              select new { ID = px.Ngay_Lap.Month , Name = px.Ngay_Lap.Month }).Single();
                this.ThangComboBox.DataSource = result;
                this.ThangComboBox.DisplayMember = "Name";
                this.ThangComboBox.ValueMember = "ID";
            }
            //
            //Load Nam ComboBox
            //
            if (db.tb_PhieuXuatHang.Any())
            {
                var result = (from px in db.tb_PhieuXuatHang.ToList()
                              select new { ID = px.Ngay_Lap.Year, Name = px.Ngay_Lap.Year }).Single();
                this.ThangComboBox.DataSource = result;
                this.ThangComboBox.DisplayMember = "Name";
                this.ThangComboBox.ValueMember = "ID";
            }
            //
            //Load Hang Hoa ComboBox
            //
            if (db.tb_HangHoa.Any())
            {
                var result = from hh in db.tb_HangHoa.ToList()
                             select new { ID = hh.Ma_HangHoa, Name = hh.Ten_HangHoa };
                this.HangHoaComboBox.DataSource = result;
                this.HangHoaComboBox.ValueMember = "ID";
                this.HangHoaComboBox.DisplayMember = "Name";

            }

        }

        public void TimKiem()
        {
            int madaily = int.Parse(this.DaiLyComboBox.SelectedValue.ToString());
            int thang = int.Parse(this.ThangComboBox.SelectedValue.ToString());
            int nam = int.Parse(this.NamComboBox.SelectedValue.ToString());
            if (db.tb_PhieuXuatHang.Any())
            {
                var result = from px in db.tb_PhieuXuatHang.ToList()
                             where px.Ma_DaiLy == madaily && px.Ngay_Lap.Month == thang && px.Ngay_Lap.Year == nam
                             select px;
                if (result.Count()>0)
                {
                    this.PhieuXuatHangData.DataSource = null;
                    this.PhieuXuatHangData.DataSource = result;
                    this.PhieuXuatHangData.Enabled = true;
                    LoadNoHienTai();
                }
                else
                {
                    this.PhieuXuatHangData.DataSource = null;
                }
            }
        }

        private void LoadNoHienTai()
        {
            var pxhs = from pxh in db.tb_PhieuXuatHang.ToList()
                      where pxh.Ma_DaiLy == int.Parse(this.DaiLyComboBox.SelectedValue.ToString())
                       select pxh;
            var ptts = from ptt in db.tb_PhieuThuTien.ToList()
                       where ptt.Ma_DaiLy == int.Parse(this.DaiLyComboBox.SelectedValue.ToString())
                       select ptt;

            this.NoHienTaiTextBox.Text = (pxhs.Sum(p => p.TongTien) + ptts.Sum(p => p.So_Tien_Thu) + this.tempChiTiet_XuatHangs.Sum(p => p.Thanh_Tien)).ToString();
        }

        


        private void LoadChiTietPhieuXuatHang()
        {
            var result = from ctpx in db.tb_ChiTiet_XuatHang.ToList()
                         where ctpx.Ma_PhieuXuat == this.currentIDPhieuXuatHang
                         select ctpx;
            this.ChiTietPhieuXuatHangData.DataSource = null;
            this.ChiTietPhieuXuatHangData.DataSource = result;

        }

        public bool ThemChiTietPhieuXuat()
        {
            //
            //Lay thong tin
            //
            int idchitietphieuxuat = 1;
            if (db.tb_ChiTiet_XuatHang.Any())
            {
                idchitietphieuxuat = db.tb_ChiTiet_XuatHang.Max(p => p.Ma_ChiTiet_XuatHang)+1;

            }
           
            int idphieuxuathang = this.currentIDChiTietPhieuXuatHang;
            int idhanghoa = int.Parse(this.HangHoaComboBox.SelectedValue.ToString());
            int soluong = this.SoLuongTextBox.Text.All(char.IsDigit) ? int.Parse(this.SoLuongTextBox.Text) : -1;
            double dongia = (from hh in db.tb_HangHoa
                             where hh.Ma_HangHoa == int.Parse(this.HangHoaComboBox.SelectedValue.ToString())
                             select hh.Don_Gia).Single();
            double thanhtien = soluong * dongia;
            bool checkinput = this.chiTietPhieuXuatValidator.KiemTraChiTietPhieuXuat(idphieuxuathang, idhanghoa, soluong, dongia, thanhtien);
            if (checkinput==false)
            {
                this.MessageFailure = "chua dien day du thong tin";
                return false;
            }
            else
            {
                // lafm tieesp cho nay, thuc hien kiem tra tien no 
            }
            
        }


    }
}
