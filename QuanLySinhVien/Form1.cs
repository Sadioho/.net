using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLySinhVien.DB;

namespace QuanLySinhVien
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            GridLopHoc.AutoGenerateColumns = false;
            GridStudent.AutoGenerateColumns = false;
            LoadClassroom();
            LoadSutdent();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
           var f = new ThemLop();
           var rs= f.ShowDialog(); 
            if (rs == DialogResult.OK)
            {
                LoadClassroom();
            }

        }
        void LoadClassroom()
        {
            var db = new QuanLySinhVien.DB.DB();
            var ls = db.Classrooms.ToList();
            bdsLopHoc.DataSource = ls;
            GridLopHoc.DataSource = bdsLopHoc;
        }
        void LoadSutdent()
        {
            var db = new QuanLySinhVien.DB.DB();
            var ls= db.Students.ToList();
            bdsSinhVien.DataSource = ls;
            GridStudent.DataSource = bdsSinhVien;
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnThemSV_Click(object sender, EventArgs e)
        {
            var f = new ThemSV();
            var rs = f.ShowDialog();
            if (rs == DialogResult.OK)
            {
                LoadSutdent();
            }
        }

        private void btnSuaLop_Click(object sender, EventArgs e)
        {
            var lopDangChon = bdsLopHoc.Current as Classroom;
            if (lopDangChon != null)
            {
                var f = new ThemLop(lopDangChon);
                if (f.ShowDialog() == DialogResult.OK)
                {
                    LoadClassroom();
                }
            }
        }
        private void btnXoaLop_Click(object sender, EventArgs e)
        {
            var lopDangChon = bdsLopHoc.Current as Classroom;
            if (lopDangChon != null)
            {
                var rs = MessageBox.Show(
                    "Bạn có thực sự muốn xóa không?",
                    "Chú ý",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning);
                if (rs == DialogResult.OK)
                {
                    var db = new QuanLySinhVien.DB.DB();
                    //delete student of class
                    var student = db.Students.Where(st => st.IDClassroom == lopDangChon.ID).FirstOrDefault();
                    if (student != null)
                    {
                        db.Students.Remove(student);
                        db.SaveChanges();
                    
                    }
                    //Xóa lớp đang chọn

                    var lop = db.Classrooms.Where(t => t.ID == lopDangChon.ID).FirstOrDefault();
                    if (lop != null)
                    {
                        db.Classrooms.Remove(lop);
                        db.SaveChanges();
                        LoadClassroom();
                    }
                }
            }   
               
        }

        private void bdsLopHoc_CurrentChanged(object sender, EventArgs e)
        {
            var lopDangChon = bdsLopHoc.Current as Classroom;
            if (lopDangChon != null)
            {
                var db = new QuanLySinhVien.DB.DB();
                var dsSV = db.Students.Where(d => d.IDClassroom == lopDangChon.ID).ToList();
                bdsSinhVien.DataSource = dsSV;
                GridStudent.DataSource = bdsSinhVien;
            }
        }

        private void btnSuaSV_Click(object sender, EventArgs e)
        {
            var sinhVienDangChon = bdsSinhVien.Current as Student;
            if (sinhVienDangChon != null)
            {
                var f = new ThemSV(sinhVienDangChon);
                if (f.ShowDialog() == DialogResult.OK)
                {
                    LoadSutdent();
                }
            }
        }

        private void btnXoaSV_Click(object sender, EventArgs e)
        {
            var sinhVienDangChon= bdsSinhVien.Current as Student;
            if (sinhVienDangChon != null)
            {
                var rs = MessageBox.Show(
                    "Bạn có thực sự muốn xóa không?",
                    "Chú ý",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning);
                if (rs == DialogResult.OK)
                {
                    //Xóa lớp đang chọn
                    var db = new QuanLySinhVien.DB.DB();
                    var sinhvien = db.Students.Where(t => t.ID == sinhVienDangChon.ID).FirstOrDefault();
                    if (sinhvien != null)
                    {
                        db.Students.Remove(sinhvien);
                        db.SaveChanges();
                        LoadSutdent();
                    }
                }

            }
        }

        private void ImportExcel_Click(object sender, EventArgs e)
        {
            var f = new Excel();
            var rs = f.ShowDialog();
            if (rs == DialogResult.OK)
            {
                LoadSutdent();
            }
        }
    }
}
