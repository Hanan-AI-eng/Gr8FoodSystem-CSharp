using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Gr8FoodSystem
{
    public partial class FormViewFeedback : Form
    {
        string connectionString = @"Data Source=localhost;Initial Catalog=ResturantDB;Integrated Security=True;TrustServerCertificate=True";
        private int _managerID;

        public FormViewFeedback(int managerID)
        {
            InitializeComponent();
            _managerID = managerID;
            LoadAllFeedback();
            ClearFields();
        }

        private void FormViewFeedback_Load(object sender, EventArgs e)
        {
           
        }

        private void LoadAllFeedback()
        {
            try
            {
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();

                string sql = @"
                    SELECT
                        f.feedbackID,
                        f.orderID,
                        c.first_name + ' ' + c.last_name   AS customer_name,
                        f.message,
                        f.manager_reply,
                        CONVERT(VARCHAR, f.sent_date, 103)  AS sent_date,
                        CASE
                            WHEN f.manager_reply IS NULL OR f.manager_reply = ''
                            THEN 'Pending Reply'
                            ELSE 'Replied'
                        END AS reply_status
                    FROM feedback f
                    JOIN user_account c ON f.customerID = c.userID
                    ORDER BY f.sent_date DESC";

                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                dgvFeedback.DataSource = dt;
                dgvFeedback.ReadOnly = true;
                dgvFeedback.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvFeedback.RowHeadersVisible = false;
                dgvFeedback.AllowUserToAddRows = false;

                if (dgvFeedback.Columns.Count > 0)
                {
                    dgvFeedback.Columns[0].HeaderText = "ID";
                }

                if (dgvFeedback.Columns.Count > 1)
                {
                    dgvFeedback.Columns[1].HeaderText = "Order ID";
                }

                if (dgvFeedback.Columns.Count > 2)
                {
                    dgvFeedback.Columns[2].HeaderText = "Customer";
                }

                if (dgvFeedback.Columns.Count > 3)
                {
                    dgvFeedback.Columns[3].HeaderText = "Message";
                }

                if (dgvFeedback.Columns.Count > 4)
                {
                    dgvFeedback.Columns[4].HeaderText = "Manager Reply";
                }

                if (dgvFeedback.Columns.Count > 5)
                {
                    dgvFeedback.Columns[5].HeaderText = "Date";
                }

                if (dgvFeedback.Columns.Count > 6)
                {
                    dgvFeedback.Columns[6].HeaderText = "Status";
                }
                foreach (DataGridViewRow row in dgvFeedback.Rows)
                {
                    if (row.Cells[6].Value != null)
                    {
                        string status = row.Cells[6].Value.ToString();

                        if (status == "Replied")
                        {
                            row.DefaultCellStyle.BackColor = Color.LightGreen;
                            row.DefaultCellStyle.ForeColor = Color.Black;
                        }
                        else
                        {
                            row.DefaultCellStyle.BackColor = Color.LightYellow;
                            row.DefaultCellStyle.ForeColor = Color.Black;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading feedback:\n" + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnReply_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFeedbackID.Text))
            {
                MessageBox.Show("Please click on a feedback row first.",
                    "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(rtbReply.Text))
            {
                MessageBox.Show("Please type your reply before sending.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                rtbReply.Focus();
                return;
            }

            try
            {
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();

                string sql = @"UPDATE feedback
                               SET manager_reply = @reply
                               WHERE feedbackID  = @feedbackID";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@reply", rtbReply.Text.Trim());
                cmd.Parameters.AddWithValue("@feedbackID", Convert.ToInt32(txtFeedbackID.Text));
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Reply sent successfully!",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearFields();
                LoadAllFeedback();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending reply:\n" + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtFeedbackID.Clear();
            txtCustomer.Clear();
            txtOrderID.Clear();
            txtSentDate.Clear();
            txtStatus.Clear();
            rtbMessage.Clear();
            rtbReply.Clear();
            btnReply.Text = "Send Reply";
            dgvFeedback.ClearSelection();
        }



        

        private void dgvFeedback_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvFeedback.Rows[e.RowIndex];

                txtFeedbackID.Text = row.Cells[0].Value.ToString();
                txtOrderID.Text = row.Cells[1].Value.ToString();
                txtCustomer.Text = row.Cells[2].Value.ToString();
                rtbMessage.Text = row.Cells[3].Value.ToString();

                rtbReply.Text = row.Cells[4].Value?.ToString();

                txtSentDate.Text = row.Cells[5].Value.ToString();
                txtStatus.Text = row.Cells[6].Value.ToString();
            }
        }
        private void dgvFeedback_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}