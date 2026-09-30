using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gr8FoodSystem
{
    public partial class CustomerWalletForm : Form
    {
        string connectionString =
            @"Data Source=localhost;Initial Catalog=ResturantDB;Integrated Security=True;TrustServerCertificate=True";

        int userID;

        public CustomerWalletForm(int id)
        {
            InitializeComponent();
            userID = id;
        }

        private void CustomerWalletForm_Load(object sender, EventArgs e)
        {
            LoadBalance();
            LoadCustomerName();
        }

        private void LoadBalance()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql =
                    "SELECT ewallet_balance FROM user_account WHERE userID=@id";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@id", userID);

                conn.Open();

                object result = cmd.ExecuteScalar();

                txtBalance.Text = result.ToString();
            }
        }
        private void LoadCustomerName()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql =
                    @"SELECT first_name, last_name
              FROM user_account
              WHERE userID = @id";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@id", userID);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    lblCustomerName.Text =
                        "Customer: " +
                        reader["first_name"].ToString() +
                        " " +
                        reader["last_name"].ToString();
                }
            }
        }
        public CustomerWalletForm()
        {
            InitializeComponent();
        }

        

        private void btnTopUp_Click(object sender, EventArgs e)
        {
            decimal amount;

            if (!decimal.TryParse(txtAmount.Text, out amount) || amount <= 0)
            {
                MessageBox.Show("Enter valid amount.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql =
                    @"UPDATE user_account
                      SET ewallet_balance = ewallet_balance + @amount
                      WHERE userID=@id";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@id", userID);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Wallet topped up successfully.");

                LoadBalance();

                txtAmount.Clear();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
