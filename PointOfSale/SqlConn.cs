using System.IO;
using System.Data;
using System.Windows.Forms;
//using Microsoft.Data.SqlClient;
using System.Data.SqlClient;
using Microsoft.VisualBasic;

namespace PointOfSale
{
    static class SqlConn
    {
		public static string ServerSQL;
		public static string PortSQL;
		public static string UserNameSQL;
		public static string PwdSQL;
		public static string DBNameSQL;
		public static string sqL;
		public static DataSet ds = new DataSet();
		public static SqlCommand cmd;
		public static SqlDataReader dr;

		public static bool adding;
		public static bool updating;
		public static bool deleting;

		public static string strSearch = "";

		public static SqlDataAdapter da = new SqlDataAdapter();

		public static SqlConnection conn = new SqlConnection();
		public static void GetData()
		{
			string AppName = Application.ProductName;

			try
			{
				DBNameSQL = Interaction.GetSetting(AppName, "DBSection", "DB_Name", "temp");
				ServerSQL = Interaction.GetSetting(AppName, "DBSection", "DB_IP", "temp");
				PortSQL = Interaction.GetSetting(AppName, "DBSection", "DB_Port", "temp");
				UserNameSQL = Interaction.GetSetting(AppName, "DBSection", "DB_User", "temp");
				PwdSQL = Interaction.GetSetting(AppName, "DBSection", "DB_Password", "temp");
			}
			catch
			{
				Interaction.MsgBox("System registry was not established, you can set/save " + "these settings by pressing F1", MsgBoxStyle.Information);
			}

		}

		public static void ConnDB()
		{
			conn.Close();
			try
			{
				conn.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Store.mdf;Integrated Security=True";

				conn.Open();
			}
			catch
			{
				Interaction.MsgBox("The system failed to establish a connection", MsgBoxStyle.Information, "Database Settings");
			}

		}


		public static void DisconnMy()
		{
			conn.Close();
			conn.Dispose();

		}

		public static void SaveData()
		{
			string AppName = Application.ProductName;

			Interaction.SaveSetting(AppName, "DBSection", "DB_Name", DBNameSQL);
			Interaction.SaveSetting(AppName, "DBSection", "DB_IP", ServerSQL);
			Interaction.SaveSetting(AppName, "DBSection", "DB_Port", PortSQL);
			Interaction.SaveSetting(AppName, "DBSection", "DB_User", UserNameSQL);
			Interaction.SaveSetting(AppName, "DBSection", "DB_Password", PwdSQL);

			Interaction.MsgBox("Database connection settings are saved.", MsgBoxStyle.Information);
		}
	}
}
