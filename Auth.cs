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

namespace demo
{
    public partial class Auth : Form
    {
        public Auth()
        {
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string login = textBoxLogin.Text.Trim();
            string password = textBoxPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Логин и пароль не могут быть пустые!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(CacheSession.connectionString))
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

                                CacheSession.user = new Session
                                {
                                    Role = Role(roleName),
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

        private Session.UserRole Role(string roleName)
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

        private void buttonGuest_Click(object sender, EventArgs e)
        {
            CacheSession.user = new Session
            {
                Role = Session.UserRole.Guest
            };

            new Products().Show();
            this.Hide();
        }
    }
}
