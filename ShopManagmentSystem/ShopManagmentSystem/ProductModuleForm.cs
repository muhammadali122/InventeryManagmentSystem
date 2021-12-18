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

namespace ShopManagmentSystem
{
    public partial class ProductModuleForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\hp\Documents\dbMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        public ProductModuleForm()
        {
            InitializeComponent();
            LoadCategory();
        }

        public void LoadCategory()
        {
            cbCategory.Items.Clear();
            cm = new SqlCommand("SELECT CatName FROM tbcategory", con);
            con.Open();
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                cbCategory.Items.Add(dr[0].ToString());
            }
            dr.Close();
            con.Close();

            
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtFullname_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to Save this Product?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("INSERT INTO tbproduct(pname,pqty,pprice,pdescription,pcategory)VALUES(@pname,@pqty,@pprice,@pdescription,@pcategory)", con);
                    cm.Parameters.AddWithValue("@pname", txtProductname.Text);
                    cm.Parameters.AddWithValue("@pqty",Convert.ToInt16 (txtQuantity.Text));
                    cm.Parameters.AddWithValue("@pprice",Convert.ToInt16 (txtPrice.Text));
                    cm.Parameters.AddWithValue("@pdescription", txtDescription.Text);
                    cm.Parameters.AddWithValue("@pcategory", cbCategory.Text);
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Product has been successfully saved!");
                    clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void clear()
        {
            txtProductname.Clear();
            txtQuantity.Clear();
            txtPrice.Clear();
            txtDescription.Clear();
            cbCategory.Text = "";
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
            btnsave.Enabled = true;
            btnUpdate.Enabled = false;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to Update this Product?", "Updating Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("UPDATE tbproduct SET pname=@pname, pqty=@pqty, pprice=@pprice, pdescription=@pdescription, pcategory=@pcategory WHERE pid LIKE '" + lblpid.Text + "'", con);
                    cm.Parameters.AddWithValue("@pname", txtProductname.Text);
                    cm.Parameters.AddWithValue("@pqty", Convert.ToInt16(txtQuantity.Text));
                    cm.Parameters.AddWithValue("@pprice", Convert.ToInt16(txtPrice.Text));
                    cm.Parameters.AddWithValue("@pdescription", txtDescription.Text);
                    cm.Parameters.AddWithValue("@pcategory", cbCategory.Text);
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Product has been successfully Updated!");
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
