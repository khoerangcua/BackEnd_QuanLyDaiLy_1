using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_UI.BUS.Validators
{
    public class HoSoDaiLyValidator
    {
        public string MessageFailure { get; set; }

        public bool KiemTraThongTinDaiLy(string ten, string sdt, string email, string diachi)
        {
            //
            //reset messagefailure
            //
            MessageFailure = "";
            //
            //Đảm bảo điền đầy đủ thông tin
            //            
            if (ten==""|| sdt == ""|| email == ""|| diachi == "")
            {
                MessageFailure = "Chưa điền đầy đủ thông tin đại lý";
                return false;
            }
            return true;
        }
    }
}
