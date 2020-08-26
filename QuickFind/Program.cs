using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace QuickFind
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Mutex mutex = new Mutex(false, "b1c063de-2104-468e-ab02-4ca06b0c543e-20200724");
            // Check if application is already running
            try
            {
                if (mutex.WaitOne(0, false))
                {
                    if (!Directory.Exists(@"C:\Program Files\菠萝工具箱")) Directory.CreateDirectory(@"C:\Program Files\菠萝工具箱");
                    // Run the application
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new QuickForm());
                }
                else
                {
                    MessageBox.Show("程序在系统中已经启动", "QuickFind");
                }
            }
            finally
            {
                if (mutex != null)
                {
                    mutex.Close();
                    mutex = null;
                }
            }
        }
    }
}
