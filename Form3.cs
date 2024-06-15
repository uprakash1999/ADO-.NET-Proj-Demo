using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyProj
{
    public partial class Form3 : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source =(localdb)\MSSQLLocalDB; Initial Catalog = HOTEL_DB; Integrated Security = True ");
        public Form3()
        {
            InitializeComponent();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void Form3_Load(object sender, EventArgs e)
        {

            string q = "SELECT foodName, foodPrice, foodAvail FROM new_entry";
            comboBox1.Items.Clear();
            con.Open();

            SqlCommand cmd = new SqlCommand(q, con);

            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                comboBox1.Items.Add(dr["foodName"]);
            }
            con.Close();

            Random rnd = new Random();
            textBox1.Text = rnd.Next(1000, 9999).ToString();


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedFood = comboBox1.SelectedItem.ToString();
            string q = $"SELECT *FROM new_entry WHERE foodName = '{selectedFood}'";
            con.Open();
            SqlCommand cmd = new SqlCommand(q, con);
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();


            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                textBox2.Text = dr["foodPrice"].ToString();
                textBox3.Text = "1";
            }


            /*foreach (DataRow dr in dt.Rows)
            {
                textBox2.Text = dr["foodPrice"].ToString();

            }*/


        }


        //Get Amount Button
        private void button3_Click(object sender, EventArgs e)
        {
            int qty = int.Parse(textBox3.Text);
            string selectedFood = comboBox1.SelectedItem.ToString();
            string q = $"SELECT *FROM new_entry WHERE foodName = '{selectedFood}'";
            con.Open();
            SqlCommand cmd = new SqlCommand(q, con);
            SqlDataReader rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                decimal price = (decimal)rd["foodPrice"];
                textBox2.Text = price.ToString();
                textBox4.Text = (price * qty).ToString();
            }
            else
            {
                MessageBox.Show("Food Item not found!");
            }
            rd.Close();
            con.Close();

        }
        
        private void DisplayBillingData()
        {
            string q = "SELECT b.BillNo, b.BillDate, n.foodName, b.Price, b.Quantity, b.Amount FROM billing b INNER JOIN new_entry n ON b.foodId = foodId";
            SqlCommand cmd = new SqlCommand(q, con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        //Billing Add Button
        private void button1_Click(object sender, EventArgs e)
        {
            int billno = int.Parse(textBox1.Text);
            string selectedFood = comboBox1.SelectedItem.ToString();
            decimal foodPrice = decimal.Parse(textBox2.Text);
            int quantity = int.Parse(textBox3.Text);
            decimal amount = decimal.Parse(textBox4.Text);

            string q = "SELECT foodId FROM new_entry WHERE foodName = '{selectedFood}'";
            con.Open();
            SqlCommand cmd = new SqlCommand(q, con);
            int foodId = (int)cmd.ExecuteScalar();
            con.Close();

            string q1 = $"INSERT INTO billing  (BillNo, foodId, Price, Quantity, Amount) VALUES({billno}, {foodId}, {foodPrice}, {quantity}, {amount})";
            SqlCommand cmd1 = new SqlCommand(q1, con);

            con.Open();
            cmd1.ExecuteNonQuery();
            con.Close();

            DisplayBillingData();
        }
    }
}
