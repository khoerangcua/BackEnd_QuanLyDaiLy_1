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
    
    public partial class tb_Quan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_Quan()
        {
            this.tb_DaiLy = new HashSet<tb_DaiLy>();
        }
    
        public int Ma_Quan { get; set; }
        public string Ten_Quan { get; set; }
        public int DaiLy_ToiDa { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_DaiLy> tb_DaiLy { get; set; }
    }
}