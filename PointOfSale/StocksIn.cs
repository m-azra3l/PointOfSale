using Microsoft.VisualBasic;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PointOfSale
{
    public partial class StocksIn : Form
    {
        string productID;
        public StocksIn(string prodID)
        {
            InitializeComponent();
            productID = prodID;
        }

        private void GetProductInfo()
        {

            try
            {

                SqlConn.sqL = "SELECT ProductId,ProductName, Description, UnitPrice, StocksOnHand FROM Product WHERE ProductId =" + productID + "";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.dr = SqlConn.cmd.ExecuteReader();

                if (SqlConn.dr.Read() == true)
                {
                    lblProductCode.Text = SqlConn.dr[0].ToString();
                    label3.Text = SqlConn.dr[1].ToString();
                    lblDescription.Text = SqlConn.dr[2].ToString();
                    lblPrice.Text = Strings.FormatNumber(SqlConn.dr[3]).ToString();
                    lblCurrentStocks.Text = SqlConn.dr[4].ToString();
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

        private void AddStockIn()
        {
            try
            {
                SqlConn.sqL = "INSERT INTO StockIn(ProductId, Quantity, DateIn) Values('" + productID + "', '" + txtQuantity.Text + "', '" + DateTime.Now.ToString("dd/MM/yyyy") + "')";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.cmd.ExecuteNonQuery();
                Interaction.MsgBox("Stocks successfully added.", MsgBoxStyle.Information, "Add Stocks");
                UpdateProductQuantity();
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

        private void UpdateProductQuantity()
        {
            try
            {
                SqlConn.sqL = "UPDATE Product SET StocksOnhand = StocksOnHand + '" + Conversion.Val(txtQuantity.Text.Replace(",", "")) + "' WHERE ProductId = '" + productID + "'";
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

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            txtTotalStocks.Text = Strings.Format(Conversion.Val(lblCurrentStocks.Text) + Conversion.Val(txtQuantity.Text), "#,##0.00");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddStockIn();
            if (Application.OpenForms["Products"] != null)
            {
                (Application.OpenForms["Products"] as Products).LoadProducts("");
            }

            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void StocksIn_Load(object sender, EventArgs e)
        {
            GetProductInfo();
            txtQuantity.Text = "";
            txtTotalStocks.Text = "";
        }
    }
}
