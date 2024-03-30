using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Threading;

namespace ProtractorTool
{
    public partial class App : Application
    {
        private static readonly Mutex Mutex = new Mutex(true, "35252B40-C1EC-48B0-B796-EB048AC42124");
        private static MainWindow mainWindow;

        [STAThread]
        static void Main()
        {
            if (Mutex.WaitOne(TimeSpan.Zero, true))
            {
                var app = new App();
                mainWindow = new MainWindow();
                app.Run(mainWindow);
                Mutex.ReleaseMutex();
            }
            else
            {
                mainWindow.WindowState = WindowState.Maximized;
                mainWindow.Show();
            }
        }

    }
}
