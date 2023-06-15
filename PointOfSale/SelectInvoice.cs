using Microsoft.VisualBasic;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PointOfSale
{
    public partial class SelectInvoice : Form
    {
        private POS pOS = null;

        public SelectInvoice(Form callingForm)
        {
            InitializeComponent();
            pOS = callingForm as POS;
        }

        private void SelectInvoice_Load(object sender, EventArgs e)
        {
            LoadInvoice1();
        }

        public void LoadInvoice()
        {
            try
            {
                SqlConn.sqL = "SELECT * FROM Transactions WHERE InvoiceNo LIKE '" + txtCatName.Text + "%' ORDER BY InvoiceDate ";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.dr = SqlConn.cmd.ExecuteReader();

                ListViewItem x = null;
                ListView1.Items.Clear();

                while (SqlConn.dr.Read() == true)
                {
                    x = new ListViewItem(SqlConn.dr["InvoiceNo"].ToString());
                    x.SubItems.Add(SqlConn.dr["InvoiceDate"].ToString());

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

        public void LoadInvoice1()
        {
            try
            {
                SqlConn.sqL = "SELECT InvoiceNo,InvoiceDate FROM Transactions ORDER BY InvoiceDate ";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.dr = SqlConn.cmd.ExecuteReader();

                ListViewItem x = null;
                ListView1.Items.Clear();

                while (SqlConn.dr.Read() == true)
                {
                    x = new ListViewItem(SqlConn.dr["InvoiceNo"].ToString());
                    x.SubItems.Add(SqlConn.dr["InvoiceDate"].ToString());

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

        private void txtCatName_TextChanged(object sender, EventArgs e)
        {
            LoadInvoice();
        }

        private void ListView1_DoubleClick(object sender, EventArgs e)
        {
            string id = ListView1.FocusedItem.Text;
            this.pOS.MySupplierId = id;
            this.pOS.Mysupplier = ListView1.FocusedItem.SubItems[0].Text;
            this.Close();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
