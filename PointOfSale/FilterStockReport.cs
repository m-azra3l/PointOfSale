using Microsoft.VisualBasic;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace PointOfSale
{
    public partial class FilterStockReport : Form
    {
        public FilterStockReport()
        {
            InitializeComponent();
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

        public void StockIn()
        {
            try
            {
                SqlConn.sqL = "SELECT ProductId, ProductName, Quantity,DateIn FROM StockIn ORDER By DateIn";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.dr = SqlConn.cmd.ExecuteReader();

                ListViewItem x = null;
                ListView1.Items.Clear();

                while (SqlConn.dr.Read() == true)
                {
                    x = new ListViewItem(SqlConn.dr["DateIn"].ToString());
                    x.SubItems.Add(SqlConn.dr["ProductId"].ToString());
                    x.SubItems.Add(SqlConn.dr["ProductName"].ToString());
                    x.SubItems.Add(SqlConn.dr["Quantity"].ToString());

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

        public void Reset1()
        {
            dtpStartDate.Text = DateTime.Today.ToString();
            dtpEndDate.Text = DateTime.Today.ToString();
            StockIn();
        }
        public void Reset2()
        {
            dateTimePicker1.Text = DateTime.Today.ToString();
            dateTimePicker2.Text = DateTime.Today.ToString();
            StockOut();
        }

        public void StockOut()
        {
            try
            {
                SqlConn.sqL = "SELECT ProductId, ProductName, Quantity,DateOut FROM StockOut ORDER By DateOut";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.dr = SqlConn.cmd.ExecuteReader();

                ListViewItem x = null;
                listView2.Items.Clear();

                while (SqlConn.dr.Read() == true)
                {
                    x = new ListViewItem(SqlConn.dr["DateOut"].ToString());
                    x.SubItems.Add(SqlConn.dr["ProductId"].ToString());
                    x.SubItems.Add(SqlConn.dr["ProductName"].ToString());
                    x.SubItems.Add(SqlConn.dr["Quantity"].ToString());

                    listView2.Items.Add(x);
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

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConn.sqL = "SELECT ProductId, ProductName, Quantity,DateIn FROM StockIn where DateIn between '" + dtpStartDate.Text + "' AND '" + dtpEndDate.Text + "' ORDER By DateIn";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.dr = SqlConn.cmd.ExecuteReader();

                ListViewItem x = null;
                ListView1.Items.Clear();

                while (SqlConn.dr.Read() == true)
                {
                    x = new ListViewItem(SqlConn.dr["DateIn"].ToString());
                    x.SubItems.Add(SqlConn.dr["ProductId"].ToString());
                    x.SubItems.Add(SqlConn.dr["ProductName"].ToString());
                    x.SubItems.Add(SqlConn.dr["Quantity"].ToString());

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

        private void FilterStockReport_Load(object sender, EventArgs e)
        {
            Reset1();
            Reset2();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConn.sqL = "SELECT ProductId, ProductName, Quantity,DateOut FROM StockOut where DateOut between '" + dateTimePicker2.Text + "' AND '" + dateTimePicker1.Text + "' ORDER By DateOut";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.dr = SqlConn.cmd.ExecuteReader();

                ListViewItem x = null;
                ListView1.Items.Clear();

                while (SqlConn.dr.Read() == true)
                {
                    x = new ListViewItem(SqlConn.dr["DateOut"].ToString());
                    x.SubItems.Add(SqlConn.dr["ProductId"].ToString());
                    x.SubItems.Add(SqlConn.dr["ProductName"].ToString());
                    x.SubItems.Add(SqlConn.dr["Quantity"].ToString());

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

        private void button8_Click(object sender, EventArgs e)
        {
            Reset1();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Reset2();
        }

        private void button2_Click(object sender, EventArgs e)
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

                rowsTotal = listView2.Items.Count - 1;
                colsTotal = listView2.Columns.Count - 1;
                var _with1 = excelWorksheet;
                _with1.Cells.Select();
                _with1.Cells.Delete();
                for (iC = 0; iC <= colsTotal; iC++)
                {
                    _with1.Cells[1, iC + 1].Value = listView2.Columns[iC].Text;
                }
                for (I = 0; I <= rowsTotal; I++)
                {
                    for (j = 0; j <= colsTotal; j++)
                    {
                        _with1.Cells[I + 2, j + 1].value = listView2.Items[I].SubItems[j].Text;
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

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                StocksOutReport rcp = new StocksOutReport();
                Report ds = new Report();
                SqlConn.ConnDB();
                SqlConn.cmd.CommandText = "SELECT * FROM StockOut where DateOut Between '" + dateTimePicker2.Text + "' AND '" + dateTimePicker1.Text + "' ORDER By DateOut";
                SqlConn.cmd.CommandType = CommandType.Text;
                SqlConn.da.SelectCommand = SqlConn.cmd;
                SqlConn.da.Fill(ds, "StockOut");
                rcp.SetDataSource(ds);
                StocksReportViewer rpt = new StocksReportViewer();
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                StocksInReport rcp = new StocksInReport();
                Report ds = new Report();
                SqlConn.ConnDB();
                SqlConn.cmd.CommandText = "SELECT * FROM StockIn where DateIn Between '" + dtpStartDate.Text + "' AND '" + dtpEndDate.Text + "' ORDER By DateIn";
                SqlConn.cmd.CommandType = CommandType.Text;
                SqlConn.da.SelectCommand = SqlConn.cmd;
                SqlConn.da.Fill(ds, "StockIn");
                rcp.SetDataSource(ds);
                StocksReportViewer rpt = new StocksReportViewer();
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
}
