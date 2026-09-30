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
    public partial class CustomerDashboard : Form

    {
        private int userID;
        int customerID;   // store logged-in customer ID
        string connectionString = @"Data Source=localhost;Initial Catalog=ResturantDB;Integrated Security=True;TrustServerCertificate=True";

        public CustomerDashboard(int userID)
        {

            InitializeComponent();
            this.userID = userID;
            customerID = userID;
            
        }

        private void CustomerDashboard_Load(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString)) 
            {
                string sql = "Select ewallet_balance from user_account Where userID =@id";

                SqlCommand cmd =new SqlCommand(sql,connection);
                cmd.Parameters.AddWithValue("@id", customerID);

                connection.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    decimal balance =Convert.ToDecimal(result);
                    lblWallet.Text = "Wallet Balance:RM" + balance.ToString("0.00");
                }
            }
        }

        private void btnBrowseMenu_Click(object sender, EventArgs e)
        {
            BrowseMenuForm form = new BrowseMenuForm(customerID);
            form.Show();
            form.FormClosed += (s, args) => CustomerDashboard_Load(null, null); // Refresh the dashboard after order confirmation
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            login.Show();
            this.Close();
        }

        private void btnViewOrders_Click(object sender, EventArgs e)
        {
            CustomerOrdersForm form = new CustomerOrdersForm(customerID);

            form.FormClosed += (s, args) => RefreshDashboard();

            form.ShowDialog();

        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        private void lblWallet_Click(object sender, EventArgs e)
        {

        }

        private void btnUpdateProfile_Click(object sender, EventArgs e)
        {
            FormUpdateProfile form =
    new FormUpdateProfile(customerID);

            form.ShowDialog();
        }
        private void RefreshDashboard() // Method to refresh wallet balance 
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "Select ewallet_balance FROM user_account WHERE userID =@id";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@id", customerID);
                connection.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    decimal balance = Convert.ToDecimal(result);
                    lblWallet.Text = "Wallet Balance:RM" + balance.ToString("0.00");
                }
            }
        }

        private void btnWallet_Click(object sender, EventArgs e)
        {
            CustomerWalletForm wallet =
        new CustomerWalletForm(userID);

            wallet.ShowDialog();
            RefreshDashboard();
        }
    }
}
