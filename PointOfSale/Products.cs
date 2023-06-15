using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Excel = Microsoft.Office.Interop.Excel;

namespace PointOfSale
{
    public partial class Products : Form
    {
        string productID;
        public Products()
        {
            InitializeComponent();
        }
        public void LoadProducts(string strSearch)
        {
            try
            {
                SqlConn.sqL = "SELECT ProductId,ProductName, P.Description, Barcode, UnitPrice, StocksOnHand, ReorderLevel, CategoryName FROM Product as P LEFT JOIN Category C ON P.CategoryId = C.CategoryId WHERE ProductName LIKE '" + strSearch + "%' ORDER BY ProductName";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.dr = SqlConn.cmd.ExecuteReader();

                ListViewItem x = null;
                ListView1.Items.Clear();


                while (SqlConn.dr.Read() == true)
                {
                    x = new ListViewItem(SqlConn.dr["ProductId"].ToString());
                    x.SubItems.Add(SqlConn.dr["ProductName"].ToString());
                    x.SubItems.Add(SqlConn.dr["Description"].ToString());
                    x.SubItems.Add(SqlConn.dr["Barcode"].ToString());
                    x.SubItems.Add(SqlConn.dr["CategoryName"].ToString());
                    x.SubItems.Add(Strings.Format(SqlConn.dr["UnitPrice"], "#,##0.00"));
                    x.SubItems.Add(SqlConn.dr["StocksOnHand"].ToString());
                    x.SubItems.Add(SqlConn.dr["ReOrderLevel"].ToString());

                    ListView1.Items.Add(x);
                }
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.ToString());
            }
            finally
            {
                SqlConn.cmd.Dispose();
                SqlConn.conn.Close();
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            SqlConn.adding = true;
            SqlConn.updating = false;
            string init = "";
            AddEditProduct aeP = new AddEditProduct(init);
            aeP.ShowDialog();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
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
                    productID = ListView1.FocusedItem.Text;
                    AddEditProduct aeP = new AddEditProduct(productID);
                    aeP.ShowDialog();
                }
            }
            catch
            {
                Interaction.MsgBox("Please select record to update", MsgBoxStyle.Exclamation, "Update");
                return;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            SqlConn.strSearch = Interaction.InputBox("ENTER PRODUCT NAME.", "Search Product", " ");

            if (SqlConn.strSearch.Length >= 1)
            {
                LoadProducts(SqlConn.strSearch.Trim());
            }
            else if (string.IsNullOrEmpty(SqlConn.strSearch))
            {
                return;
            }
        }
        private void Products_Load(object sender, EventArgs e)
        {
            LoadProducts("");
        }

        private void btnStocksIn_Click(object sender, EventArgs e)
        {

            if (ListView1.Items.Count == 0)
            {
                Interaction.MsgBox("Please select record to add stock", MsgBoxStyle.Exclamation, "StocksIn");
                return;
            }
            try
            {
                if (string.IsNullOrEmpty(ListView1.FocusedItem.Text))
                {

                }
                else
                {

                    productID = ListView1.FocusedItem.Text;
                    StocksIn aeP = new StocksIn(productID);
                    aeP.ShowDialog();
                }
            }
            catch
            {
                Interaction.MsgBox("Please select record to add stock", MsgBoxStyle.Exclamation, "StocksIn");
                return;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
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
                    productID = ListView1.FocusedItem.Text;
                    AddEditProduct f2 = new AddEditProduct(productID);
                    f2.ShowDialog();
                }
            }
            catch
            {
                Interaction.MsgBox("Please select record to delete", MsgBoxStyle.Exclamation, "Delete");
                return;
            }
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
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
