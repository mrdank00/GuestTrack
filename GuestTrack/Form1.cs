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
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void reservationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Assuming you want to open a form of type "MyForm"
            Type formType = typeof(frmReservationDashboard);

            // Call the OpenForm method with the formType
            OpenForm(typeof(frmReservationDashboard));

        }
        private void OpenForm(Type formType)
        {
            Form openForm = null;
            foreach (Form childForm in MdiChildren)
            {
                if (childForm.GetType() == formType)
                {
                    openForm = childForm;
                    break;
                }
            }

            if (openForm == null)
            {
                openForm = (Form)Activator.CreateInstance(formType);
                openForm.MdiParent = this;
                openForm.WindowState = FormWindowState.Maximized;
                openForm.StartPosition = FormStartPosition.CenterParent;
                openForm.Show();
            }
            else
            {
                openForm.Activate();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            OpenForm(typeof(frmRoomReservation));
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            OpenForm(typeof(frmReservationDashboard));
        }

        private void roomTypeSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(typeof(frmRoomTypes));
        }

        private void roomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(typeof(frmCreateRooms));
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            OpenForm(typeof(frmGuestProfiles));
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            frmHouselistReports houselistReports = new frmHouselistReports();
            houselistReports.ShowDialog();

        }
    }
}
