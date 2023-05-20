using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace GuestTrack
{
  

    public class DatabaseManager
    {
        private readonly string connectionString;

        public DatabaseManager()
        {
            connectionString = Properties.Settings.Default.Guestconstring;
        }

        public SqlConnection Guestcon()
        {
            return new SqlConnection(connectionString);
        }

        public DataTable ExecuteQuery(string query)
        {
            using (SqlConnection connection = Guestcon())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    DataTable dataTable = new DataTable();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                    return dataTable;
                }
            }
        }

        public int ExecuteNonQuery(string query)
        {
            using (SqlConnection connection = Guestcon())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    return command.ExecuteNonQuery();
                }
            }
        }
        public List<string> GetRoomList()
        {
            List<string> roomNames = new List<string>();

            using (SqlConnection connection = Guestcon())
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT room_number FROM Rooms", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string roomNumber = reader.GetString(reader.GetOrdinal("room_number"));
                            roomNames.Add(roomNumber);
                        }
                    }
                }

                connection.Close();
            }

            return roomNames;
        }
    }

}
