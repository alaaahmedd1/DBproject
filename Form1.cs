using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DBproject__Spa_Management_
{
    public partial class Form1 : Form
    {
        //string connection
        SqlConnection con = new SqlConnection("Data Source=localhost;Initial Catalog=SpaManagement;Integrated Security=True");

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'spaManagementDataSet.PRODUCT' table. You can move, or remove it, as needed.
            this.pRODUCTTableAdapter.Fill(this.spaManagementDataSet.PRODUCT);
            this.sESSIONTableAdapter.Fill(this.spaManagementDataSet.SESSION);
            this.cLIENTTableAdapter.Fill(this.spaManagementDataSet.CLIENT);
            try
            {
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();
            }
            catch (Exception ex) { MessageBox.Show("Connection Error: " + ex.Message); }
        }

        // Adding/registering user
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

        // Adding the booking
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
                cmd.Parameters.AddWithValue("@serviceid", (textBox5.Text));
                cmd.Parameters.AddWithValue("@therapistid", int.Parse(textBox11.Text));
                cmd.Parameters.AddWithValue("@date", textBox8.Text);
                cmd.Parameters.AddWithValue("@time", DateTime.Parse(textBox8.Text + " " + textBox6.Text));

                cmd.ExecuteNonQuery();
                MessageBox.Show("Booking Added Successfully!");
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        // Delete sessions => condition (status = completed)
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM SESSION WHERE STATUS = 'Completed'", con);
                int rows = cmd.ExecuteNonQuery();
                MessageBox.Show(rows + " Completed Session(s) Deleted Successfully!");
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }


        //delete product
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM PRODUCT WHERE QUANTITY <= REORDER_LIMIT", con);
                int rows = cmd.ExecuteNonQuery();
                MessageBox.Show(rows + " Low Stock Products are Deleted Successfully!");

                SqlCommand cmdSelect = new SqlCommand("SELECT * FROM PRODUCT", con);
                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView3.DataSource = dt;
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }



        // =========================
        // UPDATE SESSION STATUS
        // =========================
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE SESSION SET STATUS='Completed' WHERE SESSION_ID=@id", con);

                cmd.Parameters.AddWithValue("@id", int.Parse(textBox4.Text));

                cmd.ExecuteNonQuery();
                MessageBox.Show("Session Status Updated Successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // =========================
        // UPDATE CLIENT PHONE
        // =========================
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE CLIENT SET PHONE=@phone WHERE CLIENT_ID=@id", con);

                cmd.Parameters.AddWithValue("@phone", textBox10.Text);
                cmd.Parameters.AddWithValue("@id", int.Parse(textBox3.Text));

                cmd.ExecuteNonQuery();

                MessageBox.Show("Client Updated Successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // button7 => ShowClients
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM CLIENT", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // button8 => ShowSessions
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM SESSION", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView2.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // =========================
        // the 6 inqueries 
        // =========================

        // Inquiry 1  
        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = @"
            SELECT TOP 1
                s.SERVICE_ID,
                s.SERVICE_NAME,
                s.CATEGORY,
                COUNT(ss.SESSION_ID) AS total_bookings
            FROM SESSION ss
            JOIN SERVICE s ON ss.SERVICE_ID = s.SERVICE_ID
            WHERE ss.SESSIONDATE >= DATEADD(MONTH, -1, GETDATE())
              AND ss.STATUS = 'Completed'
            GROUP BY s.SERVICE_ID, s.SERVICE_NAME, s.CATEGORY
            ORDER BY total_bookings DESC";

                using (SqlDataAdapter da = new SqlDataAdapter(sql, con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView4.DataSource = dt;
                }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        // Inquiry 2 
        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = @"
            SELECT THERAPIST_ID, NAME
            FROM THERAPIST
            WHERE THERAPIST_ID NOT IN (
                SELECT THERAPIST_ID
                FROM SESSION
                WHERE SESSIONDATE >= DATEADD(MONTH, -1, GETDATE())
            )";

                using (SqlDataAdapter da = new SqlDataAdapter(sql, con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView4.DataSource = dt;
                }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }


        // Inquiry 3
        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = @"
            SELECT TOP 1
                c.CLIENT_ID,
                c.FIRST_NAME + ' ' + c.LAST_NAME AS full_name,
                c.EMAIL,
                c.MEMBERSHIP_TYPE,
                SUM(sv.PRICE) AS total_spent
            FROM SESSION ss
            JOIN CLIENT c   ON ss.CLIENT_ID  = c.CLIENT_ID
            JOIN SERVICE sv ON ss.SERVICE_ID = sv.SERVICE_ID
            WHERE sv.CATEGORY = 'Premium'
              AND ss.SESSIONDATE >= DATEADD(MONTH, -1, GETDATE())
              AND ss.STATUS = 'Completed'
            GROUP BY c.CLIENT_ID, c.FIRST_NAME, c.LAST_NAME,
                     c.EMAIL, c.MEMBERSHIP_TYPE
            ORDER BY total_spent DESC";

                using (SqlDataAdapter da = new SqlDataAdapter(sql, con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView4.DataSource = dt;
                }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        // Inquiry 4 
        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = @"
            SELECT
                sv.SERVICE_ID,
                sv.SERVICE_NAME,
                sv.CATEGORY,
                sv.PRICE,
                sv.DURATION
            FROM SERVICE sv
            LEFT JOIN SESSION ss
                ON ss.SERVICE_ID = sv.SERVICE_ID
               AND ss.SESSIONDATE >= DATEADD(MONTH, -1, GETDATE())
               AND ss.STATUS = 'Completed'
            WHERE ss.SESSION_ID IS NULL
            ORDER BY sv.CATEGORY, sv.SERVICE_NAME";

                using (SqlDataAdapter da = new SqlDataAdapter(sql, con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView4.DataSource = dt;
                }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        // Inquiry 5 
        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = @"
            SELECT
                sl.SPA_NAME,
                t.THERAPIST_ID,
                t.NAME
            FROM THERAPIST t
            JOIN SPA_LOCATION sl ON t.SPA_ID = sl.SPA_ID
            WHERE NOT EXISTS (
                SELECT 1 FROM SESSION s
                WHERE s.THERAPIST_ID = t.THERAPIST_ID
                AND s.SESSIONDATE >= DATEADD(MONTH, -1, GETDATE())
            )
            ORDER BY sl.SPA_NAME";

                using (SqlDataAdapter da = new SqlDataAdapter(sql, con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView4.DataSource = dt;
                }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        // Inquiry 6 
        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = @"
            SELECT
                t.THERAPIST_ID,
                t.NAME,
                t.EMAIL,
                t.PHONE,
                t.HIRE_DATE,
                sl.SPA_NAME,
                sl.CITY,
                COUNT(ss.SESSION_ID) AS total_completed_sessions
            FROM THERAPIST t
            JOIN SPA_LOCATION sl ON t.SPA_ID = sl.SPA_ID
            LEFT JOIN SESSION ss
                ON ss.THERAPIST_ID = t.THERAPIST_ID
               AND ss.STATUS = 'Completed'
            GROUP BY t.THERAPIST_ID, t.NAME, t.EMAIL,
                     t.PHONE, t.HIRE_DATE,
                     sl.SPA_NAME, sl.CITY
            ORDER BY total_completed_sessions DESC";

                using (SqlDataAdapter da = new SqlDataAdapter(sql, con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView4.DataSource = dt;
                }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

       
    }
}


