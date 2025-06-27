using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Npgsql;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace AdminTables.Forms
{
    public partial class CreateUser : Window
    {
        string connectionString = "Host=45.139.78.55;Port=5432;Username=postgres;Password=qwer123;Database=Restoran;";
        public CreateUser()
        {
            InitializeComponent();
            LoadRoles();
        }

        private void LoadRoles()
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT id, name FROM roles";
                
                using (var cmd = new NpgsqlCommand(sql, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    RoleComboBox.Items.Clear();

                    var rolesDictionary = new Dictionary<string, int>();

                    while (reader.Read())
                    {
                        int roleId = reader.GetInt32(0);
                        string roleName = reader.GetString(1);

                        rolesDictionary.Add(roleName, roleId);

                        RoleComboBox.Items.Add(roleName);
                    }

                    RoleComboBox.Tag = rolesDictionary;
                }
            }
        }

        private void CreateUsers()
        {
            string firstName = FirstNameTextBox.Text.Trim();
            string lastName = LastNameTextBox.Text.Trim();
            string email = EmailTextBox.Text.Trim();
            string password = PasswordBox.Password.Trim();
            string role = RoleComboBox.Text.Trim();
            string phone = PhoneTextBox.Text.Trim();
            DateTime createdAt = DateTime.Now;

         
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(role) || string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

          
            phone = FormatPhoneNumber(phone);
            if (phone == null)
            {
                MessageBox.Show("Введите корректный номер телефона в формате +7XXXXXXXXXX.");
                return;
            }

            try
            {
                var rolesDictionary = (Dictionary<string, int>)RoleComboBox.Tag;

                if (!rolesDictionary.ContainsKey(role))
                {
                    MessageBox.Show("Роль не найдена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int roleId = rolesDictionary[role];

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    
                    string checkEmailSql = "SELECT COUNT(*) FROM users WHERE email = @email";
                    using (var checkCmd = new NpgsqlCommand(checkEmailSql, connection))
                    {
                        checkCmd.Parameters.AddWithValue("email", email);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Пользователь с таким email уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }

               
                    string sql = "INSERT INTO users (email, password_hash, role_id, created_at) " +
                                 "VALUES (@email, @password, @role_id, @created_at) RETURNING id";

                    int userId = 0;

                    using (var cmd = new NpgsqlCommand(sql, connection))
                    {
                        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                        if (!Regex.IsMatch(email, emailPattern))
                        {
                            MessageBox.Show("Введите корректный email.");
                            return;
                        }

                        cmd.Parameters.AddWithValue("email", email);
                        cmd.Parameters.AddWithValue("password", HashPassword(password));
                        cmd.Parameters.AddWithValue("role_id", roleId);
                        cmd.Parameters.AddWithValue("created_at", createdAt);

                        
                        userId = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                 
                    if (role == "Сотрудник")
                    {
                        string insertEmployeeSql = "INSERT INTO employees (user_id, first_name, last_name, phone, position) " +
                                                   "VALUES (@user_id, @first_name, @last_name, @phone, @position)";

                        using (var cmd = new NpgsqlCommand(insertEmployeeSql, connection))
                        {
                            cmd.Parameters.AddWithValue("user_id", userId);
                            cmd.Parameters.AddWithValue("first_name", firstName);
                            cmd.Parameters.AddWithValue("last_name", lastName);
                            cmd.Parameters.AddWithValue("phone", phone);
                            string position = PositionTextBox.Text; 
                            cmd.Parameters.AddWithValue("position", position);

                            cmd.ExecuteNonQuery();
                        }
                    }
                    else if (role == "courier")
                    {
                        string vehicleType = VehicleTypeTextBox.Text.Trim();

                      
                        if (string.IsNullOrWhiteSpace(vehicleType))
                        {
                            MessageBox.Show("Введите тип транспорта.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                       
                        string insertCourierSql = "INSERT INTO couriers (user_id, first_name, last_name, phone, vehicle_type) " +
                                                  "VALUES (@user_id, @first_name, @last_name, @phone, @vehicle_type)";

                        using (var cmd = new NpgsqlCommand(insertCourierSql, connection))
                        {
                            cmd.Parameters.AddWithValue("user_id", userId);
                            cmd.Parameters.AddWithValue("first_name", firstName);
                            cmd.Parameters.AddWithValue("last_name", lastName);
                            cmd.Parameters.AddWithValue("phone", phone);
                            cmd.Parameters.AddWithValue("vehicle_type", vehicleType);

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Курьер добавлен успешно.");
                        }
                    }

                    MessageBox.Show("Пользователь успешно создан.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        
        private string FormatPhoneNumber(string phone)
        {
            
            phone = Regex.Replace(phone, @"\D", "");

            if (phone.StartsWith("8") && phone.Length == 11)
            {
                phone = "+7" + phone.Substring(1);
            }
            else if (phone.StartsWith("7") && phone.Length == 11)
            {
                phone = "+7" + phone.Substring(1);
            }
            else if (phone.StartsWith("+7") && phone.Length == 12)
            {
                
            }
            else
            {
                return null; 
            }

            return phone;
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

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            CreateUsers();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void RoleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RoleComboBox.SelectedItem == null) return;

            string selectedRole = RoleComboBox.SelectedItem.ToString();

            PositionLabel.Visibility = Visibility.Collapsed;
            PositionTextBox.Visibility = Visibility.Collapsed;
            VehicleTypeLabel.Visibility = Visibility.Collapsed;
            VehicleTypeTextBox.Visibility = Visibility.Collapsed;

            if (selectedRole == "courier")
            {
                VehicleTypeLabel.Visibility = Visibility.Visible;
                VehicleTypeTextBox.Visibility = Visibility.Visible;
            }
            else if (selectedRole == "Сотрудник")
            {
                PositionLabel.Visibility = Visibility.Visible;
                PositionTextBox.Visibility = Visibility.Visible;
            }
        }

    }
}
