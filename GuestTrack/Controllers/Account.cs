using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows;

namespace GuestTrack.Controllers
{
    public class Account
    {
        private static DatabaseManager dbManager = new DatabaseManager();

        public int LedgerId { get; set; }
        public int GuestId { get; set; }
        public int reservationid { get; set; }
        public string Guestname { get; set; }
        public string Reference { get; set; }
        public string TransactionType { get; set; }
        public string TransactionDescription { get; set; }
        public string TransactionSource { get; set; }
        public string Debit { get; set; }
        public double? Credit { get; set; }
        public double? Balance { get; set; }

        public string Itemname { get; set; }
        public string Roomno { get; set; }
        public string Category { get; set; }
        public string Saletype { get; set; }
        public string activeuser { get; set; }
        public string Source { get; set; }
        public string User { get; set; }
        public string Paymenttype { get; set; }
        public string Narration { get; set; }
        public double? Price { get; set; }
        public double? Quantity { get; set; }
        public double? Amount { get; set; }
        public double? Amountpaid { get; set; }
        public DateTime datepaid { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        // Constructor
        public Account() { }

        public Account(int ledgerId, int guestId, string reference, string transactionType, string transactionDescription,
            string transactionSource, string debit, double? credit, double? balance)
        {
            LedgerId = ledgerId;
            GuestId = guestId;
            Reference = reference;
            TransactionType = transactionType;
            TransactionDescription = transactionDescription;
            TransactionSource = transactionSource;
            Debit = debit;
            Credit = credit;
            Balance = balance;
        }
        public Account(string itemname,string category,string saletype,string source,double price,double quantity,double amount)
        {
            Itemname = itemname;
            Category = category;
            Saletype = saletype;
            Source = source;
            Price = price;
            Quantity = quantity;
            Amount = amount;

        }

        // Insert an account entry
        public void InsertguestLedger(string postype)
        {
            using (SqlConnection connection = dbManager.Guestcon())
            {
                string insertQuery = @"INSERT INTO GuestLedger (Guestid, ref, tranxtype, tranxdescription, tranxsource,
                Debit, Credit, Balance,date,guestname,reservation_id) VALUES ( @GuestId, @Reference, @TransactionType, @TransactionDescription,
                @TransactionSource, @Debit, @Credit, @Balance,@date,@guestname,@reservation_id)";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                   
                    command.Parameters.AddWithValue("@GuestId", GuestId);
                    command.Parameters.AddWithValue("@Reference", Reference);
                    command.Parameters.AddWithValue("@TransactionType", TransactionType);
                    command.Parameters.AddWithValue("@TransactionDescription", TransactionDescription);
                    command.Parameters.AddWithValue("@TransactionSource", TransactionSource);
                    command.Parameters.AddWithValue("@date", Date);
                    command.Parameters.AddWithValue("@guestname", Guestname);
                    command.Parameters.AddWithValue("@reservation_id", reservationid);
                    
                    if (postype == "d")
                    {
                       // MessageBox.Show("Debit");
                        command.Parameters.AddWithValue("@Debit", Debit);
                        command.Parameters.AddWithValue("@Credit", 0);
                    }
                    else
                    {
                       // MessageBox.Show("Credit");
                        command.Parameters.AddWithValue("@Debit", 0);
                        command.Parameters.AddWithValue("@Credit", Credit ?? (object)DBNull.Value);
                    }
                    command.Parameters.AddWithValue("@Balance", Balance ?? (object)DBNull.Value);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                           MessageBox.Show("Account entry inserted successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.ToString());
                    }
                }
            }
        }
        public void InsertguestTranx()
        {
            using (SqlConnection connection = dbManager.Guestcon())
            {
                string insertQuery = @"INSERT INTO GuestTranx (Itemname, Price, Qty, Amount, Category, Saletype, Source)
                                   VALUES (@ItemName, @Price, @Quantity, @Amount, @Category, @SaleType, @Source)";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@ItemName", Itemname);
                    command.Parameters.AddWithValue("@Price", Price);
                    command.Parameters.AddWithValue("@Quantity", Quantity);
                    command.Parameters.AddWithValue("@Amount", Amountpaid);
                    command.Parameters.AddWithValue("@Category", Category);
                    command.Parameters.AddWithValue("@SaleType", Saletype);
                    command.Parameters.AddWithValue("@Source", Source);

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
                        MessageBox.Show("Error: " + ex.ToString());
                    }
                }
            }
        }
        public void InsertPayment()
        {
            using (SqlConnection connection = dbManager.Guestcon())
            {
                string insertQuery = @"INSERT INTO GuestPayments (Gustid, datepaid, AmountPaid, Paymenttype, narration, ActiveUser,ref,reservation_id)
                                   VALUES (@GuestId, @DatePaid, @AmountPaid, @PaymentType, @Narration, @activeUser,@ref,@reservid)";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@GuestId", GuestId);
                    command.Parameters.AddWithValue("@DatePaid", datepaid);
                    command.Parameters.AddWithValue("@AmountPaid", Amountpaid);
                    command.Parameters.AddWithValue("@PaymentType", Paymenttype);
                    command.Parameters.AddWithValue("@Narration", Narration);
                    command.Parameters.AddWithValue("@activeUser", activeuser);
                    command.Parameters.AddWithValue("@ref", Reference);
                    command.Parameters.AddWithValue("@reservid", reservationid);

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
                        MessageBox.Show("Error: " + ex.ToString());
                    }
                }
            }
        }
        public DataTable GetBookingsByDateAndRoom()
        {
            DataTable bookingsTable = new DataTable();

    
            using (SqlConnection connection = dbManager.Guestcon())
            {

                connection.Open();

                string query = "SELECT date,ref,tranxtype,debit,credit,guestname,guestid FROM GuestLedger WHERE reservation_id=@gname";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@gname", reservationid);
           

                SqlDataReader reader = command.ExecuteReader();

                //Load the data reader into the DataTable
                bookingsTable.Load(reader);

                reader.Close();
            }

            return bookingsTable;
        }


        // Update an account entry
        public void UpdateAccount()
        {
            using (SqlConnection connection = dbManager.Guestcon())
            {
                string updateQuery = @"UPDATE GuestLedger SET Guestid = @GuestId, ref = @Reference, tranxtype = @TransactionType,
                tranxdescription = @TransactionDescription, tranxsource = @TransactionSource, Debit = @Debit,
                Credit = @Credit, Balance = @Balance WHERE ledgerid = @LedgerId";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@GuestId", GuestId);
                    command.Parameters.AddWithValue("@Reference", Reference);
                    command.Parameters.AddWithValue("@TransactionType", TransactionType);
                    command.Parameters.AddWithValue("@TransactionDescription", TransactionDescription);
                    command.Parameters.AddWithValue("@TransactionSource", TransactionSource);
                    command.Parameters.AddWithValue("@Debit", Debit);
                    command.Parameters.AddWithValue("@Credit", Credit ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Balance", Balance ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@LedgerId", LedgerId);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Account entry updated successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.ToString());
                    }
                }
            }
        }

        // Delete an account entry
        public void DeleteAccount()
        {
            using (SqlConnection connection = dbManager.Guestcon())
            {
                string deleteQuery = "DELETE FROM GuestLedger WHERE ledgerid = @LedgerId";

                using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@LedgerId", LedgerId);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Account entry deleted successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.ToString());
                    }
                }
            }
        }

        // Retrieve an account entry by ledger ID
        public static Account GetAccountById(int ledgerId)
        {
            Account account = null;

            using (SqlConnection connection = dbManager.Guestcon())
            {
                connection.Open();

                string selectQuery = "SELECT * FROM GuestLedger WHERE ledgerid = @LedgerId";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@LedgerId", ledgerId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            account = new Account(
                                reader.GetInt32(reader.GetOrdinal("ledgerid")),
                                reader.GetInt32(reader.GetOrdinal("Guestid")),
                                reader.GetString(reader.GetOrdinal("ref")),
                                reader.GetString(reader.GetOrdinal("tranxtype")),
                                reader.GetString(reader.GetOrdinal("tranxdescription")),
                                reader.GetString(reader.GetOrdinal("tranxsource")),
                                reader.GetString(reader.GetOrdinal("Debit")),
                                reader.IsDBNull(reader.GetOrdinal("Credit")) ? null : (double?)reader.GetDouble(reader.GetOrdinal("Credit")),
                                reader.IsDBNull(reader.GetOrdinal("Balance")) ? null : (double?)reader.GetDouble(reader.GetOrdinal("Balance"))
                            );
                        }
                    }
                }
            }

            return account;
        }
    }
}
