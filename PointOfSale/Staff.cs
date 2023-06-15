using Microsoft.VisualBasic;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace PointOfSale
{
    public partial class Staff : Form
    {
        public string staffID;
        public Staff()
        {
            InitializeComponent();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Staff_Load(object sender, EventArgs e)
        {
            LoadStaffs("");
        }

        public void LoadStaffs(string search)
        {
            try
            {
                SqlConn.sqL = "SELECT StaffId, CONCAT(Lastname, ', ', Firstname, ' ', MI) as ClientName, CONCAT(Address, ', ',Street, ', ', City , ', ', State) as Address, CONCAT(ContactNo, ', ',Email) as Contact, Username, role FROM Staff WHERE LASTNAME LIKE '" + search.Trim() + "%' ORDER By Lastname";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.dr = SqlConn.cmd.ExecuteReader();

                ListViewItem x = null;
                ListView1.Items.Clear();

                while (SqlConn.dr.Read() == true)
                {
                    x = new ListViewItem(SqlConn.dr["StaffId"].ToString());
                    x.SubItems.Add(SqlConn.dr["ClientName"].ToString());
                    x.SubItems.Add(SqlConn.dr["Contact"].ToString());
                    x.SubItems.Add(SqlConn.dr["Address"].ToString());
                    x.SubItems.Add(SqlConn.dr["Username"].ToString());
                    x.SubItems.Add(SqlConn.dr["Role"].ToString());

                    ListView1.Items.Add(x);
                }
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message);
            }
            finally
            {
                SqlConn.cmd.Dispose();
                SqlConn.conn.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ListView1.Items.Count == 0)
            {
                Interaction.MsgBox("Please select record to update", MsgBoxStyle.Exclamation, "Update");
                return;
            }
            try
            {
                if (string.IsNullOrEmpty(ListView1.FocusedItem.Text))
                {

                }
                else
                {
                    SqlConn.adding = false;
                    SqlConn.updating = true;
                    staffID = ListView1.FocusedItem.Text;
                    AddEditStaff f2 = new AddEditStaff(staffID);
                    f2.ShowDialog();
                }
            }
            catch
            {
                Interaction.MsgBox("Please select record to update", MsgBoxStyle.Exclamation, "Update");
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConn.adding = true;
            SqlConn.updating = false;
            SqlConn.deleting = false;
            string init = "";
            AddEditStaff f2 = new AddEditStaff(init);
            f2.ShowDialog();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (ListView1.Items.Count == 0)
            {
                Interaction.MsgBox("Please select record to update", MsgBoxStyle.Exclamation, "Update");
                return;
            }
            try
            {
                if (string.IsNullOrEmpty(ListView1.FocusedItem.Text))
                {

                }
                else
                {
                    SqlConn.adding = false;
                    SqlConn.updating = true;
                    SqlConn.deleting = false;
                    staffID = ListView1.FocusedItem.Text;
                    AddEditStaff f2 = new AddEditStaff(staffID);
                    f2.ShowDialog();
                }
            }
            catch
            {
                Interaction.MsgBox("Please select record to update", MsgBoxStyle.Exclamation, "Update");
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlConn.strSearch = Interaction.InputBox("Enter Staff's Lastname/Surname.", "Search Staff", " ");

            if (SqlConn.strSearch.Length >= 1)
            {
                LoadStaffs(SqlConn.strSearch.Trim());
            }
            else if (string.IsNullOrEmpty(SqlConn.strSearch))
            {
                return;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (ListView1.Items.Count == 0)
            {
                Interaction.MsgBox("Please select record to delete", MsgBoxStyle.Exclamation, "Delete");
                return;
            }
            try
            {
                if (string.IsNullOrEmpty(ListView1.FocusedItem.Text))
                {

                }
                else
                {
                    SqlConn.adding = false;
                    SqlConn.updating = false;
                    SqlConn.deleting = true;
                    staffID = ListView1.FocusedItem.Text;
                    AddEditStaff f2 = new AddEditStaff(staffID);
                    f2.ShowDialog();
                }
            }
            catch
            {
                Interaction.MsgBox("Please select record to delete", MsgBoxStyle.Exclamation, "Delete");
                return;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int rowsTotal = 0;
            int colsTotal = 0;
            int I = 0;
            int j = 0;
            int iC = 0;
            Cursor.Current = Cursors.WaitCursor;
            Excel.Application xlApp = new Excel.Application();

            try
            {
                Excel.Workbook excelBook = xlApp.Workbooks.Add();
                Excel.Worksheet excelWorksheet = (Excel.Worksheet)excelBook.Worksheets[1];
                xlApp.Visible = true;

                rowsTotal = ListView1.Items.Count - 1;
                colsTotal = ListView1.Columns.Count - 1;
                var _with1 = excelWorksheet;
                _with1.Cells.Select();
                _with1.Cells.Delete();
                for (iC = 0; iC <= colsTotal; iC++)
                {
                    _with1.Cells[1, iC + 1].Value = ListView1.Columns[iC].Text;
                }
                for (I = 0; I <= rowsTotal; I++)
                {
                    for (j = 0; j <= colsTotal; j++)
                    {
                        _with1.Cells[I + 2, j + 1].value = ListView1.Items[I].SubItems[j].Text;
                    }
                }
                _with1.Rows["1:1"].Font.FontStyle = "Bold";
                _with1.Rows["1:1"].Font.Size = 12;

                _with1.Cells.Columns.AutoFit();
                _with1.Cells.Select();
                _with1.Cells.EntireColumn.AutoFit();
                _with1.Cells[1, 1].Select();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //RELEASE ALLOACTED RESOURCES
                Cursor.Current = Cursors.Default;
                xlApp = null;
            }
        }
    }
}
