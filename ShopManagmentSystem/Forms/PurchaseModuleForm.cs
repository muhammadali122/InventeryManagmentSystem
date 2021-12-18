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
    public partial class PurchaseModuleForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\ShopManagmentSystem\ShopManagmentSystem\ShopDatabase.mdf;Integrated Security=True");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        int qty = 0;
        public PurchaseModuleForm()
        {
            InitializeComponent();
            LoadDealer();
            LoadProduct();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        public void LoadDealer()
        {
            int i = 0;
            dgvDealer.Rows.Clear();
            cm = new SqlCommand("SELECT did ,dname FROM tbdealer WHERE CONCAT (did,dname) LIKE '%" + txtSearchDealer.Text + "%'", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvDealer.Rows.Add(i, dr[0].ToString(), dr[1].ToString());
            }
            dr.Close();
            con.Close();
        }
        public void LoadProduct()
        {
            int i = 0;
            dgvProduct.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbproduct WHERE CONCAT(pid, pname, pprice, pdescription, pcategory) LIKE '%" + txtSearchPro.Text + "%'", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvProduct.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString());
            }
            dr.Close();
            con.Close();
        }

        private void dgvDealer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtdid.Text = dgvDealer.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtdname.Text = dgvDealer.Rows[e.RowIndex].Cells[2].Value.ToString();
        }

        private void dgvProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtpid.Text = dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtpname.Text = dgvProduct.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtprice.Text = dgvProduct.Rows[e.RowIndex].Cells[4].Value.ToString();
        }
        private void btninsertPurchase_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtdid.Text == "")
                {
                    MessageBox.Show("Please Select Dealer", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (txtpid.Text == "")
                {
                    MessageBox.Show("Please Select Product", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                

                if (MessageBox.Show("Are you sure you want to Insert this Purchase?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("INSERT INTO tbpurchase(pdate, pid, did, qty, price, total)VALUES(@pdate,@pid,@did,@qty,@price,@total)", con);
                    cm.Parameters.AddWithValue("@pdate", dtpPurchaseDate.Value);
                    cm.Parameters.AddWithValue("@pid", Convert.ToInt32(txtpid.Text));
                    cm.Parameters.AddWithValue("@did", Convert.ToInt32(txtdid.Text));
                    cm.Parameters.AddWithValue("@qty", Convert.ToInt32(nudQty.Value));
                    cm.Parameters.AddWithValue("@price", Convert.ToInt32(txtprice.Text));
                    cm.Parameters.AddWithValue("@total", Convert.ToInt32(txttotal.Text));
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Purchase has been successfully Inserted!");
                    int newqty = qty + Convert.ToInt32(nudQty.Value);
                    cm = new SqlCommand("UPDATE tbproduct SET pqty=" + newqty + " WHERE pid LIKE '" + txtpid.Text + "'", con);
                    cm.Parameters.AddWithValue("pqty", Convert.ToInt32(nudQty.Text));
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    clear();
                    LoadProduct();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void clear()
        {
            dtpPurchaseDate.Value = DateTime.Now;
            txtpid.Clear();
            txtdid.Clear();
            nudQty.Value = 0;
            txtprice.Clear();
            txttotal.Clear();
            txtpname.Clear();
            txtdname.Clear();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }
        public void Getqty()
        {
            cm = new SqlCommand("SELECT pqty FROM tbproduct WHERE pid ='" + txtpid.Text + "'", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                qty = Convert.ToInt32(dr[0].ToString());
            }
            dr.Close();
            con.Close();
        }

        private void txtSearchPro_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }

       private void txtSearchDealer_TextChanged(object sender, EventArgs e)
        {
            LoadDealer();
        }

        private void nudQty_ValueChanged(object sender, EventArgs e)
        {
            Getqty();
            if (Convert.ToInt16(nudQty.Value) > 0)
            {
                int total = Convert.ToInt32(txtprice.Text) * Convert.ToInt32(nudQty.Value);
                txttotal.Text = total.ToString();
            }
        }
    }
}
