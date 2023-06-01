using GuestTrack.Controllers;
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
using System.Xml.Linq;
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
            display();
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            currentTabIndex++;
            if (currentTabIndex >= tabControl1.TabCount - 1)
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
                    try
                    {
                       
                    }
                    catch (Exception ex)
                    {
                        // Handle the exception (e.g., display an error message)
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                    finally
                    {
                        // Make sure to close the connection
                        dbManager.Guestcon().Close();
                    }

                    //else if (result == DialogResult.No)
                    //{
                    //    // User clicked No, handle the rejection or perform alternative action
                    //    // Add your code here
                    //}
                }
            }
        }
        private void guest()
        {
            string mergeQuery = @"
                        MERGE INTO guests AS target
                        USING (SELECT @Name AS Name, @Contact AS Contact, @Email AS Email, @Address AS Address, @Nationality AS Nationality, @DOB AS DOB, @IDType AS IDType, @IDNumber AS IDNumber, @SpecialRequirements AS SpecialRequirements) AS source
                        ON (target.contact = source.Contact)
                        WHEN MATCHED THEN
                            UPDATE SET
                                target.Name = source.Name,
                                target.Email = source.Email,
                                target.Address = source.Address,
                                target.Nationality = source.Nationality,
                                target.DOB = source.DOB,
                                target.IDType = source.IDType,
                                target.IDNumber = source.IDNumber,
                                target.SpecialRequirements = source.SpecialRequirements
                        WHEN NOT MATCHED THEN
                            INSERT (Name, Contact, Email, Address, Nationality, DOB, IDType, IDNumber, SpecialRequirements)
                            VALUES (source.Name, source.Contact, source.Email, source.Address, source.Nationality, source.DOB, source.IDType, source.IDNumber, source.SpecialRequirements);
                    ";
            using (SqlConnection connection = dbManager.Guestcon())
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(mergeQuery, connection))
                {
                    // Add parameters to the command
                    command.Parameters.AddWithValue("@Name", cbGuestName.Text);
                    command.Parameters.AddWithValue("@Contact", txtContact.Text);
                    command.Parameters.AddWithValue("@Email", txtemail.Text);
                    command.Parameters.AddWithValue("@Address", cbAddress.Text);
                    command.Parameters.AddWithValue("@Nationality", cbCountry.Text);
                    command.Parameters.AddWithValue("@DOB", dpDOB.Value);
                    command.Parameters.AddWithValue("@IDType", cbIDtype.Text);
                    command.Parameters.AddWithValue("@IDNumber", txtid.Text);
                    command.Parameters.AddWithValue("@SpecialRequirements", textBox2.Text);




                    // Execute the query
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Success");
                    }
                }
            }
        }
        private void reserve()
        {
            //int reservationId = int.Parse("1");
            //int hotelId = int.Parse("1");
            //int roomId = int.Parse(txtRoomId.Text);
            //int guestId = int.Parse(txtGuestId.Text);
            //DateTime checkInDate = dpCheckin.Value;
            //DateTime checkOutDate = dpCheckOut.Value;
            //string paymentInfo = txtPaymentInfo.Text;
            //string roomName = cbroomname.Text;
            //string guestName = cbGuestName.Text;
            //string reservationType = txtReservationType.Text;
            //int reservationTypeId = int.Parse(txtReservationTypeId.Text);
            //int adult = int.Parse(cbadult.Value.ToString());
            //int children = int.Parse(cbchild.Value.ToString());

            //Reservation reservation = new Reservation(reservationId, hotelId, roomId, guestId, checkInDate, checkOutDate, paymentInfo,
            //    roomName, guestName, reservationType, reservationTypeId, adult, children);

            //reservation.InsertReservation();




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
            dbManager.LoadComboBoxValues("select * from roomtypes", comboBox7, "name");
           

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
            display();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Hide();    
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            dbManager.LoadComboBoxValues("select * from rooms where roomtype='" + comboBox7.Text + "'", cbroomname, "roomname");
        }

       

        private void tabPage3_Enter(object sender, EventArgs e)
        {
            label27.Text = dpCheckin.Text;
            label28.Text = dpCheckOut.Text;
            TimeSpan difference = dpCheckOut.Value.Date - dpCheckin.Value.Date  ;

            // Access the duration in different units
            int days = difference.Days;
            label29.Text = days.ToString();
        }

        private void txtContact_Leave(object sender, EventArgs e)
        {
            string contact = txtContact.Text;
            Guest guest = Guest.FindByContact(contact);

            if (guest != null)
            {
                label33.Text = guest.IDNumber;  
                cbGuestName.Text = guest.Name;
                txtemail.Text = guest.Email;
                cbAddress.Text = guest.Address;
                // Populate other textboxes with the corresponding guest properties
            }
            else
            {
                MessageBox.Show("Guest not found");
                int newGuestId = Guest.GetNextGuestId();
                label33.Text = newGuestId.ToString();
                // Guest not found, handle accordingly
            }

        }
    }
}
