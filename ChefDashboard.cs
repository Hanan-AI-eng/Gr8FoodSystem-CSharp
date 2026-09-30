using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gr8FoodSystem
{
    public partial class ChefDashboard : Form
    {
        int chefID;
        public ChefDashboard(int id)
        {
            InitializeComponent();
            chefID = id;
        }

        private void btnOrders_Click(object sender, EventArgs e)
        {
            ViewOrderForm f = new ViewOrderForm();
            f.ShowDialog();
        }

        private void ChefDashboard_Load(object sender, EventArgs e)
        {

        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            FormUpdateProfile f =
    new FormUpdateProfile(chefID);

            f.ShowDialog();
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            ManageMenuForm f = new ManageMenuForm();
            f.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            login.Show();
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
