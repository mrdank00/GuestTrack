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
    public partial class frmChargeGuest : Form
    {
        String Guestname;
        String Guestid;
        int Reservationid;
        public frmChargeGuest(string guestname, string guestid, int reservationid)
        {
            this.InitializeComponent();
            Guestname = guestname;
            Guestid = guestid;
            Reservationid = reservationid;
        }
        public frmChargeGuest()
        {
            InitializeComponent();
        }

        private void frmChargeGuest_Load(object sender, EventArgs e)
        {
            cbGuestName.Text = Guestname;
            cbreservationid.Text = Reservationid.ToString();
            numupguestid.Text = Guestid;
        }
    }
}
