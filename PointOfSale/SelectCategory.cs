using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace PointOfSale
{
    public partial class SelectCategory : Form
    {
        private AddEditProduct frmProduct = null;

        public SelectCategory(Form callingForm)
        {
            InitializeComponent();
            frmProduct = callingForm as AddEditProduct;
        }

        private void LoadCategory()
        {
            try
            {
                SqlConn.sqL = "SELECT * FROM Category WHERE CategoryName LIKE '" + txtCatName.Text + "%' ORDER BY CategoryName ";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.dr = SqlConn.cmd.ExecuteReader();

                ListViewItem x = null;
                ListView1.Items.Clear();

                while (SqlConn.dr.Read() == true)
                {
                    x = new ListViewItem(SqlConn.dr["CategoryId"].ToString());
                    x.SubItems.Add(SqlConn.dr["CategoryName"].ToString());
                    x.SubItems.Add(SqlConn.dr["Description"].ToString());

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

        private void SelectCategory_Load(object sender, EventArgs e)
        {
            LoadCategory1();
        }

        private void txtCatName_TextChanged(object sender, EventArgs e)
        {
            LoadCategory();
        }

        private void LoadCategory1()
        {
            try
            {
                SqlConn.sqL = "SELECT * FROM Category ORDER BY CategoryName ";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.dr = SqlConn.cmd.ExecuteReader();

                ListViewItem x = null;
                ListView1.Items.Clear();

                while (SqlConn.dr.Read() == true)
                {
                    x = new ListViewItem(SqlConn.dr["CategoryId"].ToString());
                    x.SubItems.Add(SqlConn.dr["CategoryName"].ToString());
                    x.SubItems.Add(SqlConn.dr["Description"].ToString());

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

        private void ListView1_DoubleClick(object sender, EventArgs e)
        {
            string id = ListView1.FocusedItem.Text;
            this.frmProduct.CategoryID = id;
            this.frmProduct.Category = ListView1.FocusedItem.SubItems[1].Text;
            this.Close();
        }
    }
}
