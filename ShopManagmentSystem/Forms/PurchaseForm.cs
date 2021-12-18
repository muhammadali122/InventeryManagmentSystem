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
    public partial class PurchaseForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\ShopManagmentSystem\ShopManagmentSystem\ShopDatabase.mdf;Integrated Security=True");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        public PurchaseForm()
        {
            InitializeComponent();
            LoadPurchase();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            PurchaseModuleForm orderModule = new PurchaseModuleForm();
            orderModule.ShowDialog();
            LoadPurchase();
        }
        public void LoadPurchase()
        {
            double total = 0;
            int i = 0;
            dgvPurchase.Rows.Clear();
            cm = new SqlCommand("SELECT PurchaseId, pdate, O.pid, P.pname, O.did, D.dname,qty, price, total  FROM tbpurchase AS O JOIN tbdealer  AS D ON O.did=D.did JOIN tbproduct AS P ON O.pid=P.pid WHERE CONCAT (PurchaseId, pdate, O.pid, P.pname, O.did, D.dname,qty, price) LIKE '%" + txtSearch.Text + "%'", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvPurchase.Rows.Add(i, dr[0].ToString(), Convert.ToDateTime(dr[1].ToString()).ToString("dd/MM/yyyy"), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString(), dr[8].ToString());
                total += Convert.ToInt32(dr[8].ToString());
            }
            dr.Close();
            con.Close();

            lblqty.Text = i.ToString();
            lbltotal.Text = total.ToString();
        }

        private void dgvPurchase_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvPurchase.Columns[e.ColumnIndex].Name;

            if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this Purchase", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    con.Open();
                    cm = new SqlCommand("DELETE FROM tbpurchase WHERE PurchaseId LIKE '" + dgvPurchase.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", con);
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Record has been succesfully Deleted!");

                    cm = new SqlCommand("UPDATE tbproduct SET pqty=(pqty-@pqty) WHERE pid LIKE '" + dgvPurchase.Rows[e.RowIndex].Cells[3].Value.ToString() + "'", con);
                    cm.Parameters.AddWithValue("@pqty", Convert.ToInt32(dgvPurchase.Rows[e.RowIndex].Cells[7].Value.ToString()));
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();

                }
            }
            LoadPurchase();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadPurchase();
        }
    }
}
