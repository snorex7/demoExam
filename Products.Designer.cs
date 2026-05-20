namespace demo
{
    partial class Products
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Products));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonExitUser = new System.Windows.Forms.Button();
            this.labelCurrentUser = new System.Windows.Forms.Label();
            this.labelProducts = new System.Windows.Forms.Label();
            this.buttonOrders = new System.Windows.Forms.Button();
            this.comboBoxRemain = new System.Windows.Forms.ComboBox();
            this.labelRemain = new System.Windows.Forms.Label();
            this.comboBoxSupplier = new System.Windows.Forms.ComboBox();
            this.labelSupplier = new System.Windows.Forms.Label();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.labelSearch = new System.Windows.Forms.Label();
            this.buttonAddProduct = new System.Windows.Forms.Button();
            this.flowLayoutListProducts = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelProducts);
            this.panel1.Controls.Add(this.buttonExitUser);
            this.panel1.Controls.Add(this.labelCurrentUser);
            this.panel1.Location = new System.Drawing.Point(2, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1043, 50);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.buttonAddProduct);
            this.panel2.Controls.Add(this.textBoxSearch);
            this.panel2.Controls.Add(this.buttonOrders);
            this.panel2.Controls.Add(this.labelSearch);
            this.panel2.Controls.Add(this.comboBoxRemain);
            this.panel2.Controls.Add(this.labelSupplier);
            this.panel2.Controls.Add(this.labelRemain);
            this.panel2.Controls.Add(this.comboBoxSupplier);
            this.panel2.Location = new System.Drawing.Point(2, 51);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1043, 50);
            this.panel2.TabIndex = 1;
            // 
            // buttonExitUser
            // 
            this.buttonExitUser.BackColor = System.Drawing.Color.MediumSpringGreen;
            this.buttonExitUser.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonExitUser.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonExitUser.Location = new System.Drawing.Point(912, 3);
            this.buttonExitUser.Name = "buttonExitUser";
            this.buttonExitUser.Size = new System.Drawing.Size(128, 23);
            this.buttonExitUser.TabIndex = 5;
            this.buttonExitUser.Text = "Выйти из аккаунта";
            this.buttonExitUser.UseVisualStyleBackColor = false;
            this.buttonExitUser.Click += new System.EventHandler(this.buttonExitUser_Click);
            // 
            // labelCurrentUser
            // 
            this.labelCurrentUser.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelCurrentUser.Location = new System.Drawing.Point(554, 29);
            this.labelCurrentUser.Name = "labelCurrentUser";
            this.labelCurrentUser.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelCurrentUser.Size = new System.Drawing.Size(486, 19);
            this.labelCurrentUser.TabIndex = 4;
            this.labelCurrentUser.Text = "Пользователь: ";
            this.labelCurrentUser.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelProducts
            // 
            this.labelProducts.AutoSize = true;
            this.labelProducts.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelProducts.Location = new System.Drawing.Point(3, 15);
            this.labelProducts.Name = "labelProducts";
            this.labelProducts.Size = new System.Drawing.Size(118, 19);
            this.labelProducts.TabIndex = 2;
            this.labelProducts.Text = "Список товаров";
            // 
            // buttonOrders
            // 
            this.buttonOrders.BackColor = System.Drawing.Color.MediumSpringGreen;
            this.buttonOrders.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonOrders.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonOrders.Location = new System.Drawing.Point(854, 18);
            this.buttonOrders.Name = "buttonOrders";
            this.buttonOrders.Size = new System.Drawing.Size(70, 23);
            this.buttonOrders.TabIndex = 11;
            this.buttonOrders.Text = "Заказы";
            this.buttonOrders.UseVisualStyleBackColor = false;
            this.buttonOrders.Visible = false;
            // 
            // comboBoxRemain
            // 
            this.comboBoxRemain.BackColor = System.Drawing.Color.MediumSpringGreen;
            this.comboBoxRemain.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRemain.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxRemain.FormattingEnabled = true;
            this.comboBoxRemain.Location = new System.Drawing.Point(425, 15);
            this.comboBoxRemain.Name = "comboBoxRemain";
            this.comboBoxRemain.Size = new System.Drawing.Size(121, 23);
            this.comboBoxRemain.TabIndex = 17;
            this.comboBoxRemain.TextChanged += new System.EventHandler(this.FilterChanged);
            // 
            // labelRemain
            // 
            this.labelRemain.AutoSize = true;
            this.labelRemain.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelRemain.Location = new System.Drawing.Point(371, 18);
            this.labelRemain.Name = "labelRemain";
            this.labelRemain.Size = new System.Drawing.Size(48, 15);
            this.labelRemain.TabIndex = 16;
            this.labelRemain.Text = "Остаток";
            // 
            // comboBoxSupplier
            // 
            this.comboBoxSupplier.BackColor = System.Drawing.Color.MediumSpringGreen;
            this.comboBoxSupplier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSupplier.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxSupplier.FormattingEnabled = true;
            this.comboBoxSupplier.Location = new System.Drawing.Point(244, 14);
            this.comboBoxSupplier.Name = "comboBoxSupplier";
            this.comboBoxSupplier.Size = new System.Drawing.Size(121, 23);
            this.comboBoxSupplier.TabIndex = 15;
            this.comboBoxSupplier.TextChanged += new System.EventHandler(this.FilterChanged);
            // 
            // labelSupplier
            // 
            this.labelSupplier.AutoSize = true;
            this.labelSupplier.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelSupplier.Location = new System.Drawing.Point(174, 18);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(64, 15);
            this.labelSupplier.TabIndex = 14;
            this.labelSupplier.Text = "Поставщик";
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.BackColor = System.Drawing.Color.MediumSpringGreen;
            this.textBoxSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxSearch.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxSearch.Location = new System.Drawing.Point(47, 16);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(120, 21);
            this.textBoxSearch.TabIndex = 13;
            this.textBoxSearch.TextChanged += new System.EventHandler(this.FilterChanged);
            // 
            // labelSearch
            // 
            this.labelSearch.AutoSize = true;
            this.labelSearch.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelSearch.Location = new System.Drawing.Point(2, 18);
            this.labelSearch.Name = "labelSearch";
            this.labelSearch.Size = new System.Drawing.Size(39, 15);
            this.labelSearch.TabIndex = 12;
            this.labelSearch.Text = "Поиск";
            // 
            // buttonAddProduct
            // 
            this.buttonAddProduct.BackColor = System.Drawing.Color.MediumSpringGreen;
            this.buttonAddProduct.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonAddProduct.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonAddProduct.Location = new System.Drawing.Point(930, 18);
            this.buttonAddProduct.Name = "buttonAddProduct";
            this.buttonAddProduct.Size = new System.Drawing.Size(104, 23);
            this.buttonAddProduct.TabIndex = 18;
            this.buttonAddProduct.Text = "Добавить товар";
            this.buttonAddProduct.UseVisualStyleBackColor = false;
            this.buttonAddProduct.Visible = false;
            this.buttonAddProduct.Click += new System.EventHandler(this.buttonAddProduct_Click);
            // 
            // flowLayoutListProducts
            // 
            this.flowLayoutListProducts.AutoScroll = true;
            this.flowLayoutListProducts.Location = new System.Drawing.Point(2, 96);
            this.flowLayoutListProducts.Name = "flowLayoutListProducts";
            this.flowLayoutListProducts.Size = new System.Drawing.Size(1043, 416);
            this.flowLayoutListProducts.TabIndex = 2;
            // 
            // Products
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1048, 515);
            this.Controls.Add(this.flowLayoutListProducts);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Products";
            this.Text = "Список товаров";
            this.Load += new System.EventHandler(this.Products_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonExitUser;
        private System.Windows.Forms.Label labelCurrentUser;
        private System.Windows.Forms.Label labelProducts;
        private System.Windows.Forms.Button buttonAddProduct;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.Button buttonOrders;
        private System.Windows.Forms.Label labelSearch;
        private System.Windows.Forms.ComboBox comboBoxRemain;
        private System.Windows.Forms.Label labelSupplier;
        private System.Windows.Forms.Label labelRemain;
        private System.Windows.Forms.ComboBox comboBoxSupplier;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutListProducts;
    }
}