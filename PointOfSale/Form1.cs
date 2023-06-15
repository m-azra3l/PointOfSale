using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using ZXing;

namespace PointOfSale
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        FilterInfoCollection filterCol;
        VideoCaptureDevice videoCaptureDevice;

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = dateTimePicker1.Text;
            textBox2.Text = DateTime.Now.ToString("dd/MM/yyyy");//dateTimePicker1.Value.ToString("dd/MM/yyyy");
            filterCol = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filter in filterCol)
            {
                cboCamera.Items.Add(filter.Name);
            }
            cboCamera.SelectedIndex = 0;
            //videoCaptureDevice = new VideoCaptureDevice(filterCol[cboCamera.SelectedIndex]?.MonikerString);
            //videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
            //videoCaptureDevice.Start();
        }


        private void VideoCaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            BarcodeReader reader = new BarcodeReader();
            var ressult = reader.Decode(bitmap);
            if(ressult != null)
            {
                textBox3.Invoke(new MethodInvoker(delegate (){
                    textBox3.Text = ressult.ToString();
                }));
            }
            pictureBox1.Image = bitmap;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SqlConn.sqL = "SELECT * from Product where BarCode= '" + textBox3.Text + "'";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.dr = SqlConn.cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (SqlConn.dr.Read() == true)
                {
                    int a;
                    a = Convert.ToInt32(SqlConn.dr["StocksOnHand"].ToString());
                    if( a > 0)
                    {
                        textBox4.Text = SqlConn.dr["ProductId"].ToString();
                        textBox5.Text = SqlConn.dr["ProductName"].ToString();
                        textBox6.Text = SqlConn.dr["UnitPrice"].ToString();
                        textBox7.Text = SqlConn.dr["StocksOnHand"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Product is out of stock", "Out of Stock", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox4.Text = "";
                        textBox5.Text = "";
                        textBox6.Text = "";
                        textBox7.Text = "";
                    }
                }
                else
                {
                    MessageBox.Show("Product not available", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox4.Text = "";
                    textBox5.Text = "";
                    textBox6.Text = "";
                    textBox7.Text = "";
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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(videoCaptureDevice != null)
            {
                if (videoCaptureDevice.IsRunning)
                {
                    videoCaptureDevice.Stop();
                }                    
            }
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            videoCaptureDevice = new VideoCaptureDevice(filterCol[cboCamera.SelectedIndex].MonikerString);
            videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
            videoCaptureDevice.Start();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F1:
                    textBox1.Text = "";
                    break;
                case Keys.F2:
                    textBox2.Text = "";
                    break;
                case Keys.F3:
                    break;
                case Keys.F4:
                    break;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                textBox1.Text = "";
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                textBox2.Text = "";
            }
        }
    }
}
