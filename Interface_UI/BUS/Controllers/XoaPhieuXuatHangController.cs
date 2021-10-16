using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
 using Interface_UI.DAO;
namespace Interface_UI.BUS.Controllers
{
    public class XoaPhieuXuatHangController
    {
        public string MessageFailure { get; set; }
        private QuanLyDaiLyEntities db;

        public ComboBox DaiLyComboBox
        { get; set; }
        public ComboBox ThangComboBox { get; set; }
        public ComboBox NamComboBox { get; set; }
        public DataGridView Data { get; set; }

        private int CurrentIDPhieuXuat { get; set; }

        public XoaPhieuXuatHangController()
        {
            this.MessageFailure = "";
            this.db = new QuanLyDaiLyEntities();
            this.Data.RowEnter += Data_RowEnter;
        }

        private void Data_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            this.CurrentIDPhieuXuat = int.Parse(this.Data.Rows[e.RowIndex].Cells[0].Value.ToString());
        }

        public void ChayLanDau()
        {
            //
            //Load Dai Ly ComboBox
            //
            if (this.DaiLyComboBox!=null&& db.tb_DaiLy.Any())
            {
                var result1 = from dl in db.tb_DaiLy
                              select new { ID = dl.Ma_DaiLy, Name = dl.Ten_DaiLy };
                this.DaiLyComboBox.DataSource = result1.ToList();
            }
            //
            //Load Thang ComboBox
            //
            if (this.ThangComboBox!=null)
            {
                var result2 = (from thang in db.tb_PhieuXuatHang
                              select new { ID = thang.Ngay_Lap.Month, Value = thang.Ngay_Lap.Month }).Distinct();
                this.ThangComboBox.DataSource = result2.ToList();
                this.ThangComboBox.ValueMember = "ID";
                this.ThangComboBox.DisplayMember = "Value";
            }
            //Load Nam ComboBox
            //
            //
            if (this.NamComboBox!=null)
            {
                var result3 = (from thang in db.tb_PhieuXuatHang
                               select new { ID = thang.Ngay_Lap.Year, Value = thang.Ngay_Lap.Year }).Distinct();
                this.ThangComboBox.DataSource = result3.ToList();
                this.ThangComboBox.ValueMember = "ID";
                this.ThangComboBox.DisplayMember = "Value";
            }
        }

        public void TimKiem()
        {
            //
            //Lay thong tin
            //

            int madaily = int.Parse(this.DaiLyComboBox.SelectedValue.ToString());
            int thang = int.Parse(this.ThangComboBox.SelectedValue.ToString());
            int nam = int.Parse(this.NamComboBox.SelectedValue.ToString());
            //
            //Thuc hien tim kiem
            //

            var result1 = from px in db.tb_PhieuXuatHang.ToList()
                          where px.Ma_DaiLy == madaily && px.Ngay_Lap.Month == thang && px.Ngay_Lap.Year == nam
                          select px;
            if (result1.Count()>0)
            {
                this.Data.DataSource = null;
                Data.DataSource = result1;
                
            }
            else
            {
                this.Data.DataSource = null;
            }
                          
        }
        public bool XoaPhieuXuatHang()
        {
            tb_PhieuXuatHang result1 = (from pxh in db.tb_PhieuXuatHang
                           where pxh.Ma_PhieuXuat == this.CurrentIDPhieuXuat
                           select pxh).Single();

            this.db.tb_PhieuXuatHang.Remove(result1);
            if (db.SaveChanges()>0)
            {
                return true;
            }
            else
            {
                this.MessageFailure = "Xoa Khong Thanh Cong";
                return false;
            }
        } 
    }

}
