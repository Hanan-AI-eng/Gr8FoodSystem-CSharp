using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Gr8FoodSystem
{
    public partial class FormWalletReport : Form
    {
        private string connStr = "Data Source=localhost;Initial Catalog=ResturantDB;Integrated Security=True;TrustServerCertificate=True";

        public FormWalletReport()
        {
            InitializeComponent();
        }

        private void FormWalletReport_Load(object sender, EventArgs e)
        {
            FillCustomerDropdown();
            FillMonthDropdown();
            FillYearDropdown();
            LoadReport(0, 0, 0);
        }

        private void FillCustomerDropdown()
        {
            cboCustomer.Items.Clear();
            cboCustomer.Items.Add(new CustomerItem("All Customers", 0));

            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand(
                @"SELECT userID, first_name + ' ' + last_name AS fullName
                  FROM user_account WHERE Role = 'Customer'
                  ORDER BY first_name", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                cboCustomer.Items.Add(new CustomerItem(
                    reader["fullName"].ToString(),
                    Convert.ToInt32(reader["userID"])));
            reader.Close();
            conn.Close();
            cboCustomer.SelectedIndex = 0;
        }

        private void FillMonthDropdown()
        {
            cboMonth.Items.Clear();
            cboMonth.Items.Add("All Months");
            cboMonth.Items.Add("January");
            cboMonth.Items.Add("February");
            cboMonth.Items.Add("March");
            cboMonth.Items.Add("April");
            cboMonth.Items.Add("May");
            cboMonth.Items.Add("June");
            cboMonth.Items.Add("July");
            cboMonth.Items.Add("August");
            cboMonth.Items.Add("September");
            cboMonth.Items.Add("October");
            cboMonth.Items.Add("November");
            cboMonth.Items.Add("December");
            cboMonth.SelectedIndex = 0;
        }

        private void FillYearDropdown()
        {
            cboYear.Items.Clear();
            cboYear.Items.Add("All Years");
            int thisYear = DateTime.Now.Year;
            for (int y = thisYear; y >= thisYear - 3; y--)
                cboYear.Items.Add(y.ToString());
            cboYear.SelectedIndex = 0;
        }

        private void LoadReport(int customerID, int month, int year)
        {
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            string sql = @"
                SELECT
                    wt.transactionID                             AS [ID],
                    u.first_name + ' ' + u.last_name            AS [Customer],
                    wt.type                                      AS [Type],
                    wt.amount                                    AS [Amount (RM)],
                    CONVERT(VARCHAR, wt.transaction_date, 103)   AS [Date],
                    wt.description                               AS [Description]
                FROM wallet_transaction wt
                JOIN user_account u ON wt.userID = u.userID
                WHERE u.Role = 'Customer'
                  AND (@customerID = 0 OR wt.userID                  = @customerID)
                  AND (@month      = 0 OR MONTH(wt.transaction_date) = @month)
                  AND (@year       = 0 OR YEAR(wt.transaction_date)  = @year)
                ORDER BY wt.transaction_date DESC";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@customerID", customerID);
            cmd.Parameters.AddWithValue("@month",      month);
            cmd.Parameters.AddWithValue("@year",       year);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();

            dgvWallet.DataSource         = dt;
            dgvWallet.ReadOnly           = true;
            dgvWallet.SelectionMode      = DataGridViewSelectionMode.FullRowSelect;
            dgvWallet.RowHeadersVisible  = false;
            dgvWallet.AllowUserToAddRows = false;

            // Colour rows by transaction type
            foreach (DataGridViewRow row in dgvWallet.Rows)
            {
                if (row.Cells[2].Value == null) continue;
                string type = row.Cells[2].Value.ToString().ToLower();
                if (type.Contains("top"))
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                else if (type.Contains("deduct"))
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
                else if (type.Contains("refund"))
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.LightBlue;
            }

            // Calculate totals
            decimal totalTopUp = 0, totalDeducted = 0, totalRefund = 0;
            foreach (DataRow row in dt.Rows)
            {
                string type    = row["Type"].ToString().ToLower();
                decimal amount = Math.Abs(Convert.ToDecimal(row["Amount (RM)"]));
                if (type.Contains("top"))         totalTopUp    += amount;
                else if (type.Contains("deduct")) totalDeducted += amount;
                else if (type.Contains("refund")) totalRefund   += amount;
            }

            lblSummary.Text =
                "Records: " + dt.Rows.Count +
                "     |     Top Ups: RM " + totalTopUp.ToString("F2") +
                "     |     Deductions: RM " + totalDeducted.ToString("F2") +
                "     |     Refunds: RM " + totalRefund.ToString("F2");

            if (dt.Rows.Count == 0)
                MessageBox.Show("No transactions found for the selected filters.",
                    "No Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            int customerID = ((CustomerItem)cboCustomer.SelectedItem).ID;
            int month      = cboMonth.SelectedIndex;
            int year       = 0;
            if (cboYear.SelectedIndex > 0)
                year = Convert.ToInt32(cboYear.SelectedItem.ToString());
            LoadReport(customerID, month, year);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            cboCustomer.SelectedIndex = 0;
            cboMonth.SelectedIndex    = 0;
            cboYear.SelectedIndex     = 0;
            LoadReport(0, 0, 0);
        }

        private class CustomerItem
        {
            public string Name { get; }
            public int    ID   { get; }
            public CustomerItem(string name, int id) { Name = name; ID = id; }
            public override string ToString() => Name;
        }

        private void InitializeComponent()
        {
            this.cboCustomer = new System.Windows.Forms.ComboBox();
            this.cboMonth = new System.Windows.Forms.ComboBox();
            this.cboYear = new System.Windows.Forms.ComboBox();
            this.dgvWallet = new System.Windows.Forms.DataGridView();
            this.lblSummary = new System.Windows.Forms.Label();
            this.btnFilter = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.lblMonth = new System.Windows.Forms.Label();
            this.lblYear = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnBack = new System.Windows.Forms.Button();
            this.lblWalletReportTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWallet)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboCustomer
            // 
            this.cboCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCustomer.FormattingEnabled = true;
            this.cboCustomer.Location = new System.Drawing.Point(139, 39);
            this.cboCustomer.Name = "cboCustomer";
            this.cboCustomer.Size = new System.Drawing.Size(101, 21);
            this.cboCustomer.TabIndex = 0;
            // 
            // cboMonth
            // 
            this.cboMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMonth.FormattingEnabled = true;
            this.cboMonth.Location = new System.Drawing.Point(355, 40);
            this.cboMonth.Name = "cboMonth";
            this.cboMonth.Size = new System.Drawing.Size(115, 21);
            this.cboMonth.TabIndex = 1;
            // 
            // cboYear
            // 
            this.cboYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboYear.FormattingEnabled = true;
            this.cboYear.Location = new System.Drawing.Point(564, 43);
            this.cboYear.Name = "cboYear";
            this.cboYear.Size = new System.Drawing.Size(118, 21);
            this.cboYear.TabIndex = 2;
            // 
            // dgvWallet
            // 
            this.dgvWallet.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvWallet.BackgroundColor = System.Drawing.Color.White;
            this.dgvWallet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWallet.Location = new System.Drawing.Point(49, 163);
            this.dgvWallet.Name = "dgvWallet";
            this.dgvWallet.RowHeadersVisible = false;
            this.dgvWallet.RowHeadersWidth = 82;
            this.dgvWallet.Size = new System.Drawing.Size(652, 144);
            this.dgvWallet.TabIndex = 3;
            // 
            // lblSummary
            // 
            this.lblSummary.AutoSize = true;
            this.lblSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSummary.Location = new System.Drawing.Point(14, 320);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(0, 13);
            this.lblSummary.TabIndex = 4;
            // 
            // btnFilter
            // 
            this.btnFilter.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFilter.ForeColor = System.Drawing.Color.White;
            this.btnFilter.Location = new System.Drawing.Point(49, 89);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(128, 33);
            this.btnFilter.TabIndex = 5;
            this.btnFilter.Text = "Filter";
            this.btnFilter.UseVisualStyleBackColor = false;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.ForeColor = System.Drawing.Color.White;
            this.btnReset.Location = new System.Drawing.Point(573, 89);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(128, 33);
            this.btnReset.TabIndex = 6;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // lblCustomer
            // 
            this.lblCustomer.AutoSize = true;
            this.lblCustomer.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomer.Location = new System.Drawing.Point(45, 39);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(88, 21);
            this.lblCustomer.TabIndex = 7;
            this.lblCustomer.Text = "Customer:";
            // 
            // lblMonth
            // 
            this.lblMonth.AutoSize = true;
            this.lblMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblMonth.Location = new System.Drawing.Point(285, 40);
            this.lblMonth.Name = "lblMonth";
            this.lblMonth.Size = new System.Drawing.Size(64, 20);
            this.lblMonth.TabIndex = 8;
            this.lblMonth.Text = "Month:";
            // 
            // lblYear
            // 
            this.lblYear.AutoSize = true;
            this.lblYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblYear.Location = new System.Drawing.Point(508, 43);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(52, 20);
            this.lblYear.TabIndex = 9;
            this.lblYear.Text = "Year:";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.lblWalletReportTitle);
            this.panel1.Controls.Add(this.btnBack);
            this.panel1.Controls.Add(this.lblSummary);
            this.panel1.Controls.Add(this.dgvWallet);
            this.panel1.Controls.Add(this.lblYear);
            this.panel1.Controls.Add(this.cboCustomer);
            this.panel1.Controls.Add(this.lblMonth);
            this.panel1.Controls.Add(this.cboMonth);
            this.panel1.Controls.Add(this.lblCustomer);
            this.panel1.Controls.Add(this.cboYear);
            this.panel1.Controls.Add(this.btnReset);
            this.panel1.Controls.Add(this.btnFilter);
            this.panel1.Location = new System.Drawing.Point(31, 21);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(725, 364);
            this.panel1.TabIndex = 10;
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Bold);
            this.btnBack.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnBack.Location = new System.Drawing.Point(573, 313);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(128, 33);
            this.btnBack.TabIndex = 10;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // lblWalletReportTitle
            // 
            this.lblWalletReportTitle.AutoSize = true;
            this.lblWalletReportTitle.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWalletReportTitle.Location = new System.Drawing.Point(44, 135);
            this.lblWalletReportTitle.Name = "lblWalletReportTitle";
            this.lblWalletReportTitle.Size = new System.Drawing.Size(225, 25);
            this.lblWalletReportTitle.TabIndex = 11;
            this.lblWalletReportTitle.Text = "Customer Wallet Report";
            // 
            // FormWalletReport
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.ClientSize = new System.Drawing.Size(798, 431);
            this.Controls.Add(this.panel1);
            this.Name = "FormWalletReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Wallet Report";
            this.Load += new System.EventHandler(this.FormWalletReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWallet)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.ComboBox    cboCustomer;
        private System.Windows.Forms.ComboBox    cboMonth;
        private System.Windows.Forms.ComboBox    cboYear;
        private System.Windows.Forms.DataGridView dgvWallet;
        private System.Windows.Forms.Label       lblSummary;
        private System.Windows.Forms.Button      btnFilter;
        private System.Windows.Forms.Button      btnReset;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.Label lblMonth;
        private Panel panel1;
        private Button btnBack;
        private Label lblWalletReportTitle;
        private System.Windows.Forms.Label lblYear;

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}