using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static demo.Session;

namespace demo
{
    public partial class Products : Form
    {
        private bool filter = false;
        private bool filterload = false;
        public Products()
        {
            InitializeComponent();
        }

        private void Products_Load(object sender, EventArgs e)
        {
            SetupAccess();
            SetupFilter();

            LoadProducts();

            if (CacheSession.user != null)
            {
                if (CacheSession.user.Role == UserRole.Guest)
                {
                    labelCurrentUser.Text = "Пользователь: Гость";
                }
                else
                {
                    labelCurrentUser.Text = $"Пользователь: {CacheSession.user.SurName} {CacheSession.user.FirstName} {CacheSession.user.MiddleName}";
                }
            }
        }

        private void SetupAccess()
        {
            UserRole role = CacheSession.user == null ? UserRole.Guest : CacheSession.user.Role;

            filter = role == UserRole.Manager || role == UserRole.Admin;

            textBoxSearch.Enabled = filter;

            if (textBoxSearch.Enabled == false)
            {
                textBoxSearch.BackColor = Color.LightGray;
                textBoxSearch.Text = "Недоступно";
            }

            comboBoxSupplier.Enabled = filter;
            comboBoxRemain.Enabled = filter;

            buttonOrders.Visible = filter;

            if (CacheSession.user.Role == Session.UserRole.Admin)
                buttonAddProduct.Visible = true;
        }

        private void buttonExitUser_Click(object sender, EventArgs e)
        {
            CacheSession.user = null;

            new Auth().Show();

            this.Close();
        }

        private void LoadProducts()
        {
            flowLayoutListProducts.Controls.Clear();

            try
            {
                using (SqlConnection connection = new SqlConnection(CacheSession.connectionString))
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

                        if (filter)
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
                                ProductItem card = new ProductItem();

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

                                if (CacheSession.user.Role == Session.UserRole.Admin)
                                {
                                    card.ProductSelect += ProductSelect;
                                }

                                flowLayoutListProducts.Controls.Add(card);
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

        private void ProductSelect(object sender, EventArgs e)
        {
            ProductItem card = sender as ProductItem;

            if (card == null)
                return;

            using (ProductItemEdit form = new ProductItemEdit(card.ProductId))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadProducts();
                }
            }
        }

        private void FilterChanged(object sender, EventArgs e)
        {
            if (filterload)
                return;

            LoadProducts();
        }

        private void SetupFilter()
        {
            filterload = true;

            comboBoxRemain.Items.Clear();
            comboBoxRemain.Items.Add("Без сортировки");
            comboBoxRemain.Items.Add("По возрастанию");
            comboBoxRemain.Items.Add("По убыванию");
            comboBoxRemain.SelectedIndex = 0;

            LoadSuppliers();

            filterload = false;
        }

        private void LoadSuppliers()
        {
            comboBoxSupplier.Items.Clear();
            comboBoxSupplier.Items.Add("Все поставщики");

            try
            {
                using (SqlConnection connection = new SqlConnection(CacheSession.connectionString))
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

            string selectSupplier = comboBoxSupplier.SelectedItem.ToString();

            if (selectSupplier == "Все поставщики")
                return;

            sql.Append(" AND [Поставщик] = @supplier");
            cmd.Parameters.AddWithValue("@supplier", selectSupplier);
        }

        private void AddSort(StringBuilder sql)
        {
            if (comboBoxRemain.SelectedItem == null)
            {
                sql.Append(" ORDER BY [Наименование товара]");
                return;
            }

            string selectSort = comboBoxRemain.SelectedItem.ToString();

            if (selectSort == "По возрастанию")
            {
                sql.Append(" ORDER BY [Кол-во на складе] ASC");
            }
            else if (selectSort == "По убыванию")
            {
                sql.Append(" ORDER BY [Кол-во на складе] DESC");
            }
            else
            {
                sql.Append(" ORDER BY [Наименование товара]");
            }
        }

        private void buttonAddProduct_Click(object sender, EventArgs e)
        {
            using (ProductItemEdit form = new ProductItemEdit())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadProducts();
                }
            }
        }
    }
}
