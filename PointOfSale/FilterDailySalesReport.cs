using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace PointOfSale
{
    public partial class FilterDailySalesReport : Form
    {
        string staffid;

        public FilterDailySalesReport()
        {
            InitializeComponent();
        }

        public string Mysupplier
        {
            get { return textBox2.Text; }
            set { textBox2.Text = value; }
        }

        public string MySupplierId
        {
            get { return staffid; }
            set { staffid = value; }
        }

        public void StaffReport()
        {
            try
            {
                SqlConn.sqL = "SELECT InvoiceNo,InvoiceDate,StaffId,StaffUsername,TotalAmount,AmtPaid,PaymentType FROM Transactions WHERE InvoiceDate=@d1 and StaffId=@d2 ORDER By InvoiceDate";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.cmd.Parameters.AddWithValue("@d1", DateTimePicker1.Text);
                SqlConn.cmd.Parameters.AddWithValue("@d2", textBox2.Text);
                SqlConn.dr = SqlConn.cmd.ExecuteReader();

                ListViewItem x = null;
                ListView1.Items.Clear();

                while (SqlConn.dr.Read() == true)
                {
                    x = new ListViewItem(SqlConn.dr["InvoiceNo"].ToString());
                    x.SubItems.Add(SqlConn.dr["InvoiceDate"].ToString());
                    x.SubItems.Add(SqlConn.dr["StaffId"].ToString());
                    x.SubItems.Add(SqlConn.dr["StaffUsername"].ToString());
                    x.SubItems.Add(SqlConn.dr["TotalAmount"].ToString());
                    x.SubItems.Add(SqlConn.dr["AmtPaid"].ToString());
                    x.SubItems.Add(SqlConn.dr["PaymentType"].ToString());
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

        public void InvoiceReport()
        {
            try
            {
                SqlConn.sqL = "SELECT InvoiceNo,InvoiceDate,StaffId,StaffUsername,TotalAmount,AmtPaid,PaymentType FROM Transactions WHERE InvoiceDate='" + DateTimePicker1.Text + "' ORDER By InvoiceDate";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.dr = SqlConn.cmd.ExecuteReader();

                ListViewItem x = null;
                ListView1.Items.Clear();

                while (SqlConn.dr.Read() == true)
                {
                    x = new ListViewItem(SqlConn.dr["InvoiceNo"].ToString());
                    x.SubItems.Add(SqlConn.dr["InvoiceDate"].ToString());
                    x.SubItems.Add(SqlConn.dr["StaffId"].ToString());
                    x.SubItems.Add(SqlConn.dr["StaffUsername"].ToString());
                    x.SubItems.Add(SqlConn.dr["TotalAmount"].ToString());
                    x.SubItems.Add(SqlConn.dr["AmtPaid"].ToString());
                    x.SubItems.Add(SqlConn.dr["PaymentType"].ToString());
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

        public void LoadReport()
        {
            try
            {
                SqlConn.sqL = "SELECT InvoiceNo,InvoiceDate,StaffId,StaffUsername,TotalAmount,AmtPaid,PaymentType FROM Transactions ORDER By InvoiceDate";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.dr = SqlConn.cmd.ExecuteReader();

                ListViewItem x = null;
                ListView1.Items.Clear();

                while (SqlConn.dr.Read() == true)
                {
                    x = new ListViewItem(SqlConn.dr["InvoiceNo"].ToString());
                    x.SubItems.Add(SqlConn.dr["InvoiceDate"].ToString());
                    x.SubItems.Add(SqlConn.dr["StaffId"].ToString());
                    x.SubItems.Add(SqlConn.dr["StaffUsername"].ToString());
                    x.SubItems.Add(SqlConn.dr["TotalAmount"].ToString());
                    x.SubItems.Add(SqlConn.dr["AmtPaid"].ToString());
                    x.SubItems.Add(SqlConn.dr["PaymentType"].ToString());
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

        public void Reset()
        {
            textBox2.Text = "";
            DateTimePicker1.Text = DateTime.Today.ToString();
            LoadReport();
        }

        private void rbUser_CheckedChanged(object sender, EventArgs e)
        {
            if (rbUser.Checked == true)
            {
                rbInvoice.Checked = false;
            }
        }

        private void rbInvoice_CheckedChanged(object sender, EventArgs e)
        {
            if (rbInvoice.Checked == true)
            {
                rbUser.Checked = false;
            }
        }

        //LoadReport
        private void button3_Click(object sender, EventArgs e)
        {
            if ((rbUser.Checked == false) && (rbInvoice.Checked == false))
            {
                Interaction.MsgBox("Please select report by User or Invoice No", MsgBoxStyle.Information, "Select Report");
                return;
            }


            if (rbUser.Checked == true)
            {
                if (textBox2.Text == "")
                {
                    Interaction.MsgBox("Please select report by staff id", MsgBoxStyle.Information, "Select Staff");
                    return;
                }
                else
                {
                    StaffReport();
                }
            }
            else
            {
                InvoiceReport();                
            }
        }

        private void button4_Click(object sender, EventArgs e)
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

        //ViewReport
        private void button2_Click(object sender, EventArgs e)
        {
            if ((rbUser.Checked == false) && (rbInvoice.Checked == false))
            {
                Interaction.MsgBox("Please select report by User or Invoice No", MsgBoxStyle.Information, "Select Report");
                return;
            }


            if (rbUser.Checked == true)
            {
                if(textBox2.Text == "")
                {
                    Interaction.MsgBox("Please select report by staff id", MsgBoxStyle.Information, "Select Staff");
                    return;
                }
                else
                {
                    try
                    {
                        SalesReportbyStaff rcp = new SalesReportbyStaff();
                        SqlDataAdapter da = new SqlDataAdapter();
                        Report ds = new Report();
                        SqlConn.ConnDB();
                        SqlConn.cmd.CommandText = "SELECT InvoiceNo,InvoiceDate,NonVATTotal,VATPer,VATAmount,TotalAmount,Discount,NonDisTotal,AmtPaid,PaymentType,Change,StaffId,StaffUsername FROM Transactions WHERE InvoiceDate=@d1 and StaffId=@d2 ORDER By InvoiceDate";
                        SqlConn.cmd.Parameters.AddWithValue("@d1", DateTimePicker1.Text);
                        SqlConn.cmd.Parameters.AddWithValue("@d2", textBox2.Text);
                        SqlConn.cmd.CommandType = CommandType.Text;
                        da.SelectCommand = SqlConn.cmd;
                        da.Fill(ds, "Transactions");
                        rcp.SetDataSource(ds);
                        SalesReportViewer rpt = new SalesReportViewer();
                        rpt.crystalReportViewer1.ReportSource = rcp;
                        rpt.Show();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        SqlConn.cmd.Dispose();
                        SqlConn.conn.Close();
                    }
                }
            }
            else
            {
                try
                {
                    SalesReportbyInvoice rcp = new SalesReportbyInvoice();
                    SqlDataAdapter da = new SqlDataAdapter();
                    Report ds = new Report();
                    SqlConn.ConnDB();
                    SqlConn.cmd.CommandText = "SELECT InvoiceNo,InvoiceDate,NonVATTotal,VATPer,VATAmount,TotalAmount,Discount,NonDisTotal,AmtPaid,PaymentType,Change,StaffId,StaffUsername FROM Transactions WHERE InvoiceDate=@d1 ORDER By InvoiceDate";
                    SqlConn.cmd.Parameters.AddWithValue("@d1", DateTimePicker1.Text);
                    SqlConn.cmd.CommandType = CommandType.Text;
                    da.SelectCommand = SqlConn.cmd;
                    da.Fill(ds, "Transactions");
                    rcp.SetDataSource(ds);
                    SalesReportViewer rpt = new SalesReportViewer();
                    rpt.crystalReportViewer1.ReportSource = rcp;
                    rpt.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    SqlConn.cmd.Dispose();
                    SqlConn.conn.Close();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void FilterDailySalesReport_Load(object sender, EventArgs e)
        {
            LoadReport();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SelectStaffId flc = new SelectStaffId(this);
            flc.ShowDialog();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
