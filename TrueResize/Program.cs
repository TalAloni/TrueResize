using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace TrueResize
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            bool success = TrueCryptLibrary.Tests.TestSHA512();
            if (success)
            {
                Application.Run(new MainForm());
            }
            else
            {
                // In the .NET Framework version 2.0, the HMACSHA512 class produced results that were not consistent with other implementations of HMAC-SHA-512. The .NET Framework version 2.0 Service Pack 1 updates this class.
                MessageBox.Show("This software requires the .NET Framework version 2.0 SP1+", "Unsupported .Net Framework Version");
            }
        }

        public static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            HandleUnhandledException(e.Exception);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject != null)
            {
                Exception ex = (Exception)e.ExceptionObject;
                HandleUnhandledException(ex);
            }
        }

        private static void HandleUnhandledException(Exception ex)
        {
            string message = String.Format("Exception: {0}: {1} Source: {2} {3}", ex.GetType(), ex.Message, ex.Source, ex.StackTrace);
            MessageBox.Show(message, "Error");
            Application.Exit();
        }
    }
}