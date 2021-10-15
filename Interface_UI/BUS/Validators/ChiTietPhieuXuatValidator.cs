using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_UI.BUS.Validators
{
    class ChiTietPhieuXuatValidator
    {
        public string MessageFailure { get; set; }
        public ChiTietPhieuXuatValidator()
        {
            this.MessageFailure = "";
        }
        public bool KiemTraChiTietPhieuXuat(int maPhieuXuat, int maHangHoa, int soLuong, double donGia, double thanhTien)
        {
            if (maPhieuXuat==-1&& maHangHoa == -1&& soLuong == -1&& donGia == -1&& thanhTien == -1)
            {
                this.MessageFailure = "Chưa điền đầy đủ thông tin";
                return false;
            }
            return true;
        }
    }
}
