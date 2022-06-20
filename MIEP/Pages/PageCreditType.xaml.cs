using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MIEP.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageCreditType.xaml
    /// </summary>
    public partial class PageCreditType : Page
    {
        public PageCreditType()
        {
            InitializeComponent();
        }

        private void btnSimple_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            mainWindow.MainFrame.Navigate(new PageCreditCalc(0));
        }

        private void btnOptimal_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            mainWindow.MainFrame.Navigate(new PageCreditCalc(1));
        }

        private void btnLgot_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            mainWindow.MainFrame.Navigate(new PageCreditCalc(2));
        }
    }
}
