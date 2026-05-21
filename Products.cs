using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static demoA.Session;

namespace demoA
{
    public partial class Products : Form
    {
        static string connectionString =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DemoExam;Integrated Security=true";

        private bool canUseFilters = false;
        private bool filtersAreLoading = false;

        public Products()
        {
            InitializeComponent();
        }

        private void Products_Load(object sender, EventArgs e)
        {
            if (CurrentSession.CurrentUser != null)
            {
                if (CurrentSession.CurrentUser.Role == UserRole.Guest)
                {
                    labelUser.Text = "Пользователь: Гость";
                }
                else
                {
                    labelUser.Text =
                        $"Пользователь: {CurrentSession.CurrentUser.SurName} {CurrentSession.CurrentUser.FirstName} {CurrentSession.CurrentUser.MiddleName}";
                }
            }

            SetupRoleAccess();
            SetupFilterControls();

            LoadProducts();
        }

        private void SetupRoleAccess()
        {
            UserRole role = CurrentSession.CurrentUser == null ? UserRole.Guest : CurrentSession.CurrentUser.Role;

            canUseFilters = role == UserRole.Manager || role == UserRole.Admin;

            textBoxSearch.Enabled = canUseFilters;
            if (textBoxSearch.Enabled == false)
            {
                textBoxSearch.BackColor = Color.LightGray;
                textBoxSearch.Text = "Недоступно";
            }
            comboBoxSupplier.Enabled = canUseFilters;
            comboBoxRemain.Enabled = canUseFilters;

            btnOrders.Visible = canUseFilters;
            if (CurrentSession.CurrentUser.Role == Session.UserRole.Admin)
                btnAddProduct.Visible = true;
        }

        private void SetupFilterControls()
        {
            filtersAreLoading = true;

            comboBoxRemain.Items.Clear();
            comboBoxRemain.Items.Add("Без сортировки");
            comboBoxRemain.Items.Add("По возрастанию");
            comboBoxRemain.Items.Add("По убыванию");
            comboBoxRemain.SelectedIndex = 0;

            LoadSuppliers();

            filtersAreLoading = false;
        }

