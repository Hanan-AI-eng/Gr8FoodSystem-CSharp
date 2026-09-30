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
    public partial class CustomerOrdersForm : Form
    {
        int customerID;   // store logged-in customer ID
        string customerName;
        string connectionString = @"Data Source=localhost;Initial Catalog=ResturantDB;Integrated Security=True;TrustServerCertificate=True";
        public CustomerOrdersForm(int customerID)
        {
            InitializeComponent();
            this.customerID = customerID;
            
        }

        private void CustomerOrdersForm_Load(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql =
                    "SELECT first_name FROM user_account WHERE userID=@id";

                SqlCommand cmd = new SqlCommand(sql, connection);

                cmd.Parameters.AddWithValue("@id", customerID);

                connection.Open();

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    customerName = result.ToString();
                }
            }
            LoadOrders();


        }

        private void LoadOrders()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = @"SELECT orderID, order_date, status, total_cost FROM order_request WHERE customerID=@id ORDER BY order_date DESC";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@id", customerID);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dgvOrders.DataSource = dataTable;
            }
        }


       

        

        private void btnCancelOrder_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count ==0 )
            {
                MessageBox.Show("Please select an order to cancel.");
                return;
            }
            string status = dgvOrders.SelectedRows[0].Cells["status"].Value.ToString();
            if (status !="Pending")
            {
                MessageBox.Show("Only pending orders can be cancelled");
                return;
            }
            int orderID = Convert.ToInt32(dgvOrders.SelectedRows[0].Cells["orderID"].Value);
            decimal amount = Convert.ToDecimal(dgvOrders.SelectedRows[0].Cells["total_cost"].Value);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cancelCmd = new SqlCommand("UPDATE order_request SET status='Cancelled' WHERE orderID=@oid", connection); // cancel the order
                cancelCmd.Parameters.AddWithValue("@oid", orderID);
                cancelCmd.ExecuteNonQuery();

                SqlCommand refundCmd = new SqlCommand("UPDATE user_account SET ewallet_balance = ewallet_balance + @amt WHERE userID=@cid", connection); // refund the amount to wallet
                refundCmd.Parameters.AddWithValue("@amt", amount);
                refundCmd.Parameters.AddWithValue("@cid", customerID);
                refundCmd.ExecuteNonQuery();


            }
            MessageBox.Show("Order cancelled and amount refunded to wallet.");
            LoadOrders(); // refresh the order list
        }

        private void btnFeedback_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an order to provide feedback.");//1. Ensure an order is selected
                return;
            }
            string status = dgvOrders.SelectedRows[0].Cells["status"].Value.ToString();

            if (!status.Equals("completed", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Feedback can only be provided for completed orders.");
                return;
            }
            int orderID =Convert.ToInt32(dgvOrders.SelectedRows[0].Cells["orderID"].Value);//3. Get the order ID of the selected order
            CustomerFeedbackForm feedbackForm = new CustomerFeedbackForm(customerID, orderID); //4. Open the feedback form, passing the customer ID and order ID
            feedbackForm.ShowDialog();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        

        private void dgvOrders_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dgvOrders_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int orderID = Convert.ToInt32(
                    dgvOrders.Rows[e.RowIndex].Cells["orderID"].Value);

                string status =
                    dgvOrders.Rows[e.RowIndex].Cells["status"].Value.ToString();

                decimal total =
                    Convert.ToDecimal(
                    dgvOrders.Rows[e.RowIndex].Cells["total_cost"].Value);

                lblSelectedOrder.Text =
                    "Customer ID: " + customerID +
                    " | Order #" + orderID +
                    " | Status: " + status +
                    " | RM " + total.ToString("0.00");
            }
        }
    }
}
