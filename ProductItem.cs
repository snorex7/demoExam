using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace demo
{
    public partial class ProductItem : UserControl
    {
        public int ProductId { get; private set; }

        public event EventHandler ProductSelect;
        public ProductItem()
        {
            //commit
            InitializeComponent();

            RegisterClick(this);
        }

        public void SetProductData(
            int productId,
            string category,
            string name,
            string description,
            string manufacturer,
            string supplier,
            decimal price,
            string unit,
            int count,
            decimal discount,
            string photo)
        {
            if (CacheSession.user.Role == Session.UserRole.Admin)
                Cursor = Cursors.Hand;

            ProductId = productId;

            labelBase.Text = $"{category} | {name}";

            labelDescription.Text = description;
            labelFabric.Text = manufacturer;
            labelSupplier.Text = supplier;
            labelPrice.Text = price.ToString("N2") + " руб.";
            labelUnit.Text = unit;
            labelCount.Text = count.ToString();

            pictureBoxProduct.Image = LoadProductImage(photo);

            if (discount > 0)
            {
                labelDiscount.Text = $"Скидка:\n{discount}%";

                decimal finalPrice = price - price * discount / 100;

                labelPrice.Text = price.ToString("N2") + " руб.";
                labelPrice.ForeColor = Color.Red;
                labelPrice.Font = new Font(labelPrice.Font, FontStyle.Strikeout);

                labelFinalPrice.Text = finalPrice.ToString("N2") + " руб.";
                labelFinalPrice.ForeColor = Color.Black;
                labelFinalPrice.Font = new Font(labelFinalPrice.Font, FontStyle.Regular);
            }
            else
            {
                labelDiscount.Text = "Скидки нет";

                labelPrice.Text = price.ToString("N2") + " руб.";
                labelPrice.ForeColor = Color.Black;
                labelPrice.Font = new Font(labelPrice.Font, FontStyle.Regular);

                labelFinalPrice.Text = "";
            }

            if (count <= 0)
            {
                BackColor = Color.LightBlue;
            }
            else if (discount > 15)
            {
                BackColor = ColorTranslator.FromHtml("#2E8B57");
            }
        }

        private Image LoadProductImage(string path)
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;

                if (!string.IsNullOrWhiteSpace(path))
                {
                    path = path.Trim();

                    string full = "";

                    full = Path.Combine(baseDir, path);

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

        private void RegisterClick(Control parent)
        {
            parent.Click += ProductCard_Click;

            foreach (Control control in parent.Controls)
            {
                RegisterClick(control);
            }
        }

        private void SetCursorForAllControls(Control parent, Cursor cursor)
        {
            parent.Cursor = cursor;

            foreach (Control control in parent.Controls)
            {
                SetCursorForAllControls(control, cursor);
            }
        }

        private void ProductCard_Click(object sender, EventArgs e)
        {
            if (CacheSession.user.Role != Session.UserRole.Admin)
            {
                return;
            }

            ProductSelect?.Invoke(this, EventArgs.Empty);
        }
    }
}
