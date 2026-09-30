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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;


namespace Gr8FoodSystem
{
    public partial class BrowseMenuForm : Form
    {
        string connectionString = @"Data Source=localhost;Initial Catalog=ResturantDB;Integrated Security=True;TrustServerCertificate=True";
        List<CartItem> cart = new List<CartItem>();

        int customerID = -1;

        public BrowseMenuForm()
        {
            InitializeComponent();
            
        }
        public BrowseMenuForm(int customerID) : this()
        {
            this.customerID= customerID;
        }

        private void BrowseMenuForm_Load(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT categoryID, category_name FROM menu_category";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sql,connection);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                DataRow row = dataTable.NewRow(); // to add all category options 
                row["categoryID"] = 0;
                row["category_name"] = "All Categories";
                dataTable.Rows.InsertAt(row, 0);

                cboCategory.DataSource = dataTable;
                cboCategory.DisplayMember = "category_name";
                cboCategory.ValueMember = "categoryID";

                LoadMenuItems(0); // Load all menu items initially
                SetupCartGrid();
            }
        }
        private void LoadMenuItems(int categoryID) 
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                dgvMenu.AutoGenerateColumns = true;
                dgvMenu.DataSource = null;
                string sql; 
                if (categoryID ==0)  // all the Category display
                {
                    sql = @"SELECT itemID, item_name AS Item, price AS Price, description AS Description FROM menu_item WHERE available = 1";
                }
                else //filter by category
                {
                    sql = @"SELECT itemID,item_name AS Item,price AS Price, description AS Description FROM menu_item WHERE available = 1 AND categoryID = @catID";
                }
                SqlDataAdapter dataAdapter =new SqlDataAdapter(sql,connection);

                if (categoryID!= 0) 
                
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@catID", categoryID);
                
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                dgvMenu.DataSource = dataTable;
                if (dgvMenu.Columns.Contains("itemID"))// hide the itemID column
                {
                    dgvMenu.Columns["itemID"].Visible = false;
                }

            }
       
        }

        private void dgvMenu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void cboCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboCategory.SelectedValue is int selectedCategory)
            { 
                LoadMenuItems(selectedCategory);
            }
        }
        class CartItem
        { 
            public int ItemID { get; set; }
            public string ItemName { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            public decimal Subtotal
            {
                get { return Price * Quantity; }

            }

        }
        private void SetupCartGrid()
        {
            dgvCart.AutoGenerateColumns = false;
            dgvCart.Columns.Clear();

            dgvCart.Columns.Add(new DataGridViewTextBoxColumn()
            {
                HeaderText = "Item",
                DataPropertyName = "ItemName"
            });

            dgvCart.Columns.Add(new DataGridViewTextBoxColumn()
            {
                HeaderText = "Price",
                DataPropertyName = "Price",
            });

            dgvCart.Columns.Add(new DataGridViewTextBoxColumn()
            {
                HeaderText = "Quantity",
                DataPropertyName = "Quantity",
            });
            dgvCart.Columns.Add(new DataGridViewTextBoxColumn()
            {
                HeaderText = "Subtotal",
                DataPropertyName = "Subtotal",
            });
           
        }

        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            
            if (dgvMenu.SelectedRows.Count == 0) 
            { 
                MessageBox.Show("Please select an item to add to cart.");
                return;
            }
            int itemID = Convert.ToInt32(dgvMenu.SelectedRows[0].Cells["itemID"].Value); // get the itemID from the hidden column
            string itemName = dgvMenu.SelectedRows[0].Cells["Item"].Value.ToString();
            decimal price = Convert.ToDecimal(dgvMenu.SelectedRows[0].Cells["Price"].Value);
            int quantity = (int)nudQuantity.Value;

            CartItem existingItem = cart.FirstOrDefault(i => i.ItemID == itemID); //check if item already exists in cart
            if (existingItem != null)
            {
                existingItem.Quantity += quantity; // if the item already exists, update the quantity
            }
            else
            {

                CartItem item = new CartItem() //new item to add to cart
                {
                    ItemID = itemID,
                    ItemName = itemName,
                    Price = price,
                    Quantity = quantity
                };
                cart.Add(item);
            }

            dgvCart.DataSource = null;
            dgvCart.DataSource = cart;
            UpdateTotal();

        }
        private void UpdateTotal()
        {
            decimal total = cart.Sum(i => i.Subtotal);
            lblTotal.Text = "Total: RM " + total.ToString("0.00");
        }

        private void btnConfirmOrder_Click(object sender, EventArgs e) // wallet validation before confirm order
        {
            btnConfirmOrder.Enabled = false;

            //customer session check
            if (customerID <= 0)
            {
                MessageBox.Show("Invalid customer session.");
                btnConfirmOrder.Enabled = true;
                return;
            }

            //cart check
            if (cart.Count == 0)
            {
                MessageBox.Show("Your cart is empty.");
                btnConfirmOrder.Enabled = true;
                return;
            }

            decimal totalAmount = cart.Sum(i => i.Subtotal);
            Order order = new Order()
            {
                CustomerID = customerID,
                TotalCost = totalAmount,
                Status = "Pending"
            };

            //wallet validation
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT ewallet_balance FROM user_account WHERE userID = @id";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@id", customerID);
                connection.Open();

                decimal walletBalance = Convert.ToDecimal(cmd.ExecuteScalar());
                if (walletBalance < totalAmount)
                {
                    MessageBox.Show("Insufficient wallet balance. Please top up your wallet.");
                    btnConfirmOrder.Enabled = true;
                    return;
                }
            }

            int orderID;

            //order_request
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = @"INSERT INTO order_request (customerID, status, total_cost)
                       OUTPUT INSERTED.orderID
                       VALUES (@customerID, 'Pending', @totalCost)";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@customerID", order.CustomerID);
                cmd.Parameters.AddWithValue("@totalCost", order.TotalCost);
                connection.Open();

                orderID = (int)cmd.ExecuteScalar();
            }

            //order_item
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = @"INSERT INTO order_item (orderID, itemID, quantity, subtotal)
                       VALUES (@orderID, @itemID, @quantity, @subtotal)";

                SqlCommand cmd = new SqlCommand(sql, connection);
                connection.Open();

                foreach (CartItem item in cart)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@orderID", orderID);
                    cmd.Parameters.AddWithValue("@itemID", item.ItemID);
                    cmd.Parameters.AddWithValue("@quantity", item.Quantity);
                    cmd.Parameters.AddWithValue("@subtotal", item.Subtotal);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Order confirmed! Your order ID is: " + orderID);

            //ewallet deduction balance
            using (SqlConnection connection = new SqlConnection(connectionString)) 
            {
                string sql = @"UPDATE user_account SET ewallet_balance = ewallet_balance - @amount WHERE userID = @id";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@amount", totalAmount);
                cmd.Parameters.AddWithValue("@id", customerID);
                connection.Open();
                cmd.ExecuteNonQuery();
            }

            cart.Clear();
            dgvCart.DataSource = null;
            
            UpdateTotal();

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblCategory_Click(object sender, EventArgs e)
        {

        }

        private void nudQuantity_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
