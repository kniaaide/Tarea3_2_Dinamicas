using System;
using System.Windows.Forms;

namespace Tarea3_2_Dinamicas
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}
