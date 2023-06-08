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
        string CellContent;
        DateTime Date;

        public frmGuestAccount()
        {
            InitializeComponent();
        }

        public frmGuestAccount(string roomno, DateTime date,string cellcontent)
        {
            InitializeComponent();

            Roomno = roomno;
            CellContent = cellcontent;
            Date = date;
        }

        private void frmGuestAccount_Load(object sender, EventArgs e)
        {
            string message = $"Row Header: {CellContent}\nRow Header: {Roomno}\nColumn Header: {Date}";
            MessageBox.Show(message, "Cell Information");
            Account account = new Account();
            account.Guestname = CellContent;

            // Call the GetBookingsByDateAndRoom method to retrieve the bookings
            DataTable bookings = account.GetBookingsByDateAndRoom();

            // Update label text with value from a specific cell in the DataTable
            label1.Text = bookings.Rows[0][5].ToString();
            label3.Text = bookings.Rows[0][6].ToString();
            lblroom.Text = bookings.Rows[0][7].ToString();

            // Bind the bookings data to the DataGridView
            dataGridView1.DataSource = bookings;
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowAccountDialog(label1.Text,label3.Text,lblroom.Text);

        }

        private void ShowAccountDialog(string name, string id,string room)
        {
            frmGuestPayment frmgPayment = new frmGuestPayment(name, id,room);
            frmgPayment.ShowDialog();
        }
    }
}
