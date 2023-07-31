using System;
using System.Windows.Forms;

namespace Client
{
    public partial class PrivateChatForm : Form
    {
        public PublicChatForm pChat;

        public PrivateChatForm(PublicChatForm pchat)
        {
            InitializeComponent();
            pChat = pchat;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (txtInput.Text != string.Empty)
            {
                string user = Text.Split('-')[1];
                pChat.formLogin.Client.Send("pMessage|" + user + "|" + txtInput.Text);
                txtReceive.Text += user + " says: " + txtInput.Text + "\r\n";
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