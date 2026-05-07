using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace demoA
{
    public partial class ProductEdit : Form
    {
        static string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DemoExam;Integrated Security=true";

        private int productId;
        private string currentPhotoPath = "";
        private string selectedPhotoPath = "";

        public ProductEdit(int productId)
        {
            InitializeComponent();

            this.productId = productId;

            Text = "Редактирование товара";
            btnDelete.Visible = true;

            LoadComboBoxes();
            LoadProductData();
        }

        public ProductEdit()
        {
            InitializeComponent();

            Text = "Добавление товара";

            LoadComboBoxes();
        }

        private void LoadComboBoxes()
        {
            LoadComboBoxValues(comboBoxCategory, "[Категория товара]");
            LoadComboBoxValues(comboBoxManufacturer, "[Производитель]");
        }

        private void LoadComboBoxValues(ComboBox comboBox, string columnName)
        {
            comboBox.Items.Clear();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = $@"
                        SELECT DISTINCT {columnName}
                        FROM [Товар]
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

        private void LoadProductData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = @"
                        SELECT
                            [id товара],
                            [Артикул],
                            [Наименование товара],
                            [Единица измерения],
                            [Цена],
                            [Поставщик],
                            [Производитель],
                            [Категория товара],
                            [Действующая скидка],
                            [Кол-во на складе],
                            [Описание товара],
                            [Фото]
                        FROM [Товар]
                        WHERE [id товара] = @id";

                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", productId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                textArticle.Text = Convert.ToString(reader["Артикул"]);
                                textName.Text = Convert.ToString(reader["Наименование товара"]);
                                textUnit.Text = Convert.ToString(reader["Единица измерения"]);
                                textSupplier.Text = Convert.ToString(reader["Поставщик"]);
                                richTextDescription.Text = Convert.ToString(reader["Описание товара"]);

                                numericPrice.Value = reader["Цена"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Цена"]);
                                numericCount.Value = reader["Кол-во на складе"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Кол-во на складе"]);
                                numericDiscount.Value = reader["Действующая скидка"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Действующая скидка"]);

                                SetComboBoxValue(comboBoxCategory, Convert.ToString(reader["Категория товара"]));
                                SetComboBoxValue(comboBoxManufacturer, Convert.ToString(reader["Производитель"]));

                                currentPhotoPath = Convert.ToString(reader["Фото"]);
                                selectedPhotoPath = currentPhotoPath;

                                if (ProductPicture.Image != null)
                                {
                                    ProductPicture.Image.Dispose();
                                    ProductPicture.Image = null;
                                }

                                ProductPicture.Image = LoadProductImage(currentPhotoPath);
                            }
                            else
                            {
                                MessageBox.Show(
                                    "Товар не найден.",
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
                    "Ошибка загрузки товара:\n" + ex.Message,
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

        private Image LoadProductImage(string path)
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;

                if (!string.IsNullOrWhiteSpace(path))
                {
                    path = path.Trim();

                    string full = Path.Combine(baseDir, path);

                    if (!File.Exists(full))
                    {
                        full = Path.Combine(baseDir, "Resources", "images", "cards", path);
                    }

                    if (File.Exists(full))
                    {
                        using (FileStream fs = new FileStream(full, FileMode.Open, FileAccess.Read))
                        using (Image img = Image.FromStream(fs))
                        {
                            return new Bitmap(img);
                        }
                    }
                }
            }
            catch
            {
            }

            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string stubPath = Path.Combine(baseDir, "Resources", "images", "picture.png");

                if (File.Exists(stubPath))
                {
                    using (FileStream fs = new FileStream(stubPath, FileMode.Open, FileAccess.Read))
                    using (Image img = Image.FromStream(fs))
                    {
                        return new Bitmap(img);
                    }
                }
            }
            catch
            {
            }

            return null;
        }

        private Image LoadImageFromFile(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (Image img = Image.FromStream(fs))
            {
                return new Bitmap(img);
            }
        }

        private void ProductPicture_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Выберите изображение товара";
                dialog.Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    selectedPhotoPath = dialog.FileName;

                    if (ProductPicture.Image != null)
                    {
                        ProductPicture.Image.Dispose();
                        ProductPicture.Image = null;
                    }

                    ProductPicture.Image = LoadImageFromFile(selectedPhotoPath);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Сохранение пока не реализовано.",
                "Информация",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Удаление пока не реализовано.",
                "Информация",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
    }
}
