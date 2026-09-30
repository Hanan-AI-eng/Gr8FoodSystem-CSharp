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
    public partial class CustomerFeedbackForm : Form
    {
        int customerID;
        int orderID;
        string connectionString = @"Data Source=localhost;Initial Catalog=ResturantDB;Integrated Security=True;TrustServerCertificate=True";

        public CustomerFeedbackForm(int customerID, int orderID)
        {
            InitializeComponent();
            this.customerID = customerID;
            this.orderID = orderID;
        }

        private void CustomerFeedbackForm_Load(object sender, EventArgs e)
        {
            lblOrderInfo.Text = "Feedback for Order #" + orderID;

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = @"INSERT INTO feedback 
(orderID, customerID, message, sent_date) 
VALUES (@oid, @cid, @message, GETDATE())";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@oid", orderID);
                cmd.Parameters.AddWithValue("@cid", customerID);
                cmd.Parameters.AddWithValue("@message", txtComment.Text);
                connection.Open();
                cmd.ExecuteNonQuery();

            }
            MessageBox.Show("Thank you for your feedback!");
            this.Close();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
