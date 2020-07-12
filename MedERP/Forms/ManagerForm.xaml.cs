using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MedERP
{
    /// <summary>
    /// Логика взаимодействия для ManagerForm.xaml
    /// </summary>
    public partial class ManagerForm : Window
    {
        private Timer logonTimer;

        public ManagerForm()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            logonTimer = new Timer(GetTime, null, 0, 500);
        }

        private void GetTime(object state)
        {
            var CurentDateTime = DateTime.Now;

            this.Dispatcher.Invoke((Action)(() =>
            {
                tbTime.Text = CurentDateTime.ToString("HH:mm");
                tbSec.Content = CurentDateTime.Second.ToString("00");
                tbDate.Text = CurentDateTime.Day + " " + CurentDateTime.ToString("MMMM") + ", " +
                              CurentDateTime.ToString("dddd");
            }));
        }
    }
}
