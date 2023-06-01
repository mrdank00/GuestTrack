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
using System.Globalization;

namespace GuestTrack
{
    public partial class frmReservationDashboard : Form
    {
        DatabaseManager dbManager = new DatabaseManager();

        public frmReservationDashboard()
        {
            InitializeComponent();
            dataGridView1.RowTemplate.Height = 50;
        }

        private void frmReservationDashboard_Load(object sender, EventArgs e)
        {
            PopulateDates(dateTimePicker1.Value);
        }
        private void PopulateDates(DateTime exactDate)
        {


            List<Tuple<string, string>> roomList = dbManager.GetRoomList();



            // Add date columns and populate rows
            DateTime currentDate = exactDate;
            DateTime endDate = currentDate.AddDays(14);
            int columnCount = 2; // Starting column index for dates
            foreach (Tuple<string, string> roomTuple in roomList)
            {
                string status = roomTuple.Item1;
                string roomName = roomTuple.Item2;
                dataGridView1.Rows.Add(status, roomName);
            }

            // Add date columns dynamically
            while (currentDate <= endDate)
            {
                DataGridViewTextBoxColumn dateColumn = new DataGridViewTextBoxColumn();
                dateColumn.HeaderText = currentDate.ToString("ddd, dd-MMM-yy");
                dateColumn.Name = currentDate.ToString("yyyyMMdd"); // Set a unique name for the column

                // Add the date column to the DataGridView control
                dataGridView1.Columns.Insert(columnCount, dateColumn);

                // Move to the next date
                currentDate = currentDate.AddDays(1);
                columnCount += 1;
            }
            LoadReservations();
        }
        private void HighlightDates(DateTime startDate, DateTime endDate, string roomName, string guestName)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
               
                // Check if the row corresponds to the specified room name
                if (row.Cells[1].Value != null && row.Cells[1].Value.ToString() == roomName)
                {
                   
                    // Iterate over the date columns
                    for (int columnIndex = 2; columnIndex < dataGridView1.Columns.Count; columnIndex++)
                    {
                      //  MessageBox.Show(roomName);
                        // Get the date from the column header text
                        DateTime columnDate = DateTime.ParseExact(dataGridView1.Columns[columnIndex].HeaderText, "ddd, dd-MMM-yy", CultureInfo.InvariantCulture);
                        DataGridViewCell cell = row.Cells[columnIndex];
                        cell.Style.BackColor = Color.White;
                        cell.Value = "";
                        
                        // Check if the date is within the specified range
                        if (columnDate >= startDate && columnDate <= endDate)
                        {
                           
                            // Get the cell corresponding to the room and date
                           

                            // Set the cell's background color to red
                            cell.Style.BackColor = Color.IndianRed;

                            if (row.Cells[1].Value.ToString() == roomName)
                            {
                                cell.Value = "<" + guestName + ">";

                            }
                        }
                    }
                }
            }
        }

        private void LoadReservations()
        {
          
            string query = "SELECT check_in_date, check_out_date, roomname, guestname FROM Reservations";

            using (SqlConnection connection = dbManager.Guestcon())
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        DateTime reservationDateIn = reader.GetDateTime(0);
                        DateTime reservationDateOut = reader.GetDateTime(1);
                        string roomName = reader.GetString(2);
                        string guestName = reader.GetString(3);
                      
                        // Call the HighlightDates method with the retrieved date and room
                        HighlightDates(reservationDateIn, reservationDateOut, roomName, guestName);
                    }

                    reader.Close();
                }
            }
        }
        private void ShiftDateColumns(int shiftAmount)
        {
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                if (DateTime.TryParseExact(column.HeaderText, "ddd, dd-MMM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    DateTime shiftedDate = date.AddDays(shiftAmount);
                    column.HeaderText = shiftedDate.ToString("ddd, dd-MMM-yy");
                }
            }
        }
        private void SetAndShiftDateColumns(DateTime exactDate, int shiftAmount)
        {
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                if (DateTime.TryParseExact(column.HeaderText, "ddd, dd-MMM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    DateTime shiftedDate = exactDate.AddDays(shiftAmount);
                    column.HeaderText = exactDate.ToString("ddd, dd-MMM-yy");
                }
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            ShiftDateColumns(-1);
            LoadReservations();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShiftDateColumns(+1);
            LoadReservations();
        }

        private void dataGridView1_ColumnNameChanged(object sender, DataGridViewColumnEventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ShiftDateColumns(-7);
            LoadReservations();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ShiftDateColumns(+7);
            LoadReservations();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            PopulateDates(dateTimePicker1.Value);

        }

        private void addReservationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRoomReservation frm= new frmRoomReservation();
            frm.ShowDialog(this);   
        }
    }

}
