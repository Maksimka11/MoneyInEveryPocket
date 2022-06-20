using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MIEP.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageInvestmentCalc.xaml
    /// </summary>
    public partial class PageInvestmentCalc : Page
    {
        public PageInvestmentCalc()
        {
            InitializeComponent();
            tbSumm.TextChanged += tbSumm_TextChanged;
            tbTerm.TextChanged += tbTerm_TextChanged;
            tbDeposit.TextChanged += tbDeposit_TextChanged;
            sliderSumm.ValueChanged += sliderSumm_ValueChanged;
            sliderTerm.ValueChanged += sliderTerm_ValueChanged;
            sliderDeposit.ValueChanged += sliderDeposit_ValueChanged;
        }

        private void tbSumm_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tbSumm_TextChanged(object sender, TextChangedEventArgs e)
        {
            int Summ = 1;
            if (!string.IsNullOrEmpty(tbSumm.Text)) Summ = Convert.ToInt32(tbSumm.Text);
            else return;
            sliderSumm.Value = Summ;
            Calculate();
        }

        private void sliderSumm_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tbSumm.Text = sliderSumm.Value.ToString();
            Calculate();
        }

        private void sliderTerm_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tbTerm.Text = sliderTerm.Value.ToString();
            Calculate();
        }

        private void tbTerm_TextChanged(object sender, TextChangedEventArgs e)
        {
            int Term = 1;
            if (!string.IsNullOrEmpty(tbTerm.Text)) Term = Convert.ToInt32(tbTerm.Text);
            else return;
            sliderTerm.Value = Term;
            Calculate();
        }

        private void tbDeposit_TextChanged(object sender, TextChangedEventArgs e)
        {
            int Deposit = 1;
            if (!string.IsNullOrEmpty(tbDeposit.Text)) Deposit = Convert.ToInt32(tbDeposit.Text);
            else return;
            sliderDeposit.Value = Deposit;
            Calculate();
        }

        private void sliderDeposit_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tbDeposit.Text = sliderDeposit.Value.ToString();
            Calculate();
        }
        double IncomeStab;
        double IncomeStand;
        double IncomeOpt;
        private void Calculate()
        {
            if (string.IsNullOrEmpty(tbSumm.Text) | string.IsNullOrEmpty(tbTerm.Text) | string.IsNullOrEmpty(tbDeposit.Text)) return;
            int Summ = Convert.ToInt32(tbSumm.Text);
            int Term = Convert.ToInt32(tbTerm.Text);
            int Deposit = Convert.ToInt32(tbDeposit.Text);

            IncomeStab = (Summ * 8 * Term / 360) / 100;
            tbIncomeStab.Text = Math.Round(IncomeStab, 2) + " Руб.";

            IncomeStand = GetIncomeStand(Summ, Term, Deposit);
            tbIncomeStand.Text = Math.Round(IncomeStand, 2) + " Руб.";

            IncomeOpt = GetIncomeOpt(Summ, Term, Deposit);
            tbIncomeOpt.Text = Math.Round(IncomeOpt, 2) + " Руб.";
        }

        private double GetIncomeStand(int Summ, int Term, int Deposit)
        {
            double Income = 0;
            for (int i = 1; i < Term; i++)
            {
                if (i % 30 == 0) Summ += Deposit;
                Income += Summ * 0.06 / 360;
            }

            return Income;

        }
        double ProcOpt;
        private double GetIncomeOpt(int Summ, int Term, int Deposit)
        {
            ProcOpt = 0.05;
            double Income = 0;
            for (int i = 1; i < Term; i++)
            {
                if (i % 30 == 0) ProcOpt += ProcOpt / 360;
                if (i % 30 == 0) Summ += Deposit;
                Income += Summ * ProcOpt / 360;
            }
            return Math.Round(Income, 2);
        }


        private void btnParam_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                double[,] mas = new double[3, 4];
                mas[0, 0] = IncomeStab;
                mas[0, 1] = IncomeStab + Convert.ToDouble(tbSumm.Text);
                mas[0, 2] = 0.08;
                mas[0, 3] = Convert.ToDouble(tbTerm.Text);
                mas[1, 0] = IncomeOpt;
                mas[1, 1] = IncomeOpt + Convert.ToDouble(tbSumm.Text) + (Convert.ToInt32(tbTerm.Text) / 30 * Convert.ToInt32(tbDeposit.Text));
                mas[1, 2] = ProcOpt;
                mas[1, 3] = Convert.ToDouble(tbTerm.Text);
                mas[2, 0] = IncomeStand;
                mas[2, 1] = IncomeStand + Convert.ToDouble(tbSumm.Text) + (Convert.ToInt32(tbTerm.Text) / 30 * Convert.ToInt32(tbDeposit.Text));
                mas[2, 2] = 0.06;
                mas[2, 3] = Convert.ToDouble(tbTerm.Text);

                MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
                mainWindow.MainFrame.Navigate(new PageCompareDeposits(mas));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }



    }
}
