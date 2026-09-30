using Gr8FoodSystem;
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
    public partial class LoginForm : Form

    {
        string connectionString = @"Data Source=localhost;Initial Catalog=ResturantDB;Integrated Security=True;TrustServerCertificate=True";

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string loginID = txtLoginID.Text.Trim();
            string password = txtPassword.Text.Trim();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql =@"SELECT userID, Role FROM user_account WHERE loginID = @loginID AND password = @password";

                SqlCommand cmd = new SqlCommand (sql, connection);
                cmd.Parameters.AddWithValue("@loginID", loginID);
                cmd.Parameters.AddWithValue("@password", password);
                connection.Open();
                SqlDataReader dataReader =cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    int userID = Convert.ToInt32(dataReader["userID"]);
                    string role = dataReader["Role"].ToString();

                    if (role == "Customer")
                    {
                        CustomerDashboard dashboard = new CustomerDashboard(userID);
                        dashboard.Show();
                        this.Hide();
                    }
                    else if (role == "Chef")
                    {
                        ChefDashboard chef = new ChefDashboard(userID);
                        chef.Show();
                        this.Hide();
                    }
                    else if (role == "Manager")
                    {
                        FormManagerHome managerForm = new FormManagerHome(userID);
                        managerForm.Show();
                        this.Hide();
                    }
                    else if (role == "Admin")
                    {
                        FormAdminHome adminForm = new FormAdminHome(userID);
                        adminForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("This role is not implemented yet.");
                    }


                }

                else
                { 
                    MessageBox.Show("Invalid login credentials. Please try again.");
                }

                dataReader.Close();
            }


        }


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panelLogin_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblSubtitle_Click(object sender, EventArgs e)
        {

        }
    }
}
