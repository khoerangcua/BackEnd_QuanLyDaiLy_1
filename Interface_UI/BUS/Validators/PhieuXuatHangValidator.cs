using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_UI.BUS.Validators
{
    public class PhieuXuatHangValidator
    {
        public string MessageFailure { get; set; }

        public PhieuXuatHangValidator()
        {
            this.MessageFailure = "";
        }

        public bool KiemTraThongTinPhieuXuatHang(int maDaiLy)
        {
            if (maDaiLy==-1)
            {
                this.MessageFailure = "Chưa điền đầy đủ thông tin";
                return false;
            }
            return true;
        }
    }
}
