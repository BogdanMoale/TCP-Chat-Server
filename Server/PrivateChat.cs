using System;
using System.Linq;
using System.Windows.Forms;

namespace Server
{
    public partial class PrivateChat : Form
    {
        private readonly Main Main;

        public PrivateChat(Main main)
        {
            InitializeComponent();
            Main = main;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (txtInput.Text != string.Empty)
            {
                foreach (var client in from ListViewItem item in Main.clientList.SelectedItems select (Client) item.Tag)
                {
                    client.Send("pMessage|" + txtInput.Text);                   
                }
                txtReceive.Text += "Server says: " + txtInput.Text + "\r\n";
                txtInput.Text = string.Empty;
            }
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSend.PerformClick();
            }
        }

        private void txtReceive_TextChanged(object sender, EventArgs e)
        {
            txtReceive.SelectionStart = txtReceive.TextLength;
        }
    }
}