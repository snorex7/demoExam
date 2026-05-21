using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace demoA
{
    public partial class OrderEdit : Form
    {
        static string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DemoExam;Integrated Security=true";

        private int orderId;
        public OrderEdit(int orderId)
        {
            InitializeComponent();

            this.orderId = orderId;

            Text = "Редактирование заказа";
            btnDelete.Visible = true;

            LoadComboBoxes();
            LoadOrderData();
        }

        public OrderEdit()
        {
            InitializeComponent();

            Text = "Добавление заказа";
            //textID.Visible = false;
            //labelID.Visible = false;

            LoadComboBoxes();
        }

        private void LoadComboBoxes()
        {
            LoadComboBoxValues(comboBoxStatus, "[Заказы]", "[Статус заказа]");
            LoadAddressComboBox();
        }

        private void LoadComboBoxValues(ComboBox comboBox, string tableName,string columnName)
        {
            comboBox.Items.Clear();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = $@"
                        SELECT DISTINCT {columnName}
                        FROM {tableName}
                        WHERE {columnName} IS NOT NULL
                        ORDER BY {columnName}";

                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string value = Convert.ToString(reader[0]);

                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                comboBox.Items.Add(value);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ошибка загрузки списка:\n" + ex.Message,
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void LoadAddressComboBox()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = @"
                        SELECT
                            [id пункта] AS PointId,
                            CONCAT(
                                [Индекс], N', ',
                                [Город], N', ',
                                [Улица], N', ',
                                NCHAR(1076), N'. ',
                                [Дом]
                            ) AS PointAddress
                        FROM [Пункты выдачи]
                        ORDER BY [id пункта]
                    ";

                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        DataTable table = new DataTable();

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(table);
                        }

                        comboBoxAddress.DataSource = table;
                        comboBoxAddress.DisplayMember = "PointAddress";
                        comboBoxAddress.ValueMember = "PointId";
                        comboBoxAddress.SelectedIndex = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ошибка загрузки адресов пунктов выдачи:\n" + ex.Message,
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LoadOrderData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = @"
                        SELECT
                            p.[id позиции] AS PositionId,
                            z.[Номер заказа] AS OrderId,
                            p.[Артикул] AS Article,
                            z.[Статус заказа] AS Status,
                            z.[Адрес пункта выдачи] AS PointId,
                            z.[Дата заказа] AS OrderDate,
                            z.[Дата доставки] AS DeliveryDate

                        FROM [Позиция] p

                        INNER JOIN [Заказы] z
                            ON z.[Номер заказа] = p.[Id заказа]

                        WHERE p.[id позиции] = @id
                    ";

                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", orderId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                textArticle.Text = Convert.ToString(reader["Article"]).Trim();

                                SetComboBoxValue(
                                    comboBoxStatus,
                                    Convert.ToString(reader["Status"])
                                );

                                if (reader["PointId"] != DBNull.Value)
                                {
                                    comboBoxAddress.SelectedValue = Convert.ToInt32(reader["PointId"]);
                                }

                                if (reader["OrderDate"] != DBNull.Value)
                                {
                                    dateTimePickerOrder.Value = Convert.ToDateTime(reader["OrderDate"]);
                                }

                                if (reader["DeliveryDate"] != DBNull.Value)
                                {
                                    dateTimePickerDelivery.Value = Convert.ToDateTime(reader["DeliveryDate"]);
                                }
                            }
                            else
                            {
                                MessageBox.Show(
                                    "Заказ не найден.",
                                    "Ошибка",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error
                                );

                                Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ошибка загрузки заказа:\n" + ex.Message,
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void SetComboBoxValue(ComboBox comboBox, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                comboBox.SelectedIndex = -1;
                return;
            }

            if (!comboBox.Items.Contains(value))
            {
                comboBox.Items.Add(value);
            }

            comboBox.SelectedItem = value;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateOrderData())
                return;

            string message;

            if (orderId > 0)
            {
                message = "Вы уверены, что хотите изменить заказ?";
            }
            else
            {
                message = "Вы уверены, что хотите добавить заказ?";
            }

            DialogResult result = MessageBox.Show(
                message,
                "Подтверждение действия",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes)
                return;

            try
            {
                if (orderId > 0)
                {
                    UpdateOrder();
                }
                else
                {
                    InsertOrder();
                }

                MessageBox.Show(
                    "Заказ успешно сохранён.",
                    "Сохранение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ошибка сохранения заказа:\n" + ex.Message,
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void UpdateOrder()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string article = textArticle.Text.Trim();
                        string status = comboBoxStatus.SelectedItem.ToString();
                        int pointId = Convert.ToInt32(comboBoxAddress.SelectedValue);
                        DateTime orderDate = dateTimePickerOrder.Value.Date;
                        DateTime deliveryDate = dateTimePickerDelivery.Value.Date;

                        int productId = GetProductIdByArticle(connection, transaction, article);

                        string updatePositionSql = @"
                            UPDATE [Позиция]
                            SET
                                [Id товара] = @productId,
                                [Артикул] = @article
                            WHERE [id позиции] = @positionId
                        ";

                        using (SqlCommand cmd = new SqlCommand(updatePositionSql, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@productId", productId);
                            cmd.Parameters.AddWithValue("@article", article);
                            cmd.Parameters.AddWithValue("@positionId", orderId);

                            cmd.ExecuteNonQuery();
                        }

                        string updateOrderSql = @"
                            UPDATE z
                            SET
                                z.[Статус заказа] = @status,
                                z.[Адрес пункта выдачи] = @pointId,
                                z.[Дата заказа] = @orderDate,
                                z.[Дата доставки] = @deliveryDate
                            FROM [Заказы] z
                            INNER JOIN [Позиция] p
                                ON p.[Id заказа] = z.[Номер заказа]
                            WHERE p.[id позиции] = @positionId
                        ";

                        using (SqlCommand cmd = new SqlCommand(updateOrderSql, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@status", status);
                            cmd.Parameters.AddWithValue("@pointId", pointId);
                            cmd.Parameters.AddWithValue("@orderDate", orderDate);
                            cmd.Parameters.AddWithValue("@deliveryDate", deliveryDate);
                            cmd.Parameters.AddWithValue("@positionId", orderId);

                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private void InsertOrder()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string article = textArticle.Text.Trim();
                        string status = comboBoxStatus.SelectedItem.ToString();
                        int pointId = Convert.ToInt32(comboBoxAddress.SelectedValue);
                        DateTime orderDate = dateTimePickerOrder.Value.Date;
                        DateTime deliveryDate = dateTimePickerDelivery.Value.Date;

                        int productId = GetProductIdByArticle(connection, transaction, article);

                        string insertOrderSql = @"
                            INSERT INTO [Заказы]
                            (
                                [Дата заказа],
                                [Дата доставки],
                                [Адрес пункта выдачи],
                                [Статус заказа]
                            )
                            OUTPUT INSERTED.[Номер заказа]
                            VALUES
                            (
                                @orderDate,
                                @deliveryDate,
                                @pointId,
                                @status
                            )
                        ";

                        int newOrderId;

                        using (SqlCommand cmd = new SqlCommand(insertOrderSql, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@orderDate", orderDate);
                            cmd.Parameters.AddWithValue("@deliveryDate", deliveryDate);
                            cmd.Parameters.AddWithValue("@pointId", pointId);
                            cmd.Parameters.AddWithValue("@status", status);

                            newOrderId = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        string insertPositionSql = @"
                            INSERT INTO [Позиция]
                            (
                                [Id товара],
                                [Id заказа],
                                [Артикул],
                                [Количество]
                            )
                            VALUES
                            (
                                @productId,
                                @orderId,
                                @article,
                                @count
                            )
                        ";

                        using (SqlCommand cmd = new SqlCommand(insertPositionSql, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@productId", productId);
                            cmd.Parameters.AddWithValue("@orderId", newOrderId);
                            cmd.Parameters.AddWithValue("@article", article);
                            cmd.Parameters.AddWithValue("@count", 1);

                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private int GetProductIdByArticle(SqlConnection connection, SqlTransaction transaction, string article)
        {
            string sql = @"
                    SELECT TOP 1 [id товара]
                    FROM [Товар]
                    WHERE LTRIM(RTRIM([Артикул])) = @article
                ";

            using (SqlCommand cmd = new SqlCommand(sql, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@article", article.Trim());

                object result = cmd.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                {
                    throw new Exception("Товар с таким артикулом не найден.");
                }

                return Convert.ToInt32(result);
            }
        }

        private bool ValidateOrderData()
        {
            if (string.IsNullOrWhiteSpace(textArticle.Text))
            {
                MessageBox.Show("Введите артикул.", "Проверка данных", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textArticle.Focus();
                return false;
            }

            if (comboBoxStatus.SelectedItem == null)
            {
                MessageBox.Show("Выберите статус заказа.", "Проверка данных", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxStatus.Focus();
                return false;
            }

            if (comboBoxAddress.SelectedValue == null)
            {
                MessageBox.Show("Выберите адрес пункта выдачи.", "Проверка данных", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxAddress.Focus();
                return false;
            }

            return true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (orderId <= 0)
            {
                MessageBox.Show(
                    "Нельзя удалить заказ, который ещё не сохранён.",
                    "Удаление",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            DialogResult result = MessageBox.Show(
                "Вы уверены, что хотите удалить заказ?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes)
                return;

            try
            {
                DeleteOrder();

                MessageBox.Show(
                    "Заказ успешно удалён.",
                    "Удаление",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ошибка удаления заказа:\n" + ex.Message,
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void DeleteOrder()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int realOrderId;

                        string getOrderIdSql = @"
                            SELECT [Id заказа]
                            FROM [Позиция]
                            WHERE [id позиции] = @positionId
                        ";

                        using (SqlCommand cmd = new SqlCommand(getOrderIdSql, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@positionId", orderId);

                            object result = cmd.ExecuteScalar();

                            if (result == null || result == DBNull.Value)
                            {
                                throw new Exception("Позиция заказа не найдена.");
                            }

                            realOrderId = Convert.ToInt32(result);
                        }

                        string deletePositionSql = @"
                            DELETE FROM [Позиция]
                            WHERE [id позиции] = @positionId
                        ";

                        using (SqlCommand cmd = new SqlCommand(deletePositionSql, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@positionId", orderId);
                            cmd.ExecuteNonQuery();
                        }

                        string countPositionsSql = @"
                            SELECT COUNT(*)
                            FROM [Позиция]
                            WHERE [Id заказа] = @orderId
                        ";

                        int positionsCount;

                        using (SqlCommand cmd = new SqlCommand(countPositionsSql, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@orderId", realOrderId);
                            positionsCount = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        if (positionsCount == 0)
                        {
                            string deleteOrderSql = @"
                                DELETE FROM [Заказы]
                                WHERE [Номер заказа] = @orderId
                            ";

                            using (SqlCommand cmd = new SqlCommand(deleteOrderSql, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@orderId", realOrderId);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
