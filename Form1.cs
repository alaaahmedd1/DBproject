using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DBproject__Spa_Management_
{
    public partial class Form1 : Form
    {
        // نص الاتصال
        SqlConnection con = new SqlConnection("Data Source=localhost;Initial Catalog=SpaManagement;Integrated Security=True");

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();
            }
            catch (Exception ex) { MessageBox.Show("Connection Error: " + ex.Message); }
        }

        // زر Register (تأكد أن اسمه البرمجي button4)
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO CLIENT (CLIENT_ID, PHONE, FIRST_NAME, LAST_NAME, EMAIL, MEMBERSHIP_DATE, MEMBERSHIP_TYPE) " +
                    "VALUES (@id, @phone, @fname, @lname, @email, GETDATE(), @mtype)", con);

                cmd.Parameters.AddWithValue("@id", int.Parse(textBox3.Text));
                cmd.Parameters.AddWithValue("@phone", textBox10.Text);
                cmd.Parameters.AddWithValue("@fname", textBox1.Text);
                cmd.Parameters.AddWithValue("@lname", textBox7.Text);
                cmd.Parameters.AddWithValue("@email", textBox2.Text);
                cmd.Parameters.AddWithValue("@mtype", textBox12.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Client Registered Successfully!");
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        // زر AddBooking (تأكد أن اسمه البرمجي button3)
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO SESSION (SESSION_ID, CLIENT_ID, SPA_ID, SERVICE_ID, THERAPIST_ID, SESSIONDATE, SESSION_TIME, STATUS) " +
                    "VALUES (@sessionid, @clientid, @spaid, @serviceid, @therapistid, @date, @time, 'Active')", con);

                cmd.Parameters.AddWithValue("@sessionid", int.Parse(textBox4.Text));
                cmd.Parameters.AddWithValue("@clientid", int.Parse(textBox3.Text));
                cmd.Parameters.AddWithValue("@spaid", int.Parse(textBox9.Text));
                cmd.Parameters.AddWithValue("@serviceid", int.Parse(textBox5.Text));
                cmd.Parameters.AddWithValue("@therapistid", int.Parse(textBox11.Text));
                cmd.Parameters.AddWithValue("@date", textBox8.Text);
                cmd.Parameters.AddWithValue("@time", textBox6.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Booking Added Successfully!");
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        // زر DeleteSession (تأكد أن اسمه البرمجي button2)
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM SESSION WHERE STATUS = 'Cancelled'", con);
                int rows = cmd.ExecuteNonQuery();
                MessageBox.Show(rows + " Cancelled Session(s) Deleted Successfully!");
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        // زر DeleteProduct (تأكد أن اسمه البرمجي button1)
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM PRODUCT WHERE QUANTITY = 0", con);
                int rows = cmd.ExecuteNonQuery();
                MessageBox.Show(rows + " Out of Stock Product(s) Deleted Successfully!");
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }
    }
}