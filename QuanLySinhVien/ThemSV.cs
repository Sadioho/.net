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
    public partial class ThemSV : Form
    {
        Student sinhVien;
        public ThemSV()
        {
            InitializeComponent();
            load_MaLop();
            this.Text = "Them sinh vien";
        }
        public ThemSV(Student sinhVien)
        {
              
            InitializeComponent();
            load_MaLop();
            this.Text = "Sua sinh vien";
            this.sinhVien = sinhVien;
            this.txtHo.Text = this.sinhVien.FirstName;
            this.txtTen.Text = this.sinhVien.LastName;
            this.txtMaSV.Text = this.sinhVien.ID;
            this.txtNoiSinh.Text = this.sinhVien.PlaceOfBirth;
            this.date.Text = this.sinhVien.DateOfBirth.ToString();
            var tam = this.sinhVien.IDClassroom;
            var DB = new QuanLySinhVien.DB.DB();
            var lop = DB.Classrooms.Where(t => t.ID == tam).FirstOrDefault();
            comboBoxLopHoc.Text = lop.Name;
        }
        //Load IDLop ra combobox
        List<Classroom> ls;
        void load_MaLop()
        {
            var db = new QuanLySinhVien.DB.DB();
            ls = db.Classrooms.OrderBy(t => t.Name).ToList();
            bnIDLop.DataSource = ls;
            comboBoxLopHoc.DataSource = bnIDLop;
            comboBoxLopHoc.DisplayMember = "Name";
            comboBoxLopHoc.ValueMember="ID";


        }

        private void btnThemSV_Click(object sender, EventArgs e)
        {
            date.Format = DateTimePickerFormat.Short;
            var ID = txtMaSV.Text;
            var FirstName = txtHo.Text;
            var LastName = txtTen.Text;
            DateTime dateOfBirth = DateTime.Parse(date.Text);
            var PlaceOfBirth = txtNoiSinh.Text;
            int Gender = int.Parse(comboBoxGender.SelectedIndex.ToString());
           // var IDClassroom = comboBoxLopHoc.Text;
            var IDClassroom = comboBoxLopHoc.SelectedValue.ToString();

            if (sinhVien == null)
            {
                var student = new Student
                {
                    ID = ID,
                    FirstName = FirstName,
                    LastName = LastName,
                    DateOfBirth = dateOfBirth,
                    PlaceOfBirth = PlaceOfBirth,
                    Gender = Gender,
                    IDClassroom = IDClassroom

                };
                var db = new QuanLySinhVien.DB.DB();
                db.Students.Add(student);



                try
                {
                    db.SaveChanges();
                    MessageBox.Show("Thêm sinh viên thành công", "thông báo !", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                }catch(Exception ex)
                {
                    MessageBox.Show("Mã sv trùng","thông báo !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
               

            }
            else
            {
                var db = new QuanLySinhVien.DB.DB();
                var sv = db.Students.Where(a => a.ID == sinhVien.ID).FirstOrDefault();
                sv.ID = ID;
                sv.FirstName =FirstName;
                sv.LastName = LastName;
                sv.Gender = Gender;
                sv.DateOfBirth = dateOfBirth;
                sv.PlaceOfBirth = PlaceOfBirth;
                sv.IDClassroom = IDClassroom;
                db.SaveChanges();
                DialogResult = DialogResult.OK;
            }


        }
       
     
    }
}
