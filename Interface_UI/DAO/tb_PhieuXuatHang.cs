//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Interface_UI.DAO
{
    using System;
    using System.Collections.Generic;
    
    public partial class tb_PhieuXuatHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_PhieuXuatHang()
        {
            this.tb_ChiTiet_XuatHang = new HashSet<tb_ChiTiet_XuatHang>();
        }
    
        public int Ma_PhieuXuat { get; set; }
        public int Ma_DaiLy { get; set; }
        public System.DateTime Ngay_Lap { get; set; }
        public double TongTien { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_ChiTiet_XuatHang> tb_ChiTiet_XuatHang { get; set; }
        public virtual tb_DaiLy tb_DaiLy { get; set; }
    }
}