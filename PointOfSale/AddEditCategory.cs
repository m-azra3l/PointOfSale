using Microsoft.VisualBasic;
using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace PointOfSale
{
    public partial class AddEditCategory : Form
    {
        string categoryID;
        public AddEditCategory(string catID)
        {
            InitializeComponent();
            categoryID = catID;
        }

        private void CLearFields()
        {
            lblCategoryNo.Text = "";
            txtCatName.Text = "";
            txtDescription.Text = "";
        }

        private void Auto()
        {
            lblCategoryNo.Text = "C-" + GetUniqueKey(6);

        }
        public static string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[62];
            chars = "123456789".ToCharArray();
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[maxSize];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        private void LoadUpdateCategory()
        {
            try
            {
                SqlConn.sqL = "SELECT * FROM Category WHERE CategoryId = '" + categoryID + "'";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.dr = SqlConn.cmd.ExecuteReader();

                if (SqlConn.dr.Read() == true)
                {
                    lblCategoryNo.Text = SqlConn.dr["CategoryId"].ToString();
                    txtCatName.Text = SqlConn.dr["CategoryName"].ToString();
                    txtDescription.Text = SqlConn.dr["Description"].ToString();
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

        private void AddCategory()
        {
            try
            {
                SqlConn.sqL = "INSERT INTO Category(CategoryId,CategoryName, Description) VALUES('" + lblCategoryNo.Text + "','" + txtCatName.Text + "', '" + txtDescription.Text + "')";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.cmd.ExecuteNonQuery();
                Interaction.MsgBox("New category successfully added.", MsgBoxStyle.Information, "Add Category");
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

        private void UpdateCategory()
        {
            try
            {
                SqlConn.sqL = "UPDATE Category SET CategoryName= '" + txtCatName.Text + "', Description = '" + txtDescription.Text + "' WHERE CategoryId = '" + categoryID + "'";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.cmd.ExecuteNonQuery();
                Interaction.MsgBox("Category successfully updated.", MsgBoxStyle.Information, "Update Category");
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

        private void AddEditCategory_Load(object sender, EventArgs e)
        {
            bool isadding = SqlConn.adding.Equals(true) & SqlConn.updating.Equals(false) & SqlConn.deleting.Equals(false);
            bool isupdating = SqlConn.adding.Equals(false) & SqlConn.updating.Equals(true) & SqlConn.deleting.Equals(false);
            bool isdeleting = SqlConn.adding.Equals(false) & SqlConn.updating.Equals(false) & SqlConn.deleting.Equals(true);
            if (isadding)
            {
                lblTitle.Text = "Add New Category";
                CLearFields();
                Auto();
            }
            else if (isupdating)
            {
                lblTitle.Text = "Update Category";
                LoadUpdateCategory();

            }
            else if (isdeleting)
            {
                lblTitle.Text = "Delete Category";
                LoadUpdateCategory();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (SqlConn.adding == true)
            {
                AddCategory();
            }
            else
            {
                UpdateCategory();

            }
            if (Application.OpenForms["Categories"] != null)
            {
                (Application.OpenForms["Categories"] as Categories).LoadCategories("");
            }

            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to delete this category record?", "Delete Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DeleteCategory();
            }
            if (Application.OpenForms["Categories"] != null)
            {
                (Application.OpenForms["Categories"] as Categories).LoadCategories("");
            }

            this.Close();
        }

        private void DeleteCategory()
        {
            try
            {
                SqlConn.sqL = "delete from Category where CategoryId = '" + categoryID + "'";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.cmd.ExecuteNonQuery();
                Interaction.MsgBox("Category successfully deleted", MsgBoxStyle.Information, "Delete Category");

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
    }
}
