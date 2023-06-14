using GuestTrack.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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
            comboBox1.SelectedIndex=0; 
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
                    try
                    {
                       
                        guest();
                        reserve();
                        booking();
                      
                        Account();
                    }
                    catch (Exception ex)
                    {
                        // Handle the exception (e.g., display an error message)
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                    finally
                    {
                        // Make sure to close the connection
                       
                    }

                   
                }
            }
        }
        private void guest()
        {

            // Add parameters to the command
            string Name = cbGuestName.Text;
            string Contact = txtContact.Text;
            string Email = txtemail.Text;
            string Address = cbAddress.Text;
            string Nationality = cbCountry.Text;
            DateTime DOB = dpDOB.Value;
            string IDType = cbIDtype.Text;
            string IDNumber = txtid.Text;
            string SpecialRequirements = textBox2.Text;

            Guest guest = new Guest();

            guest.Name = Name;
            guest.Contact = Contact;
            guest.Email = Email;
            guest.Address = Address;
            guest.Nationality = Nationality;
            guest.DOB=DOB; guest.IDType=IDType;
            guest.IDNumber=IDNumber;
            guest.SpecialRequirements = SpecialRequirements;
            guest.InsertGuest();
        }
        private void reserve()
        {
        
            int hotelId = 1;
            int roomId = int.Parse(lblRoomID.Text);
            int guestId = int.Parse(numupguestid.Text);
            DateTime checkInDate = dpCheckin.Value;
            DateTime checkOutDate = dpCheckOut.Value;
            string roomName = cbroomname.Text;
            string guestName = cbGuestName.Text;
            string reservationtype = comboBox8.Text;
            int adult = int.Parse(cbadult.Value.ToString());
            int children = int.Parse(cbchild.Value.ToString());
            //var status = "C";
            Reservation reservation = new Reservation();
           
            reservation.HotelId = hotelId;
            reservation.RoomId = roomId;
            reservation.GuestId = guestId;
            reservation.CheckInDate = checkInDate;
            reservation.CheckOutDate = checkOutDate;
            reservation.RoomName = roomName;
            reservation.ReservationType = reservationtype;
            reservation.GuestName = guestName;
            reservation.Adult = adult;
            reservation.Children = children;
            reservation.status = children;

            reservation.InsertReservation();
           
        }
        private void booking()
        {
            Booking booking = new Booking();
            booking.RoomId= int.Parse(lblRoomID.Text);
            booking.RoomId= int.Parse(lblRoomID.Text);
            booking.reservationid= int.Parse(numReservationid.Text);
            booking.AmountPaid = decimal.Parse(txtAmtpaid.Text);
            booking.GuestId= int.Parse(numupguestid.Text);
            booking.CheckInDate= dpCheckin.Value;
            booking.CheckOutDate= dpCheckOut.Value;
            booking.InsertBooking();
        }

        private void Account()
        {
            Account account= new Account();
            account.Itemname= cbroomtype.Text+":"+cbroomname.Text;
            account.TransactionSource = "FrontDesk";
            account.Source = "FrontDesk";
            account.Quantity= 1;
            account.GuestId= int.Parse(numupguestid.Value.ToString());
            account.Price= double.Parse(lblRoomrate.Text);
            account.datepaid= dpDatepaid.Value;
            account.Date= dpDatepaid.Value;
            account.Roomno = cbroomname.Text;
            account.Guestname = cbGuestName.Text;

            account.Category = "Accomodation";
            account.Paymenttype = "Booking Payment";
            account.Narration = "Payment for reservation for room " + cbroomname.Text;
            account.activeuser =  cbGuestName.Text;
            account.Saletype = "Guest Sale";
            account.Category = "Accomodation";
            account.Amountpaid = double.Parse(txtAmtpaid.Text);
            account.Reference = numReservationid.Text;
            account.TransactionType = "Accomodation";
            account.TransactionDescription = "Room Booking for Room:"+cbroomname.Text;
            account.Debit = txtRate.Text;
            account.Credit = double.Parse(txtAmtpaid.Text); 
           // Balance = balance;

            account.InsertguestTranx();
          

            if (!(checkBox1.Checked = true))
            {
                account.InsertguestLedger("d");
              
            }
            else
            {
                account.InsertguestLedger("d");
                account.InsertguestLedger("c");
                account.InsertPayment();
               
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
            dbManager.LoadComboBoxValues("select * from roomtypes", cbroomtype, "name");
            numReservationid.Value = Reservation.GetNextReservationId();

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
            dbManager.LoadComboBoxValues("select * from rooms where roomtype='" + cbroomtype.Text + "'", cbroomname, "roomname");
        }

       

        private void tabPage3_Enter(object sender, EventArgs e)
        {
            label27.Text = dpCheckin.Text;
            label28.Text = dpCheckOut.Text;
            lblRoomrate.Text = txtRate.Text;
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
                label33.Text =guest.GuestId.ToString();  
                numupguestid.Text = guest.GuestId.ToString();  
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
                numupguestid.Text = newGuestId.ToString();

                // Guest not found, handle accordingly
            }

        }

        private void cbroomname_SelectedIndexChanged(object sender, EventArgs e)
        {
            string roomName = cbroomname.Text; // Replace with the actual room name
            Room room = Room.GetRoomByName(roomName);

            if (room != null)
            {
                // Room found, perform operations on the room object
                // ...
                txtRate.Text=room.Price.ToString();
                lblRoomID.Text=room.RoomId.ToString();
            }
            else
            {
                MessageBox.Show("NO such room");
                // Room not found
            }


        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked==true)
            {
                groupBox1.Visible=true;
                
            }
            else
            {
                groupBox1.Visible = false;
            }
        }

        private void txtContact_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex==1) {
                label26.Visible = true;
                comboBox2.Visible = true;
                //dbManager.LoadComboBoxValues("select * from rooms where roomtype='" + comboBox7.Text + "'", cbroomname, "roomname");
            }
            else
            {
                label26.Visible = false;
                comboBox2.Visible = false;
            }
        }

        private void numReservationid_ValueChanged(object sender, EventArgs e)
        {
         
           
        }
    }
}
