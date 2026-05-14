using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
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
            textID.Visible = false;
            labelID.Visible = false;

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
                                textID.Text = Convert.ToString(reader["id товара"]);
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
                    if (!CheckImageSize(dialog.FileName))
                        return;

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
            if (!ValidateProductData())
                return;

            string message;

            if (productId > 0)
            {
                message = "Вы уверены, что хотите изменить товар?";
            }
            else
            {
                message = "Вы уверены, что хотите добавить товар?";
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
                string photoFileName = SaveSelectedPhoto();

                if (productId > 0)
                {
                    UpdateProduct(photoFileName);
                }
                else
                {
                    InsertProduct(photoFileName);
                }

                MessageBox.Show(
                    "Товар успешно сохранён.",
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
                    "Ошибка сохранения товара:\n" + ex.Message,
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Вы уверены, что хотите удалить этот товар?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    if (ProductInOrders(connection))
                    {
                        MessageBox.Show(
                            "Нельзя удалить товар, который присутствует в заказе.",
                            "Удаление невозможно",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );

                        return;
                    }

                    string sql = @"
                DELETE FROM [Товар]
                WHERE [id товара] = @id";

                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", productId);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Товар успешно удалён.",
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
                    "Ошибка удаления товара:\n" + ex.Message,
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private bool ProductInOrders(SqlConnection connection)
        {
            string sql = @"
            SELECT COUNT(*)
            FROM [Позиция]
            WHERE [id товара] = @id";

            using (SqlCommand cmd = new SqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("@id", productId);

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                return count > 0;
            }
        }

        private bool ValidateProductData()
        {
            if (string.IsNullOrWhiteSpace(textArticle.Text))
            {
                MessageBox.Show("Введите артикул товара.", "Проверка данных", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textArticle.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(textName.Text))
            {
                MessageBox.Show("Введите наименование товара.", "Проверка данных", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(textUnit.Text))
            {
                MessageBox.Show("Введите единицу измерения.", "Проверка данных", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textUnit.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(textSupplier.Text))
            {
                MessageBox.Show("Введите поставщика.", "Проверка данных", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textSupplier.Focus();
                return false;
            }

            if (comboBoxCategory.SelectedItem == null)
            {
                MessageBox.Show("Выберите категорию товара.", "Проверка данных", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxCategory.Focus();
                return false;
            }

            if (comboBoxManufacturer.SelectedItem == null)
            {
                MessageBox.Show("Выберите производителя.", "Проверка данных", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxManufacturer.Focus();
                return false;
            }

            return true;
        }

        private string SaveSelectedPhoto()
        {
            if (string.IsNullOrWhiteSpace(selectedPhotoPath))
            {
                return NormalizePhotoName(currentPhotoPath);
            }

            if (selectedPhotoPath == currentPhotoPath)
            {
                return NormalizePhotoName(currentPhotoPath);
            }

            if (!File.Exists(selectedPhotoPath))
            {
                return NormalizePhotoName(selectedPhotoPath);
            }

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            string cardsFolder = Path.Combine(
                baseDir,
                "Resources",
                "images",
                "cards"
            );

            if (!Directory.Exists(cardsFolder))
            {
                Directory.CreateDirectory(cardsFolder);
            }

            string extension = Path.GetExtension(selectedPhotoPath);

            if (string.IsNullOrWhiteSpace(extension))
            {
                extension = ".jpg";
            }

            string newFileName = Guid.NewGuid().ToString("N") + extension;
            string newFullPath = Path.Combine(cardsFolder, newFileName);

            if (productId > 0)
            {
                DeleteOldPhoto(currentPhotoPath, newFileName);
            }

            return newFileName;
        }

        private bool CheckImageSize(string imagePath)
        {
            using (Image img = Image.FromFile(imagePath))
            {
                if (img.Width > 300 || img.Height > 200)
                {
                    MessageBox.Show(
                        "Размер изображения должен быть не более 300x200 пикселей.\n\n" +
                        $"Выбранное изображение: {img.Width}x{img.Height}",
                        "Неверный размер изображения",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return false;
                }
            }

            return true;
        }

        private void DeleteOldPhoto(string oldPhotoPath, string newFileName)
        {
            try
            {
                string oldFileName = NormalizePhotoName(oldPhotoPath);

                if (string.IsNullOrWhiteSpace(oldFileName))
                    return;

                if (oldFileName == newFileName)
                    return;

                if (oldFileName.ToLower() == "picture.png")
                    return;

                string baseDir = AppDomain.CurrentDomain.BaseDirectory;

                string oldFullPath = Path.Combine(
                    baseDir,
                    "Resources",
                    "images",
                    "cards",
                    oldFileName
                );

                if (File.Exists(oldFullPath))
                {
                    File.Delete(oldFullPath);
                }
            }
            catch
            {
            }
        }

        private string NormalizePhotoName(string photoPath)
        {
            if (string.IsNullOrWhiteSpace(photoPath))
                return "";

            return Path.GetFileName(photoPath.Trim());
        }

        private void UpdateProduct(string photoFileName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                UPDATE [Товар]
                SET
                    [Артикул] = @article,
                    [Наименование товара] = @name,
                    [Единица измерения] = @unit,
                    [Цена] = @price,
                    [Поставщик] = @supplier,
                    [Производитель] = @manufacturer,
                    [Категория товара] = @category,
                    [Действующая скидка] = @discount,
                    [Кол-во на складе] = @count,
                    [Описание товара] = @description,
                    [Фото] = @photo
                WHERE [id товара] = @id";

                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@id", productId);
                    cmd.Parameters.AddWithValue("@article", textArticle.Text.Trim());
                    cmd.Parameters.AddWithValue("@name", textName.Text.Trim());
                    cmd.Parameters.AddWithValue("@unit", textUnit.Text.Trim());
                    cmd.Parameters.AddWithValue("@price", numericPrice.Value);
                    cmd.Parameters.AddWithValue("@supplier", textSupplier.Text.Trim());
                    cmd.Parameters.AddWithValue("@manufacturer", comboBoxManufacturer.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@category", comboBoxCategory.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@discount", numericDiscount.Value);
                    cmd.Parameters.AddWithValue("@count", numericCount.Value);
                    cmd.Parameters.AddWithValue("@description", richTextDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@photo", photoFileName);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void InsertProduct(string photoFileName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                INSERT INTO [Товар]
                (
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
                )
                VALUES
                (
                    @article,
                    @name,
                    @unit,
                    @price,
                    @supplier,
                    @manufacturer,
                    @category,
                    @discount,
                    @count,
                    @description,
                    @photo
                )";

                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@article", textArticle.Text.Trim());
                    cmd.Parameters.AddWithValue("@name", textName.Text.Trim());
                    cmd.Parameters.AddWithValue("@unit", textUnit.Text.Trim());
                    cmd.Parameters.AddWithValue("@price", numericPrice.Value);
                    cmd.Parameters.AddWithValue("@supplier", textSupplier.Text.Trim());
                    cmd.Parameters.AddWithValue("@manufacturer", comboBoxManufacturer.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@category", comboBoxCategory.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@discount", numericDiscount.Value);
                    cmd.Parameters.AddWithValue("@count", numericCount.Value);
                    cmd.Parameters.AddWithValue("@description", richTextDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@photo", photoFileName);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
