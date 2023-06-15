using Microsoft.VisualBasic;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;

namespace PointOfSale
{
    public partial class POS : Form
    {
        string StaffId, Username, Role, myinvoice;

        FilterInfoCollection filterCol;
        VideoCaptureDevice videoCaptureDevice;

        public POS(string username,string role,string staffID)
        {
            InitializeComponent();
            StaffId = staffID;
            Username = username;
            Role = role;
        }

        private void CloseCamera()
        {
            if (videoCaptureDevice != null)
            {
                if (videoCaptureDevice.IsRunning)
                {
                    videoCaptureDevice.Stop();
                }
            }
        }

        private void VideoCaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            BarcodeReader reader = new BarcodeReader();
            var result = reader.Decode(bitmap);
            if (result != null)
            {
                txtbarcode.Invoke(new MethodInvoker(delegate () {
                    txtbarcode.Text = result.ToString();
                }));
            }
        }

        private void MyCaptureDevice()
        {
            filterCol = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filter in filterCol)
            {
                cboCamera.Items.Add(filter.Name);
            }
            cboCamera.SelectedIndex = 0;
            videoCaptureDevice = new VideoCaptureDevice(filterCol[cboCamera.SelectedIndex].MonikerString);
            videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
            videoCaptureDevice.Start();
        }

        public string Mysupplier
        {
            get { return txtInvoiceNo.Text; }
            set { txtInvoiceNo.Text = value; }
        }

        public string MySupplierId
        {
            get { return myinvoice; }
            set { myinvoice = value; }
        }

        public void GetVATInfo()
        {
            try
            {
                SqlConn.sqL = "SELECT VatId, VatPercent FROM VATSettings";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.dr = SqlConn.cmd.ExecuteReader();


                while (SqlConn.dr.Read() == true)
                {
                    txtTaxPer.Tag = SqlConn.dr[0];
                    txtTaxPer.Text = SqlConn.dr[1].ToString();
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
            CloseCamera();
            if(Role == "Admin")
            {
                this.Close();
            }
            else
            {
                SqlConn.cmd.Dispose();
                SqlConn.conn.Close();
                Application.Exit();
            }
        }

        private void UpdateTransaction()
        {
            try
            {
                
                if (txtTaxPer.Text == "")
                {
                    MessageBox.Show("Please enter tax percentage", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtTaxPer.Focus();
                    return;
                }
                if (txtCash.Text == "" || txtCash.Text == "0.00")
                {
                    MessageBox.Show("Please enter total payment", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCash.Focus();
                    return;
                }
                if (cmbPaymentType.Text == "Select")
                {
                    MessageBox.Show("Please select payment type", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cmbPaymentType.Focus();
                    return;
                }
               
                if (ListView1.Items.Count == 0)
                {
                    MessageBox.Show("sorry no product added", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                Auto();
                string nondistotal = Convert.ToString(Calculate1().ToString());
                string discount = Convert.ToString(Calculate2().ToString());
                SqlConn.sqL = "insert Into Transactions(InvoiceNo,InvoiceDate,NonVATTotal,VATAmount,TotalAmount,Discount,NonDisTotal,AmtPaid,PaymentType,Change,StaffId,StaffUsername,VATPer) VALUES ('" + txtInvoiceNo.Text + "','" + dtpInvoiceDate.Text + "','" + txtSubTotal.Text + "'," + txtTaxAmt.Text + "," + txtTA.Text + "," + Convert.ToString(Calculate2().ToString()) + "," + Convert.ToString(Calculate1().ToString()) + "," + txtCash.Text + ",'" + cmbPaymentType.Text + "','" + txtChange.Text + "','" + StaffId + "','" + Username + "','" + txtTaxPer.Text +"')";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.cmd.ExecuteReader();
                if (SqlConn.conn.State == ConnectionState.Open)
                {
                    SqlConn.cmd.Dispose();
                    SqlConn.conn.Close();
                }
                SqlConn.cmd.Dispose();
                SqlConn.conn.Close();

                for (int i = 0; i <= ListView1.Items.Count - 1; i++)
                {

                    SqlConn.sqL = "insert Into TransactionDetails(InvoiceNo,ProductId,ProductName,Quantity,ItemPrice,PTAmount,Discount) VALUES (@d1,@d2,@d3,@d4,@d5,@d6,@d7)";
                    SqlConn.ConnDB();
                    SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                    SqlConn.cmd.Parameters.AddWithValue("d1", txtInvoiceNo.Text);
                    SqlConn.cmd.Parameters.AddWithValue("d3", ListView1.Items[i].SubItems[2].Text);
                    SqlConn.cmd.Parameters.AddWithValue("d4", ListView1.Items[i].SubItems[4].Text);
                    SqlConn.cmd.Parameters.AddWithValue("d5", ListView1.Items[i].SubItems[3].Text);
                    SqlConn.cmd.Parameters.AddWithValue("d2", ListView1.Items[i].SubItems[1].Text);
                    SqlConn.cmd.Parameters.AddWithValue("d6", ListView1.Items[i].SubItems[5].Text);
                    SqlConn.cmd.Parameters.AddWithValue("d7", ListView1.Items[i].SubItems[6].Text);
                    SqlConn.ConnDB();
                    SqlConn.cmd.ExecuteNonQuery();
                    SqlConn.cmd.Dispose();
                    SqlConn.conn.Close();
                }

                for (int i = 0; i <= ListView1.Items.Count - 1; i++)
                {
                    SqlConn.ConnDB();
                    SqlConn.sqL = "insert Into StockOut(ProductId, ProductName,Quantity,DateOut) Values(@d1,@d2,@d3,@d4)";
                    SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                    SqlConn.cmd.Parameters.AddWithValue("d1", ListView1.Items[i].SubItems[1].Text);
                    SqlConn.cmd.Parameters.AddWithValue("d2", ListView1.Items[i].SubItems[2].Text);
                    SqlConn.cmd.Parameters.AddWithValue("d3", ListView1.Items[i].SubItems[4].Text);
                    SqlConn.cmd.Parameters.AddWithValue("d4", dtpInvoiceDate.Text);
                    SqlConn.cmd.ExecuteNonQuery();
                    SqlConn.cmd.Dispose();
                    SqlConn.conn.Close();
                }

                for (int i = 0; i <= ListView1.Items.Count - 1; i++)
                {
                    SqlConn.ConnDB();
                    SqlConn.sqL = "update Product set StocksOnHand = StocksOnHand - " + ListView1.Items[i].SubItems[4].Text + " where ProductId= '" + ListView1.Items[i].SubItems[1].Text + "'";
                    SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                    SqlConn.cmd.ExecuteNonQuery();
                    SqlConn.cmd.Dispose();
                    SqlConn.conn.Close();
                }

                Save.Enabled = false;
                button1.Enabled = false;
                btnPrint.Enabled = true;
                GetData();
                MessageBox.Show("Transaction successful", "Transaction", MessageBoxButtons.OK, MessageBoxIcon.Information);
                pictureBox1_Click(this, new EventArgs());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Auto()
        {
            txtInvoiceNo.Text = "IN-" + GetUniqueKey(6);

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

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                DataGridViewRow dr = dataGridView1.SelectedRows[0];
                txtProductID.Text = dr.Cells[0].Value.ToString();
                txtProductName.Text = dr.Cells[1].Value.ToString();
                txtPrice.Text = dr.Cells[2].Value.ToString();
                txtAvailableQty.Text = dr.Cells[3].Value.ToString();
                txtSaleQty.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            string strRowNumber = (e.RowIndex + 1).ToString();
            SizeF size = e.Graphics.MeasureString(strRowNumber, this.Font);
            if (dataGridView1.RowHeadersWidth < Convert.ToInt32((size.Width + 20)))
            {
                dataGridView1.RowHeadersWidth = Convert.ToInt32((size.Width + 20));
            }
            Brush b = SystemBrushes.ControlText;
            e.Graphics.DrawString(strRowNumber, this.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2));

        }

        private void txtProduct_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SqlConn.sqL = "SELECT ProductId,ProductName,UnitPrice,StocksOnHand,Description,BarCode from Product where ProductName like '" + txtProduct.Text + "%' group by ProductId,ProductName,UnitPrice,StocksOnHand,Description,BarCode having(StocksOnHand>0) order by ProductName";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.dr = SqlConn.cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dataGridView1.Rows.Clear();
                while (SqlConn.dr.Read() == true)
                {
                    dataGridView1.Rows.Add(SqlConn.dr[0], SqlConn.dr[1], SqlConn.dr[2], SqlConn.dr[3], SqlConn.dr[4], SqlConn.dr[5]);
                }

                SqlConn.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SqlConn.cmd.Dispose();
                SqlConn.conn.Close();
            }
        }

        private void POS_Load(object sender, EventArgs e)
        {
            //MyCaptureDevice();
            this.txtStaffId.Text = StaffId;
            this.txtStaffRole.Text = Role.ToUpper();
            this.txtStaffUsername.Text = Username.ToUpper();
            textBox1.Text = DateTime.Now.ToString();
            cmbPaymentType.Text = "Select";
            txtPrice.Text = "0.00";
            txtAvailableQty.Text = "0";
            txtSaleQty.Text = "0";
            txtTotalAmount.Text = "0.00";
            txtDiscountAmount.Text = "0";
            txtDiscountPer.Text = "0";
            txtSubTotal.Text = "0.00";
            txtTaxAmt.Text = "0";
            txtTotal.Text = "0.00";
            txtTA.Text = "0.00";
            txtCash.Text = "0.00";
            txtChange.Text = "0.00";
            GetVATInfo();
            GetData();
        }

        private void Reset()
        {
            txtInvoiceNo.Text = "";
            cmbPaymentType.Text = "Select";
            dtpInvoiceDate.Text = DateTime.Today.ToString();
            txtProductName.Text = "";
            txtProductID.Text = "";
            txtPrice.Text = "0.00";
            txtAvailableQty.Text = "0";
            txtSaleQty.Text = "0";
            txtTotalAmount.Text = "0.00";
            ListView1.Items.Clear();
            txtDiscountAmount.Text = "0";
            txtDiscountPer.Text = "0";
            txtSubTotal.Text = "0.00";
            txtTaxAmt.Text = "0";
            txtTotal.Text = "0.00";
            txtTA.Text = "0.00";
            txtCash.Text = "0.00";
            txtChange.Text = "0.00";
            txtProduct.Text = "";
            Save.Enabled = true;
            btnRemove.Enabled = false;
            btnPrint.Enabled = false;
            ListView1.Enabled = true;
            Button7.Enabled = true;
            button1.Enabled = true;

        }

        public void Calculate()
        {
            CalculateTax();
            txtTotal.Text = Convert.ToDouble((Convert.ToDouble(txtSubTotal.Text)) + (Convert.ToDouble(txtTaxAmt.Text))).ToString();
        }

        private void txtDiscountPer_TextChanged(object sender, EventArgs e)
        {
            CalculateDiscount();
        }

        public void CalculateDiscount()
        {
            if (txtDiscountPer.Text != "" || txtDiscountPer.Text != "0")
            {
                txtDiscountAmount.Text = Convert.ToDouble((Convert.ToDouble(txtTotalAmount.Text) * Convert.ToDouble(txtDiscountPer.Text) / 100)).ToString();
            }
            else if (txtDiscountPer.Text == "" || txtDiscountPer.Text == "0")
            {
                txtDiscountAmount.Text = "0";
            }
        }

        public void CalculateTax()
        {
            if (txtTaxPer.Text != "" || txtTaxPer.Text != "0")
            {
                txtTaxAmt.Text = Convert.ToDouble((Convert.ToDouble(txtSubTotal.Text) * Convert.ToDouble(txtTaxPer.Text) / 100)).ToString();

            }
            else if (txtTaxPer.Text == "" || txtTaxPer.Text == "0")
            {
                txtTaxAmt.Text = "0";
            }
        }

        public double Calculate1() 
        {
            int i = 0;
            int j = 0;
            int k = 0;
            i = 0;
            j = 0;
            k = 0;


            try
            {

                j = ListView1.Items.Count;
                for (i = 0; i <= j - 1; i++)
                {
                    k = k + Convert.ToInt32(ListView1.Items[i].SubItems[5].Text);
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return k;
        }

        public double Calculate2()
        {
            int i = 0;
            int j = 0;
            int k = 0;
            i = 0;
            j = 0;
            k = 0;


            try
            {

                j = ListView1.Items.Count;
                for (i = 0; i <= j - 1; i++)
                {
                    k = k + Convert.ToInt32(ListView1.Items[i].SubItems[6].Text);
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return k;
        }

        public double Subtotal()
        {
            double a = Convert.ToDouble(Calculate1().ToString());
            double b = Convert.ToDouble(Calculate2().ToString());
            double c = a - b;
            return c;
        }

        private void txtCash_TextChanged(object sender, EventArgs e)
        {
            try
            {

                if (txtCash.Text == "" || txtCash.Text == "0.00")
                {
                    txtChange.Text = "0.00";
                }
                else
                {
                    double total = double.Parse(txtTA.Text);
                    double tendered = double.Parse(txtCash.Text);
                    double change;

                    change = tendered - total;
                    txtChange.Text = change.ToString("N2");


                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtCash_KeyDown(object sender, KeyEventArgs e)
        {
            double a = Convert.ToDouble(txtCash.Text);  
            double b = Convert.ToDouble(txtTA.Text);  
            if(e.KeyCode == Keys.Enter)
            {
                if(a >= b)
                {
                    UpdateTransaction();
                    pictureBox1_Click(this, new EventArgs());
                }
                else
                {
                    MessageBox.Show("Total amount can't be greater than amount tendered.\n\nBalance payment before proceeding.", "Invalid Payment");
                }
            }
            else if(e.KeyCode == Keys.Escape)
            {
                pictureBox1_Click(this, new EventArgs());
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            cmbPaymentType.Text = "Select";
            txtTA.Text = "1000.00";
            txtCash.Text = "0.00";
            txtChange.Text = "0.00";
            checkBox1.Checked = false;
            panel1.Visible = false;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (ListView1.Items.Count > 0) 
            {
                this.panel1.Visible = true;
                this.txtTA.Text = this.txtTotal.Text;
                //this.txtTA.Text = "1000.00";
                this.txtCash.Text = "0.00";
                this.txtCash.Focus();
                this.txtChange.SelectAll();
            }
            else
            {
                MessageBox.Show("There are no items in cart.", "Empty Cart");
            }
            /*this.panel1.Visible = true;
            //this.txtTA.Text = this.txtTotal.Text;
            this.txtTA.Text = "1000.00";
            this.txtCash.Text = "0.00";
            this.txtCash.Focus();
            this.txtChange.SelectAll();*/
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked == true)
            {
                this.txtCash.Text = this.txtTA.Text;
                this.txtCash.Focus();
            }
        }

        private void NewRecord_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox1.Text = DateTime.Now.ToString();
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRemove.Enabled = true;
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtProductName.Text == "")
                {
                    MessageBox.Show("Please retrieve product name", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (txtSaleQty.Text == "")
                {
                    MessageBox.Show("Please enter no. of sale quantity", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSaleQty.Focus();
                    return;
                }
                int SaleQty = Convert.ToInt32(txtSaleQty.Text);
                if (SaleQty == 0)
                {
                    MessageBox.Show("No. of sale quantity can not be zero", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSaleQty.Focus();
                    return;
                }

                if (ListView1.Items.Count == 0)
                {
                    ListViewItem lst = new ListViewItem();
                    lst.SubItems.Add(txtProductID.Text);
                    lst.SubItems.Add(txtProductName.Text);
                    lst.SubItems.Add(txtPrice.Text);
                    lst.SubItems.Add(txtSaleQty.Text);
                    lst.SubItems.Add(txtTotalAmount.Text);
                    lst.SubItems.Add(txtDiscountAmount.Text);
                    ListView1.Items.Add(lst);
                    txtSubTotal.Text = Subtotal().ToString();
                    
                    Calculate();
                    txtProductName.Text = "";
                    txtProductID.Text = "";
                    txtPrice.Text = "0.00";
                    txtAvailableQty.Text = "0";
                    txtSaleQty.Text = "0";
                    txtTotalAmount.Text = "0.00";
                    txtProduct.Text = "";
                    txtDiscountPer.Text = "0";
                    return;
                }

                for (int j = 0; j <= ListView1.Items.Count - 1; j++)
                {
                    if (ListView1.Items[j].SubItems[1].Text == txtProductID.Text)
                    {
                        ListView1.Items[j].SubItems[1].Text = txtProductID.Text;
                        ListView1.Items[j].SubItems[2].Text = txtProductName.Text;
                        ListView1.Items[j].SubItems[3].Text = txtPrice.Text;
                        ListView1.Items[j].SubItems[4].Text = (Convert.ToInt32(ListView1.Items[j].SubItems[4].Text) + Convert.ToInt32(txtSaleQty.Text)).ToString();
                        ListView1.Items[j].SubItems[5].Text = (Convert.ToDouble(ListView1.Items[j].SubItems[5].Text) + Convert.ToDouble(txtTotalAmount.Text)).ToString();
                        ListView1.Items[j].SubItems[6].Text = (Convert.ToDouble(ListView1.Items[j].SubItems[6].Text) + Convert.ToDouble(txtDiscountAmount.Text)).ToString();
                        txtSubTotal.Text = Subtotal().ToString();
                        Calculate();
                        txtProductName.Text = "";
                        txtProductID.Text = "";
                        txtPrice.Text = "0.00";
                        txtAvailableQty.Text = "0";
                        txtSaleQty.Text = "0";
                        txtTotalAmount.Text = "0.00";
                        txtDiscountPer.Text = "0";
                        return;

                    }
                }

                ListViewItem lst1 = new ListViewItem();
                lst1.SubItems.Add(txtProductID.Text);
                lst1.SubItems.Add(txtProductName.Text);
                lst1.SubItems.Add(txtPrice.Text);
                lst1.SubItems.Add(txtSaleQty.Text);
                lst1.SubItems.Add(txtTotalAmount.Text);
                lst1.SubItems.Add(txtDiscountAmount.Text);
                ListView1.Items.Add(lst1);
                txtSubTotal.Text = Subtotal().ToString();
                Calculate();
                txtProductName.Text = "";
                txtProductID.Text = "";
                txtPrice.Text = "0.00";
                txtAvailableQty.Text = "0";
                txtSaleQty.Text = "0";
                txtTotalAmount.Text = "0.00";
                txtDiscountPer.Text = "0";
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (ListView1.Items.Count == 0)
                {
                    MessageBox.Show("No items to remove", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    int itmCnt = 0;
                    int i = 0;
                    int t = 0;

                    ListView1.FocusedItem.Remove();
                    itmCnt = ListView1.Items.Count;
                    t = 1;

                    for (i = 1; i <= itmCnt + 1; i++)
                    {
                        //Dim lst1 As New ListViewItem(i)
                        //ListView1.Items(i).SubItems(0).Text = t
                        t = t + 1;

                    }
                    txtSubTotal.Text = Subtotal().ToString();
                    Calculate();
                    CalculateTax();
                }

                btnRemove.Enabled = false;
                if (ListView1.Items.Count == 0)
                {
                    txtSubTotal.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void GetData()
        {
            try
            {

                SqlConn.sqL = "SELECT Product.ProductId,ProductName,UnitPrice,StocksOnHand,Description,BarCode from Product group by Product.ProductId,ProductName,UnitPrice,StocksOnHand,Description,BarCode having(StocksOnHand>0) order by ProductName";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.dr = SqlConn.cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dataGridView1.Rows.Clear();
                while (SqlConn.dr.Read() == true)
                {
                    dataGridView1.Rows.Add(SqlConn.dr[0], SqlConn.dr[1], SqlConn.dr[2], SqlConn.dr[3], SqlConn.dr[4], SqlConn.dr[5]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SqlConn.cmd.Dispose();
                SqlConn.conn.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SelectInvoice flc = new SelectInvoice(this);
            flc.ShowDialog();
            btnPrint.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtProductName.Text = "";
            txtPrice.Text = "0.00";
            txtAvailableQty.Text = "0";
            txtSaleQty.Text = "0";
            txtTotalAmount.Text = "0.00";
            txtDiscountAmount.Text = "0.00";
            txtDiscountPer.Text = "0";
        }

        private void txtbarcode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SqlConn.sqL = "SELECT * from Product where BarCode= '" + txtbarcode.Text + "'";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.dr = SqlConn.cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (SqlConn.dr.Read() == true)
                {
                    int a;
                    a = Convert.ToInt32(SqlConn.dr["StocksOnHand"].ToString());
                    if (a > 0)
                    {
                        txtProductID.Text = SqlConn.dr["ProductId"].ToString();
                        txtProductName.Text = SqlConn.dr["ProductName"].ToString();
                        txtPrice.Text = SqlConn.dr["UnitPrice"].ToString();
                        txtAvailableQty.Text = SqlConn.dr["StocksOnHand"].ToString();
                        txtSaleQty.Focus();
                    }
                    else
                    {
                        MessageBox.Show("Product is out of stock", "Out of Stock", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtProductID.Text = "";
                        txtProductName.Text = "";
                        txtPrice.Text = "0.00";
                        txtAvailableQty.Text = "0";
                        SqlConn.conn.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Product not available", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtProductID.Text = "";
                    txtProductName.Text = "";
                    txtPrice.Text = "0.00";
                    txtAvailableQty.Text = "0";
                    SqlConn.conn.Close();
                }
                SqlConn.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SqlConn.cmd.Dispose();
                SqlConn.conn.Close();
            }
        }

        private void NewRecord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                Reset();
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                Receipt rcp = new Receipt();  
                Report ds = new Report();
                SqlConn.ConnDB();
                SqlConn.cmd.CommandText = "SELECT * from Transactions,TransactionDetails where Transactions.InvoiceNo=TransactionDetails.InvoiceNo and Transactions.InvoiceNo='" + txtInvoiceNo.Text + "'";
                SqlConn.cmd.CommandType = CommandType.Text;
                SqlConn.da.SelectCommand = SqlConn.cmd;
                SqlConn.da.Fill(ds, "Transactions");
                SqlConn.da.Fill(ds, "TransactionDetails");
                rcp.SetDataSource(ds);
                ReceiptViewer rpt = new ReceiptViewer();
                rpt.crystalReportViewer1.ReportSource = rcp;
                rpt.Show();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SqlConn.cmd.Dispose();
                SqlConn.conn.Close();
            }
        }

        private void txtSaleQty_TextChanged(object sender, EventArgs e)
        {
            double val1 = 0.00;
            double val2 = 0.00;
            double.TryParse(txtPrice.Text, out val1);
            double.TryParse(txtSaleQty.Text, out val2);
            double I = (val1 * val2);
            txtTotalAmount.Text = I.ToString();
        }

        private void txtSaleQty_Validating(object sender, CancelEventArgs e)
        {

            int val1 = 0;
            int val2 = 0;
            int.TryParse(txtAvailableQty.Text, out val1);
            int.TryParse(txtSaleQty.Text, out val2);
            if (val2 > val1)
            {
                MessageBox.Show("Selling quantities are more than available quantities", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSaleQty.Text = "";
                txtTotalAmount.Text = "";
                txtSaleQty.Focus();
                return;
            }
        }


    }
}
