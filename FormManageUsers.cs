using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Gr8FoodSystem
{
    public partial class FormManageUsers : Form
    {
        // ── Direct SQL connection string — change . to your server name if needed
     
        private string connStr = "Data Source=.;Initial Catalog=ResturantDB;Integrated Security=True";
        private GroupBox groupBox1;
        private Button btnClear;
        private Button btnDelete;
        private Button btnUpdate;
        private Button btnAdd;
        private ComboBox cboRole;
        private TextBox txtPhone;
        private TextBox txtEmail;
        private TextBox txtLastName;
        private TextBox txtFirstName;
        private TextBox txtPassword;
        private TextBox txtLoginID;
        private TextBox txtUserID;
        
        private System.ComponentModel.IContainer components;
        private DataGridView dgvUsers;
        private Label lblRole;
        private Label lblPhone;
        private Label lblEmail;
        private Label lblLastName;
        private Label lblFirstName;
        private Label lblPassword;
        private Label lblLoginID;
        private Label lblUserID;
        private Panel panel1;
        private Label lblTitle;
        private Button btnBack;
        private int _adminID;

        public FormManageUsers(int adminID)
        {
            InitializeComponent();
            _adminID = adminID;

        }

        // ════════════════════════════════════════════════════════════════════
        // FORM LOAD — fill role dropdown + load all users into grid
        // ════════════════════════════════════════════════════════════════════
        private void FormManageUsers_Load(object sender, EventArgs e)
        {
            
            // Fill role combo box with the 4 valid roles
            cboRole.Items.Clear();
            cboRole.Items.Add("Admin");
            cboRole.Items.Add("Manager");
            cboRole.Items.Add("Chef");
            cboRole.Items.Add("Customer");
            cboRole.SelectedIndex = 3; // default to Customer

            LoadAllUsers();
        }

        // ════════════════════════════════════════════════════════════════════
        // LOAD ALL USERS into the DataGridView
        // ════════════════════════════════════════════════════════════════════
        private void LoadAllUsers()
        {
            try
            {
                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();

                string sql = @"SELECT userID, loginID, first_name, last_name, 
                                      email, phone_no, Role, ewallet_balance 
                               FROM user_account 
                               ORDER BY Role, first_name";

                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();

                dgvUsers.AutoGenerateColumns = true;
                dgvUsers.DataSource = null;
                dgvUsers.DataSource = dt;

              

                dgvUsers.ReadOnly        = true;
                dgvUsers.SelectionMode   = DataGridViewSelectionMode.FullRowSelect;
                dgvUsers.RowHeadersVisible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message,
                                "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ════════════════════════════════════════════════════════════════════
        // CLICK A ROW — fills the text boxes with that user's data
        // ════════════════════════════════════════════════════════════════════
        private void dgvUsers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null) return;

            DataGridViewRow row = dgvUsers.CurrentRow;

            txtUserID.Text    = row.Cells["userID"].Value.ToString();
            txtLoginID.Text   = row.Cells["loginID"].Value.ToString();
            txtFirstName.Text = row.Cells["first_name"].Value.ToString();
            txtLastName.Text  = row.Cells["last_name"].Value.ToString();
            txtEmail.Text     = row.Cells["email"].Value.ToString();
            txtPhone.Text     = row.Cells["phone_no"].Value.ToString();
            cboRole.Text      = row.Cells["Role"].Value.ToString();
            txtPassword.Clear(); // never show existing password
        }

        // ════════════════════════════════════════════════════════════════════
        // ADD USER button
        // ════════════════════════════════════════════════════════════════════
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Validate all fields
            if (!ValidateFields(requirePassword: true)) return;

            try
            {
                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();

                // Check if loginID already exists
                string checkSql = "SELECT COUNT(*) FROM user_account WHERE loginID = @loginID";
                SqlCommand checkCmd = new SqlCommand(checkSql, conn);
                checkCmd.Parameters.AddWithValue("@loginID", txtLoginID.Text.Trim());
                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    MessageBox.Show("This Login ID already exists. Please choose a different one.",
                                    "Duplicate Login ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    conn.Close();
                    return;
                }

                // Insert the new user
                string sql = @"INSERT INTO user_account
(loginID, password, first_name, last_name, email, phone_no, Role, ewallet_balance)
VALUES
(@loginID, @password, @firstName, @lastName, @email, @phone, @role, 0.00)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@loginID",   txtLoginID.Text.Trim());
                cmd.Parameters.AddWithValue("@password",  txtPassword.Text.Trim());
                cmd.Parameters.AddWithValue("@firstName", txtFirstName.Text.Trim());
                cmd.Parameters.AddWithValue("@lastName",  txtLastName.Text.Trim());
                cmd.Parameters.AddWithValue("@email",     txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@phone",     txtPhone.Text.Trim());
                cmd.Parameters.AddWithValue("@role",      cboRole.SelectedItem.ToString());

                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("User added successfully!",
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
                LoadAllUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding user: " + ex.Message,
                                "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ════════════════════════════════════════════════════════════════════
        // UPDATE USER button
        // ════════════════════════════════════════════════════════════════════
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Make sure a user is selected from the grid
            if (string.IsNullOrEmpty(txtUserID.Text))
            {
                MessageBox.Show("Please click on a user in the list first.",
                                "No User Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateFields(requirePassword: false)) return;

            try
            {
                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();
                string sql = @"UPDATE user_account 
               SET loginID   = @loginID,
                   password   = @password,
                   first_name = @firstName,
                   last_name  = @lastName,
                   email      = @email,
                   phone_no   = @phone,
                   Role       = @role
               WHERE userID = @userID";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@loginID", txtLoginID.Text.Trim());
                cmd.Parameters.AddWithValue("@password", txtPassword.Text.Trim());
                cmd.Parameters.AddWithValue("@firstName", txtFirstName.Text.Trim());
                cmd.Parameters.AddWithValue("@lastName",  txtLastName.Text.Trim());
                cmd.Parameters.AddWithValue("@email",     txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@phone",     txtPhone.Text.Trim());
                cmd.Parameters.AddWithValue("@role",      cboRole.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@userID",    Convert.ToInt32(txtUserID.Text));

                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("User updated successfully!",
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
                LoadAllUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating user: " + ex.Message,
                                "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ════════════════════════════════════════════════════════════════════
        // DELETE USER button
        // ════════════════════════════════════════════════════════════════════
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Make sure a user is selected
            if (string.IsNullOrEmpty(txtUserID.Text))
            {
                MessageBox.Show("Please click on a user in the list first.",
                                "No User Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedUserID = Convert.ToInt32(txtUserID.Text);

            // Prevent admin from deleting their own account
            if (selectedUserID == _adminID)
            {
                MessageBox.Show("You cannot delete your own admin account.",
                                "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Ask for confirmation before deleting
            string userName = txtFirstName.Text + " " + txtLastName.Text;
            DialogResult confirm = MessageBox.Show(
                $"Are you sure you want to delete '{userName}'?\nThis cannot be undone.",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();

                string sql = "DELETE FROM user_account WHERE userID = @userID";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userID", selectedUserID);
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("User deleted successfully.",
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
                LoadAllUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting user: " + ex.Message +
                                "\n\nNote: You cannot delete a user who has existing orders or transactions.",
                                "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ════════════════════════════════════════════════════════════════════
        // CLEAR button — empties all text boxes
        // ════════════════════════════════════════════════════════════════════
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        // ════════════════════════════════════════════════════════════════════
        // HELPER: clears all input fields
        // ════════════════════════════════════════════════════════════════════
        private void ClearFields()
        {
            txtUserID.Clear();
            txtLoginID.Clear();
            txtPassword.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            cboRole.SelectedIndex = 3;
            dgvUsers.ClearSelection();
            dgvUsers.CurrentCell = null;
        }

        // ════════════════════════════════════════════════════════════════════
        // HELPER: validates input fields before add/update
        // requirePassword = true when adding (new users need a password)
        // requirePassword = false when updating (password field ignored)
        // ════════════════════════════════════════════════════════════════════
        private bool ValidateFields(bool requirePassword)
        {
            if (string.IsNullOrWhiteSpace(txtLoginID.Text))
            {
                MessageBox.Show("Login ID cannot be empty.",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLoginID.Focus();
                return false;
            }

            if (requirePassword && string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Password cannot be empty.",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                MessageBox.Show("First Name cannot be empty.",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFirstName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("Last Name cannot be empty.",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLastName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text) ||
                !txtEmail.Text.Contains("@") || !txtEmail.Text.Contains("."))
            {
                MessageBox.Show("Please enter a valid email address (must contain @ and .).",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Phone number cannot be empty.",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return false;
            }

            if (cboRole.SelectedItem == null)
            {
                MessageBox.Show("Please select a role.",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboRole.Focus();
                return false;
            }

            return true;
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnBack = new System.Windows.Forms.Button();
            this.lblRole = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblLastName = new System.Windows.Forms.Label();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblLoginID = new System.Windows.Forms.Label();
            this.lblUserID = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.cboRole = new System.Windows.Forms.ComboBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtLoginID = new System.Windows.Forms.TextBox();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnBack);
            this.groupBox1.Controls.Add(this.lblRole);
            this.groupBox1.Controls.Add(this.lblPhone);
            this.groupBox1.Controls.Add(this.lblEmail);
            this.groupBox1.Controls.Add(this.lblLastName);
            this.groupBox1.Controls.Add(this.lblFirstName);
            this.groupBox1.Controls.Add(this.lblPassword);
            this.groupBox1.Controls.Add(this.lblLoginID);
            this.groupBox1.Controls.Add(this.lblUserID);
            this.groupBox1.Controls.Add(this.btnClear);
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.btnUpdate);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.cboRole);
            this.groupBox1.Controls.Add(this.txtPhone);
            this.groupBox1.Controls.Add(this.txtEmail);
            this.groupBox1.Controls.Add(this.txtLastName);
            this.groupBox1.Controls.Add(this.txtFirstName);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.txtLoginID);
            this.groupBox1.Controls.Add(this.txtUserID);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(26, 180);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(733, 213);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "User Information";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Location = new System.Drawing.Point(572, 157);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(108, 45);
            this.btnBack.TabIndex = 20;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // lblRole
            // 
            this.lblRole.AutoSize = true;
            this.lblRole.Location = new System.Drawing.Point(487, 70);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(75, 36);
            this.lblRole.TabIndex = 19;
            this.lblRole.Text = "Role:";
            // 
            // lblPhone
            // 
            this.lblPhone.AutoSize = true;
            this.lblPhone.Location = new System.Drawing.Point(317, 70);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(100, 36);
            this.lblPhone.TabIndex = 18;
            this.lblPhone.Text = "Phone:";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(6, 70);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(88, 36);
            this.lblEmail.TabIndex = 17;
            this.lblEmail.Text = "Email:";
            // 
            // lblLastName
            // 
            this.lblLastName.AutoSize = true;
            this.lblLastName.Location = new System.Drawing.Point(608, 26);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(150, 36);
            this.lblLastName.TabIndex = 16;
            this.lblLastName.Text = "Last Name:";
            // 
            // lblFirstName
            // 
            this.lblFirstName.AutoSize = true;
            this.lblFirstName.Location = new System.Drawing.Point(487, 26);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(154, 36);
            this.lblFirstName.TabIndex = 15;
            this.lblFirstName.Text = "First Name:";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(317, 26);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(136, 36);
            this.lblPassword.TabIndex = 14;
            this.lblPassword.Text = "Password:";
            // 
            // lblLoginID
            // 
            this.lblLoginID.AutoSize = true;
            this.lblLoginID.Location = new System.Drawing.Point(120, 26);
            this.lblLoginID.Name = "lblLoginID";
            this.lblLoginID.Size = new System.Drawing.Size(124, 36);
            this.lblLoginID.TabIndex = 13;
            this.lblLoginID.Text = "Login ID:";
            // 
            // lblUserID
            // 
            this.lblUserID.AutoSize = true;
            this.lblUserID.Location = new System.Drawing.Point(6, 26);
            this.lblUserID.Name = "lblUserID";
            this.lblUserID.Size = new System.Drawing.Size(110, 36);
            this.lblUserID.TabIndex = 12;
            this.lblUserID.Text = "User ID:";
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(418, 157);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(108, 45);
            this.btnClear.TabIndex = 11;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(279, 157);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(108, 45);
            this.btnDelete.TabIndex = 10;
            this.btnDelete.Text = "Delete User";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Location = new System.Drawing.Point(145, 157);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(108, 45);
            this.btnUpdate.TabIndex = 9;
            this.btnUpdate.Text = "Update User";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(9, 157);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(108, 45);
            this.btnAdd.TabIndex = 8;
            this.btnAdd.Text = "Add User";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // cboRole
            // 
            this.cboRole.FormattingEnabled = true;
            this.cboRole.Location = new System.Drawing.Point(490, 86);
            this.cboRole.Name = "cboRole";
            this.cboRole.Size = new System.Drawing.Size(127, 44);
            this.cboRole.TabIndex = 7;
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(320, 86);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(100, 42);
            this.txtPhone.TabIndex = 6;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(6, 86);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(247, 42);
            this.txtEmail.TabIndex = 5;
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(611, 42);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(100, 42);
            this.txtLastName.TabIndex = 4;
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(490, 42);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(100, 42);
            this.txtFirstName.TabIndex = 3;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(320, 42);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(100, 42);
            this.txtPassword.TabIndex = 2;
            // 
            // txtLoginID
            // 
            this.txtLoginID.Location = new System.Drawing.Point(123, 42);
            this.txtLoginID.Name = "txtLoginID";
            this.txtLoginID.Size = new System.Drawing.Size(100, 42);
            this.txtLoginID.TabIndex = 1;
            // 
            // txtUserID
            // 
            this.txtUserID.Enabled = false;
            this.txtUserID.Location = new System.Drawing.Point(6, 42);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.ReadOnly = true;
            this.txtUserID.Size = new System.Drawing.Size(64, 42);
            this.txtUserID.TabIndex = 0;
            // 
            // dgvUsers
            // 
            this.dgvUsers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.Location = new System.Drawing.Point(26, 44);
            this.dgvUsers.MultiSelect = false;
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.RowHeadersVisible = false;
            this.dgvUsers.RowHeadersWidth = 82;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.Size = new System.Drawing.Size(711, 130);
            this.dgvUsers.TabIndex = 2;
            this.dgvUsers.SelectionChanged += new System.EventHandler(this.dgvUsers_SelectionChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.lblTitle);
            this.panel1.Controls.Add(this.dgvUsers);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(767, 396);
            this.panel1.TabIndex = 3;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblTitle.Location = new System.Drawing.Point(26, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(443, 65);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "User Management";
            // 
            // FormManageUsers
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.ClientSize = new System.Drawing.Size(802, 431);
            this.Controls.Add(this.panel1);
            this.Name = "FormManageUsers";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FormManageUsers_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvUsers_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
