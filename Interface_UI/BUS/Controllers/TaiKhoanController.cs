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
    public class TaiKhoanController
    {
        public string MessageFailure { get; set; }
        private TaiKhoanValidator taiKhoanValidator;
        public int CurrentID { get; set; }
        QuanLyDaiLyEntities db;
        //dang nhap
        public TextBox TaiKhoan { get; set; }
        public TextBox MatKhau { get; set; }
        //thay doi mat kahu
        public TextBox MatKhauCu { get; set; }
        public TextBox MatKhauMoi { get; set; }
        public TextBox NhapLaiMatKhauMoi { get; set; }


        public TaiKhoanController()
        {
            this.db = new QuanLyDaiLyEntities();
            this.taiKhoanValidator = new TaiKhoanValidator();
            this.MessageFailure = "";
            this.CurrentID = -1;



        }

        public bool DangNhap()
        {
            //
            //reset messagefailure
            //
            this.MessageFailure = "";
            //
            // Lấy thông tin
            //
            string taikhoan = this.TaiKhoan.Text;
            string matkhau = this.MatKhau.Text;
            //
            //kiểm tra thông tin đầu vào
            //            
            bool checkinput = taiKhoanValidator.KiemTraThongTinDangNhap(taikhoan, matkhau);
            if (checkinput == false)
            {
                this.MessageFailure = taiKhoanValidator.MessageFailure;
                return false;

            }
            else
            {
                //
                //Kiểm tra tài khoản và mật khẩu trong csdl
                //
                var result = db.tb_TaiKhoan.FirstOrDefault(tk => tk.TenTaiKhoan == taikhoan && tk.MatKhau == matkhau);
                if (result == default)
                {
                    this.MessageFailure = "Sai tài khoản hoặc mật khẩu";
                    return false;
                }
                else
                {
                    //
                    //reset form đăng nhập và lưu lại id tài khoản
                    //
                    this.TaiKhoan.Text = "";
                    this.MatKhau.Text = "";
                    this.CurrentID = result.ID_TaiKhoan;
                    return true;
                }
            }
        }

        public bool ThayDoiMatKhau(int currentID)
        {
            //
            //reset messagefailure
            //
            this.MessageFailure = "";
            //
            //Lấy thông tin
            //
            string mkcu = this.MatKhauCu.Text;
            string mkmoi = this.MatKhauMoi.Text;
            string nhaplaimkmoi = this.NhapLaiMatKhauMoi.Text;
            //
            //Kiểm tra thông tin đầu vào
            //
            bool checkinput = this.taiKhoanValidator.KiemTraThongTinDoiMatKhau(mkcu, mkmoi, nhaplaimkmoi);
            if (checkinput == false)
            {
                this.MessageFailure = taiKhoanValidator.MessageFailure;
                return false;
            }
            else
            {
                //
                //Xác thực id và mật khẩu hiện tại
                //
                var result = db.tb_TaiKhoan.FirstOrDefault(tk => tk.ID_TaiKhoan == currentID && tk.MatKhau == mkcu);
                if (result == default)
                {
                    this.MessageFailure = "Mật khẩu không đúng";
                    return false;
                }
                else
                {
                    result.MatKhau = mkmoi;
                    int result2 = db.SaveChanges();
                    //
                    //Kiểm tra lưu thay đổi thành công
                    //
                    if (result2 == 0)
                    {
                        this.MessageFailure = "Thay đổi mật khẩu không thành công";
                        return false;
                    }
                    else
                    {
                        //
                        //reset form thay đổi mật khẩu
                        //
                        this.MatKhauCu.Text = "";
                        this.MatKhauMoi.Text = "";
                        this.NhapLaiMatKhauMoi.Text = "";
                        return true;
                    }
                }
            }
        }
    }
}
