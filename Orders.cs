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
using static demoA.Session;

namespace demoA
{
    public partial class Orders : Form
    {
        static string connectionString =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DemoExam;Integrated Security=true";
        public Orders()
        {
            InitializeComponent();
        }

        private void Orders_Load(object sender, EventArgs e)
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
            LoadOrders();
        }

        private void SetupRoleAccess()
        {
            UserRole role = CurrentSession.CurrentUser == null ? UserRole.Guest : CurrentSession.CurrentUser.Role;

            if (CurrentSession.CurrentUser.Role == Session.UserRole.Admin)
                btnAddOrder.Visible = true;
        }

        private void LoadOrders()
        {
            flowLayoutPanelOrderList.Controls.Clear();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    StringBuilder sql = new StringBuilder();

                    sql.Append(@"
                        SELECT
                            p.[id позиции] AS OrderId,
                            p.[Артикул] AS Article,
                            z.[Статус заказа] AS Status,

                            CONCAT(
                                pv.[Индекс], ', ',
                                pv.[Город], ', ',
                                pv.[Улица], ', ',
                                NCHAR(1076), '. ',
                                pv.[Дом]
                            ) AS PointAddress,

                            CONVERT(varchar(10), z.[Дата заказа], 104) AS OrderDate,
                            CONVERT(varchar(10), z.[Дата доставки], 104) AS DeliveryDate

                        FROM [Заказы] z
                        INNER JOIN [Позиция] p
                            ON p.[Id заказа] = z.[Номер заказа]

                        INNER JOIN [Пункты выдачи] pv
                            ON pv.[id пункта] = z.[Адрес пункта выдачи]

                        ORDER BY z.[Номер заказа]
                    ");

                    using (SqlCommand cmd = new SqlCommand(sql.ToString(), connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                OrderItem card = new OrderItem();

                                card.SetOrderData(
                                    reader["OrderId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["OrderId"]),
                                    Convert.ToString(reader["Article"]),
                                    Convert.ToString(reader["Status"]),
                                    Convert.ToString(reader["PointAddress"]),
                                    Convert.ToString(reader["OrderDate"]),
                                    Convert.ToString(reader["DeliveryDate"])
                                );

                                if (CurrentSession.CurrentUser.Role == Session.UserRole.Admin)
                                {
                                    card.OrderSelected += OrderSelected;
                                }

                                flowLayoutPanelOrderList.Controls.Add(card);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ошибка загрузки заказов:\n" + ex.Message,
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void OrderSelected(object sender, EventArgs e)
        {
            OrderItem card = sender as OrderItem;

            if (card == null)
                return;

            using (OrderEdit form = new OrderEdit(card.OrderId))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadOrders();
                }
            }
        }

        private void btnExitUser_Click(object sender, EventArgs e)
        {
            CurrentSession.CurrentUser = null;

            new Login().Show();

            this.Close();
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            new Products().Show();

            this.Close();
        }

        private void btnAddOrder_Click(object sender, EventArgs e)
        {
            using (OrderEdit form = new OrderEdit())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadOrders();
                }
            }
        }
    }
}
