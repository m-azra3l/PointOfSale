using Microsoft.VisualBasic;
using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PointOfSale
{
    public partial class AddEditStaff : Form
    {
        string LSStaffID;
        public AddEditStaff(string staffID)
        {

            InitializeComponent();
            LSStaffID = staffID;
        }

        private void Auto()
        {
            lblProductNo.Text = "ST-" + GetUniqueKey(8);

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
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (SqlConn.adding == true)
            {
                AddStaff();
            }
            else
            {
                UpdateStaff();
            }

            if (Application.OpenForms["Staff"] != null)
            {
                (Application.OpenForms["Staff"] as Staff).LoadStaffs("");
            }

            this.Close();
        }

       private void AddStaff()
        {
            try
            {
                SqlConn.sqL = "INSERT INTO STAFF(StaffId,Lastname, Firstname, MI, Address, Street, City, State, ContactNo, Email, Username, Role, Password) VALUES('" + lblProductNo.Text + "','" + txtLastname.Text + "', '" + txtFirstname.Text + "', '" + txtMI.Text + "', '" + textBox1.Text +  "', '" + txtStreet.Text + "', '" + txtCity.Text + "', '" + txtProvince.Text + "', '" + txtContractNo.Text + "', '"+ textBox2.Text + "', '" + txtUsername.Text + "', '" + txtRole.Text + "', '" + txtPassword.Text + "')";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.cmd.ExecuteNonQuery();
                Interaction.MsgBox("New staff successfully added.", MsgBoxStyle.Information, "Add Staff");
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

        private void UpdateStaff()
        {
            try
            {
                SqlConn.sqL = "Update Staff SET Lastname = '" + txtLastname.Text + "', Firstname = '" + txtFirstname.Text + "', MI = '" + txtMI.Text + "', Address= '" + textBox1.Text + "', Street= '" + txtStreet.Text + "', City = '" + txtCity.Text + "', State = '" + txtProvince.Text + "', ContactNo = '" + txtContractNo.Text + "', Email = '" + textBox2.Text + "', Username ='" + txtUsername.Text + "', Role = '" + txtRole.Text + "', Password = '" + txtPassword.Text + "' WHERE StaffId = '" + LSStaffID + "'";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.cmd.ExecuteNonQuery();
                Interaction.MsgBox("Staff record successfully updated", MsgBoxStyle.Information, "Update Staff");

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

        private void DeleteStaff()
        {
            try
            {
                SqlConn.sqL = "delete from Staff where StaffId = '" + LSStaffID + "'";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.cmd.ExecuteNonQuery();
                Interaction.MsgBox("Staff record successfully deleted", MsgBoxStyle.Information, "Delete Staff");

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

        private void LoadUpdateStaff()
        {
            try
            {
                SqlConn.sqL = "SELECT * FROM STAFF WHERE StaffId = '" + LSStaffID + "'";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.dr = SqlConn.cmd.ExecuteReader();

                if (SqlConn.dr.Read() == true)
                {
                    lblProductNo.Text = SqlConn.dr["StaffID"].ToString();
                    txtLastname.Text = SqlConn.dr["lastname"].ToString();
                    txtFirstname.Text = SqlConn.dr["Firstname"].ToString();
                    txtMI.Text = SqlConn.dr["MI"].ToString();
                    textBox1.Text = SqlConn.dr["Address"].ToString();
                    txtStreet.Text = SqlConn.dr["Street"].ToString();
                    txtCity.Text = SqlConn.dr["City"].ToString();
                    txtProvince.Text = SqlConn.dr["State"].ToString();
                    txtContractNo.Text = SqlConn.dr["ContactNo"].ToString();
                    textBox2.Text = SqlConn.dr["Email"].ToString();
                    txtUsername.Text = SqlConn.dr["username"].ToString();
                    txtRole.Text = SqlConn.dr["Role"].ToString();
                    txtPassword.Text = SqlConn.dr["Password"].ToString();
                    txtConfirmPWD.Text = SqlConn.dr["Password"].ToString();

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
            lblProductNo.Text = "";
            txtLastname.Text = "";
            txtFirstname.Text = "";
            txtMI.Text = "";
            txtStreet.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            txtCity.Text = "";
            txtProvince.Text = "";
            txtContractNo.Text = "";
            txtUsername.Text = "";
            txtRole.Text = "";
            txtPassword.Text = "";
            txtConfirmPWD.Text = "";
        }

        private void AddEditStaff_Load(object sender, EventArgs e)
        {
            bool isadding = SqlConn.adding.Equals(true) & SqlConn.updating.Equals(false) & SqlConn.deleting.Equals(false);
            bool isupdating = SqlConn.adding.Equals(false) & SqlConn.updating.Equals(true) & SqlConn.deleting.Equals(false);
            bool isdeleting = SqlConn.adding.Equals(false) & SqlConn.updating.Equals(false) & SqlConn.deleting.Equals(true);
            if (isadding)
            {
                lblTitle.Text = "Adding New Staff";
                ClearFields();
                Auto();
            }
            else if(isupdating)
            {
                lblTitle.Text = "Updating Staff";
                LoadUpdateStaff();

            }
            else if(isdeleting)
            {
                lblTitle.Text = "Deleting Staff";
                LoadUpdateStaff();
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to delete this Staff record?", "Delete Staff", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DeleteStaff();
            }
            if (Application.OpenForms["Staff"] != null)
            {
                (Application.OpenForms["Staff"] as Staff).LoadStaffs("");
            }

            this.Close();
        }
    }
}
