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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            {
                // Reset to the first tab if all tabs have been visited
                //currentTabIndex = 0;
                button2.Text = "save";
            }

            tabControl1.SelectedIndex = currentTabIndex;

            if (button2.Text.ToLower() == "save")
            {
                DialogResult result = MessageBox.Show("Do you want to save this reservation?", "Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    if (dbManager.Guestcon().State == ConnectionState.Closed)
                    {
                        dbManager.Guestcon().Open();
                    }

                    string insertQuery = "INSERT INTO guests (Name, contact, Email, address, nationality, dob, idtype, idnumber, special_requirements) VALUES (@Name, @Contact, @Email, @Address, @Nationality, @DOB, @IDType, @IDNumber, @SpecialRequirements)";

                    using (SqlCommand command = new SqlCommand(insertQuery, dbManager.Guestcon()))
                    {
                        command.Parameters.AddWithValue("@Name", cbGuestName.Text);
                        command.Parameters.AddWithValue("@Email", textBox1.Text);
                        command.Parameters.AddWithValue("@Address", cbAddress.Text);
                        command.Parameters.AddWithValue("@Nationality", cbCountry.Text);
                        command.Parameters.AddWithValue("@DOB", dpDOB.Value.Date);
                        command.Parameters.AddWithValue("@IDType", cbIDtype.SelectedItem.ToString());
                        command.Parameters.AddWithValue("@IDNumber", txtid.Text);
                        command.Parameters.AddWithValue("@SpecialRequirements", textBox2.Text);
                        command.Parameters.AddWithValue("@Contact", txtContact.Text);

                        // Ensure the connection is open before executing the command
                        if (command.Connection.State == ConnectionState.Closed)
                        {
                            command.Connection.Open();
                        }

                        // Execute the query
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Success");
                        }
                    }

                }
                //else if (result == DialogResult.No)
                //{
                //    // User clicked No, handle the rejection or perform alternative action
                //    // Add your code here
                //}
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            currentTabIndex--;
            if (currentTabIndex < 0)
                currentTabIndex = tabControl1.TabCount - 1;
                 button2.Text = "Next";

            tabControl1.SelectedIndex = currentTabIndex;
        }
        private void display()
        {
            string selectQuery = "SELECT * FROM roomtypes";
            DataTable results = dbManager.ExecuteQuery(selectQuery);

            cbCountry.Items.Clear(); // Clear existing items in the ComboBox

            foreach (DataRow row in results.Rows)
            {
                string roomType = row["name"].ToString(); // Replace "RoomType" with the actual column name

                // Add the room type to the ComboBox items
                cbCountry.Items.Add(roomType);
            }

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
            display();
        }
    }
}
