// ============================================================
//  FormAdminHome.cs
using System;
using System.Windows.Forms;

namespace Gr8FoodSystem
{
    public partial class FormAdminHome : Form
    {
        private Label lblWelcome;
        private Button btnManageUsers;
        private Button btnSalesReport;
        private Button btnUpdateProfile;
        private Panel panel1;
        private Button btnExit;
        private Label lblSubtitle;
        private int _adminID;

        public FormAdminHome(int adminID)
        {
            InitializeComponent();
            _adminID = adminID;
        }

        // ================= LOAD =================
        private void FormAdminHome_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = "Welcome, System Admin!";
        }

        // ================= BUTTONS =================
        private void btnManageUsers_Click(object sender, EventArgs e)
        {
            FormManageUsers frm = new FormManageUsers(_adminID);
            frm.ShowDialog();
        }

        private void btnSalesReport_Click(object sender, EventArgs e)
        {
            FormSalesReport frm = new FormSalesReport();
            frm.ShowDialog();
        }

        private void btnUpdateProfile_Click(object sender, EventArgs e)
        {
            FormUpdateProfile frm = new FormUpdateProfile(_adminID);
            frm.ShowDialog();
        }

        // ================= UI =================
        private void InitializeComponent()
        {
            this.lblWelcome = new System.Windows.Forms.Label();
            this.btnManageUsers = new System.Windows.Forms.Button();
            this.btnSalesReport = new System.Windows.Forms.Button();
            this.btnUpdateProfile = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.875F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblWelcome.Location = new System.Drawing.Point(234, 12);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(239, 31);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Welcome, Admin ";
            this.lblWelcome.Click += new System.EventHandler(this.lblWelcome_Click);
            // 
            // btnManageUsers
            // 
            this.btnManageUsers.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnManageUsers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManageUsers.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManageUsers.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnManageUsers.Location = new System.Drawing.Point(261, 86);
            this.btnManageUsers.Name = "btnManageUsers";
            this.btnManageUsers.Size = new System.Drawing.Size(199, 60);
            this.btnManageUsers.TabIndex = 1;
            this.btnManageUsers.Text = "Manage Users";
            this.btnManageUsers.UseVisualStyleBackColor = false;
            this.btnManageUsers.Click += new System.EventHandler(this.btnManageUsers_Click_1);
            // 
            // btnSalesReport
            // 
            this.btnSalesReport.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnSalesReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalesReport.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalesReport.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnSalesReport.Location = new System.Drawing.Point(261, 152);
            this.btnSalesReport.Name = "btnSalesReport";
            this.btnSalesReport.Size = new System.Drawing.Size(199, 60);
            this.btnSalesReport.TabIndex = 2;
            this.btnSalesReport.Text = "Sales Report";
            this.btnSalesReport.UseVisualStyleBackColor = false;
            this.btnSalesReport.Click += new System.EventHandler(this.btnSalesReport_Click_1);
            // 
            // btnUpdateProfile
            // 
            this.btnUpdateProfile.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnUpdateProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdateProfile.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdateProfile.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnUpdateProfile.Location = new System.Drawing.Point(261, 218);
            this.btnUpdateProfile.Name = "btnUpdateProfile";
            this.btnUpdateProfile.Size = new System.Drawing.Size(199, 60);
            this.btnUpdateProfile.TabIndex = 3;
            this.btnUpdateProfile.Text = "Update Profile";
            this.btnUpdateProfile.UseVisualStyleBackColor = false;
            this.btnUpdateProfile.Click += new System.EventHandler(this.btnUpdateProfile_Click_1);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.lblSubtitle);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnManageUsers);
            this.panel1.Controls.Add(this.lblWelcome);
            this.panel1.Controls.Add(this.btnUpdateProfile);
            this.panel1.Controls.Add(this.btnSalesReport);
            this.panel1.Location = new System.Drawing.Point(31, 33);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(725, 364);
            this.panel1.TabIndex = 4;
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.125F, System.Drawing.FontStyle.Bold);
            this.lblSubtitle.Location = new System.Drawing.Point(177, 43);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(363, 26);
            this.lblSubtitle.TabIndex = 5;
            this.lblSubtitle.Text = "What would you like to do today?";
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnExit.Location = new System.Drawing.Point(261, 284);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(199, 60);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "Logout";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // FormAdminHome
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.ClientSize = new System.Drawing.Size(799, 431);
            this.Controls.Add(this.panel1);
            this.Name = "FormAdminHome";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Admin Home";
            this.Load += new System.EventHandler(this.FormAdminHome_Load_1);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        private void btnManageUsers_Click_1(object sender, EventArgs e)
        {
            FormManageUsers frm = new FormManageUsers(1);
            frm.Show();
        }

        private void btnSalesReport_Click_1(object sender, EventArgs e)
        {
           
            FormSalesReport frm = new FormSalesReport();
            frm.Show();
        }

        private void btnUpdateProfile_Click_1(object sender, EventArgs e)
        {
           
            FormUpdateProfile frm = new FormUpdateProfile(1);
            frm.Show();
        }

        private void FormAdminHome_Load_1(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            login.Show();
            this.Close();
        }

        private void lblWelcome_Click(object sender, EventArgs e)
        {

        }
    }
    }
    
