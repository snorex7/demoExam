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

namespace demoA
{
    public partial class OrderItem : UserControl
    {
        public int OrderId { get; private set; }

        public event EventHandler OrderSelected;
        public OrderItem()
        {
            InitializeComponent();

            RegisterClickEvents(this);
        }

        private void RegisterClickEvents(Control parent)
        {
            parent.Click += OrderItem_Click;

            foreach (Control control in parent.Controls)
            {
                RegisterClickEvents(control);
            }
        }

        private void OrderItem_Click(object sender, EventArgs e)
        {
            if (CurrentSession.CurrentUser.Role != Session.UserRole.Admin)
            {
                return;
            }

            OrderSelected?.Invoke(this, EventArgs.Empty);
        }

        private void SetCursorForAllControls(Control parent, Cursor cursor)
        {
            parent.Cursor = cursor;

            foreach (Control control in parent.Controls)
            {
                SetCursorForAllControls(control, cursor);
            }
        }

        public void SetOrderData(
            int orderid,
            string article,
            string status, 
            string point_address,
            string order_date,
            string delivery_date)
        {
            if (CurrentSession.CurrentUser.Role == Session.UserRole.Admin)
                Cursor = Cursors.Hand;

            OrderId = orderid;

            labelArticle.Text = article;
            labelStatus.Text = status;  
            labelPointAddress.Text = point_address;
            labelOrderDate.Text = order_date;
            labelDeliveryDate.Text = delivery_date;
        }


    }
}
