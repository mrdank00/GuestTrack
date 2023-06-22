using GuestTrack.Controllers;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace GuestTrack
{
    public partial class frmGuestAccount : Form
    {
        string Roomno;
        int CellContent;
        DateTime Date;
        decimal dsum = 0;
        decimal csum = 0;
        decimal bal = 0;

        public frmGuestAccount()
        {
            InitializeComponent();
        }

        public frmGuestAccount(string roomno, DateTime date,int cellcontent)
        {
            InitializeComponent();

            Roomno = roomno;
            CellContent = cellcontent;
            Date = date;
        }

        private void frmGuestAccount_Load(object sender, EventArgs e)
        {
            //string message = $"Row Header: {CellContent}\nRow Header: {Roomno}\nColumn Header: {Date}";
            //MessageBox.Show(message, "Cell Information");
            Account account = new Account();
            account.reservationid = CellContent;

            // Call the GetBookingsByDateAndRoom method to retrieve the bookings
            DataTable bookings = account.GetBookingsByDateAndRoom();

            // Update label text with value from a specific cell in the DataTable
            if (bookings.Rows.Count>0)
            {
                label1.Text = bookings.Rows[0][5].ToString();
                label3.Text = bookings.Rows[0][6].ToString();
                lblreservationid.Text = CellContent.ToString();
            }
           

            // Bind the bookings data to the DataGridView
            dataGridView1.DataSource = bookings;
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowAccountDialog(label1.Text,label3.Text,int.Parse(lblreservationid.Text));

        }

        private void ShowAccountDialog(string name, string id,int reservationid)
        {
            frmGuestPayment frmgPayment = new frmGuestPayment(name, id,reservationid);
            frmgPayment.ShowDialog();
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {


            decimal csum = 0;
            decimal dsum = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[4].Value != null && decimal.TryParse(row.Cells[4].Value.ToString(), out decimal cValue))
                {
                    csum += cValue;
                }
                if (row.Cells[3].Value != null && decimal.TryParse(row.Cells[3].Value.ToString(), out decimal dValue))
                {
                    dsum += dValue;
                }
            }

            lblCredit.Text = csum.ToString();
            lbldebit.Text = dsum.ToString();
            lblbalance.Text = (dsum - csum).ToString();


            // Now the variable 'sum' contains the sum of the values in the specified column

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ShowchargeDialog(label1.Text, label3.Text, int.Parse(lblreservationid.Text));
        }
        private void ShowchargeDialog(string name, string id, int reservationid)
        {
            frmChargeGuest frmcharge = new frmChargeGuest(name, id, reservationid);
            frmcharge.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to continue?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Reservation reservation = new Reservation();
                reservation.ReservationId = int.Parse(lblreservationid.Text);
                reservation.UpdateReservationStatus(9);
                // User clicked 'Yes', perform the desired action
            }
           
        }
    }
}
