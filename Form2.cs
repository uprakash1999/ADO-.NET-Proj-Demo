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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(@"Data Source =(localdb)\MSSQLLocalDB; Initial Catalog = HOTEL_DB; Integrated Security = True ");
        private void Form2_Load(object sender, EventArgs e)
        {

        }

        //Adding food
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string fn = textBox1.Text;
                string ft = "";
                try
                {
                    if (radioButton1.Checked)
                    {
                        ft = "Veg";
                    }
                    else if (radioButton2.Checked)
                    {
                        ft = "Non Veg";
                    }
                }
                catch
                {
                    MessageBox.Show("Choose appropriate food type");
                }
                float fp = float.Parse(textBox2.Text);
                string fa = comboBox1.Text;

                string q = "INSERT INTO new_entry(foodName, foodType, foodPrice, foodAvail) VALUES('" + fn + "','" + ft + "','" + fp + "','" + fa + "')";
                SqlCommand cmd = new SqlCommand(q, con);

                con.Open();

                int r = cmd.ExecuteNonQuery();
                if (r > 0)
                {
                    MessageBox.Show("Food Item Added");
                }
                else
                {
                    MessageBox.Show("Failed!");
                }
                // con.Close(); testing
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed Entry" + ex.Message);
            }
        }

        //Load Existing Food Items
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string fid = textBox3.Text;
                string q = "SELECT foodName, foodType, foodPrice, foodAvail FROM new_entry WHERE foodId = " + fid;
                con.Open();
                SqlCommand cmd = new SqlCommand(q, con);
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    textBox1.Text = rd["foodName"].ToString();
                    textBox2.Text = rd["foodPrice"].ToString();
                    comboBox1.Text = rd["foodAvail"].ToString();
                    if (rd["foodType"].ToString() == "Veg")
                    {
                        radioButton1.Checked = true;
                    }
                    else if (rd["foodType"].ToString() == "Non Veg")
                    {
                        radioButton2.Checked = true;
                    }

                }
                else
                {
                    MessageBox.Show("Food ID Not Found");
                }
                con.Close();
            }
            catch
            {
                MessageBox.Show("Not Found or Failed Entry");
            }


        }


        //Update Existing Food Item
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                int i = int.Parse(textBox3.Text);
                string fn = textBox1.Text;
                string ft = radioButton1.Checked ? "Veg" : (radioButton2.Checked ? "Non Veg" : "");
                float fp = float.Parse(textBox2.Text);
                string fa = comboBox1.Text;
                string q = "UPDATE new_entry SET foodName = " + fn + ", foodPrice = " + fp + ", foodAvail = " + fa + ", foodType = " + ft;
                SqlCommand cmd = new SqlCommand(q, con);
                con.Open();

                int r = cmd.ExecuteNonQuery();
                con.Close();

                if (r > 0)
                {
                    MessageBox.Show("Food Item Updated");
                }
                else
                {
                    MessageBox.Show("Update Failed!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update entry: " + ex.Message);
            }
        }


        //Deletion Code
        private void button2_Click(object sender, EventArgs e)
        {
            int i = int.Parse(textBox3.Text);
            string q = "DELETE FROM new_entry WHERE foodId = " + i;
            SqlCommand cmd = new SqlCommand(q, con);
            con.Open();
            int r = cmd.ExecuteNonQuery();
            con.Close();

            if (r > 0)
            {
                MessageBox.Show(i + " Food Item Deleted");
            }
            else
            {
                MessageBox.Show("Deletion Failed!");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source =(localdb)\MSSQLLocalDB; Initial Catalog = HOTEL_DB; Integrated Security = True ");
            string q = "SELECT *FROM new_entry";
            SqlCommand cmd = new SqlCommand(q, con);

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            sda.Fill(ds);
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "table";
        }
    }
}
