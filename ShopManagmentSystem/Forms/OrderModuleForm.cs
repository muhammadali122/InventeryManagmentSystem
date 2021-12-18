using ShopManagmentSystem.Models;
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
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\ShopManagmentSystem\ShopManagmentSystem\ShopDatabase.mdf;Integrated Security=True");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        int qty = 0;
        public OrderModuleForm()
        {
            InitializeComponent();
            LoadCustomer();
            LoadProduct();
        }
        private List<OrderList> PrintingOrder = new List<OrderList>();
        decimal TotalAmount = 0;
        decimal SalesTax = 0;
        decimal TotalToPay = 0;
        int NumbrOfOtemPerPage=0;
        int NumberOfItemPrinted = 0;

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        public void LoadCustomer()
        {
            int i = 0;
            dgvCustomer.Rows.Clear();
            cm = new SqlCommand("SELECT cid ,cname FROM tbcustomer WHERE CONCAT (cid,cname) LIKE '%" + txtSearchCust.Text + "%'", con);
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
            cm = new SqlCommand("SELECT * FROM tbproduct WHERE CONCAT (pid, pname, pcategory, pdescription) LIKE '%" + txtSearchPro.Text + "%'", con);
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


        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Getqty();
            if (Convert.ToInt16(nudQty.Value) > qty)
            {
                MessageBox.Show("Instock Quantity is not enough", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nudQty.Value = nudQty.Value - 1;
                return;
            }
            if (Convert.ToInt16(nudQty.Value) > 0)
            {
                var total = Convert.ToInt16(txtprice.Text) * Convert.ToInt16(nudQty.Value);
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
                if (txtcname.Text == "")
                {
                   MessageBox.Show("Please Select Customer", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (txtpid.Text == "")
                {
                    MessageBox.Show("Please Select Product!!!", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if(nudQty.Value == 0)
                {
                    MessageBox.Show("Please Select Quantity!!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Are you sure you want to Insert this Order?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("INSERT INTO tborder(odate, pid, cid, qty, price, total)VALUES(@odate,@pid,@cid,@qty,@price,@total)", con);
                    cm.Parameters.AddWithValue("@odate", dtpOrderDate.Value);
                    cm.Parameters.AddWithValue("@pid", Convert.ToInt32(txtpid.Text));
                    cm.Parameters.AddWithValue("@cid", Convert.ToInt32(txtcid.Text));
                    cm.Parameters.AddWithValue("@qty", Convert.ToInt32(nudQty.Value));
                    cm.Parameters.AddWithValue("@price", Convert.ToInt32(txtprice.Text));
                    cm.Parameters.AddWithValue("@total", Convert.ToInt32(txttotal.Text));
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    int newqty = qty - Convert.ToInt32(nudQty.Value);
                    OrderList order = new OrderList()
                    {
                        ProductName = txtpname.Text,
                        Quanity=nudQty.Value,
                        UnitPrice=Convert.ToDecimal( txtprice.Text.Trim()),
                        TotalPrice=Convert.ToDecimal(txttotal.Text.Trim()),
                    };
                    PrintingOrder.Add(order);
                    dgvPrint.DataSource = null;
                    dgvPrint.DataSource = PrintingOrder;
                   
                    txtpname.Clear();
                    nudQty.Value = 0;
                    txtprice.Clear();


                    cm = new SqlCommand("UPDATE tbproduct SET pqty="+newqty+" WHERE pid LIKE '" + txtpid.Text + "'", con);
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
            dtpOrderDate.Value = DateTime.Now;
            txtpid.Clear();
           
            nudQty.Value = 0;
            txtprice.Clear();
            txttotal.Clear();
            txtpname.Clear();
           
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

        private void txtSearchCust_TextChanged_1(object sender, EventArgs e)
        {
            LoadCustomer();
        }
        private void txtSearchPro_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }


        private void btnPrintOrder_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }
      
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString("UNIVERSITY SHOP MANAGMENT" , new Font("Times New Roman", 22, FontStyle.Bold), Brushes.Black, new Point(180, 50));
            e.Graphics.DrawString("_________________________________" , new Font("Times New Roman", 22, FontStyle.Bold), Brushes.Black, new Point(180, 55));
            e.Graphics.DrawString(" SYSTEM ", new Font("Times New Roman", 22, FontStyle.Bold), Brushes.Black, new Point(350, 90));
            e.Graphics.DrawString(" ________", new Font("Times New Roman", 22, FontStyle.Bold), Brushes.Black, new Point(350, 95));
            e.Graphics.DrawString("Near UOM Chakdara Lower Dir KPK ", new Font("Times New Roman", 10, FontStyle.Bold), Brushes.Black, new Point(310, 130));
            e.Graphics.DrawString("Contact No: +92342-1245639 ", new Font("Times New Roman", 10, FontStyle.Bold), Brushes.Black, new Point(340, 150));
            e.Graphics.DrawString("Email: Uomshop11@gmail.com ", new Font("Times New Roman", 10, FontStyle.Bold), Brushes.Black, new Point(340, 170));
            e.Graphics.DrawString(lblDashLine.Text, new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(25, 200));
            e.Graphics.DrawString("Client Name: " + txtcname.Text.Trim(), new Font("Modern 20", 12, FontStyle.Bold), Brushes.Black, new Point(25, 230));
            e.Graphics.DrawString("Date: " + DateTime.Now, new Font("Modern 20", 12, FontStyle.Bold), Brushes.Black, new Point(25, 250));
            e.Graphics.DrawString(lblDashLine.Text, new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(25,280));
            e.Graphics.DrawString("Product Name ", new Font("Modern 20", 12, FontStyle.Bold), Brushes.Black, new Point(25, 310));
            e.Graphics.DrawString("Quantity ", new Font("Modern 20", 12, FontStyle.Bold), Brushes.Black, new Point(300, 310));
            e.Graphics.DrawString("Unit Price ", new Font("Modern 20", 12, FontStyle.Bold), Brushes.Black, new Point(500, 310));
            e.Graphics.DrawString("Total Price ", new Font("Modern 20", 12, FontStyle.Bold), Brushes.Black, new Point(700, 310));
            e.Graphics.DrawString(lblDashLine.Text, new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(25, 330));
            int YPosition = 350;
            for(int i=NumberOfItemPrinted; i<PrintingOrder.Count; i++)
            {
                NumbrOfOtemPerPage ++;
                if (NumbrOfOtemPerPage <=20)
                {
                    NumberOfItemPrinted++;
                    if(NumberOfItemPrinted<= PrintingOrder.Count)
                    {
                        e.Graphics.DrawString(PrintingOrder[i].ProductName, new Font("Modern 20", 12, FontStyle.Bold), Brushes.Black, new Point(25, YPosition));
                        e.Graphics.DrawString(PrintingOrder[i].Quanity.ToString(), new Font("Modern 20", 12, FontStyle.Bold), Brushes.Black, new Point(300, YPosition));
                        e.Graphics.DrawString(PrintingOrder[i].UnitPrice.ToString(), new Font("Modern 20", 12, FontStyle.Bold), Brushes.Black, new Point(500, YPosition));
                        e.Graphics.DrawString(PrintingOrder[i].TotalPrice.ToString(), new Font("Modern 20", 12, FontStyle.Bold), Brushes.Black, new Point(700, YPosition));
                        YPosition += 30;
                        TotalAmount += PrintingOrder[i].TotalPrice;
                    }
                    else
                    {
                        e.HasMorePages = false;
                    }
                   
                }
                else
                {
                    NumbrOfOtemPerPage = 0;
                    e.HasMorePages = true;
                    return;
                }
            }
            SalesTax = (0 * TotalAmount) / 100;
            TotalToPay = TotalAmount + SalesTax;
            e.Graphics.DrawString(lblDashLine.Text, new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(25, YPosition));
            e.Graphics.DrawString("Total Amount:          ₨. /- "+ TotalAmount, new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(530, YPosition+30));
            e.Graphics.DrawString("Sales Tax (0.00%):  ₨. /- "+ SalesTax, new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(530, YPosition + 60));
            e.Graphics.DrawString("Total To Pay:           ₨. /- "+ TotalToPay, new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(530, YPosition + 90));
            NumbrOfOtemPerPage = 0;
            NumberOfItemPrinted = 0;
        }

        private void btnprint_Click(object sender, EventArgs e)
        {
            printDocument1.Print();
        }
    }
}