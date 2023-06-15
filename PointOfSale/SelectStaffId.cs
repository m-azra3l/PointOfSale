using Microsoft.VisualBasic;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PointOfSale
{
    public partial class SelectStaffId : Form
    {
        private FilterDailySalesReport filterDailySalesReport = null;

        public SelectStaffId(Form callingForm)
        {
            InitializeComponent();
            filterDailySalesReport = callingForm as FilterDailySalesReport;
        }

        private void SelectStaffId_Load(object sender, EventArgs e)
        {
            LoadStaff1();
        }

        public void LoadStaff()
        {
            try
            {
                SqlConn.sqL = "SELECT * FROM Staff WHERE Username LIKE '" + txtCatName.Text + "%' ORDER BY Username ";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.dr = SqlConn.cmd.ExecuteReader();

                ListViewItem x = null;
                ListView1.Items.Clear();

                while (SqlConn.dr.Read() == true)
                {
                    x = new ListViewItem(SqlConn.dr["StaffId"].ToString());
                    x.SubItems.Add(SqlConn.dr["Username"].ToString());
                    x.SubItems.Add(SqlConn.dr["Role"].ToString());

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
            this.filterDailySalesReport.MySupplierId = id;
            this.filterDailySalesReport.Mysupplier = ListView1.FocusedItem.SubItems[0].Text;
            this.Close();
        }

        private void txtCatName_TextChanged(object sender, EventArgs e)
        {
            LoadStaff();
        }
        private void LoadStaff1()
        {
            try
            {
                SqlConn.sqL = "SELECT StaffId,Username,Role FROM Staff ORDER BY Username ";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.dr = SqlConn.cmd.ExecuteReader();

                ListViewItem x = null;
                ListView1.Items.Clear();

                while (SqlConn.dr.Read() == true)
                {
                    x = new ListViewItem(SqlConn.dr["StaffId"].ToString());
                    x.SubItems.Add(SqlConn.dr["Username"].ToString());
                    x.SubItems.Add(SqlConn.dr["Role"].ToString());

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
