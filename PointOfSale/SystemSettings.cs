using Microsoft.VisualBasic;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PointOfSale
{
    public partial class SystemSettings : Form
    {
        bool isAddingVat;
        public SystemSettings()
        {
            InitializeComponent();
        }

        private void SystemSettings_Load(object sender, EventArgs e)
        {
            //Company Setting
            txtName.Tag = "";
            txtName.Text = "";
            txtAddress.Text = "";
            txtPhoneNo.Text = "";
            txtEmail.Text = "";
            txtWebsite.Text = "";
            txtTINNumber.Text = "";
            GetCompanyInfo();

            //VAT Setting
            txtPercent.Text = "";
            GetVATInfo();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region "Company"


        public void AddEditCompany(bool isAdding)
        {
            try
            {
                if (isAdding == true)
                {
                    SqlConn.sqL = "INSERT INTO CompanyInfo(Name, Address, PhoneNo, Email, Website, TIN) VALUES(@Name, @Address, @PhoneNo, @Email, @Website, @TINNumber)";
                }
                else
                {
                    SqlConn.sqL = "UPDATE CompanyInfo SET Name=@Name, Address=@Address, PhoneNo=@PhoneNO, Email=@Email, Website=@Website, TIN =@TINNumber WHERE CompanyId=@CompanyID ";
                }
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);

                SqlConn.cmd.Parameters.AddWithValue("@Name", txtName.Text);
                SqlConn.cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                SqlConn.cmd.Parameters.AddWithValue("@PhoneNo", txtPhoneNo.Text);
                SqlConn.cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                SqlConn.cmd.Parameters.AddWithValue("@Website", txtWebsite.Text);
                SqlConn.cmd.Parameters.AddWithValue("@TINNumber", txtTINNumber.Text);

                if (isAdding == false)
                {
                    SqlConn.cmd.Parameters.AddWithValue("@CompanyID", txtName.Tag);
                }

                int i = SqlConn.cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    if (isAdding == true)
                    {
                        Interaction.MsgBox("Company Information Successfully Added", MsgBoxStyle.Information, "Adding Company");
                    }
                    else
                    {
                        Interaction.MsgBox("Company Information Successfully Updated", MsgBoxStyle.Information, "Editing Company");
                    }
                }
                else
                {
                    Interaction.MsgBox("Saving Company Information Failed", MsgBoxStyle.Exclamation, "Failed");
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

        public void GetCompanyInfo()
        {
            try
            {
                SqlConn.sqL = "SELECT CompanyId, Name, Address, PhoneNo, Email, Website, TIN FROM CompanyInfo";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.dr = SqlConn.cmd.ExecuteReader();


                if (SqlConn.dr.Read())
                {
                    txtName.Tag = SqlConn.dr[0].ToString();
                    txtName.Text = SqlConn.dr[1].ToString();
                    txtAddress.Text = SqlConn.dr[2].ToString();
                    txtPhoneNo.Text = SqlConn.dr[3].ToString();
                    txtEmail.Text = SqlConn.dr[4].ToString();
                    txtWebsite.Text = SqlConn.dr[5].ToString();
                    txtTINNumber.Text = SqlConn.dr[6].ToString();
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

        private bool IsAdding()
        {
            bool ret = false;
            try
            {
                SqlConn.sqL = "SELECT * FROM CompanyInfo";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.dr = SqlConn.cmd.ExecuteReader();
                if (SqlConn.dr.Read() == true)
                {
                    ret = false;
                }
                else
                {
                    ret = true;
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
            return ret;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddEditCompany(IsAdding());
        }

        #endregion

        #region "VAT"

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtPercent.Text == "")
            {
                AddEditVAT(isAddingVat);
            }
            else
            {
                AddEditVAT(isAddingVat);
            }
        }

        public void AddEditVAT(bool isAdding)
        {
            try
            {

                if (isAdding == true)
                {
                    SqlConn.sqL = "INSERT INTO VATSettings(VatPercent) VALUES(@VatPercent)";
                }
                else
                {
                    SqlConn.sqL = "UPDATE VATSettings SET VatPercent=@VatPercent WHERE VATId=@VATID ";
                }
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);

                SqlConn.cmd.Parameters.AddWithValue("@VatPercent", txtPercent.Text);

                if (isAdding == false)
                {
                    SqlConn.cmd.Parameters.AddWithValue("@VATID", txtPercent.Tag);
                }

                int i = SqlConn.cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    if (isAdding == true)
                    {
                        Interaction.MsgBox("VAT Information Successfully Added", MsgBoxStyle.Information, "Adding VAT");
                    }
                    else
                    {
                        Interaction.MsgBox("VAT Information Successfully Updated", MsgBoxStyle.Information, "Editing VAT");
                    }

                }
                else
                {
                    Interaction.MsgBox("Saving VAT Information Failed", MsgBoxStyle.Exclamation, "Failed");
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

        public void GetVATInfo()
        {
            try
            {
                SqlConn.sqL = "SELECT VATId, VatPercent FROM VATSettings";
                SqlConn.ConnDB();
                SqlConn.cmd = new SqlCommand(SqlConn.sqL, SqlConn.conn);
                SqlConn.dr = SqlConn.cmd.ExecuteReader();


                if (SqlConn.dr.Read())
                {
                    txtPercent.Tag = SqlConn.dr[0];
                    txtPercent.Text = SqlConn.dr[1].ToString();
                    isAddingVat = false;
                }
                else
                {
                    isAddingVat = true;
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


        #endregion
    }
}
