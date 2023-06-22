using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows.Forms;
namespace GuestTrack
{
    public class DatabaseManager
    {
        private readonly string connectionString;
        public SqlTransaction tranx;

        public DatabaseManager()
        {
            connectionString = Properties.Settings.Default.servernet;
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
        public List<Tuple<string, string>> GetRoomList()
        {
            List<Tuple<string, string>> roomNames = new List<Tuple<string, string>>();

            using (SqlConnection connection = Guestcon())
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT room_number,status FROM Rooms", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string roomNumber = reader.GetString(reader.GetOrdinal("room_number"));
                            string roomStatus = reader.GetString(reader.GetOrdinal("status"));
                            roomNames.Add(Tuple.Create(roomStatus, roomNumber));
                        }
                    }
                }

                connection.Close();
            }

            return roomNames;
        }
        public void LoadComboBoxValues(string query, ComboBox comboBox, string displayMember)
        {
            using (SqlConnection connection = Guestcon())
            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        comboBox.Items.Add(reader[displayMember].ToString());
                        
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }



    }

}
