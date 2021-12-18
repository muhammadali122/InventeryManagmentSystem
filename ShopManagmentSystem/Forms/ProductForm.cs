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
    public partial class ProductForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\ShopManagmentSystem\ShopManagmentSystem\ShopDatabase.mdf;Integrated Security=True");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        public ProductForm()
        {
            InitializeComponent();
            LoadProduct();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ProductModuleForm moduleForm = new ProductModuleForm();
            moduleForm.btnsave.Enabled = true;
            moduleForm.btnUpdate.Enabled = false;
            moduleForm.ShowDialog();
            LoadProduct();
        }

        private void dgvProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvPurchase.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                ProductModuleForm ProductModule = new ProductModuleForm();
                ProductModule.lblpid.Text = dgvPurchase.Rows[e.RowIndex].Cells[1].Value.ToString();
                ProductModule.txtProductname.Text = dgvPurchase.Rows[e.RowIndex].Cells[2].Value.ToString();
                ProductModule.txtQuantity.Text = dgvPurchase.Rows[e.RowIndex].Cells[3].Value.ToString();
                ProductModule.txtPrice.Text = dgvPurchase.Rows[e.RowIndex].Cells[4].Value.ToString();
                ProductModule.txtDescription.Text = dgvPurchase.Rows[e.RowIndex].Cells[5].Value.ToString();
                ProductModule.cbCategory.Text = dgvPurchase.Rows[e.RowIndex].Cells[6].Value.ToString();


                ProductModule.btnsave.Enabled = false;
                ProductModule.btnUpdate.Enabled = true;
                ProductModule.ShowDialog();
            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this Product", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    con.Open();
                    cm = new SqlCommand("DELETE FROM tbproduct WHERE pid LIKE '" + dgvPurchase.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", con);
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Record has been succesfully Deleted!");

                }
            }
            LoadProduct();
        }

        public void LoadProduct()
        {
            int i = 0;
            dgvPurchase.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbproduct WHERE CONCAT(pid, pname, pprice, pdescription, pcategory) LIKE '%"+txtsearch.Text+"%'", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvPurchase.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString());
            }
            dr.Close();
            con.Close();
        }

        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }
    }
}
