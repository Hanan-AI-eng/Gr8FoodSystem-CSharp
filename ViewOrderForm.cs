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
    public partial class ViewOrderForm : Form
    {
        public ViewOrderForm()
        {
            InitializeComponent();
        }

        private void ViewOrderForm_Load(object sender, EventArgs e)
        {
            LoadOrders();
        }
        private void dgvOrders_CellClick(object sender, DataGridViewCellEventArgs e)

        {

            if (e.RowIndex >= 0)
            {
                int orderID = Convert.ToInt32(
                    dgvOrders.Rows[e.RowIndex].Cells["orderID"].Value);

                string status =
                    dgvOrders.Rows[e.RowIndex].Cells["status"].Value.ToString();

                lblSelectedOrder.Text =
                    "Selected Order: #" + orderID +
                    " | Status: " + status;
            }

        }



        private void LoadOrders()
        {
            DBConnection db = new DBConnection();
            SqlConnection conn = db.GetConnection();

            string query = @"SELECT 
                o.orderID,
                u.first_name + ' ' + u.last_name AS Customer,
                o.status,
                o.order_date,
                o.total_cost
            FROM order_request o
            JOIN user_account u ON o.customerID = u.userID";

            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();

            da.Fill(dt);
            dgvOrders.DataSource = dt;
        }

       

        private void dgvOrders_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            if (dgvOrders.CurrentRow == null)

            {

                MessageBox.Show("Please select an order!");

                return;

            }

            if (cmbStatus.SelectedIndex == -1)

            {

                MessageBox.Show("Please select a status!");

                return;

            }

            int orderID = Convert.ToInt32(dgvOrders.CurrentRow.Cells["orderID"].Value);

            string status = cmbStatus.Text;

            DBConnection db = new DBConnection();

            SqlConnection conn = db.GetConnection();

            string query = "UPDATE order_request SET status=@status WHERE orderID=@id";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@status", status);

            cmd.Parameters.AddWithValue("@id", orderID);

            conn.Open();

            cmd.ExecuteNonQuery();

            conn.Close();

            MessageBox.Show("Order updated!");

            LoadOrders(); // refresh
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
    }

