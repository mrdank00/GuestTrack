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

namespace GuestTrack
{
    public partial class frmCreateRooms : Form
    {
        DatabaseManager dbManager = new DatabaseManager();
        public frmCreateRooms()
        {
            InitializeComponent();
        }

        private void frmCreateRooms_Load(object sender, EventArgs e)
        {
            Display();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Room room = new Room();
            room.HotelId = 1;
            room.RoomName = txtRoomName.Text;
            room.RoomNumber = txtRoomno.Text;
            room.RoomType = cbRoomtype.Text;
            room.AvailabilityStatus = "Available";
            room.Status = "Clean";
            room.InsertRoom();
            Display();
        }
        private void Display()
        {
            string selectQuery = "SELECT * FROM rooms";
            DataTable results = dbManager.ExecuteQuery(selectQuery);
            dataGridView1.DataSource = results;

            string selectQuer = "SELECT name FROM roomtypes";
            DataTable result1 = dbManager.ExecuteQuery(selectQuer);
            cbRoomtype.DataSource = result1;
            cbRoomtype.DisplayMember = "name";


        }
    }
}
