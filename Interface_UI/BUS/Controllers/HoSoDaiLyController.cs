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
            this.MessageFailure = "";

        }
        #endregion


        #region Methods

        public void ChayLanDau()
        {
            //
            // Load Quận ComboBox
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
            // Load loại đại lý ComboBox
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
            //reset messagefailure
            //
            this.MessageFailure = "";
            //
            // Lấy thông tin đại lý
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
            bool checkinput = this.hoSoDaiLyValidator.KiemTraThongTinDaiLy(ten,sdt,email, diachi);
            if (checkinput == false)
            {
                this.MessageFailure = hoSoDaiLyValidator.MessageFailure;
                return false;
            }
            else
            {
                //
                // Tạo mới đối tượng Đại Lý và lưu vào csdl
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
                db.tb_DaiLy.Add(dailynew);
                int result3 = db.SaveChanges();
                //
                //Kiểm tra lưu thành công
                //
                if (result3 == 0)
                {
                    this.MessageFailure = "Thêm đại lý không thành công";
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
            //
            //Reset messagefailure
            //
            this.MessageFailure = "";
            //
            //Lấy thông tin
            //
            string id = this.MaDaiLyText.Text;
            //
            //Kiểm tra thông tin đầu vào
            //
            if (id == "")
            {
                this.MessageFailure = "Chưa chọn đại lý";
                return false;

            }
            else
            {
                //
                //Kiểm tra đại lý có tồn tại trong csdl
                //
                var result = db.tb_DaiLy.FirstOrDefault(dl => dl.Ma_DaiLy == int.Parse(id));
                if (result == default)
                {
                    this.MessageFailure = "Đại lý không tồn tại";
                    return false;

                }
                else
                {
                    //
                    //Xóa đại lý khỏi csdl
                    //
                    db.tb_DaiLy.Remove(result);
                    int result2 = db.SaveChanges();
                    //
                    //Kiểm tra xóa thành công
                    //
                    if (result2 == 0)
                    {
                        this.MessageFailure = "Xóa đại lý không thành công";
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }

        public bool CapNhatDaiLy()
        {
            //
            //reset messagefailure
            //
            this.MessageFailure = "";
            //
            //Lấy thông tin đại lý
            //
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
                this.MessageFailure = "Lỗi : chưa điền id";
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
                    this.MessageFailure = "Đại lý không tồn tại";
                    return false;

                }
                else
                {
                    //
                    //Kiểm tra thông tin đầu vào
                    //
                    bool checkinput = this.hoSoDaiLyValidator.KiemTraThongTinDaiLy(ten, sdt, email, diachi);
                    if (checkinput == false)
                    {
                        this.MessageFailure = hoSoDaiLyValidator.MessageFailure;
                        return false;
                    }
                    else
                    {
                        //
                        //Thực hiện thay đổi thông tin đại lý và lưu vào csdl
                        //
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
                            this.MessageFailure = "Thay đổi thông tin đại lý không thành công";
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
            //
            //Lấy thông tin đại lý
            //
            string ten = this.TenDaiLyText_TimKiem.Text;
            int maquan = int.Parse(this.QuanComboBox_TimKiem.SelectedValue.ToString());
            //
            //Thực thi tìm kiếm
            //
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