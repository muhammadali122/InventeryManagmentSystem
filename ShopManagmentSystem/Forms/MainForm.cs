using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopManagmentSystem
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            
        }

        private Form activeForm = null;
        private void openChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelMain.Controls.Add(childForm);
            panelMain.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void btnuser_Click(object sender, EventArgs e)
        {
            openChildForm(new UserForm());
        }

        private void btncustomer_Click(object sender, EventArgs e)
        {
            openChildForm(new CustomerForm());
        }

        private void panelMain_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btncategories_Click(object sender, EventArgs e)
        {
            openChildForm(new CategoriesForm());
        }

        private void btnproduct_Click(object sender, EventArgs e)
        {
            openChildForm(new ProductForm());
        }

        private void btnorder_Click(object sender, EventArgs e)
        {
            openChildForm(new OrderForm());
        }

        private void btnlogout_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            this.Hide();
            this.Close();
            login.ShowDialog();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            openChildForm(new PurchaseForm());
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            openChildForm(new DealerForm());
        }
    }
}
