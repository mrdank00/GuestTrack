using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.SqlClient;

namespace GuestTrack
{
    public partial class frmlogin : Form
    {
        private static DatabaseManager dbManager = new DatabaseManager();
        public frmlogin()
        {
            InitializeComponent();
           
        }
      
       
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (AuthenticateUser(username, password))
            {
                frmMain frmMain = new frmMain();
                frmMain.Show();
                this.Hide();

                // Open the main form or perform other actions
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
                // Clear the text fields or perform other actions
                txtUsername.Text = string.Empty;
                txtPassword.Text = string.Empty;
            }
        }
        private bool AuthenticateUser(string username, string password)
        {
            // Perform authentication logic here
            // You can query the login table in the database to check if the entered credentials are valid

            using (SqlConnection connection = dbManager.Guestcon())
            {
                connection.Open();

                string selectQuery = "SELECT COUNT(*) FROM Userprofiles WHERE Username = @Username AND Passwordhash = @Password";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    int count = (int)command.ExecuteScalar();

                    // If count is greater than 0, the user is authenticated
                    return count > 0;
                }
            }
        }
    }
    }

