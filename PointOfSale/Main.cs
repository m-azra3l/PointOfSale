using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace PointOfSale
{
    public partial class Main : Form
    {
        private int childFormNumber = 0;
        string Username, Role, StaffId;

        public Main(string username,string role, string staffID)
        {
            InitializeComponent();
            Username = username;
            StaffId = staffID;
            Role = role;
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

      
        public void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }



        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            if(Role == "Admin")
            {
                SqlConn.GetData();
                ToolStripStatusLabel4.Text = DateTime.Now.ToString();
                this.lbluser.Text = Username.ToUpper();
            }
            else
            {
                viewMenu.Enabled = false;
                statusStrip.Visible = false;
                SqlConn.GetData();
                ToolStripStatusLabel4.Text = DateTime.Now.ToString();
                this.lbluser.Text = Username.ToUpper();
                POS newMDIChild = new POS(Username, Role, StaffId);
                newMDIChild.MdiParent = this;
                newMDIChild.Show();
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Categories newMDIChild = new Categories();
            newMDIChild.MdiParent = this;
            newMDIChild.Show();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            Products newMDIChild = new Products();
            newMDIChild.MdiParent = this;
            newMDIChild.Show();
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            SystemSettings newMDIChild = new SystemSettings();
            newMDIChild.MdiParent = this;
            newMDIChild.Show();
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            Staff newMDIChild = new Staff();
            newMDIChild.MdiParent = this;
            newMDIChild.Show();
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            FilterDailySalesReport newMDIChild = new FilterDailySalesReport();
            newMDIChild.MdiParent = this;
            newMDIChild.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ToolStripStatusLabel4.Text = DateTime.Now.ToString();
        }

        private void Main_Closing(object sender, CancelEventArgs e)
        {
            Login newLogin = new Login();
            newLogin.Show();
            e.Cancel = true;
            this.Close();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            POS newMDIChild = new POS(Username,Role,StaffId);
            newMDIChild.MdiParent = this;
            newMDIChild.Show();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            Supplier newMDIChild = new Supplier();
            newMDIChild.MdiParent = this;
            newMDIChild.Show();
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            FilterStockReport newMDIChild = new FilterStockReport();
            newMDIChild.MdiParent = this;
            newMDIChild.Show();
        }

        private void Calculator_Click(object sender, EventArgs e)
        {
            Calculator newMDIChild = new Calculator();
            newMDIChild.MdiParent = this;
            newMDIChild.Show();
        }

        private void Calendar_Click(object sender, EventArgs e)
        {
            Calendar newMDIChild = new Calendar();
            newMDIChild.MdiParent = this;
            newMDIChild.Show();
        }
    }
}