        private void LoadSuppliers()
        {
            comboBoxSupplier.Items.Clear();
            comboBoxSupplier.Items.Add("Все поставщики");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = @"
                        SELECT DISTINCT [Поставщик]
                        FROM [Товар]
                        WHERE [Поставщик] IS NOT NULL
                        ORDER BY [Поставщик]";

                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBoxSupplier.Items.Add(Convert.ToString(reader["Поставщик"]));
                        }
                    }
                }

                comboBoxSupplier.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ошибка загрузки поставщиков:\n" + ex.Message,
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void FilterChanged(object sender, EventArgs e)
        {
            if (filtersAreLoading)
                return;

            LoadProducts();
        }

        private void LoadProducts()
        {
            flowLayoutProducts.Controls.Clear();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    StringBuilder sql = new StringBuilder();

                    sql.Append(@"
                        SELECT
                            [id товара] AS ProductId,
                            [Категория товара] AS Category,
                            [Наименование товара] AS ProductName,
                            [Описание товара] AS Description,
                            [Производитель] AS Manufacturer,
                            [Поставщик] AS Supplier,
                            [Цена] AS Price,
                            [Единица измерения] AS Unit,
                            [Кол-во на складе] AS Count,
                            [Действующая скидка] AS Discount,
                            [Фото] AS Photo
                        FROM [Товар]
                        WHERE 1 = 1
                    ");

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = connection;

                        if (canUseFilters)
                        {
                            AddSearchCondition(sql, cmd);
                            AddSupplierFilter(sql, cmd);
                            AddSort(sql);
                        }
                        else
                        {
                            sql.Append(" ORDER BY [Наименование товара]");
                        }

                        cmd.CommandText = sql.ToString();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProductCard card = new ProductCard();

                                card.SetProductData(
                                    reader["ProductId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ProductId"]),
                                    Convert.ToString(reader["Category"]),
                                    Convert.ToString(reader["ProductName"]),
                                    Convert.ToString(reader["Description"]),
                                    Convert.ToString(reader["Manufacturer"]),
                                    Convert.ToString(reader["Supplier"]),
                                    reader["Price"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Price"]),
                                    Convert.ToString(reader["Unit"]),
                                    reader["Count"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Count"]),
                                    reader["Discount"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Discount"]),
                                    Convert.ToString(reader["Photo"])
                                );

                                if (CurrentSession.CurrentUser.Role == Session.UserRole.Admin)
                                {
                                    card.ProductSelected += ProductSelected;
                                }

                                flowLayoutProducts.Controls.Add(card);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ошибка загрузки товаров:\n" + ex.Message,
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void ProductSelected(object sender, EventArgs e)
        {
            ProductCard card = sender as ProductCard;

            if (card == null)
                return;

            using (ProductEdit form = new ProductEdit(card.ProductId))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadProducts();
                }
            }
        }

        private void AddSearchCondition(StringBuilder sql, SqlCommand cmd)
        {
            string searchText = textBoxSearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchText))
                return;

            string[] words = searchText.Split(
                new char[] { ' ' },
                StringSplitOptions.RemoveEmptyEntries
            );

            for (int i = 0; i < words.Length; i++)
            {
                string paramName = "@search" + i;

                sql.Append($@"
                    AND (
                        ISNULL([Артикул], '') LIKE {paramName}
                        OR ISNULL([Категория товара], '') LIKE {paramName}
                        OR ISNULL([Наименование товара], '') LIKE {paramName}
                        OR ISNULL([Описание товара], '') LIKE {paramName}
                        OR ISNULL([Производитель], '') LIKE {paramName}
                        OR ISNULL([Поставщик], '') LIKE {paramName}
                        OR ISNULL([Единица измерения], '') LIKE {paramName}
                        OR ISNULL(CONVERT(NVARCHAR(50), [Цена]), '') LIKE {paramName}
                        OR ISNULL(CONVERT(NVARCHAR(50), [Действующая скидка]), '') LIKE {paramName}
                        OR ISNULL(CONVERT(NVARCHAR(50), [Кол-во на складе]), '') LIKE {paramName}
                        OR ISNULL([Фото], '') LIKE {paramName}
                    )
                ");

                cmd.Parameters.AddWithValue(paramName, "%" + words[i] + "%");
            }
        }

        private void AddSupplierFilter(StringBuilder sql, SqlCommand cmd)
        {
            if (comboBoxSupplier.SelectedItem == null)
                return;

            string selectedSupplier = comboBoxSupplier.SelectedItem.ToString();

            if (selectedSupplier == "Все поставщики")
                return;

            sql.Append(" AND [Поставщик] = @supplier");
            cmd.Parameters.AddWithValue("@supplier", selectedSupplier);
        }

        private void AddSort(StringBuilder sql)
        {
            if (comboBoxRemain.SelectedItem == null)
            {
                sql.Append(" ORDER BY [Наименование товара]");
                return;
            }

            string selectedSort = comboBoxRemain.SelectedItem.ToString();

            if (selectedSort == "По возрастанию")
            {
                sql.Append(" ORDER BY [Кол-во на складе] ASC");
            }
            else if (selectedSort == "По убыванию")
            {
                sql.Append(" ORDER BY [Кол-во на складе] DESC");
            }
            else
            {
                sql.Append(" ORDER BY [Наименование товара]");
            }
        }

        private void btnExitUser_Click(object sender, EventArgs e)
        {
            CurrentSession.CurrentUser = null;

            new Login().Show();

            this.Close();
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            using (ProductEdit form = new ProductEdit())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadProducts();
                }
            }
        }

        private void btnOrders_Click(object sender, EventArgs e)
        {
            new Orders().Show();

            this.Close();
        }
    }
}