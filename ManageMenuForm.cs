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
    public partial class ManageMenuForm : Form
    {
        public ManageMenuForm()
        {
            InitializeComponent();
        }
        private void LoadCategory()
        {
            DBConnection db = new DBConnection();
            SqlConnection conn = db.GetConnection();

            string query = "SELECT categoryID, category_name FROM dbo.menu_category";

            SqlDataAdapter da = new SqlDataAdapter(query, conn);

            DataTable dt = new DataTable();

            da.Fill(dt);

            cmbCategory.DataSource = dt;
            cmbCategory.DisplayMember = "category_name";
            cmbCategory.ValueMember = "categoryID";
        }
        private void ManageMenuForm_Load(object sender, EventArgs e)
        {
            LoadMenu();
            LoadCategory();

        }

        

        private void LoadMenu()
        {
            DBConnection db = new DBConnection();
            SqlConnection conn = db.GetConnection();

            string query = "SELECT * FROM dbo.menu_item";

            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();

            da.Fill(dt);

            dgvMenu.DataSource = dt;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {// CHECK EMPTY FIELDS 
            if (txtName.Text == "" || txtPrice.Text == "")
            {
                MessageBox.Show("Please fill all fields!");
                return;
            }// CHECK COMBOX SELECTION
            if (cmbCategory.SelectedIndex == -1 || cmbAvailable.SelectedIndex == -1)
            {
                MessageBox.Show("Please select category and availability!");
                return;
            }
            
            DBConnection db = new DBConnection();
            SqlConnection conn = db.GetConnection();

            string query = @"INSERT INTO menu_item
    (categoryID, chefID, item_name, price, description, available)
    VALUES (@cat, @chef, @name, @price, @desc, @avail)";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@cat", Convert.ToInt32(cmbCategory.SelectedValue));
            cmd.Parameters.AddWithValue("@chef", 3);
            cmd.Parameters.AddWithValue("@name", txtName.Text);

            
            decimal price;
            if (!decimal.TryParse(txtPrice.Text, out price))
            {
                MessageBox.Show("Enter valid price!");
                return;
            }

            
            cmd.Parameters.AddWithValue("@price", price);

            cmd.Parameters.AddWithValue("@desc", txtDesc.Text);
            cmd.Parameters.AddWithValue("@avail", cmbAvailable.Text == "Available");

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            MessageBox.Show("Item Added!");

            LoadMenu();
        }

        private void dgvMenu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvMenu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvMenu.Rows[e.RowIndex];

                txtName.Text = row.Cells["item_name"].Value.ToString();
                txtPrice.Text = row.Cells["price"].Value.ToString();
                txtDesc.Text = row.Cells["description"].Value.ToString();
                cmbCategory.Text = row.Cells["categoryID"].Value.ToString();
                cmbAvailable.Text = (bool)row.Cells["available"].Value ? "Available" : "Not Available";
            }
        }

        

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DBConnection db = new DBConnection();
            SqlConnection conn = db.GetConnection();

            int id = Convert.ToInt32(dgvMenu.CurrentRow.Cells["itemID"].Value);

            string query = "UPDATE menu_item SET available = 0 WHERE itemID=@id";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            MessageBox.Show("Item marked as unavailable!");

            LoadMenu();
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtName.Clear();
            txtPrice.Clear();
            txtDesc.Clear();

            cmbCategory.SelectedIndex = -1;
            cmbAvailable.SelectedIndex = -1;
        }

        

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        

        private void btnEdit_Click(object sender, EventArgs e)
        {
            DBConnection db = new DBConnection();
            SqlConnection conn = db.GetConnection();

            int id = Convert.ToInt32(dgvMenu.CurrentRow.Cells["itemID"].Value);

            string query = @"UPDATE menu_item
                     SET categoryID=@cat,
                         item_name=@name,
                         price=@price,
                         description=@desc,
                         available=@avail
                     WHERE itemID=@id";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@cat", Convert.ToInt32(cmbCategory.SelectedValue));
            cmd.Parameters.AddWithValue("@name", txtName.Text);
            cmd.Parameters.AddWithValue("@price", decimal.Parse(txtPrice.Text));
            cmd.Parameters.AddWithValue("@desc", txtDesc.Text);
            cmd.Parameters.AddWithValue("@avail", cmbAvailable.Text == "Available");

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            MessageBox.Show("Updated!");

            LoadMenu();
        }
    }
}



