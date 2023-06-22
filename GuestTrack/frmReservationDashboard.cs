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
using GuestTrack.Controllers;
using System.Runtime.Remoting.Messaging;

namespace GuestTrack
{
    public partial class frmReservationDashboard : Form
    {
        private static DatabaseManager dbManager = new DatabaseManager();
        
        public frmReservationDashboard()
        {
            InitializeComponent();
            dataGridView1.RowTemplate.Height = 50;
        }
       

        private void frmReservationDashboard_Load(object sender, EventArgs e)
        {
            
            

           //PopulateDates(dateTimePicker1.Value);
        }
        private void PopulateDates(DateTime exactDate)
        {

            List<Tuple<string, string>> roomList = dbManager.GetRoomList();



            // Add date columns and populate rows
            DateTime currentDate = exactDate.AddDays(-1);
            DateTime endDate = currentDate.AddDays(20);
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
        private void HighlightDates(DateTime startDate, DateTime endDate, string roomName, string guestName, int reservid)
        {
            Reservation reservation = new Reservation();
   
            // Find the rows corresponding to the room name
            IEnumerable<DataGridViewRow> roomRows = dataGridView1.Rows
                .Cast<DataGridViewRow>()
                .Where(row => row.Cells[1].Value?.ToString() == roomName);

            foreach (DataGridViewRow roomRow in roomRows)
            {
                endDate = endDate.AddDays(-1);

                // Iterate over the date columns
                for (int columnIndex = 2; columnIndex < dataGridView1.Columns.Count; columnIndex++)
                {
                    // Get the date from the column header text
                    DateTime columnDate = DateTime.ParseExact(dataGridView1.Columns[columnIndex].HeaderText, "ddd, dd-MMM-yy", CultureInfo.InvariantCulture);
                    DataGridViewCell cell = roomRow.Cells[columnIndex];

                    // Check if the date is within the specified range
                    if (columnDate >= startDate && columnDate <= endDate)
                    {
                        int reservationStatus = reservation.GetReservationStatus(reservid); // Fetch reservation status from the database based on the reservation ID
                        Color color = reservation.GetColorCode(reservationStatus); // Fetch the color code based on the reservation status

                        cell.Style.BackColor = color;
                        cell.Value = guestName;
                        cell.Tag = reservid;
                    }
                }
            }
        }

        private void clearcells()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                for (int i = 2; i < row.Cells.Count; i++)
                {
                    DataGridViewCell cell = row.Cells[i];
                    cell.Style.BackColor = Color.White;
                    cell.Value = "";
                }
            }
        }


