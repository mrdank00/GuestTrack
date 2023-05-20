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

namespace GuestTrack
{
    public partial class frmCreateRooms : Form
    {
        DatabaseManager dbManager = new DatabaseManager();
        public frmCreateRooms()
        {
            InitializeComponent();
        }

        private void frmCreateRooms_Load(object sender, EventArgs e)
        {
            Display();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dbManager.Guestcon().State == ConnectionState.Closed)
            {
                dbManager.Guestcon().Open();
            }

            string insertQuery = "INSERT INTO Rooms (Name, description) VALUES (@Name, @description)";

            using (SqlConnection connection = dbManager.Guestcon())
            {
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    // Add parameters to the command
                    command.Parameters.AddWithValue("@Name", textBox1.Text);
                    command.Parameters.AddWithValue("@description", textBox2.Text);

                    // Open the connection if it's closed (optional)
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    // Execute the query
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Display();
                        MessageBox.Show("Success");
                    }
                }
            }
        }
        private void Display()
        {
            string selectQuery = "SELECT * FROM rooms";
            DataTable results = dbManager.ExecuteQuery(selectQuery);

            dataGridView1.DataSource = results;

        }
    }
}
