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

namespace demoA
{
    public partial class Login : Form
    {
        static string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DemoExam;Integrated Security=true";
        SqlConnection connection = new SqlConnection(connectionString);

        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string login = textLogin.Text.Trim();
            string password = textPassword.Text;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите логин и пароль.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = @"
                    SELECT TOP 1
                        [Роль сотрудника] AS RoleName,
                        [Фамилия] AS SurName,
                        [Имя] AS FirstName,
                        [Отчество] AS MiddleName
                    FROM [Пользователи]
                    WHERE [Логин] = @login AND [Пароль] = @password";

                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@login", login);
                        cmd.Parameters.AddWithValue("@password", password);

                        using (var r = cmd.ExecuteReader())
                        {
                            if (r.Read())
                            {
                                string roleName = Convert.ToString(r["RoleName"]);

                                CurrentSession.CurrentUser = new Session
                                {
                                    Role = ParseRole(roleName),
                                    SurName = Convert.ToString(r["SurName"]),
                                    FirstName = Convert.ToString(r["FirstName"]),
                                    MiddleName = Convert.ToString(r["MiddleName"])
                                };

                                new Products().Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения/запроса:\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Session.UserRole ParseRole(string roleName)
        {
            switch (roleName)
            {
                case "Авторизированный клиент":
                    return Session.UserRole.Client;

                case "Менеджер":
                    return Session.UserRole.Manager;

                case "Администратор":
                    return Session.UserRole.Admin;

                default:
                    return Session.UserRole.Guest;
            }
        }

        private void btnGuest_Click(object sender, EventArgs e)
        {
            CurrentSession.CurrentUser = new Session
            {
                Role = Session.UserRole.Guest
            };

            new Products().Show();
            this.Hide();
        }
    }
}
