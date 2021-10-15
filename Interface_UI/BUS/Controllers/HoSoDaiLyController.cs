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
    class HoSoDaiLyController
    {


        #region Fields and Properties
        public string MessageFailure { get; set; }
        //
        // Data Access
        //
        QuanLyDaiLyEntities db;

        //
        //Validator
        //
        HoSoDaiLyValidator hoSoDaiLyValidator;

        //
        //Controls
        //

        public TextBox MaDaiLyText { get; set; }
        public TextBox TenDaiLyText_HienThi { get; set; }
        public TextBox TenDaiLyText_TimKiem { get; set; }
        public TextBox DiaChiText { get; set; }
        public TextBox DienThoaiText { get; set; }
        public TextBox NgayTiepNhanText { get; set; }
        public TextBox EmailText { get; set; }
        public DataGridView HoSoDaiLyDataGridView { get; set; }
        public ComboBox LoaiDaiLyComboBox { get; set; }
        public ComboBox QuanComboBox_HienThi { get; set; }
        public ComboBox QuanComboBox_TimKiem { get; set; }
        #endregion

        #region Constructor
        public HoSoDaiLyController()
        {
            this.db = new QuanLyDaiLyEntities();
            this.hoSoDaiLyValidator = new HoSoDaiLyValidator();

        }
        #endregion


        #region Methods

        public void ChayLanDau()
        {

            //
            // Load Quận
            //
            var result1 = from q in db.tb_Quan
                          select new
                          {
                              MaQuan = q.Ma_Quan,
                              TenQuan = q.Ten_Quan
                          };

            QuanComboBox_HienThi.DataSource = result1.ToList();
            QuanComboBox_HienThi.ValueMember = "MaQuan";
            QuanComboBox_TimKiem.DisplayMember = "TenQuan";

            QuanComboBox_TimKiem.DataSource = result1.ToList();
            QuanComboBox_TimKiem.ValueMember = "MaQuan";
            QuanComboBox_TimKiem.DisplayMember = "TenQuan";
            //
            // Load loại đại lý
            //
            LoaiDaiLyComboBox = new ComboBox();
            var result2 = from ldl in db.tb_LoaiDaiLy
                          select new
                          {
                              MaLoai = ldl.Ma_Loai_DaiLy,
                              TenLoai = ldl.Ten_Loai

                          };
            LoaiDaiLyComboBox.DataSource = result2.ToList();
            LoaiDaiLyComboBox.ValueMember = "MaLoai";
            LoaiDaiLyComboBox.DisplayMember = "TenLoai";


        }

        public void LoadDaiLy()
        {
            if (db.tb_DaiLy.Any())
            {
                var result = from dl in db.tb_DaiLy
                             join mdl in db.tb_LoaiDaiLy on dl.Ma_Loai_DaiLy equals mdl.Ma_Loai_DaiLy
                             join q in db.tb_Quan on dl.Ma_Quan equals q.Ma_Quan
                             select new { ID = dl.Ma_DaiLy, Ten = dl.Ten_DaiLy, SDT = dl.Dien_Thoai, Quan = q.Ten_Quan, Email = dl.Email, LoaiDaiLy = mdl.Ten_Loai, DiaChi = dl.Dia_Chi, NgayTiepNhan = dl.Ngay_Tiep_Nhan.ToString() };
                this.HoSoDaiLyDataGridView.DataSource = null;
                this.HoSoDaiLyDataGridView.DataSource = result.ToList();
            }
        }

        public bool ThemDaiLy()
        {
            //
            // Lấy giá trị trên form
            //
            int id = 0;
            if (db.tb_DaiLy.Any())
            {
                id = (from dl in db.tb_DaiLy
                      select dl.Ma_DaiLy).Max() + 1;
            }
            else
            {
                id = 1;
            }

            string ten = this.TenDaiLyText_HienThi.Text;
            string sdt = this.DienThoaiText.Text;
            int maquan = int.Parse(this.QuanComboBox_HienThi.SelectedValue.ToString());
            string email = this.EmailText.Text;
            int maloai = int.Parse(this.LoaiDaiLyComboBox.SelectedValue.ToString());
            string diachi = this.DiaChiText.Text;
            DateTime ngaytiepnhan = DateTime.Now;

            //
            //Kiểm tra thông tin đầu vào
            //
            bool result2 = this.hoSoDaiLyValidator.KiemTraThongTinDaiLy(id, ten, sdt, maquan, email, maloai, diachi);
            if (result2 == false)
            {
                //TODO: Thông bao lỗi và gọi envent
                return false;
            }
            else
            {
                //
                // Tạo mới đối tượng Đại Lý
                //
                tb_DaiLy dailynew = new tb_DaiLy
                {
                    Ma_DaiLy = id,
                    Ten_DaiLy = ten,
                    Dien_Thoai = sdt,
                    Ma_Quan = maquan,
                    Email = email,
                    Ma_Loai_DaiLy = maloai,
                    Dia_Chi = diachi,
                    Ngay_Tiep_Nhan = ngaytiepnhan

                };
                //
                // Thêm vào csdl
                //
                db.tb_DaiLy.Add(dailynew);
                int result3 = db.SaveChanges();

                if (result3 == 0)
                {
                    //TODO: Lưu lỗi
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public bool XoaDaiLy()
        {
            string id = this.MaDaiLyText.Text;
            if (id == "")
            {
                //TODO: lưu lỗi trống id
                return false;

            }
            else
            {
                var result = db.tb_DaiLy.FirstOrDefault(dl => dl.Ma_DaiLy == int.Parse(id));
                if (result == default)
                {
                    //TODO: lưu lỗi không tìm thấy trong csdl
                    return false;

                }
                else
                {
                    db.tb_DaiLy.Remove(result);
                    int result2 = db.SaveChanges();
                    if (result2 == 0)
                    {
                        //TODO: lưu lỗi lưu vào csdl không thành công
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }

        public bool LuuThayDoi()
        {
            int id = this.MaDaiLyText.Text == null ? -1 : int.Parse(this.MaDaiLyText.Text);
            string ten = this.TenDaiLyText_HienThi.Text;
            string sdt = this.DienThoaiText.Text;
            int maquan = int.Parse(this.QuanComboBox_HienThi.SelectedValue.ToString());
            string email = this.EmailText.Text;
            int maloai = int.Parse(this.LoaiDaiLyComboBox.SelectedValue.ToString());
            string diachi = this.DiaChiText.Text;

            //
            // Kiểm tra id có trống ?
            //
            if (id == -1)
            {
                //TODO: lưu lỗi trống id
                return false;

            }
            else
            {
                //
                // Kiểm tra tồn tại
                //
                var result = db.tb_DaiLy.FirstOrDefault(dl => dl.Ma_DaiLy == id);
                if (result == default)
                {
                    //TODO: lưu lỗi không tìm thấy trong csdl
                    return false;

                }
                else
                {
                    //
                    //Kiểm tra input
                    //
                    bool result2 = this.hoSoDaiLyValidator.KiemTraThongTinDaiLy(id, ten, sdt, maquan, email, maloai, diachi);
                    if (result2 == false)
                    {
                        //TODO: lưu lỗi
                        return false;
                    }
                    else
                    {
                        result.Ten_DaiLy = ten;
                        result.Ma_Quan = maquan;
                        result.Dien_Thoai = sdt;
                        result.Email = email;
                        result.Ma_Loai_DaiLy = maloai;
                        result.Dia_Chi = diachi;
                        int result3 = db.SaveChanges();
                        //
                        //Kiểm tra lưu vào cơ sở dữ liệu
                        //
                        if (result3 == 0)
                        {
                            //TODO: lưu lỗi lưu vào csdl không thành công
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }

                }
            }
        }

        public void TimKiemDaiLy()
        {
            string ten = this.TenDaiLyText_TimKiem.Text;
            int maquan = int.Parse(this.QuanComboBox_TimKiem.SelectedValue.ToString());
            if (ten == "")
            {
                //
                //Load tất cả đại lý theo mã quận
                //
                var result = from dl in db.tb_DaiLy
                             join mdl in db.tb_LoaiDaiLy on dl.Ma_Loai_DaiLy equals mdl.Ma_Loai_DaiLy
                             join q in db.tb_Quan on dl.Ma_Quan equals q.Ma_Quan
                             where dl.Ma_Quan == maquan
                             select new { ID = dl.Ma_DaiLy, Ten = dl.Ten_DaiLy, SDT = dl.Dien_Thoai, Quan = q.Ten_Quan, Email = dl.Email, LoaiDaiLy = mdl.Ten_Loai, DiaChi = dl.Dia_Chi, NgayTiepNhan = dl.Ngay_Tiep_Nhan.ToString() };
                this.HoSoDaiLyDataGridView.DataSource = null;
                this.HoSoDaiLyDataGridView.DataSource = result.ToList();
            }
            else
            {
                //
                //Tìm kiếm đại lý gần đúng theo tên và mã quận
                //
                var result = from dl in db.tb_DaiLy
                             join mdl in db.tb_LoaiDaiLy on dl.Ma_Loai_DaiLy equals mdl.Ma_Loai_DaiLy
                             join q in db.tb_Quan on dl.Ma_Quan equals q.Ma_Quan
                             where dl.Ma_Quan == maquan && dl.Ten_DaiLy.Contains(ten)
                             select new { ID = dl.Ma_DaiLy, Ten = dl.Ten_DaiLy, SDT = dl.Dien_Thoai, Quan = q.Ten_Quan, Email = dl.Email, LoaiDaiLy = mdl.Ten_Loai, DiaChi = dl.Dia_Chi, NgayTiepNhan = dl.Ngay_Tiep_Nhan.ToString() };
                this.HoSoDaiLyDataGridView.DataSource = null;
                this.HoSoDaiLyDataGridView.DataSource = result.ToList();
            }
        }

        #endregion




    }
}