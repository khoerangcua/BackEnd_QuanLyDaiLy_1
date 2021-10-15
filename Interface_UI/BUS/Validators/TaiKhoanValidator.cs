using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_UI.BUS.Validators
{
    class TaiKhoanValidator
    {
        public string MessageFailure { get; set; }

        public bool KiemTraThongTinDangNhap(string taikhoan, string matkhau)
        {
            //
            //reset messagefailure
            //
            MessageFailure = "";
            //
            //Đảm bảo điền đầy đủ thông tin
            //
            if (taikhoan == "" || matkhau == "")
            {
                MessageFailure = "Chưa điền tài khoản hoặc mật khẩu";
                return false;
            }
            return true;
        }
        public bool KiemTraThongTinDoiMatKhau(string matkhaucu, string matkhaumoi, string nhaplaimatkhaumoi)
        {
            //
            //reset messagefailure
            //
            MessageFailure = "";
            //
            // đảm bảo điền đầy đủ thông tin
            //

            if (matkhaucu == "" || matkhaumoi == "" || nhaplaimatkhaumoi == "")
            {
                MessageFailure = "Chưa nhập đầy đủ thông tin";
                return false;
            }
            //
            // đảm bảo mật khẩu nhập lại chính xác
            //
            if (matkhaumoi != nhaplaimatkhaumoi)
            {
                MessageFailure = "Nhập lại mật khẩu không trùng khớp";
                return false;
            }
            return true;

        }
    }
}
