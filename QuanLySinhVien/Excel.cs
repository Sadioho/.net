using ExcelDataReader;
using QuanLySinhVien.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Z.Dapper.Plus;

namespace QuanLySinhVien
{
    public partial class Excel : Form
    {
        public Excel()
        {
            InitializeComponent();
        }
        DataTableCollection tableCollection;
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dataTable = tableCollection[cboSheet.SelectedItem.ToString()];
            dataSinhVien.DataSource = dataTable;
            if (dataTable != null)
            {
                List<Student> students = new List<Student>();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Student student = new Student();
                    student.ID = dataTable.Rows[i]["ID"].ToString();
                    student.FirstName = dataTable.Rows[i]["FirstName"].ToString();
                    student.LastName = dataTable.Rows[i]["LastName"].ToString();
                    student.DateOfBirth = DateTime.Parse(dataTable.Rows[i]["DateOfBirth"].ToString());
                    student.PlaceOfBirth = dataTable.Rows[i]["PlaceOfBirth"].ToString();
                    student.Gender = int.Parse(dataTable.Rows[i]["Gender"].ToString());
                    student.IDClassroom = dataTable.Rows[i]["IDClassroom"].ToString();
                    students.Add(student);
                }
                bnSv.DataSource = students;
            }
        }
       
        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog()
            { Filter = "Excel Wordbook|*.xlsx|Excel 97-2003 Wordbook|*.xls" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFileName.Text = openFileDialog.FileName;
                    using (var stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                    {
                        using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (_) => new ExcelDataTableConfiguration
                                {
                                    UseHeaderRow = true
                                }
                            });
                            tableCollection = result.Tables;
                            cboSheet.Items.Clear();
                            foreach (DataTable table in tableCollection)
                                cboSheet.Items.Add(table.TableName); // Add sheet to combobox

                        }
                    }

                }
            }
        }

        private void btnThemFile_Click(object sender, EventArgs e)
        {
            try
            {
                string conectionString = "Server=MSI\\SQLEXPRESS;Database=QLSVNhom1;User=sa; Password=123456";
                DapperPlusManager.Entity<Student>().Table("Student");
                List<Student> students = bnSv.DataSource as List<Student>;
                if (students != null)
                {
                    using (IDbConnection db = new SqlConnection(conectionString))
                    {
                        db.BulkInsert(students);
                    }
                }
                MessageBox.Show("Import dữ liệu từ file excel thành công!", "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
