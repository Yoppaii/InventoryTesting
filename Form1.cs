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


namespace Inventory2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent(); 
        }

        SqlConnection con = new SqlConnection(@"Data Source=.\sqlexpress;Initial Catalog=Inventory2DB;Integrated Security=True");
        SqlCommand cmd;
        SqlDataReader reader;
        SqlDataAdapter adapter;

        string id;
        bool Mode = true;
        

        private void button1_Click(object sender, EventArgs e)
        {


            if (Mode == true)
            {
                if (txtName.Text == "" || txtPrice.Text == "" || txtStocks.Text == "" || cmbCategory.Text == "")
                {
                    MessageBox.Show("Fill up the information", "Required", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else
                {
                    try
                    {
                        con.Open();
                        cmd = new SqlCommand("INSERT INTO product_TB (name, price, stocks, category) values (@name, @price, @stocks, @category)", con);
                        cmd.Parameters.AddWithValue("name", txtName.Text);
                        cmd.Parameters.AddWithValue("price", int.Parse(txtPrice.Text));
                        cmd.Parameters.AddWithValue("stocks", int.Parse(txtStocks.Text));
                        cmd.Parameters.AddWithValue("category", cmbCategory.Text);

                        MessageBox.Show("Product added");

                        cmd.ExecuteNonQuery();

                        cmd = new SqlCommand("SELECT * FROM product_TB", con);
                        adapter = new SqlDataAdapter(cmd);
                        reader = cmd.ExecuteReader();

                        dataGridView1.Rows.Clear();

                        while (reader.Read())
                        {
                            dataGridView1.Rows.Add(reader[0], reader[1], reader[2], reader[3], reader[4]);
                        }

                        txtName.Clear();
                        txtPrice.Clear();
                        txtStocks.Clear();
                        cmbCategory.ResetText();
                        txtName.Focus();
                    }
                    catch(Exception)
                    {
                        MessageBox.Show("Enter valid data type");
                    }                    
                }

            }

            else
            {
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                con.Open();

                cmd = new SqlCommand("UPDATE product_TB SET name = @name, price = @price, stocks = @stocks, category = @category WHERE id = @id", con);
                cmd.Parameters.AddWithValue("name", txtName.Text);
                cmd.Parameters.AddWithValue("price", int.Parse(txtPrice.Text));
                cmd.Parameters.AddWithValue("stocks", int.Parse(txtStocks.Text));
                cmd.Parameters.AddWithValue("category", cmbCategory.Text);
                cmd.Parameters.AddWithValue("id", id);

                MessageBox.Show("Product updated");

                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("SELECT * FROM product_TB", con);
                adapter = new SqlDataAdapter(cmd);
                reader = cmd.ExecuteReader();

                dataGridView1.Rows.Clear();

                while (reader.Read())
                {
                    dataGridView1.Rows.Add(reader[0], reader[1], reader[2], reader[3], reader[4]);
                }

                txtName.Clear();
                txtPrice.Clear();
                txtStocks.Clear();
                txtName.Focus();
               
            }
            con.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            try
            {
                con.Open();
                
                cmd = new SqlCommand("SELECT * FROM product_TB", con);
                adapter = new SqlDataAdapter(cmd);
                reader = cmd.ExecuteReader();

                dataGridView1.Rows.Clear();

                while(reader.Read())
                {
                    dataGridView1.Rows.Add(reader[0], reader[1], reader[2], reader[3], reader[4]);
                }

                con.Close();

            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void getID(string id)
        {
            con.Open();

            cmd = new SqlCommand("SELECT * FROM product_TB WHERE id = '"+id+"'", con);
            reader = cmd.ExecuteReader();

            while(reader.Read())
            {
                txtName.Text = reader[1].ToString();
                txtPrice.Text = reader[2].ToString();
                txtStocks.Text = reader[3].ToString();
                cmbCategory.Text = reader[4].ToString();
            }
            con.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == dataGridView1.Columns["Edit"].Index && e.RowIndex >= 0)
            {
                Mode = false;
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();

                getID(id );

                btnAdd.Text = "Update";
            }
            else if (e.ColumnIndex == dataGridView1.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                if(MessageBox.Show("Confrim to delete","Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    con.Open();

                    Mode = false;
                    id = dataGridView1.CurrentRow.Cells[0].Value.ToString();

                    cmd = new SqlCommand("DELETE FROM product_TB WHERE id = @id ", con);
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Item deleted");

                    cmd = new SqlCommand("SELECT * FROM product_TB", con);
                    adapter = new SqlDataAdapter(cmd);
                    reader = cmd.ExecuteReader();

                    dataGridView1.Rows.Clear();

                    while (reader.Read())
                    {
                        dataGridView1.Rows.Add(reader[0], reader[1], reader[2], reader[3], reader[4]);
                    }

                    con.Close();
                }
                else
                {

                }
                
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtName.Clear();
            txtPrice.Clear();
            txtStocks.Clear();
            cmbCategory.ResetText();
            txtName.Focus();
            btnAdd.Text = "Add";

            Mode = true;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();

                cmd = new SqlCommand("SELECT * FROM product_TB", con);
                adapter = new SqlDataAdapter(cmd);
                reader = cmd.ExecuteReader();

                dataGridView1.Rows.Clear();

                while (reader.Read())
                {
                    dataGridView1.Rows.Add(reader[0], reader[1], reader[2], reader[3], reader[4]);
                }
                con.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();

                cmd = new SqlCommand("SELECT * FROM product_TB WHERE Name LIKE '%" + txtSearch.Text + "%' or Price LIKE '%"+txtSearch.Text+ "%' or Stocks LIKE '%" + txtSearch.Text + "%' or Category LIKE '%" + txtSearch.Text + "%' ", con);
                //cmd.Parameters.AddWithValue("@Name", txtSearch.Text);
                //cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = txtSearch.Text;
                adapter = new SqlDataAdapter(cmd);
                reader = cmd.ExecuteReader();

                dataGridView1.Rows.Clear();

                while (reader.Read())
                {
                    dataGridView1.Rows.Add(reader[0], reader[1], reader[2], reader[3], reader[4]);
                }
                con.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)13)
            {
                btnSearch.PerformClick();
            }
        }
    }
}
