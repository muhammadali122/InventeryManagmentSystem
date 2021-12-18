using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ShopManagmentSystem
{
    public partial class UserForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\ShopManagmentSystem\ShopManagmentSystem\ShopDatabase.mdf;Integrated Security=True");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        public UserForm()
        {
            InitializeComponent();
            LoadUser();
        }
        public void LoadUser()
        {
            int i = 0;
            dgvuser.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbuser", con);
            con.Open();
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                i++;
                dgvuser.Rows.Add(i,dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString());
            }
            dr.Close();
            con.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            UserModuleForm userModule = new UserModuleForm();
            userModule.btnsave.Enabled = true;
            userModule.btnUpdate.Enabled = false;
            userModule.ShowDialog();
            LoadUser();
             
        }

        private void dgvuser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvuser.Columns[e.ColumnIndex].Name;
            if(colName=="Edit")
            {
                UserModuleForm userModule = new UserModuleForm();
                userModule.txtUsername.Text = dgvuser.Rows[e.RowIndex].Cells[1].Value.ToString();
                userModule.txtFullname.Text = dgvuser.Rows[e.RowIndex].Cells[2].Value.ToString();
                userModule.txtPassword.Text = dgvuser.Rows[e.RowIndex].Cells[3].Value.ToString();
                userModule.txtPhone.Text = dgvuser.Rows[e.RowIndex].Cells[4].Value.ToString();

                userModule.btnsave.Enabled = false;
                userModule.btnUpdate.Enabled = true;
                userModule.txtUsername.Enabled = false;
                userModule.ShowDialog();
            }
            else if(colName=="Delete")
            {
                if(MessageBox.Show("Are you sure you want to delete this User","Delete Record",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    con.Open();
                    cm = new SqlCommand("DELETE FROM tbuser WHERE username LIKE '"+dgvuser.Rows[e.RowIndex].Cells[1].Value.ToString()+"'",con);
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Record has been succesfully Deleted!");

                }
            }
            LoadUser();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
