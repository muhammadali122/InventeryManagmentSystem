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
    public partial class DealerForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\ShopManagmentSystem\ShopManagmentSystem\ShopDatabase.mdf;Integrated Security=True");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        public DealerForm()
        {
            InitializeComponent();
            loadDealer();
        }

        public void loadDealer()
        {
            int i = 0;
            dgvDealer.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbdealer", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvDealer.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString());
            }
            dr.Close();
            con.Close();
        }

        private void dgvDealer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvDealer.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                DealerModuleForm customerModule = new DealerModuleForm();
                customerModule.lbldid.Text = dgvDealer.Rows[e.RowIndex].Cells[1].Value.ToString();
                customerModule.txtDname.Text = dgvDealer.Rows[e.RowIndex].Cells[2].Value.ToString();
                customerModule.txtDphone.Text = dgvDealer.Rows[e.RowIndex].Cells[3].Value.ToString();

                customerModule.btnsave.Enabled = false;
                customerModule.btnUpdate.Enabled = true;
                customerModule.ShowDialog();
            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this Dealer", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    con.Open();
                    cm = new SqlCommand("DELETE FROM tbdealer WHERE did LIKE '" + dgvDealer.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", con);
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Record has been succesfully Deleted!");

                }
            }
            loadDealer();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DealerModuleForm moduleForm = new DealerModuleForm();
            moduleForm.btnsave.Enabled = true;
            moduleForm.btnUpdate.Enabled = false;
            moduleForm.ShowDialog();
            loadDealer();
        }
    }
}
