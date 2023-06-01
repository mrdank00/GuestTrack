using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace GuestTrack.Controllers
{
    public class Guest
    {
        //  DatabaseManager dbManager = new DatabaseManager();
        private static DatabaseManager dbManager = new DatabaseManager();

        public int GuestId { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Nationality { get; set; }
        public string SpecialRequirements { get; set; }
        public DateTime DOB { get; set; }
        public string IDType { get; set; }
        public string IDNumber { get; set; }

        public Guest(int guestid, string name, string contact, string email, string address, string nationality,
            string specialRequirements, DateTime dob, string idType, string idNumber)
        {
            GuestId = guestid;
            Name = name;
            Contact = contact;
            Email = email;
            Address = address;
            Nationality = nationality;
            SpecialRequirements = specialRequirements;
            DOB = dob;
            IDType = idType;
            IDNumber = idNumber;
        }

        public void InsertGuest()
        {
            using (SqlConnection connection = dbManager.Guestcon())
            {
                string insertQuery = @"INSERT INTO Guests (name, contact, Email, Address, nationality, specialrequirements, DOB, idtype, idnumber)
                                   VALUES (@Name, @Contact, @Email, @Address, @Nationality, @SpecialRequirements, @DOB, @IDType, @IDNumber)";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", Name);
                    command.Parameters.AddWithValue("@Contact", Contact);
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@Address", Address);
                    command.Parameters.AddWithValue("@Nationality", Nationality);
                    command.Parameters.AddWithValue("@SpecialRequirements", SpecialRequirements);
                    command.Parameters.AddWithValue("@DOB", DOB);
                    command.Parameters.AddWithValue("@IDType", IDType);
                    command.Parameters.AddWithValue("@IDNumber", IDNumber);

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

        public static Guest FindByContact(string contact)
        {
            Guest guest = null;

            // Assuming you have a database connection and execute the query to find the guest by contact
            using (SqlConnection connection = dbManager.Guestcon())
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Guests WHERE contact = @Contact";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Contact", contact);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            guest = new Guest(
                                reader.GetInt32(reader.GetOrdinal("guest_id")), // Provide the guestid parameter
                                reader.GetString(reader.GetOrdinal("name")),
                                reader.GetString(reader.GetOrdinal("contact")),
                                reader.GetString(reader.GetOrdinal("Email")),
                                reader.GetString(reader.GetOrdinal("Address")),
                                reader.GetString(reader.GetOrdinal("nationality")),
                                reader.GetString(reader.GetOrdinal("specialrequirements")),
                                reader.GetDateTime(reader.GetOrdinal("DOB")),
                                reader.GetString(reader.GetOrdinal("idtype")),
                                reader.GetString(reader.GetOrdinal("idnumber"))
                            );
                        }
                    }
                }
            }

            return guest;
        }

        public static int GetNextGuestId()
        {
            int nextGuestId = 0;

            // Assuming you have a database connection and execute the query to retrieve the maximum guest ID
            using (SqlConnection connection = dbManager.Guestcon())
            {
                connection.Open();

                string selectQuery = "SELECT MAX(guest_id) FROM Guests";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    object result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        nextGuestId = Convert.ToInt32(result) + 1;
                    }
                    else
                    {
                        nextGuestId = 1; // If no existing guest IDs, start from 1
                    }
                }
            }

            return nextGuestId;
        }





    }
}


