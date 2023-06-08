using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace GuestTrack.Controllers
{

    public class Booking
    {
        private static DatabaseManager dbManager = new DatabaseManager();
        public int BookingId { get; set; }
        public int RoomId { get; set; }
        public int GuestId { get; set; }
        public int reservationid { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal AmountPaid { get; set; }
        
        public Booking() { }

        // Constructor
        public Booking(int bookingId, int roomId, int guestId, DateTime checkInDate, DateTime checkOutDate, decimal amountPaid)
        {
            BookingId = bookingId;
            RoomId = roomId;
            GuestId = guestId;
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
            AmountPaid = amountPaid;
        }

        // Insert a new booking
        public void InsertBooking()
        {
            using (SqlConnection connection = dbManager.Guestcon())
            {
                string insertQuery = @"INSERT INTO Bookings (room_id, guest_id, check_in_date, check_out_date, amount_paid,reservationid)
                                   VALUES (@RoomId, @GuestId, @CheckInDate, @CheckOutDate, @AmountPaid,@reservationid)";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@RoomId", RoomId);
                    command.Parameters.AddWithValue("@GuestId", GuestId);
                    command.Parameters.AddWithValue("@CheckInDate", CheckInDate);
                    command.Parameters.AddWithValue("@CheckOutDate", CheckOutDate);
                    command.Parameters.AddWithValue("@AmountPaid", AmountPaid);
                    command.Parameters.AddWithValue("@reservationid", reservationid);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Booking inserted successfully");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }

        // Update an existing booking
        public void UpdateBooking()
        {
            using (SqlConnection connection = dbManager.Guestcon())
            {
                string updateQuery = @"UPDATE Bookings SET room_id = @RoomId, guest_id = @GuestId,
                                   check_in_date = @CheckInDate, check_out_date = @CheckOutDate,
                                   amount_paid = @AmountPaid WHERE booking_id = @BookingId";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@RoomId", RoomId);
                    command.Parameters.AddWithValue("@GuestId", GuestId);
                    command.Parameters.AddWithValue("@CheckInDate", CheckInDate);
                    command.Parameters.AddWithValue("@CheckOutDate", CheckOutDate);
                    command.Parameters.AddWithValue("@AmountPaid", AmountPaid);
                    command.Parameters.AddWithValue("@BookingId", BookingId);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Booking updated successfully");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }

        // Delete a booking
        public void DeleteBooking()
        {
            using (SqlConnection connection = new SqlConnection("YOUR_CONNECTION_STRING"))
            {
                string deleteQuery = "DELETE FROM Bookings WHERE booking_id = @BookingId";

                using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@BookingId", BookingId);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Booking deleted successfully");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }

        // Get a booking by booking ID
        public static Booking GetBookingById(int bookingId)
        {
            Booking booking = null;

            using (SqlConnection connection = dbManager.Guestcon())
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Bookings WHERE booking_id = @BookingId";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@BookingId", bookingId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            booking = new Booking(
                                reader.GetInt32(reader.GetOrdinal("booking_id")),
                                reader.GetInt32(reader.GetOrdinal("room_id")),
                                reader.GetInt32(reader.GetOrdinal("guest_id")),
                                reader.GetDateTime(reader.GetOrdinal("check_in_date")),
                                reader.GetDateTime(reader.GetOrdinal("check_out_date")),
                                reader.GetDecimal(reader.GetOrdinal("amount_paid"))
                            );
                        }
                    }
                }
            }

            return booking;
        }


    }
}
