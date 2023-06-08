using GuestTrack.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuestTrack
{
    public partial class frmGuestPayment : Form
    {
        String Guestname;
        String Guestid;
        String Room;
        public frmGuestPayment(string guestname,string guestid,string room)
        {
            this.InitializeComponent();
            Guestname = guestname;
            Guestid = guestid;
            Room = room;
        }
        public frmGuestPayment()
        {
            InitializeComponent();
        }

        private void frmGuestPayment_Load(object sender, EventArgs e)
        {
            cbGuestName.Text= Guestname;
            cbroomname.Text= Room;
            numupguestid.Text= Guestid; 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Account();
        }
        private void Account()
        {
            Account account = new Account();
        
            account.TransactionSource = "FrontDesk";
            account.Source = "FrontDesk";
          
            account.GuestId = int.Parse(numupguestid.Value.ToString());
      
            account.datepaid = dpDatePaid.Value;
        
            account.Paymenttype = "Booking Payment";
            account.Guestname =cbGuestName.Text;
            account.Reference = "Booking Payment";
            account.Narration = "Payment for reservation for room " + cbroomname.Text;
            account.activeuser = cbGuestName.Text;
            account.Roomno = cbroomname.Text;
            account.Saletype = "Guest Sale";
            account.Category = "Accomodation";
            account.Amountpaid = double.Parse(txtAmount.Text);
            account.Reference = txtRef.Text;
            account.TransactionType = "Accomodation";
            account.TransactionDescription = "Room Booking for Room:" + cbroomname.Text;
          
            account.Credit = double.Parse(txtAmount .Text);
            // Balance = balance;

                account.InsertguestLedger("c");
                account.InsertPayment();

         

        }
    }
}
