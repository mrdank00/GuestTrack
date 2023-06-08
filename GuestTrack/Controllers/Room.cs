using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace GuestTrack.Controllers
{

    public class Room
    {
        private static DatabaseManager dbManager = new DatabaseManager();

        public int RoomId { get; set; }
        public int HotelId { get; set; }
        public string RoomNumber { get; set; }
        public int RoomTypeId { get; set; }
        public string AvailabilityStatus { get; set; }
        public decimal? Price { get; set; }
        public string RoomName { get; set; }
        public string Status { get; set; }
        public string RoomType { get; set; }

        public Room(int roomId, int hotelId, string roomNumber, int roomTypeId, string availabilityStatus, decimal? price, string roomName, string status, string roomType)
        {
            RoomId = roomId;
            HotelId = hotelId;
            RoomNumber = roomNumber;
            RoomTypeId = roomTypeId;
            AvailabilityStatus = availabilityStatus;
            Price = price;
            RoomName = roomName;
            Status = status;
            RoomType = roomType;
        }


        public void InsertRoom()
        {
            using (SqlConnection connection = dbManager.Guestcon())
            {
                string insertQuery = @"INSERT INTO Rooms (hotel_id, room_number, room_type_id, availability_status, price, Roomname, status, roomtype)
                               VALUES (@HotelId, @RoomNumber, @RoomTypeId, @AvailabilityStatus, @Price, @RoomName, @Status, @RoomType)";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@HotelId", HotelId);
                    command.Parameters.AddWithValue("@RoomNumber", RoomNumber);
                    command.Parameters.AddWithValue("@RoomTypeId", RoomTypeId);
                    command.Parameters.AddWithValue("@AvailabilityStatus", AvailabilityStatus);
                    command.Parameters.AddWithValue("@Price", Price.HasValue ? (object)Price.Value : (object)DBNull.Value);
                    command.Parameters.AddWithValue("@RoomName", RoomName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Status", Status ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@RoomType", RoomType ?? (object)DBNull.Value);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Insert successful");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }


        public static Room GetRoomByName(string roomName)
        {
            Room room = null;

            using (SqlConnection connection = dbManager.Guestcon())
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Rooms WHERE roomname = @RoomId";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@RoomId", roomName);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            room = new Room(
                                reader.GetInt32(reader.GetOrdinal("room_id")),
                                reader.GetInt32(reader.GetOrdinal("hotel_id")),
                                reader.GetString(reader.GetOrdinal("room_number")),
                                reader.GetInt32(reader.GetOrdinal("room_type_id")),
                                reader.GetString(reader.GetOrdinal("availability_status")),
                                reader.IsDBNull(reader.GetOrdinal("price")) ? null : (decimal?)reader.GetDecimal(reader.GetOrdinal("price")),
                                reader.IsDBNull(reader.GetOrdinal("Roomname")) ? null : reader.GetString(reader.GetOrdinal("Roomname")),
                                reader.IsDBNull(reader.GetOrdinal("status")) ? null : reader.GetString(reader.GetOrdinal("status")),
                                reader.IsDBNull(reader.GetOrdinal("roomtype")) ? null : reader.GetString(reader.GetOrdinal("roomtype"))
                            );
                        }
                    }
                }
            }

            return room;
        }



        public void UpdateRoom()
        {
            using (SqlConnection connection = dbManager.Guestcon())
            {
                string updateQuery = @"UPDATE Rooms SET hotel_id = @HotelId, room_number = @RoomNumber, room_type_id = @RoomTypeId,
                               availability_status = @AvailabilityStatus, price = @Price, Roomname = @RoomName, status = @Status,
                               roomtype = @RoomType WHERE room_id = @RoomId";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@HotelId", HotelId);
                    command.Parameters.AddWithValue("@RoomNumber", RoomNumber);
                    command.Parameters.AddWithValue("@RoomTypeId", RoomTypeId);
                    command.Parameters.AddWithValue("@AvailabilityStatus", AvailabilityStatus);
                    command.Parameters.AddWithValue("@Price", Price.HasValue ? (object)Price.Value : (object)DBNull.Value);
                    command.Parameters.AddWithValue("@RoomName", RoomName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Status", Status ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@RoomType", RoomType ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@RoomId", RoomId);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Update successful");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }


        public void DeleteRoom()
        {
            using (SqlConnection connection = dbManager.Guestcon())
            {
                string deleteQuery = "DELETE FROM Rooms WHERE room_id = @RoomId";

                using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@RoomId", RoomId);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Delete successful");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }
        public static List<Room> GetAllRooms()
        {
            List<Room> rooms = new List<Room>();

            using (SqlConnection connection = dbManager.Guestcon())
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Rooms";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Room room = new Room(
                                reader.GetInt32(reader.GetOrdinal("room_id")),
                                reader.GetInt32(reader.GetOrdinal("hotel_id")),
                                reader.GetString(reader.GetOrdinal("room_number")),
                                reader.GetInt32(reader.GetOrdinal("room_type_id")),
                                reader.GetString(reader.GetOrdinal("availability_status")),
                                reader.IsDBNull(reader.GetOrdinal("price")) ? null : (decimal?)reader.GetDecimal(reader.GetOrdinal("price")),
                                reader.IsDBNull(reader.GetOrdinal("Roomname")) ? null : reader.GetString(reader.GetOrdinal("Roomname")),
                                reader.IsDBNull(reader.GetOrdinal("status")) ? null : reader.GetString(reader.GetOrdinal("status")),
                                reader.IsDBNull(reader.GetOrdinal("roomtype")) ? null : reader.GetString(reader.GetOrdinal("roomtype"))
                            );

                            rooms.Add(room);
                        }
                    }
                }
            }

            return rooms;
        }

        //public List<Tuple<string, string>> GetRoomList()
        //{
        //    List<Tuple<string, string>> roomNames = new List<Tuple<string, string>>();

        //    using (SqlConnection connection = dbManager.Guestcon())
        //    {
        //        connection.Open();

        //        using (SqlCommand command = new SqlCommand("SELECT room_number, status FROM Rooms", connection))
        //        {
        //            using (SqlDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    string roomNumber = reader.GetString(reader.GetOrdinal("room_number"));
        //                    string roomStatus = reader.GetString(reader.GetOrdinal("status"));
        //                    roomNames.Add(Tuple.Create(roomStatus, roomNumber));
        //                }
        //            }
        //        }

        //        connection.Close();
        //    }

        //    return roomNames;
        //}


    }
}