        public void LoadReservations()
        {
            clearcells();
            Reservation reservation = new Reservation();
            reservation.UpdateReservationArrivalStatus(4);

            string query = "SELECT check_in_date, check_out_date, roomname, guestname,reservation_id FROM Reservations";

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
                        int reservatid = reader.GetInt32(4);
                      
                        // Call the HighlightDates method with the retrieved date and room
                        HighlightDates(reservationDateIn, reservationDateOut, roomName, guestName,reservatid);
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
            frmRoomReservation frm = new frmRoomReservation();
            frm.ShowDialog();
            LoadReservations();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            PopulateDates(dateTimePicker1.Value);

        }
        private DataGridViewCell storedCell;
        private void addReservationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRoomReservation frm = new frmRoomReservation();
            frm.ShowDialog();
            LoadReservations();


        }

        private void ShowAccountDialog(string roomNo,DateTime date,int cellcontent)
        {
            frmGuestAccount frm = new frmGuestAccount(roomNo,date,cellcontent);
            frm.ShowDialog();
            LoadReservations();
        }




        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridView.HitTestInfo hitTestInfo = dataGridView1.HitTest(e.X, e.Y);
                if (hitTestInfo.Type == DataGridViewHitTestType.Cell)
                {
                    // Store the clicked cell information
                    storedCell = dataGridView1.Rows[hitTestInfo.RowIndex].Cells[hitTestInfo.ColumnIndex];
                }
            }
        }

        private void accountToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (storedCell != null)
            {
                // Get the content of the stored cell
                int cellData = (int)storedCell.Tag;


                // Get the row header value (leftmost cell value)
                string rowHeader = storedCell.OwningRow.Cells[1].Value.ToString();

                // Get the column header value
                string columnHeader = storedCell.OwningColumn.HeaderText;
                DateTime columnDate;

                // Parse the column header as a DateTime object
                if (DateTime.TryParseExact(columnHeader, "ddd, dd-MMM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out columnDate))
                {
                    // Show the content, row header, and column header in a MessageBox
                    //string message = $"Cell Content: {cellData}\nRow Header: {rowHeader}\nColumn Header: {columnHeader}";
                    //MessageBox.Show(message, "Cell Information");

                    // Pass the row header and column header to the ShowAccountDialog method
                    ShowAccountDialog(rowHeader, columnDate,cellData);
                }
                else
                {
                    // Handle the case where the column header cannot be parsed as a valid DateTime
                    //MessageBox.Show("Invalid column header format.", "Error");
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell clickedCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];

            if (clickedCell.Tag != null)
            {
                string secretReservationId = clickedCell.Tag.ToString();
                MessageBox.Show(secretReservationId);
                // Do something with the secretReservationId
            }
        }

        private void frmReservationDashboard_Shown(object sender, EventArgs e)
        {
            PopulateDates(dateTimePicker1.Value);
        }
        

        private void button5_Click(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text;
            //SearchReservations(searchText);
          
        }
        private void SearchReservations(string searchText)
        {
            // Clear any existing selection
            dataGridView1.ClearSelection();

            // Iterate over the rows and cells to find the matching cells
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && cell.Value.ToString().IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        dataGridView1.CurrentCell = cell; // Set the current cell to bring it into view
                        cell.Selected = true; // Select the cell if the value contains the search text
                        dataGridView1.FirstDisplayedScrollingRowIndex = cell.RowIndex; // Scroll to the row containing the cell
                        dataGridView1.Focus(); // Set focus to the DataGridView
                        return; // Exit the method after finding the first match
                    }
                }
            }
        }

        private void checkInGuestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (storedCell != null)
            {
                // Get the content of the stored cell
                int cellData = (int)storedCell.Tag;


                // Get the row header value (leftmost cell value)
                string rowHeader = storedCell.OwningRow.Cells[1].Value.ToString();

                // Get the column header value
                string columnHeader = storedCell.OwningColumn.HeaderText;
                DateTime columnDate;

                // Parse the column header as a DateTime object
                if (DateTime.TryParseExact(columnHeader, "ddd, dd-MMM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out columnDate))
                {

                    Reservation reservation = new Reservation();
                    reservation.ReservationId = cellData;
                    reservation.UpdateReservationStatus(5);
                    LoadReservations();
                }
                else
                {
                    // Handle the case where the column header cannot be parsed as a valid DateTime
                    //MessageBox.Show("Invalid column header format.", "Error");
                }
            }
          
        }

        private void sendSMSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (storedCell != null)
            {
                // Get the content of the stored cell
                int cellData = (int)storedCell.Tag;


                // Get the row header value (leftmost cell value)
                string rowHeader = storedCell.OwningRow.Cells[1].Value.ToString();

                // Get the column header value
                string columnHeader = storedCell.OwningColumn.HeaderText;
                DateTime columnDate;

                // Parse the column header as a DateTime object
                if (DateTime.TryParseExact(columnHeader, "ddd, dd-MMM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out columnDate))
                {

                    Reservation.GetReservationById(cellData);
                    
                }
                else
                {
                    // Handle the case where the column header cannot be parsed as a valid DateTime
                    //MessageBox.Show("Invalid column header format.", "Error");
                }
            }
        }
    }

}
