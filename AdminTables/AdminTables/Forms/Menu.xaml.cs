using System;
using System.Collections.Generic;
using System.Data;
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

namespace AdminTables.Forms
{
    /// <summary>
    /// Логика взаимодействия для Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        string connectionString = "Host=45.139.78.55;Port=5432;Username=postgres;Password=qwer123;Database=Restoran;";

        public Menu()
        {
            InitializeComponent();
            LoadOrders();
            LoadFood();
            CheckInfoStol.Visibility = Visibility.Hidden;
            Orders.Visibility = Visibility.Hidden;
            Food.Visibility = Visibility.Hidden;
            Start_BT.Visibility = Visibility.Hidden;
        }

        private void LoadOrders()
        {
            string query = @"
    SELECT
        o.id,
        c.first_name AS ""Имя"",
        c.last_name AS ""Фамилия"",
        c.phone AS ""Телефон"",
        o.order_time AS ""Время заказа"",
        s.name AS ""Статус"",
        ot.name AS ""Тип доставки"",
        d.name AS ""Блюдо"",
        o.delivery_address AS ""Адрес доставки"",
        o.total_price AS ""Общая стоимость""
    FROM
        public.orders o
    JOIN public.customers c ON o.customer_id = c.id
    JOIN public.statuses s ON o.status_id = s.id
    JOIN public.order_types ot ON o.order_type_id = ot.id
    LEFT JOIN public.order_items oi ON o.id = oi.order_id
    LEFT JOIN public.dishes d ON oi.dish_id = d.id
";


            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        if (dataTable.Rows.Count == 0)
                        {
                            MessageBox.Show("Данные не найдены.");
                        }

                        ViewOrders.ItemsSource = dataTable.DefaultView;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
                }
            }
        }


        private string GetReservationInfo(int tableId, DateTime reservationDate)
        {
            string reservationInfo = string.Empty;
            string query = @"
SELECT 
    c.first_name, 
    c.last_name, 
    c.phone, 
    r.created_at, 
    r.end_time
FROM 
    public.reservations r
JOIN 
    public.customers c ON r.customer_id = c.id
WHERE 
    r.table_id = @tableId
    AND DATE(r.reservation_time) = @reservationDate";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@tableId", tableId);
                        command.Parameters.AddWithValue("@reservationDate", reservationDate.Date);

                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                DateTime createdAt = reader["created_at"] != DBNull.Value
                                    ? Convert.ToDateTime(reader["created_at"])
                                    : DateTime.MinValue;

                                DateTime endTime = reader["end_time"] != DBNull.Value
                                    ? Convert.ToDateTime(reader["end_time"])
                                    : DateTime.MinValue;

                                reservationInfo =
                                    $"{reader["first_name"] ?? "N/A"}\n" +
                                    $"{reader["last_name"] ?? "N/A"}\n" +
                                    $"{reader["phone"] ?? "N/A"}\n" +
                                    $"{createdAt:dd.MM.yyyy}\n" +           
                                    $"{createdAt:HH:mm}\n" +                
                                    $"{endTime:HH:mm}";                     
                            }
                            else
                            {
                                reservationInfo = "Стол не зарезервирован";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    reservationInfo = $"Ошибка получения информации: {ex.Message}";
                }
            }

            return reservationInfo;
        }




        private void InfoTable(string reservationInfo)
        {
            string[] info = reservationInfo.Split('\n');
            if (info.Length >= 6)
            {
                tb_Name.Text = info[0];
                tb_LastName.Text = info[1];
                tb_Phone.Text = info[2];
                tb_Time.Text = info[3];        
                tb_startTime.Text = info[4];    
                tb_endTime.Text = info[5];     
            }
            else
            {
                tb_Name.Text = "Информация не найдена.";
                tb_LastName.Text = string.Empty;
                tb_Phone.Text = string.Empty;
                tb_Time.Text = string.Empty;
                tb_startTime.Text = string.Empty;
                tb_endTime.Text = string.Empty;
            }
        }




        private void LoadFood()
        {
            string query = @"
SELECT 
    c.name AS ""Категория"",
    d.name AS ""Название блюда"", 
    d.description AS ""Описание"",
    d.price AS ""Цена""
FROM Dishes d
JOIN Categories c ON d.category_id = c.id;";



            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        if (dataTable.Rows.Count == 0)
                        {
                            MessageBox.Show("Данные не найдены.");
                        }


                        FoodView.ItemsSource = dataTable.DefaultView;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
                }
            }

        }

        private void CreateFood()
        {
            string category = TB_Cat.Text.Trim();
            string dishName = TB_Name.Text.Trim();
            string priceText = TB_Price.Text.Trim();
            string description = TB_discript.Text.Trim();
            
           

          
            if (string.IsNullOrWhiteSpace(category) || string.IsNullOrWhiteSpace(dishName) ||
                string.IsNullOrWhiteSpace(priceText) || string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            if (!decimal.TryParse(priceText, out decimal price) || price <= 0)
            {
                MessageBox.Show("Введите корректную цену.");
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    
                    string checkCategoryQuery = "SELECT id FROM Categories WHERE name = @category";
                    int categoryId;

                    using (var cmd = new NpgsqlCommand(checkCategoryQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("category", category);
                        object result = cmd.ExecuteScalar();

                       
                        if (result == null)
                        {
                            string insertCategoryQuery = "INSERT INTO Categories (name) VALUES (@category) RETURNING id";

                            using (var insertCmd = new NpgsqlCommand(insertCategoryQuery, connection))
                            {
                                insertCmd.Parameters.AddWithValue("category", category);
                                categoryId = Convert.ToInt32(insertCmd.ExecuteScalar());
                            }
                        }
                        else
                        {
                            categoryId = Convert.ToInt32(result);
                        }
                    }

                    
                    string insertDishQuery = "INSERT INTO Dishes (name, description, price, category_id) " +
                                             "VALUES (@name, @description, @price, @category_id)";

                    using (var cmd = new NpgsqlCommand(insertDishQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("name", dishName);
                        cmd.Parameters.AddWithValue("description", description);
                        cmd.Parameters.AddWithValue("price", price);
                        cmd.Parameters.AddWithValue("category_id", categoryId);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Блюдо успешно добавлено.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
              
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void СreateButton_Click(object sender, RoutedEventArgs e)
        {
            CreateUser Menu = new CreateUser();
            Menu.Show();
        }

        private void CurerButton_Click(object sender, RoutedEventArgs e)
        {
            Orders.Visibility = System.Windows.Visibility.Hidden;
            CheckStol.Visibility = System.Windows.Visibility.Hidden;
            Food.Visibility = System.Windows.Visibility.Visible;

        }

        private void ViewStolButton_Click_1(object sender, RoutedEventArgs e)
        {
            Orders.Visibility = System.Windows.Visibility.Hidden;
            CheckStol.Visibility = System.Windows.Visibility.Visible;
            Food.Visibility = System.Windows.Visibility.Hidden;
        }
        

        private void CheckOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            Orders.Visibility = System.Windows.Visibility.Visible;
            CheckStol.Visibility = System.Windows.Visibility.Hidden;
            Food.Visibility = System.Windows.Visibility.Hidden;
        }

        private void TableOneButton_Click(object sender, RoutedEventArgs e)
        {
            CheckInfoStol.Visibility = Visibility.Visible;

            
            DateTime selectedDate = DataCheck.SelectedDate ?? DateTime.Today;

            InfoTable(GetReservationInfo(1, selectedDate));
        }


        private void TableTwoButton_Click(object sender, RoutedEventArgs e)
        {
            CheckInfoStol.Visibility = Visibility.Visible;


            DateTime selectedDate = DataCheck.SelectedDate ?? DateTime.Today;

            InfoTable(GetReservationInfo(2, selectedDate));
        }

        private void TableThreeButton_Click(object sender, RoutedEventArgs e)
        {
            CheckInfoStol.Visibility = Visibility.Visible;


            DateTime selectedDate = DataCheck.SelectedDate ?? DateTime.Today;

            InfoTable(GetReservationInfo(3, selectedDate));
        }

        private void TableFourButton_Click(object sender, RoutedEventArgs e)
        {
            CheckInfoStol.Visibility = Visibility.Visible;


            DateTime selectedDate = DataCheck.SelectedDate ?? DateTime.Today;

            InfoTable(GetReservationInfo(4, selectedDate));
        }

        private void TableFiveButton_Click(object sender, RoutedEventArgs e)
        {
            CheckInfoStol.Visibility = Visibility.Visible;


            DateTime selectedDate = DataCheck.SelectedDate ?? DateTime.Today;

            InfoTable(GetReservationInfo(5, selectedDate));
        }

        private void TableSixButton_Click(object sender, RoutedEventArgs e)
        {
            CheckInfoStol.Visibility = Visibility.Visible;


            DateTime selectedDate = DataCheck.SelectedDate ?? DateTime.Today;

            InfoTable(GetReservationInfo(6, selectedDate));
        }

        private void CloseInfo(object sender, RoutedEventArgs e)
        {
            
            CheckInfoStol.Visibility = Visibility.Hidden;
        }

        private void dobavit(object sender, RoutedEventArgs e)
        {
            CreateFood();
        }
        private void ActiveOrders()
        {

            string query = @"
    SELECT
        o.id,
        c.first_name AS ""Имя"",
        c.last_name AS ""Фамилия"",
        c.phone AS ""Телефон"",
        o.order_time AS ""Время заказа"",
        s.name AS ""Статус"",
        ot.name AS ""Тип доставки"",
        d.name AS ""Блюдо"",
        o.delivery_address AS ""Адрес доставки"",
        o.total_price AS ""Общая стоимость""
    FROM
        public.orders o
    JOIN public.customers c ON o.customer_id = c.id
    JOIN public.statuses s ON o.status_id = s.id
    JOIN public.order_types ot ON o.order_type_id = ot.id
    LEFT JOIN public.order_items oi ON o.id = oi.order_id
    LEFT JOIN public.dishes d ON oi.dish_id = d.id
    WHERE s.name = 'Ожидание' AND ot.name = 'Получение в ресторане'";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        

                        ViewOrders.ItemsSource = dataTable.DefaultView;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
                }
            }
        }
        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            ActiveOrders();
            Start_BT.Visibility = Visibility.Visible;

        }
        private void DiliveryOrders()
        {

            string query = @"
    SELECT
        o.id,
        c.first_name AS ""Имя"",
        c.last_name AS ""Фамилия"",
        c.phone AS ""Телефон"",
        o.order_time AS ""Время заказа"",
        s.name AS ""Статус"",
        ot.name AS ""Тип доставки"",
        d.name AS ""Блюдо"",
        o.delivery_address AS ""Адрес доставки"",
        o.total_price AS ""Общая стоимость""
    FROM
        public.orders o
    JOIN public.customers c ON o.customer_id = c.id
    JOIN public.statuses s ON o.status_id = s.id
    JOIN public.order_types ot ON o.order_type_id = ot.id
    LEFT JOIN public.order_items oi ON o.id = oi.order_id
    LEFT JOIN public.dishes d ON oi.dish_id = d.id
    WHERE ot.name = 'Доставка'";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);



                        ViewOrders.ItemsSource = dataTable.DefaultView;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
                }
            }
        }

        private void Start_BT_Click(object sender, RoutedEventArgs e)
        {
            if (ViewOrders.SelectedItem is DataRowView selectedRow)
            {
                int orderId = Convert.ToInt32(selectedRow["id"]); 

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        string updateQuery = "UPDATE public.orders SET status_id = 3 WHERE id = @orderId";

                        using (NpgsqlCommand command = new NpgsqlCommand(updateQuery, connection))
                        {
                            command.Parameters.AddWithValue("@orderId", orderId);
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Статус заказа обновлён на 'Выполнено'.");
                                LoadOrders(); 
                            }
                            else
                            {
                                MessageBox.Show("Не удалось обновить заказ.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при обновлении: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ.");
            }
        }

   

        private void ALL_RB_Click(object sender, RoutedEventArgs e)
        {
            LoadOrders();
            Start_BT.Visibility = Visibility.Hidden;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Dilivery_RB_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Dilivery_RB_Click(object sender, RoutedEventArgs e)
        {
            DiliveryOrders();

        }
    }
}
