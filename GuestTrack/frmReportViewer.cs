using CrystalDecisions.Windows.Forms;
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
    public partial class frmReportViewer : Form
    {
        public CrystalReportViewer CrystalReportViewer1 { get; set; }
        public frmReportViewer()
        {
            InitializeComponent();
        }
    }
}
