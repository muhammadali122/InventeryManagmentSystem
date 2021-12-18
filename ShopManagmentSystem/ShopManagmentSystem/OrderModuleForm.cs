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
    
    public partial class OrderModuleForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\hp\Documents\dbMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        int qty = 0;
        public OrderModuleForm()
        {
            InitializeComponent();
            LoadCustomer();
            LoadProduct();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        public void LoadCustomer()
        {
            int i = 0;
            dgvCustomer.Rows.Clear();
            cm = new SqlCommand("SELECT cid ,cname FROM tbcustomer WHERE CONCAT (cid,cname) LIKE '%"+txtSearchCust.Text+"%'", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvCustomer.Rows.Add(i, dr[0].ToString(), dr[1].ToString());
            }
            dr.Close();
            con.Close();
        }

        public void LoadProduct()
        {
            int i = 0;
            dgvProducts.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbproduct WHERE CONCAT(pid, pname, pprice, pdescription, pcategory) LIKE '%" + txtSearchPro.Text + "%'", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvProducts.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString());
            }
            dr.Close();
            con.Close();
        }

        private void txtSearchCust_TextChanged(object sender, EventArgs e)
        {
            LoadCustomer();
        }

        private void txtSearchPro_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Getqty();
            if (Convert.ToInt16(nudQty.Value)>qty)
            {
                MessageBox.Show("Instock Quantity is not enough", "warnign", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nudQty.Value = nudQty.Value-1;
                return;
            }
            if (Convert.ToInt16(nudQty.Value) > 0)
            {
                int total = Convert.ToInt16(txtprice.Text) * Convert.ToInt16(nudQty.Value);
                txttotal.Text = total.ToString();
            }
        }

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtcid.Text = dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtcname.Text = dgvCustomer.Rows[e.RowIndex].Cells[2].Value.ToString();

        }

        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtpid.Text = dgvProducts.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtpname.Text = dgvProducts.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtprice.Text = dgvProducts.Rows[e.RowIndex].Cells[4].Value.ToString();
        }

        private void btninsertOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtcid.Text=="")
                {
                    MessageBox.Show("Please Select Customer", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if(txtpid.Text=="")
                {
                    MessageBox.Show("Please Select Product", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
               
                if (MessageBox.Show("Are you sure you want to Insert this Order?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("INSERT INTO tborder(odate, pid, cid, qty, price, total)VALUES(@odate,@pid,@cid,@qty,@price,@total)", con);
                    cm.Parameters.AddWithValue("@odate", dtpOrderDate.Value);
                    cm.Parameters.AddWithValue("@pid", Convert.ToInt32 (txtpid.Text));
                    cm.Parameters.AddWithValue("@cid", Convert.ToInt32 (txtcid.Text));
                    cm.Parameters.AddWithValue("@qty", Convert.ToInt32 (nudQty.Value));
                    cm.Parameters.AddWithValue("@price", Convert.ToInt32 (txtprice.Text));
                    cm.Parameters.AddWithValue("@total", Convert.ToInt32 (txttotal.Text));
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Order has been successfully Inserted!");

                    cm = new SqlCommand("UPDATE tbproduct SET pqty=(pqty-@pqty) WHERE pid LIKE '" + txtpid.Text + "'", con);
                    cm.Parameters.AddWithValue("@pqty", Convert.ToInt16(nudQty.Text));
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
            dtpOrderDate.Value=DateTime.Now; 
            txtpid.Clear();
            txtcid.Clear();
            nudQty.Value=1;
            txtprice.Clear();
            txttotal.Clear();
            txtpname.Clear();
            txtcname.Clear();
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

        private void dgvProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}