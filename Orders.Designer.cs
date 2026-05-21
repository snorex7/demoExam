namespace demoA
{
    partial class Orders
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
            this.flowLayoutPanelOrderList = new System.Windows.Forms.FlowLayoutPanel();
            this.panelTitle = new System.Windows.Forms.Panel();
            this.labelOrders = new System.Windows.Forms.Label();
            this.btnExitUser = new System.Windows.Forms.Button();
            this.labelUser = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAddOrder = new System.Windows.Forms.Button();
            this.btnProducts = new System.Windows.Forms.Button();
            this.panelTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanelOrderList
            // 
            this.flowLayoutPanelOrderList.AutoScroll = true;
            this.flowLayoutPanelOrderList.Location = new System.Drawing.Point(2, 98);
            this.flowLayoutPanelOrderList.Name = "flowLayoutPanelOrderList";
            this.flowLayoutPanelOrderList.Size = new System.Drawing.Size(1063, 351);
            this.flowLayoutPanelOrderList.TabIndex = 0;
            // 
            // panelTitle
            // 
            this.panelTitle.Controls.Add(this.labelOrders);
            this.panelTitle.Controls.Add(this.btnExitUser);
            this.panelTitle.Controls.Add(this.labelUser);
            this.panelTitle.Location = new System.Drawing.Point(2, 4);
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Size = new System.Drawing.Size(1063, 64);
            this.panelTitle.TabIndex = 1;
            // 
            // labelOrders
            // 
            this.labelOrders.AutoSize = true;
            this.labelOrders.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelOrders.Location = new System.Drawing.Point(3, 13);
            this.labelOrders.Name = "labelOrders";
            this.labelOrders.Size = new System.Drawing.Size(71, 23);
            this.labelOrders.TabIndex = 2;
            this.labelOrders.Text = "Заказы";
            // 
            // btnExitUser
            // 
            this.btnExitUser.BackColor = System.Drawing.Color.MediumSpringGreen;
            this.btnExitUser.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnExitUser.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnExitUser.Location = new System.Drawing.Point(943, 3);
            this.btnExitUser.Name = "btnExitUser";
            this.btnExitUser.Size = new System.Drawing.Size(117, 23);
            this.btnExitUser.TabIndex = 5;
            this.btnExitUser.Text = "Выйти из аккаунта";
            this.btnExitUser.UseVisualStyleBackColor = false;
            this.btnExitUser.Click += new System.EventHandler(this.btnExitUser_Click);
            // 
            // labelUser
            // 
            this.labelUser.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelUser.Location = new System.Drawing.Point(425, 31);
            this.labelUser.Name = "labelUser";
            this.labelUser.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelUser.Size = new System.Drawing.Size(635, 23);
            this.labelUser.TabIndex = 4;
            this.labelUser.Text = "Пользователь: ";
            this.labelUser.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnAddOrder);
            this.panel1.Controls.Add(this.btnProducts);
            this.panel1.Location = new System.Drawing.Point(2, 66);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1063, 33);
            this.panel1.TabIndex = 2;
            // 
            // btnAddOrder
            // 
            this.btnAddOrder.BackColor = System.Drawing.Color.MediumSpringGreen;
            this.btnAddOrder.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddOrder.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnAddOrder.Location = new System.Drawing.Point(10, 3);
            this.btnAddOrder.Name = "btnAddOrder";
            this.btnAddOrder.Size = new System.Drawing.Size(117, 23);
            this.btnAddOrder.TabIndex = 11;
            this.btnAddOrder.Text = "Добавить заказ";
            this.btnAddOrder.UseVisualStyleBackColor = false;
            this.btnAddOrder.Visible = false;
            this.btnAddOrder.Click += new System.EventHandler(this.btnAddOrder_Click);
            // 
            // btnProducts
            // 
            this.btnProducts.BackColor = System.Drawing.Color.MediumSpringGreen;
            this.btnProducts.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnProducts.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnProducts.Location = new System.Drawing.Point(940, 3);
            this.btnProducts.Name = "btnProducts";
            this.btnProducts.Size = new System.Drawing.Size(117, 23);
            this.btnProducts.TabIndex = 5;
            this.btnProducts.Text = "Список товаров";
            this.btnProducts.UseVisualStyleBackColor = false;
            this.btnProducts.Click += new System.EventHandler(this.btnProducts_Click);
            // 
            // Orders
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1071, 450);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelTitle);
            this.Controls.Add(this.flowLayoutPanelOrderList);
            this.Name = "Orders";
            this.Text = "Заказы";
            this.Load += new System.EventHandler(this.Orders_Load);
            this.panelTitle.ResumeLayout(false);
            this.panelTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelOrderList;
        private System.Windows.Forms.Panel panelTitle;
        private System.Windows.Forms.Button btnExitUser;
        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.Label labelOrders;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnProducts;
        private System.Windows.Forms.Button btnAddOrder;
    }
}