// ============================================================
//  FormManagerHome.cs
//  Main menu screen for the Manager.
//  Has 3 buttons: View Feedback, Wallet Report, Update Profile
//
//  CONTROLS NEEDED IN DESIGNER:
//    Label   → lblWelcome
//    Label   → lblSubtitle
//    Button  → btnViewFeedback
//    Button  → btnWalletReport
//    Button  → btnUpdateProfile
//
//  Form size: 400 x 340
//  StartPosition: CenterScreen
// ============================================================

using System;
using System.Windows.Forms;

namespace Gr8FoodSystem
{
    public partial class FormManagerHome : Form
    {
        // Stores the manager's userID — passed to all other forms
        private int _managerID;

        // Constructor — receives managerID from login or Program.cs
        public FormManagerHome(int managerID)
        {
            InitializeComponent();
            _managerID = managerID;
        }

        // ── FORM LOAD — show welcome message ──────────────────────────────────
        // Double-click the form background in designer to wire this up
        private void FormManagerHome_Load(object sender, EventArgs e)
        {
            // Create a Manager object and use its method for the welcome message
            

            lblWelcome.Text = "Welcome, Manager!";
            lblSubtitle.Text = "What would you like to do today?";
        }

        // ── BUTTON: View Feedback ─────────────────────────────────────────────
        // Double-click btnViewFeedback in designer to wire this up
        private void btnViewFeedback_Click(object sender, EventArgs e)
        {
            FormViewFeedback f = new FormViewFeedback(_managerID);
            f.ShowDialog();
        }

        // ── BUTTON: Wallet Report ─────────────────────────────────────────────
        // Double-click btnWalletReport in designer to wire this up
        private void btnWalletReport_Click(object sender, EventArgs e)
        {
            FormWalletReport f = new FormWalletReport();
            f.ShowDialog();
        }

        // ── BUTTON: Update Profile ────────────────────────────────────────────
        // Double-click btnUpdateProfile in designer to wire this up
        private void btnUpdateProfile_Click(object sender, EventArgs e)
        {
            FormUpdateProfile f =
    new FormUpdateProfile(_managerID);

            f.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormWalletReport f = new FormWalletReport();
            f.ShowDialog();
        }

        

        private void btnViewFeedback_Click_1(object sender, EventArgs e)
        {
            FormViewFeedback f = new FormViewFeedback(_managerID);
            f.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            login.Show();
            this.Close();
        }

        private void lblSubtitle_Click(object sender, EventArgs e)
        {

        }

        
    }
}