using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для PageCreditCalc.xaml
    /// </summary>
    public partial class PageCreditCalc : Page
    {
        int CreditPercent;
        int MinTerm;

        public PageCreditCalc(int id)
        {
            InitializeComponent();

            tbSumm.TextChanged += tbSumm_TextChanged;
            tbTerm.TextChanged += tbTerm_TextChanged;
            sliderSumm.ValueChanged += sliderSumm_ValueChanged;
            sliderTerm.ValueChanged += sliderTerm_ValueChanged;

            if (id == 0)
            {
                CreditPercent = 20;
                lblPercent.Content = CreditPercent + "%";
                MinTerm = 12;
                sliderTerm.Minimum = 12;
            }
            if (id == 1)
            {
                CreditPercent = 18;
                MinTerm = 36;
                lblPercent.Content = CreditPercent + "%";
                sliderTerm.Minimum = 36;
            }
            if (id == 2)
            {
                CreditPercent = 16;
                lblPercent.Content = CreditPercent + "%";
                MinTerm = 60;
                sliderTerm.Minimum = 60;
            }

            

        }

        private void tbSumm_TextChanged(object sender, TextChangedEventArgs e)
        {
            int summ = 1;
            if (!string.IsNullOrEmpty(tbSumm.Text)) summ = Convert.ToInt32(tbSumm.Text);
            else return;
            sliderSumm.Value = summ;
            CalculateCredit();
        }

        private void tbSumm_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void sliderSumm_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tbSumm.Text = sliderSumm.Value.ToString();
            CalculateCredit();
        }

        private void CalculateCredit()
        {
            if (tbSumm.Text == "" | tbTerm.Text == "") return;
            double sk = Convert.ToDouble(tbSumm.Text);
            double ps = CreditPercent * 0.01 / 12;
            double kp = Convert.ToDouble(tbTerm.Text);
            double Paymants = sk * ((ps * Math.Pow(1 + ps, kp)) / (Math.Pow(1 + ps, kp) - 1));
            tbPayments.Text = $"{Math.Round(Paymants,2)} Руб.";

            double OverPay = Paymants * kp - sk;
            tbOverpay.Text = $"{Math.Round(OverPay, 2)} Руб.";
        }

        private void tbTerm_TextChanged(object sender, TextChangedEventArgs e)
        {
            int Term = 1;
            if (!string.IsNullOrEmpty(tbTerm.Text)) Term = Convert.ToInt32(tbTerm.Text);
            else return;            
            sliderTerm.Value = Term;
            CalculateCredit();
        }

        private void sliderTerm_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tbTerm.TextChanged -= tbTerm_TextChanged; 
            tbTerm.Text = sliderTerm.Value.ToString();
            tbTerm.TextChanged += tbTerm_TextChanged;
            CalculateCredit();
        }

        private void btnSchedule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double sk = Convert.ToDouble(tbSumm.Text);
                double ps = CreditPercent * 0.01 / 12;
                double kp = Convert.ToDouble(tbTerm.Text);
                MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
                mainWindow.MainFrame.Navigate(new PagePaymentsSchedule(sk, ps, kp));
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
    }
}
