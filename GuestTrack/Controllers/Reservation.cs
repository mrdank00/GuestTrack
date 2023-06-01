using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuestTrack.Controllers
{
    public class Reservation
    {
        DatabaseManager dbManager = new DatabaseManager();

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
                    string insertQuery = @"INSERT INTO Reservations ( hotel_id, room_id, guest_id, check_in_date, check_out_date, payment_info, RoomName, Guestname, ReservationType, Reservationtypeid, Adult, Children) 
                                   VALUES ( @HotelId, @RoomId, @GuestId, @CheckInDate, @CheckOutDate, @PaymentInfo, @RoomName, @GuestName, @ReservationType, @ReservationTypeId, @Adult, @Children)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                    
                        command.Parameters.AddWithValue("@HotelId", HotelId);
                        command.Parameters.AddWithValue("@RoomId", RoomId);
                        command.Parameters.AddWithValue("@GuestId", GuestId);
                        command.Parameters.AddWithValue("@CheckInDate", CheckInDate);
                        command.Parameters.AddWithValue("@CheckOutDate", CheckOutDate);
                        command.Parameters.AddWithValue("@PaymentInfo", PaymentInfo);
                        command.Parameters.AddWithValue("@RoomName", RoomName);
                        command.Parameters.AddWithValue("@GuestName", GuestName);
                        command.Parameters.AddWithValue("@ReservationType", ReservationType);
                        command.Parameters.AddWithValue("@ReservationTypeId", ReservationTypeId);
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
                            Console.WriteLine("Error: " + ex.Message);
                        }
                    }
                }
            }
        

        // Additional properties


    }
}
