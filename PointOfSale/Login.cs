using Microsoft.VisualBasic;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PointOfSale
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            SqlConn.cmd.Dispose();
            SqlConn.conn.Close();
            Application.Exit();
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConn.sqL = "SELECT * FROM Staff WHERE Username = '" + txtusername.Text + "' AND Password = '" + txtPassword.Text + "'";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.dr = SqlConn.cmd.ExecuteReader();

                if (SqlConn.dr.Read() == true)
                {
                    Main m = new Main(SqlConn.dr["Username"].ToString(), SqlConn.dr["Role"].ToString(), SqlConn.dr["StaffID"].ToString());
                    m.Show();
                    this.Hide();

                    /*if (SqlConn.dr["Role"].ToString().ToUpper() == "ADMIN")
                    {
                        Main m = new Main(SqlConn.dr["Username"].ToString(), SqlConn.dr["Role"].ToString(), SqlConn.dr["StaffID"].ToString());
                        m.Show();
                        this.Hide();
                    }
                    else
                    {
                        //POS m = new POS(SqlConn.dr["Username"].ToString(), SqlConn.dr["Role"].ToString(), SqlConn.dr["StaffID"].ToString());
                        Main m = new Main(SqlConn.dr["Username"].ToString(), SqlConn.dr["Role"].ToString(), SqlConn.dr["StaffID"].ToString());
                        m.Show();
                        this.Hide();
                    }*/
                }
                else
                {
                    Interaction.MsgBox("Invalid Password. Please try again.", MsgBoxStyle.Exclamation, "Login");
                }

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

        private void Login_Load(object sender, EventArgs e)
        {
            SqlConn.GetData();
        }
    }
}
