using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Cryptography;
using Npgsql;

namespace AdminTables
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string connectionString = "Host=45.139.78.55;Port=5432;Username=postgres;Password=qwer123;Database=Restoran;";
        public MainWindow()
        {
            InitializeComponent();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;

            AuthResult result = AuthenticateUser(email, password);

            switch (result)
            {
                case AuthResult.Success:
                    Forms.Menu mainWindow = new Forms.Menu();
                    mainWindow.Show();
                    this.Close();
                    break;

                case AuthResult.InvalidCredentials:
                    MessageBox.Show("Неверный email или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;

                case AuthResult.NoAdminRights:
                    MessageBox.Show("Не хватает уровня доступа для входа в систему.", "Доступ запрещён", MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;

                case AuthResult.Error:
                   
                    break;
            }
        }

        enum AuthResult
        {
            Success,
            InvalidCredentials,
            NoAdminRights,
            Error
        }



        private AuthResult AuthenticateUser(string email, string password)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                SELECT u.password_hash, r.name AS role_name
                FROM users u
                JOIN roles r ON u.role_id = r.id
                WHERE u.email = @Email";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);

                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                               
                                return AuthResult.InvalidCredentials;
                            }

                            string storedPasswordHash = reader["password_hash"].ToString();
                            string roleName = reader["role_name"].ToString();

                            string enteredPasswordHash = HashPassword(password);

                            if (storedPasswordHash != enteredPasswordHash)
                            {
                                
                                return AuthResult.InvalidCredentials;
                            }

                            if (roleName != "Администратор")
                            {
                               
                                return AuthResult.NoAdminRights;
                            }

                            return AuthResult.Success;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return AuthResult.Error;
            }
        }



        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();

                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2")); 
                }

                return builder.ToString();
            }
        }

    }
}