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

namespace GuestTrack
{
    public partial class frmRoomReservation : Form
    {
        DatabaseManager dbManager = new DatabaseManager();
        public frmRoomReservation()
        {
            InitializeComponent();
            dateTimePicker2.Format = DateTimePickerFormat.Time;
            dateTimePicker2.ShowUpDown = true;
            dateTimePicker5.Format = DateTimePickerFormat.Time;
            dateTimePicker5.ShowUpDown = true;
          
    }
        private int currentTabIndex = 0;
        private void frmRoomReservation_Load(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            currentTabIndex++;
            if (currentTabIndex >= tabControl1.TabCount)
                //currentTabIndex = 0;
                button2.Text = "Save";
            DialogResult result = MessageBox.Show("Do you want to proceed?", "Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                string insertQuery = "INSERT INTO Reservation (Name, Email) VALUES (@Name, @Email)";

                // Create a new SqlCommand object
                using (SqlCommand command = new SqlCommand(insertQuery, dbManager.Guestcon()))
                {
                    // Add parameters to the command
                    command.Parameters.AddWithValue("@Name", "John Doe");
                    command.Parameters.AddWithValue("@Email", "john@example.com");

                    // Execute the query
                    int rowsAffected = command.ExecuteNonQuery();
                }

            }
            else if (result == DialogResult.No)
            {
                // User clicked No, handle the rejection or perform alternative action
                // Add your code here
            }


            tabControl1.SelectedIndex = currentTabIndex;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            currentTabIndex--;
            if (currentTabIndex < 0)
                currentTabIndex = tabControl1.TabCount - 1;
                 button2.Text = "Next";

            tabControl1.SelectedIndex = currentTabIndex;
        }
    }
}
