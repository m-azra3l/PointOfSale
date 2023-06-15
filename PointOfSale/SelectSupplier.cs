using Microsoft.VisualBasic;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PointOfSale
{
    public partial class SelectSupplier : Form
    {
        private AddEditProduct frmProduct = null;
        public SelectSupplier(Form callingForm)
        {
            InitializeComponent();
            frmProduct = callingForm as AddEditProduct;
        }

        private void ListView1_DoubleClick(object sender, EventArgs e)
        {
            string id = ListView1.FocusedItem.Text;
            this.frmProduct.MySupplierId = id;
            this.frmProduct.Mysupplier = ListView1.FocusedItem.SubItems[1].Text;
            this.Close();
        }

        private void txtCatName_TextChanged(object sender, EventArgs e)
        {
            LoadSupplier();
        }

        private void SelectSupplier_Load(object sender, EventArgs e)
        {
            LoadSupplier1();
        }

        private void LoadSupplier1()
        {
            try
            {
                SqlConn.sqL = "SELECT * FROM Supplier ORDER BY SupplierName ";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.dr = SqlConn.cmd.ExecuteReader();

                ListViewItem x = null;
                ListView1.Items.Clear();

                while (SqlConn.dr.Read() == true)
                {
                    x = new ListViewItem(SqlConn.dr["SupplierId"].ToString());
                    x.SubItems.Add(SqlConn.dr["SupplierName"].ToString());
                    x.SubItems.Add(SqlConn.dr["Address"].ToString());

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

        private void LoadSupplier()
        {
            try
            {
                SqlConn.sqL = "SELECT * FROM Supplier WHERE SupplierName LIKE '" + txtCatName.Text + "%' ORDER BY SupplierName ";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.dr = SqlConn.cmd.ExecuteReader();

                ListViewItem x = null;
                ListView1.Items.Clear();

                while (SqlConn.dr.Read() == true)
                {
                    x = new ListViewItem(SqlConn.dr["SupplierId"].ToString());
                    x.SubItems.Add(SqlConn.dr["SupplierName"].ToString());
                    x.SubItems.Add(SqlConn.dr["Address"].ToString());

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

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
