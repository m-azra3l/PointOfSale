using Microsoft.VisualBasic;
using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace PointOfSale
{
    public partial class AddEditProduct : Form
    {
        string productID;
        string categoryID;
        string supplierId;
        public AddEditProduct(string prodID)
        {
            InitializeComponent();
            productID = prodID;
        }

        private void Auto()
        {
            lblProductNo.Text = "P-" + GetUniqueKey(6);

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

        private void ClearFields()
        {
            lblProductNo.Text = "";
            txtDescription.Text = "";
            txtBarcode.Text = "";
            txtCategory.Text = "";
            txtUnitPrice.Text = "";
            txtStocksOnHand.Text = "";
            txtReorderLevel.Text = "";
        }

        public string Mysupplier 
        { 
            get { return textBox2.Text; } 
            set { textBox2.Text = value; } 
        }

        public string MySupplierId
        {
            get { return supplierId; }
            set { supplierId = value; }
        }

        public string Category
        {
            get { return txtCategory.Text; }
            set { txtCategory.Text = value; }
        }

        public string CategoryID
        {
            get { return categoryID; }
            set { categoryID = value; }
        }

        private void LoadUpdateCategory()
        {
            try
            {
                SqlConn.sqL = "SELECT ProductId, ProductName, P.Description, Barcode, P.CategoryId, CategoryName, UnitPrice, StocksOnHand, ReorderLevel,SupplierName FROM Product as P LEFT JOIN Category as C ON P.CategoryId = C.CategoryId WHERE ProductId = '" + productID + "'";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.dr = SqlConn.cmd.ExecuteReader();

                if (SqlConn.dr.Read() == true)
                {
                    lblProductNo.Text = SqlConn.dr["ProductId"].ToString();
                    textBox1.Text = SqlConn.dr["ProductName"].ToString();
                    textBox2.Text = SqlConn.dr["ProductName"].ToString();
                    txtDescription.Text = SqlConn.dr["Description"].ToString();
                    txtBarcode.Text = SqlConn.dr["Barcode"].ToString();
                    txtCategory.Text = SqlConn.dr["CategoryName"].ToString();
                    txtCategory.Tag = SqlConn.dr["CategoryId"];
                    txtUnitPrice.Text = Strings.Format(SqlConn.dr["UnitPrice"], "#,##0.00");
                    txtStocksOnHand.Text = SqlConn.dr["StocksOnHand"].ToString();
                    txtReorderLevel.Text = SqlConn.dr["ReorderLevel"].ToString();
                    textBox2.Text = SqlConn.dr["SupplierName"].ToString();
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


        private void AddProducts()
        {
            try
            {
                SqlConn.sqL = "INSERT INTO Product(ProductId,ProductName, Description, Barcode, UnitPrice, StocksOnHand, ReorderLevel, CategoryId, SupplierName) VALUES('" + lblProductNo.Text + "', '" + textBox1.Text + "', '" + txtDescription.Text + "', '" + txtBarcode.Text.Trim() + "', '" + txtUnitPrice.Text.Replace(",", "") + "', '" + txtStocksOnHand.Text.Replace(",", "") + "', '" + txtReorderLevel.Text + "', '" + categoryID + "', '" + supplierId + "')";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.cmd.ExecuteNonQuery();
                Interaction.MsgBox("Product successfully added.", MsgBoxStyle.Information, "Add Product");
                AddStockIn();
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

        private void AddStockIn()
        {
            try
            {
                SqlConn.sqL = "INSERT INTO StockIn(ProductId, Quantity, ProductName, DateIn) Values('" + lblProductNo.Text + "', '" + txtStocksOnHand.Text + "', '" + textBox1.Text +"','" + DateTime.Now.ToString("dd/MM/yyyy") + "')";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.cmd.ExecuteNonQuery();
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

        private void UpdateProduct()
        {
            try
            {
                SqlConn.sqL = "UPDATE Product SET ProductName = '" + textBox1.Text + "',Description = '" + txtDescription.Text + "', Barcode = '" + txtBarcode.Text.Trim() + "', UnitPrice = '" + txtUnitPrice.Text.Replace(",", "") + "', StocksOnHand = '" + txtStocksOnHand.Text.Replace(",", "") + "', ReorderLevel = '" + txtReorderLevel.Text + "', CategoryId ='" + categoryID + "', SupplierName ='" + supplierId + "' WHERE ProductId = '" + productID + "'";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.cmd.ExecuteNonQuery();

                Interaction.MsgBox("Product successfully Updated.", MsgBoxStyle.Information, "Update Product");
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtCategory.Text == "")
            {
                Interaction.MsgBox("Please select category.", MsgBoxStyle.Information, "Category");
                return;
            }

            if (SqlConn.adding == true)
            {
                AddProducts();
            }
            else
            {
                UpdateProduct();

            }

            if (Application.OpenForms["Products"] != null)
            {
                (Application.OpenForms["Products"] as Products).LoadProducts("");
            }

            this.Close();
        }

        private void AddEditProduct_Load(object sender, EventArgs e)
        {
            bool isadding = SqlConn.adding.Equals(true) & SqlConn.updating.Equals(false) & SqlConn.deleting.Equals(false);
            bool isupdating = SqlConn.adding.Equals(false) & SqlConn.updating.Equals(true) & SqlConn.deleting.Equals(false);
            bool isdeleting = SqlConn.adding.Equals(false) & SqlConn.updating.Equals(false) & SqlConn.deleting.Equals(true);
            if (isadding)
            {
                lblTitle.Text = "Add New Product";
                ClearFields();
                Auto();
            }
            else if (isupdating)
            {
                lblTitle.Text = "Update Product";
                LoadUpdateCategory();

            }
            else if (isdeleting)
            {
                lblTitle.Text = "Delete Product";
                LoadUpdateCategory();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            SelectCategory flc = new SelectCategory(this);
            flc.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to delete this product?", "Delete Product", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DeleteProduct();
            }
            if (Application.OpenForms["Products"] != null)
            {
                (Application.OpenForms["Products"] as Products).LoadProducts("");
            }

            this.Close();
        }

        private void DeleteProduct()
        {
            try
            {
                SqlConn.sqL = "delete from Product where ProductId = '" + productID + "'";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.cmd.ExecuteNonQuery();
                Interaction.MsgBox("Product successfully deleted", MsgBoxStyle.Information, "Delete Product");

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

        private void button5_Click(object sender, EventArgs e)
        {
            SelectSupplier flc = new SelectSupplier(this);
            flc.ShowDialog();
        }
    }
}
