using System;
using System.Windows.Forms;

namespace Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PublicChatForm());
        }

        /// <summary>
        /// Invokes an action on the UI thread
        /// </summary>
        public static void Invoke(this Control control, Action action)
        {
            control.Invoke(action);
        }
    }
}