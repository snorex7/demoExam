using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static demoA.Session;

namespace demoA
{
    public partial class Products : Form
    {
        public Products()
        {
            InitializeComponent();
        }

        private void Products_Load(object sender, EventArgs e)
        {
            if (CurrentSession.CurrentUser != null)
            {
                if (CurrentSession.CurrentUser.Role == UserRole.Guest)
                    labelUser.Text = $"Пользователь: Гость";
                else
                    labelUser.Text = $"Пользователь: {CurrentSession.CurrentUser.SurName} {CurrentSession.CurrentUser.FirstName} {CurrentSession.CurrentUser.MiddleName}";
            }
        }

        private void btnExitUser_Click(object sender, EventArgs e)
        {
            CurrentSession.CurrentUser = null;

            new Login().Show();

            this.Close();
        }
    }
}
