// ============================================================
//  FormSalesReport.cs
// ============================================================

using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Gr8FoodSystem
{
    public partial class FormSalesReport : Form
    {
        private void btnManageUsers_Click(object sender, EventArgs e)
        {
            int adminID = 1; // later replace with logged-in user ID
            FormManageUsers frm = new FormManageUsers(adminID);
            frm.ShowDialog();
        }
        private void btnSalesReport_Click(object sender, EventArgs e)
        {
            FormSalesReport frm = new FormSalesReport();
            frm.ShowDialog();
        }

        private void btnUpdateProfile_Click(object sender, EventArgs e)
        {
            int adminID = 1; // ⚠️ TEMP — replace with actual logged-in ID
            FormUpdateProfile frm = new FormUpdateProfile(adminID);
            frm.ShowDialog();
        }

        private System.ComponentModel.IContainer components;
        private Label lblMonth;
        private Label lblYear;
        private Label lblCategory;
        private Label lblChef;
        private Panel panel1;
        private Label lblTitle;
        private Button btnBack;
        private Label lblSalesRecords;
        private string connStr = "Data Source=.;Initial Catalog=ResturantDB;Integrated Security=True";

        public FormSalesReport()
        {
            InitializeComponent();
        }

        // ================= LOAD =================
        private void FormSalesReport_Load(object sender, EventArgs e)
        {
            PopulateMonthDropdown();
            PopulateYearDropdown();
            PopulateCategoryDropdown();
            PopulateChefDropdown();

            LoadReport(0, 0, "All Categories", 0);
        }

        // ================= DROPDOWNS =================
        private void PopulateMonthDropdown()
        {
            cboMonth.Items.Clear();
            cboMonth.Items.Add("All Months");

            string[] months = {
                "January","February","March","April","May","June",
                "July","August","September","October","November","December"
            };

            cboMonth.Items.AddRange(months);
            cboMonth.SelectedIndex = 0;
        }

        private void PopulateYearDropdown()
        {
            cboYear.Items.Clear();
            cboYear.Items.Add("All Years");

            int year = DateTime.Now.Year;
            for (int i = 0; i < 4; i++)
                cboYear.Items.Add((year - i).ToString());

            cboYear.SelectedIndex = 0;
        }

        private void PopulateCategoryDropdown()
        {
            cboCategory.Items.Clear();
            cboCategory.Items.Add("All Categories");

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT category_name FROM menu_category", conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                        cboCategory.Items.Add(reader["category_name"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            cboCategory.SelectedIndex = 0;
        }

        private void PopulateChefDropdown()
        {
            cboChef.Items.Clear();
            cboChef.Items.Add(new ChefItem("All Chefs", 0));

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    string sql = @"SELECT userID, first_name + ' ' + last_name AS fullName 
                                   FROM user_account WHERE Role='Chef'";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        cboChef.Items.Add(new ChefItem(
                            reader["fullName"].ToString(),
                            Convert.ToInt32(reader["userID"])
                        ));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            cboChef.SelectedIndex = 0;
        }

        // ================= LOAD REPORT =================
        private void LoadReport(int month, int year, string category, int chefID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    string sql = @"
                    SELECT 
                        r.orderID AS [Order ID],
                        CONVERT(VARCHAR, r.order_date, 103) AS [Date],
                        c.first_name + ' ' + c.last_name AS [Customer],
                        m.item_name AS [Item],
                        cat.category_name AS [Category],
                        ch.first_name + ' ' + ch.last_name AS [Chef],
                        oi.quantity AS [Qty],
                        oi.subtotal AS [Subtotal (RM)]
                    FROM order_request r
                    JOIN user_account c ON r.customerID = c.userID
                    JOIN order_item oi ON r.orderID = oi.orderID
                    JOIN menu_item m ON oi.itemID = m.itemID
                    JOIN menu_category cat ON m.categoryID = cat.categoryID
                    JOIN user_account ch ON m.chefID = ch.userID
                    WHERE r.status='Completed'
                      AND (@month=0 OR MONTH(r.order_date)=@month)
                      AND (@year=0 OR YEAR(r.order_date)=@year)
                      AND (@category='All Categories' OR cat.category_name=@category)
                      AND (@chefID=0 OR ch.userID=@chefID)
                    ORDER BY r.order_date DESC";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@month", month);
                    cmd.Parameters.AddWithValue("@year", year);
                    cmd.Parameters.AddWithValue("@category", category);
                    cmd.Parameters.AddWithValue("@chefID", chefID);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvReport.DataSource = null;
                    dgvReport.DataSource = dt;
                    dgvReport.ReadOnly = true;
                    dgvReport.AutoGenerateColumns = true;
                    dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    decimal total = 0;
                    foreach (DataRow row in dt.Rows)
                        total += Convert.ToDecimal(row["Subtotal (RM)"]);

                    lblSummary.Text = $"Total Sales: RM {total:F2} | Records: {dt.Rows.Count}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // ================= BUTTONS =================
        private void btnFilter_Click(object sender, EventArgs e)
        {
            int month = cboMonth.SelectedIndex;

            int year = 0;
            if (cboYear.SelectedIndex > 0)
                year = Convert.ToInt32(cboYear.SelectedItem);

            string category = cboCategory.SelectedItem.ToString();
            int chefID = ((ChefItem)cboChef.SelectedItem).ID;

            LoadReport(month, year, category, chefID);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            cboMonth.SelectedIndex = 0;
            cboYear.SelectedIndex = 0;
            cboCategory.SelectedIndex = 0;
            cboChef.SelectedIndex = 0;

            LoadReport(0, 0, "All Categories", 0);
        }

        // ================= HELPER =================
        

        // ================= DESIGN =================
        private ComboBox cboMonth, cboYear, cboCategory, cboChef;
        private Button btnFilter, btnReset;

        private void dgvReport_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private DataGridView dgvReport;
        private Label lblSummary;
        private GroupBox groupBox1;

        private void InitializeComponent()
        {
            this.cboMonth = new System.Windows.Forms.ComboBox();
            this.cboYear = new System.Windows.Forms.ComboBox();
            this.cboCategory = new System.Windows.Forms.ComboBox();
            this.cboChef = new System.Windows.Forms.ComboBox();
            this.btnFilter = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.dgvReport = new System.Windows.Forms.DataGridView();
            this.lblSummary = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblMonth = new System.Windows.Forms.Label();
            this.lblYear = new System.Windows.Forms.Label();
            this.lblCategory = new System.Windows.Forms.Label();
            this.lblChef = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnBack = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSalesRecords = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboMonth
            // 
            this.cboMonth.Location = new System.Drawing.Point(18, 90);
            this.cboMonth.Name = "cboMonth";
            this.cboMonth.Size = new System.Drawing.Size(120, 21);
            this.cboMonth.TabIndex = 0;
            // 
            // cboYear
            // 
            this.cboYear.Location = new System.Drawing.Point(175, 90);
            this.cboYear.Name = "cboYear";
            this.cboYear.Size = new System.Drawing.Size(120, 21);
            this.cboYear.TabIndex = 1;
            // 
            // cboCategory
            // 
            this.cboCategory.Location = new System.Drawing.Point(368, 90);
            this.cboCategory.Name = "cboCategory";
            this.cboCategory.Size = new System.Drawing.Size(150, 21);
            this.cboCategory.TabIndex = 2;
            // 
            // cboChef
            // 
            this.cboChef.Location = new System.Drawing.Point(567, 90);
            this.cboChef.Name = "cboChef";
            this.cboChef.Size = new System.Drawing.Size(150, 21);
            this.cboChef.TabIndex = 3;
            // 
            // btnFilter
            // 
            this.btnFilter.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilter.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFilter.ForeColor = System.Drawing.SystemColors.Window;
            this.btnFilter.Location = new System.Drawing.Point(18, 126);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(100, 35);
            this.btnFilter.TabIndex = 4;
            this.btnFilter.Text = "Filter";
            this.btnFilter.UseVisualStyleBackColor = false;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.ForeColor = System.Drawing.SystemColors.Window;
            this.btnReset.Location = new System.Drawing.Point(176, 126);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(100, 35);
            this.btnReset.TabIndex = 5;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // dgvReport
            // 
            this.dgvReport.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReport.BackgroundColor = System.Drawing.Color.White;
            this.dgvReport.ColumnHeadersHeight = 46;
            this.dgvReport.Location = new System.Drawing.Point(15, 203);
            this.dgvReport.MultiSelect = false;
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.ReadOnly = true;
            this.dgvReport.RowHeadersVisible = false;
            this.dgvReport.RowHeadersWidth = 82;
            this.dgvReport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReport.Size = new System.Drawing.Size(738, 139);
            this.dgvReport.TabIndex = 6;
            this.dgvReport.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReport_CellContentClick);
            // 
            // lblSummary
            // 
            this.lblSummary.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSummary.Location = new System.Drawing.Point(3, 356);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(500, 30);
            this.lblSummary.TabIndex = 7;
            this.lblSummary.Text = "Total Sales: RM 0.00 | Records: 0";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // lblMonth
            // 
            this.lblMonth.AutoSize = true;
            this.lblMonth.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMonth.Location = new System.Drawing.Point(12, 69);
            this.lblMonth.Name = "lblMonth";
            this.lblMonth.Size = new System.Drawing.Size(65, 21);
            this.lblMonth.TabIndex = 8;
            this.lblMonth.Text = "Month:";
            // 
            // lblYear
            // 
            this.lblYear.AutoSize = true;
            this.lblYear.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYear.Location = new System.Drawing.Point(172, 69);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(48, 21);
            this.lblYear.TabIndex = 9;
            this.lblYear.Text = "Year:";
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCategory.Location = new System.Drawing.Point(365, 69);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(84, 21);
            this.lblCategory.TabIndex = 10;
            this.lblCategory.Text = "Category:";
            // 
            // lblChef
            // 
            this.lblChef.AutoSize = true;
            this.lblChef.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChef.Location = new System.Drawing.Point(564, 69);
            this.lblChef.Name = "lblChef";
            this.lblChef.Size = new System.Drawing.Size(50, 21);
            this.lblChef.TabIndex = 11;
            this.lblChef.Text = "Chef:";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.lblSalesRecords);
            this.panel1.Controls.Add(this.btnBack);
            this.panel1.Controls.Add(this.lblTitle);
            this.panel1.Controls.Add(this.dgvReport);
            this.panel1.Controls.Add(this.lblSummary);
            this.panel1.Controls.Add(this.lblChef);
            this.panel1.Controls.Add(this.btnReset);
            this.panel1.Controls.Add(this.lblCategory);
            this.panel1.Controls.Add(this.btnFilter);
            this.panel1.Controls.Add(this.lblYear);
            this.panel1.Controls.Add(this.cboChef);
            this.panel1.Controls.Add(this.lblMonth);
            this.panel1.Controls.Add(this.cboCategory);
            this.panel1.Controls.Add(this.cboMonth);
            this.panel1.Controls.Add(this.cboYear);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(767, 396);
            this.panel1.TabIndex = 12;
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnBack.ForeColor = System.Drawing.SystemColors.Window;
            this.btnBack.Location = new System.Drawing.Point(653, 348);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(100, 35);
            this.btnBack.TabIndex = 13;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblTitle.Location = new System.Drawing.Point(11, 18);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(225, 30);
            this.lblTitle.TabIndex = 12;
            this.lblTitle.Text = "Monthly Sales Report";
            // 
            // lblSalesRecords
            // 
            this.lblSalesRecords.AutoSize = true;
            this.lblSalesRecords.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSalesRecords.Location = new System.Drawing.Point(14, 179);
            this.lblSalesRecords.Name = "lblSalesRecords";
            this.lblSalesRecords.Size = new System.Drawing.Size(161, 21);
            this.lblSalesRecords.TabIndex = 14;
            this.lblSalesRecords.Text = "Sales Report Details";
            // 
            // FormSalesReport
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.ClientSize = new System.Drawing.Size(802, 431);
            this.Controls.Add(this.panel1);
            this.Name = "FormSalesReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sales Report";
            this.Load += new System.EventHandler(this.FormSalesReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}