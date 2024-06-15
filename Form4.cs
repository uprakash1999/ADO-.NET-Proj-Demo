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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            SqlConnection con = new SqlConnection(@"Data Source =(localdb)\MSSQLLocalDB; Initial Catalog = HOTEL_DB; Integrated Security = True ");
            int foodId = int.Parse(textBox1.Text);
            string q = "SELECT *FROM new_entry WHERE foodId = " + foodId;
            SqlCommand cmd = new SqlCommand(q, con);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    richTextBox1.AppendText("Food ID : " + reader["foodId"]+"\n");
                    richTextBox1.AppendText("Food Name : " + reader["foodName"] + "\n");
                    richTextBox1.AppendText("Food Type : " + reader["foodType"] + "\n");
                    richTextBox1.AppendText("Food Price : " + reader["foodPrice"] + "\n");
                }
            }
        }
    }  
}
