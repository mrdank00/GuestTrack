﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  System.Windows.Forms;

namespace GuestTrack.Controllers
{
    public class Reservation
    {
        private static DatabaseManager dbManager = new DatabaseManager();

        public int ReservationId { get; set; }
            public int HotelId { get; set; }
            public int RoomId { get; set; }
            public int GuestId { get; set; }
            public DateTime CheckInDate { get; set; }
            public DateTime CheckOutDate { get; set; }
            public string PaymentInfo { get; set; }
            public string RoomName { get; set; }
            public string GuestName { get; set; }
            public string ReservationType { get; set; }
            public int ReservationTypeId { get; set; }
            public int Adult { get; set; }
            public int Children { get; set; }

            public Reservation() { }    

            public Reservation(int reservationId, int hotelId, int roomId, int guestId, DateTime checkInDate, DateTime checkOutDate,
                string paymentInfo, string roomName, string guestName, string reservationType, int reservationTypeId, int adult, int children)
            {
                ReservationId = reservationId;
                HotelId = hotelId;
                RoomId = roomId;
                GuestId = guestId;
                CheckInDate = checkInDate;
                CheckOutDate = checkOutDate;
                PaymentInfo = paymentInfo;
                RoomName = roomName;
                GuestName = guestName;
                ReservationType = reservationType;
                ReservationTypeId = reservationTypeId;
                Adult = adult;
                Children = children;
            }

            public void InsertReservation()
            {
                
                using (SqlConnection connection = dbManager.Guestcon())
                {
                    string insertQuery = @"INSERT INTO Reservations ( hotel_id, room_id, guest_id, check_in_date, check_out_date,  RoomName, Guestname, ReservationType, Adult, Children) 
                                   VALUES ( @HotelId, @RoomId, @GuestId, @CheckInDate, @CheckOutDate,  @RoomName, @GuestName, @ReservationType, @Adult, @Children)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                    
                        command.Parameters.AddWithValue("@HotelId", HotelId);
                        command.Parameters.AddWithValue("@RoomId", RoomId);
                        command.Parameters.AddWithValue("@GuestId", GuestId);
                        command.Parameters.AddWithValue("@CheckInDate", CheckInDate);
                        command.Parameters.AddWithValue("@CheckOutDate", CheckOutDate);
                        command.Parameters.AddWithValue("@RoomName", RoomName);
                        command.Parameters.AddWithValue("@GuestName", GuestName);
                        command.Parameters.AddWithValue("@ReservationType", ReservationType);
                        command.Parameters.AddWithValue("@Adult", Adult);
                        command.Parameters.AddWithValue("@Children", Children);
                  
                    try
                        {
                            connection.Open();
                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                Console.WriteLine("Insert successful");
                            }
                            
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.ToString());
                        }
                    }
                }
            }

        public static Reservation GetReservationById(int reservationId)
        {
            Reservation reservation = null;

            using (SqlConnection connection = dbManager.Guestcon())
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Reservation WHERE reservation_id = @ReservationId";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@ReservationId", reservationId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int hotelId = reader.GetInt32(reader.GetOrdinal("hotel_id"));
                            int roomId = reader.GetInt32(reader.GetOrdinal("room_id"));
                            int guestId = reader.GetInt32(reader.GetOrdinal("guest_id"));
                            DateTime checkInDate = reader.GetDateTime(reader.GetOrdinal("check_in_date"));
                            DateTime checkOutDate = reader.GetDateTime(reader.GetOrdinal("check_out_date"));
                            string paymentInfo = reader.GetString(reader.GetOrdinal("payment_info"));
                            string roomName = reader.GetString(reader.GetOrdinal("room_name"));
                            string guestName = reader.GetString(reader.GetOrdinal("guest_name"));
                            string reservationType = reader.GetString(reader.GetOrdinal("reservation_type"));
                            int reservationTypeId = reader.GetInt32(reader.GetOrdinal("reservation_type_id"));
                            int adult = reader.GetInt32(reader.GetOrdinal("adult"));
                            int children = reader.GetInt32(reader.GetOrdinal("children"));

                            reservation = new Reservation(reservationId, hotelId, roomId, guestId, checkInDate, checkOutDate,
                                paymentInfo, roomName, guestName, reservationType, reservationTypeId, adult, children);
                        }
                    }
                }
            }

            return reservation;
        }

        public static int GetNextReservationId()
        {
            int nexreservationId = 0;

            // Assuming you have a database connection and execute the query to retrieve the maximum guest ID
            using (SqlConnection connection = dbManager.Guestcon())
            {
                connection.Open();

                string selectQuery = "SELECT MAX(Reservation_id) FROM Reservations";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    object result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        nexreservationId = Convert.ToInt32(result) + 1;
                    }
                    else
                    {
                        nexreservationId = 1; // If no existing guest IDs, start from 1
                    }
                }
            }

            return nexreservationId;
        }

        public string GetReservationStatus(int rowIndex)
        {
            string reservationStatus = string.Empty;

            using (SqlConnection connection = dbManager.Guestcon())
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT ReservationStatus FROM Reservation WHERE RowIndex = @RowIndex", connection))
                {
                    command.Parameters.AddWithValue("@RowIndex", rowIndex);

                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        reservationStatus = result.ToString();
                    }
                }
            }

            return reservationStatus;
        }

        // Get the color code associated with a specific reservation status
        public string GetColorCode(string reservationStatus)
        {
            string colorCode = string.Empty;

            using (SqlConnection connection = dbManager.Guestcon())
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT ColorCode FROM ReservationStatus WHERE ReservationStatus = @ReservationStatus", connection))
                {
                    command.Parameters.AddWithValue("@ReservationStatus", reservationStatus);

                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        colorCode = result.ToString();
                    }
                }
            }

            return colorCode;
        }
        // Additional properties

    }
}
