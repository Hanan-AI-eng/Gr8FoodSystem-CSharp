namespace Gr8FoodSystem
{
    partial class FormViewFeedback
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dgvFeedback = new System.Windows.Forms.DataGridView();
            this.txtFeedbackID = new System.Windows.Forms.TextBox();
            this.txtCustomer = new System.Windows.Forms.TextBox();
            this.txtOrderID = new System.Windows.Forms.TextBox();
            this.txtSentDate = new System.Windows.Forms.TextBox();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.rtbMessage = new System.Windows.Forms.RichTextBox();
            this.rtbReply = new System.Windows.Forms.RichTextBox();
            this.btnReply = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblFeedbackID = new System.Windows.Forms.Label();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.lblOrderID = new System.Windows.Forms.Label();
            this.lblSentDate = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblCustomerMessage = new System.Windows.Forms.Label();
            this.lblReply = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnBack = new System.Windows.Forms.Button();
            this.feedbackBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblFeedbackTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFeedback)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.feedbackBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvFeedback
            // 
            this.dgvFeedback.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvFeedback.BackgroundColor = System.Drawing.Color.White;
            this.dgvFeedback.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFeedback.Location = new System.Drawing.Point(9, 48);
            this.dgvFeedback.Name = "dgvFeedback";
            this.dgvFeedback.RowHeadersVisible = false;
            this.dgvFeedback.RowHeadersWidth = 82;
            this.dgvFeedback.Size = new System.Drawing.Size(701, 114);
            this.dgvFeedback.TabIndex = 0;
            this.dgvFeedback.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFeedback_CellClick);
            this.dgvFeedback.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFeedback_CellContentClick);
            // 
            // txtFeedbackID
            // 
            this.txtFeedbackID.Location = new System.Drawing.Point(8, 189);
            this.txtFeedbackID.Name = "txtFeedbackID";
            this.txtFeedbackID.Size = new System.Drawing.Size(100, 20);
            this.txtFeedbackID.TabIndex = 1;
            // 
            // txtCustomer
            // 
            this.txtCustomer.Location = new System.Drawing.Point(164, 189);
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.Size = new System.Drawing.Size(100, 20);
            this.txtCustomer.TabIndex = 2;
            // 
            // txtOrderID
            // 
            this.txtOrderID.Location = new System.Drawing.Point(342, 189);
            this.txtOrderID.Name = "txtOrderID";
            this.txtOrderID.Size = new System.Drawing.Size(100, 20);
            this.txtOrderID.TabIndex = 3;
            // 
            // txtSentDate
            // 
            this.txtSentDate.Location = new System.Drawing.Point(487, 189);
            this.txtSentDate.Name = "txtSentDate";
            this.txtSentDate.Size = new System.Drawing.Size(100, 20);
            this.txtSentDate.TabIndex = 4;
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(613, 189);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(92, 20);
            this.txtStatus.TabIndex = 5;
            // 
            // rtbMessage
            // 
            this.rtbMessage.Font = new System.Drawing.Font("Segoe UI", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbMessage.Location = new System.Drawing.Point(8, 235);
            this.rtbMessage.Name = "rtbMessage";
            this.rtbMessage.Size = new System.Drawing.Size(288, 84);
            this.rtbMessage.TabIndex = 6;
            this.rtbMessage.Text = "";
            // 
            // rtbReply
            // 
            this.rtbReply.Location = new System.Drawing.Point(416, 235);
            this.rtbReply.Name = "rtbReply";
            this.rtbReply.Size = new System.Drawing.Size(288, 84);
            this.rtbReply.TabIndex = 7;
            this.rtbReply.Text = "";
            // 
            // btnReply
            // 
            this.btnReply.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnReply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReply.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReply.ForeColor = System.Drawing.Color.White;
            this.btnReply.Location = new System.Drawing.Point(446, 325);
            this.btnReply.Name = "btnReply";
            this.btnReply.Size = new System.Drawing.Size(75, 29);
            this.btnReply.TabIndex = 8;
            this.btnReply.Text = "Reply";
            this.btnReply.UseVisualStyleBackColor = false;
            this.btnReply.Click += new System.EventHandler(this.btnReply_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(538, 325);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 29);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lblFeedbackID
            // 
            this.lblFeedbackID.AutoSize = true;
            this.lblFeedbackID.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFeedbackID.Location = new System.Drawing.Point(8, 173);
            this.lblFeedbackID.Name = "lblFeedbackID";
            this.lblFeedbackID.Size = new System.Drawing.Size(103, 17);
            this.lblFeedbackID.TabIndex = 10;
            this.lblFeedbackID.Text = "Feedback ID:";
            // 
            // lblCustomer
            // 
            this.lblCustomer.AutoSize = true;
            this.lblCustomer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomer.Location = new System.Drawing.Point(164, 173);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(81, 17);
            this.lblCustomer.TabIndex = 11;
            this.lblCustomer.Text = "Customer:";
            // 
            // lblOrderID
            // 
            this.lblOrderID.AutoSize = true;
            this.lblOrderID.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrderID.Location = new System.Drawing.Point(339, 173);
            this.lblOrderID.Name = "lblOrderID";
            this.lblOrderID.Size = new System.Drawing.Size(75, 17);
            this.lblOrderID.TabIndex = 12;
            this.lblOrderID.Text = "Order ID:";
            // 
            // lblSentDate
            // 
            this.lblSentDate.AutoSize = true;
            this.lblSentDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSentDate.Location = new System.Drawing.Point(484, 173);
            this.lblSentDate.Name = "lblSentDate";
            this.lblSentDate.Size = new System.Drawing.Size(85, 17);
            this.lblSentDate.TabIndex = 13;
            this.lblSentDate.Text = "Sent Date:";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(610, 173);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(59, 17);
            this.lblStatus.TabIndex = 14;
            this.lblStatus.Text = "Status:";
            // 
            // lblCustomerMessage
            // 
            this.lblCustomerMessage.AutoSize = true;
            this.lblCustomerMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerMessage.Location = new System.Drawing.Point(8, 218);
            this.lblCustomerMessage.Name = "lblCustomerMessage";
            this.lblCustomerMessage.Size = new System.Drawing.Size(150, 17);
            this.lblCustomerMessage.TabIndex = 15;
            this.lblCustomerMessage.Text = "Customer Message:";
            // 
            // lblReply
            // 
            this.lblReply.AutoSize = true;
            this.lblReply.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReply.Location = new System.Drawing.Point(414, 218);
            this.lblReply.Name = "lblReply";
            this.lblReply.Size = new System.Drawing.Size(93, 17);
            this.lblReply.TabIndex = 16;
            this.lblReply.Text = "Your Reply:";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.lblFeedbackTitle);
            this.panel1.Controls.Add(this.btnBack);
            this.panel1.Controls.Add(this.rtbReply);
            this.panel1.Controls.Add(this.dgvFeedback);
            this.panel1.Controls.Add(this.lblReply);
            this.panel1.Controls.Add(this.txtFeedbackID);
            this.panel1.Controls.Add(this.lblCustomerMessage);
            this.panel1.Controls.Add(this.txtCustomer);
            this.panel1.Controls.Add(this.lblStatus);
            this.panel1.Controls.Add(this.txtOrderID);
            this.panel1.Controls.Add(this.lblSentDate);
            this.panel1.Controls.Add(this.txtSentDate);
            this.panel1.Controls.Add(this.lblOrderID);
            this.panel1.Controls.Add(this.txtStatus);
            this.panel1.Controls.Add(this.lblCustomer);
            this.panel1.Controls.Add(this.rtbMessage);
            this.panel1.Controls.Add(this.lblFeedbackID);
            this.panel1.Controls.Add(this.btnReply);
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Location = new System.Drawing.Point(35, 36);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(725, 364);
            this.panel1.TabIndex = 17;
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Location = new System.Drawing.Point(628, 325);
            this.btnBack.Margin = new System.Windows.Forms.Padding(2);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 29);
            this.btnBack.TabIndex = 18;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // feedbackBindingSource
            // 
            this.feedbackBindingSource.DataMember = "feedback";
            // 
            // lblFeedbackTitle
            // 
            this.lblFeedbackTitle.AutoSize = true;
            this.lblFeedbackTitle.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFeedbackTitle.Location = new System.Drawing.Point(6, 15);
            this.lblFeedbackTitle.Name = "lblFeedbackTitle";
            this.lblFeedbackTitle.Size = new System.Drawing.Size(341, 30);
            this.lblFeedbackTitle.TabIndex = 19;
            this.lblFeedbackTitle.Text = "Customer Feedback Management";
            // 
            // FormViewFeedback
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.ClientSize = new System.Drawing.Size(790, 431);
            this.Controls.Add(this.panel1);
            this.HelpButton = true;
            this.Name = "FormViewFeedback";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "View Feedback";
            this.Load += new System.EventHandler(this.FormViewFeedback_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFeedback)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.feedbackBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvFeedback;
        private System.Windows.Forms.TextBox txtFeedbackID;
        private System.Windows.Forms.TextBox txtCustomer;
        private System.Windows.Forms.TextBox txtOrderID;
        private System.Windows.Forms.TextBox txtSentDate;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.RichTextBox rtbMessage;
        private System.Windows.Forms.RichTextBox rtbReply;
        private System.Windows.Forms.Button btnReply;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lblFeedbackID;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.Label lblOrderID;
        private System.Windows.Forms.Label lblSentDate;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblCustomerMessage;
        private System.Windows.Forms.Label lblReply;

        private System.Windows.Forms.BindingSource feedbackBindingSource;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Label lblFeedbackTitle;
    }
}