using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace ProjectUD
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (var mutex = new Mutex(false, Application.ProductName))
            {
                if (mutex.WaitOne(TimeSpan.FromSeconds(3))) // Подождать три секунды - вдруг предыдущий экземпляр еще закрывается
                    Application.Run(new Manager());
                else
                    //          .Start(mutex.Handle);
                    MessageBox.Show("Приложение уже запущено, доступно управление через трей.", "Приложение уже запущено", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
