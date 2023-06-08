using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using GuestTrack.Controllers;
using CrystalDecisions.Windows.Forms;

namespace GuestTrack
{
    public partial class frmHouselistReports : Form
    {
        private static DatabaseManager dbManager = new DatabaseManager();
        dsGuest dt = new dsGuest();
        public frmHouselistReports()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                string query = "SELECT * FROM reservations WHERE convert(datetime,check_in_date,105)=convert(datetime,@date,105)";

                using (SqlConnection connection = dbManager.Guestcon())
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@date", dateTimePicker1.Value);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    dt.Tables["Reservations"].Clear();
                    adapter.Fill(dt, "Reservations");

                    if (dt.Tables["Reservations"].Rows.Count > 0)
                    {
                        Arrivals report = new Arrivals();
                        report.SetDataSource(dt);

                        frmReportViewer frm = new frmReportViewer
                        {
                            CrystalReportViewer1 = new CrystalReportViewer
                            {
                                ReportSource = report
                            }
                        };

                        frm.Show();
                    }
                    else
                    {
                        MessageBox.Show("No results found");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Cursor = Cursors.Default;


        }
    }
}
