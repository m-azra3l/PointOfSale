using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PointOfSale
{
    public partial class Categories : Form
    {
        public string catgoryID;
        public Categories()
        {
            InitializeComponent();
        }

        public void LoadCategories(string strSearch)
        {
            try
            {
                SqlConn.sqL = "SELECT * FROM Category WHERE CategoryName LIKE '" + strSearch + "%' ORDER By CategoryName";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.dr = SqlConn.cmd.ExecuteReader(CommandBehavior.CloseConnection);

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

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {

            SqlConn.adding = true;
            SqlConn.updating = false;
            string init = "";
            AddEditCategory aeC = new AddEditCategory(init);
            aeC.ShowDialog();
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
                    catgoryID = ListView1.FocusedItem.Text;
                    AddEditCategory aeC = new AddEditCategory(catgoryID);
                    aeC.ShowDialog();
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
            SqlConn.strSearch = Interaction.InputBox("ENTER CATEGORY NAME.", "Search Category", " ");

            if (SqlConn.strSearch.Length >= 1)
            {
                LoadCategories(SqlConn.strSearch.Trim());
            }
            else if (string.IsNullOrEmpty(SqlConn.strSearch))
            {
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
                    catgoryID = ListView1.FocusedItem.Text;
                    AddEditCategory f2 = new AddEditCategory(catgoryID);
                    f2.ShowDialog();
                }
            }
            catch
            {
                Interaction.MsgBox("Please select record to delete", MsgBoxStyle.Exclamation, "Delete");
                return;
            }
        }
    }
}
