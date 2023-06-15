using Microsoft.VisualBasic;
using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace PointOfSale
{
    public partial class AddEditSupplier : Form
    {
        string supplierId;
        public AddEditSupplier(string supid)
        {
            InitializeComponent();
            supplierId = supid;
        }

        private void Auto()
        {
            lblCategoryNo.Text = "SU-" + GetUniqueKey(8);

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

        private void AddSupplier()
        {
            Auto();
            try
            {
                SqlConn.sqL = "INSERT INTO Supplier(SupplierId,SupplierName, Address, ContactNo, Email) VALUES('" + lblCategoryNo.Text + "','" + txtCatName.Text + "', '" + txtDescription.Text + "', '" + textBox1.Text + "', '" + textBox2.Text + "')";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.cmd.ExecuteNonQuery();
                Interaction.MsgBox("New supplier successfully added.", MsgBoxStyle.Information, "Add Supplier");
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

        private void UpdateSupplier()
        {
            try
            {
                SqlConn.sqL = "Update Supplier SET SupplierName = '" + txtCatName.Text + "', Address = '" + txtDescription.Text + "', ContactNo = '" + textBox1.Text + "', Email= '" + textBox2.Text  + "' WHERE SupplierId = '" + supplierId + "'";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.cmd.ExecuteNonQuery();
                Interaction.MsgBox("Supplier record successfully updated", MsgBoxStyle.Information, "Update Supplier");

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

        private void DeleteSupplier()
        {
            try
            {
                SqlConn.sqL = "delete from Supplier where SupplierId = '" + supplierId + "'";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.cmd.ExecuteNonQuery();
                Interaction.MsgBox("Supplier record successfully deleted", MsgBoxStyle.Information, "Delete Supplier");

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

        private void button3_Click(object sender, EventArgs e)
        {
            if (SqlConn.adding == true)
            {
                AddSupplier();
            }
            else if(SqlConn.updating == true)
            {
                UpdateSupplier();
            }

            if (Application.OpenForms["Supplier"] != null)
            {
                (Application.OpenForms["Supplier"] as Supplier).LoadSuppliers("");
            }

            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to delete this Supplier record?", "Delete Supplier", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DeleteSupplier();
            }
            if (Application.OpenForms["Supplier"] != null)
            {
                (Application.OpenForms["Supplier"] as Supplier).LoadSuppliers("");
            }

            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddEditSupplier_Load(object sender, EventArgs e)
        {
            bool isadding = SqlConn.adding.Equals(true) & SqlConn.updating.Equals(false) & SqlConn.deleting.Equals(false);
            bool isupdating = SqlConn.adding.Equals(false) & SqlConn.updating.Equals(true) & SqlConn.deleting.Equals(false);
            bool isdeleting = SqlConn.adding.Equals(false) & SqlConn.updating.Equals(false) & SqlConn.deleting.Equals(true);
            if (isadding)
            {
                lblTitle.Text = "Add New Supplier";
                ClearFields();
                Auto();
            }
            else if (isupdating)
            {
                lblTitle.Text = "Update Supplier";
                LoadUpdateSupplier();

            }
            else if (isdeleting)
            {
                lblTitle.Text = "Delete Supplier";
                LoadUpdateSupplier();
            }
        }

        private void LoadUpdateSupplier()
        {
            try
            {
                SqlConn.sqL = "SELECT * FROM Supplier WHERE SupplierId = '" + supplierId + "'";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.dr = SqlConn.cmd.ExecuteReader();

                if (SqlConn.dr.Read() == true)
                {
                    lblCategoryNo.Text = SqlConn.dr["SupplierId"].ToString();
                    txtCatName.Text = SqlConn.dr["SupplierName"].ToString();
                    txtDescription.Text = SqlConn.dr["Address"].ToString();
                    textBox2.Text = SqlConn.dr["Email"].ToString();
                    textBox1.Text = SqlConn.dr["ContactNo"].ToString();
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

        private void ClearFields()
        {
            lblCategoryNo.Text = "";
            txtDescription.Text = "";
            txtCatName.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
        }
    }
}
