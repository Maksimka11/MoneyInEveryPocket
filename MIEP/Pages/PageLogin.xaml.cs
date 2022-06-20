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
using MIEP.Model;

namespace MIEP.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageLogin.xaml
    /// </summary>
    public partial class PageLogin : Page
    {
        public PageLogin(double Summ,double Term,double Proc)
        {
            InitializeComponent();
            OperationType = "Deposit";
            Amount = Summ;
            Period = Term;
            Percet = Proc;
        }
        string OperationType;
        double Amount;
        double Period;
        double Percet;


        public PageLogin()
        {
            InitializeComponent();
        }
        int i = 0;
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(CheckData()))
            {
                MessageBox.Show(CheckData());
                return;
            }

            using(MIEPEntities db = new MIEPEntities())
            {
                User user = db.User.FirstOrDefault(x => x.Login == tbLogin.Text);
                if (user == null || user.Password != tbPassword.Text)
                {
                    MessageBox.Show("Логин или пароль неверны");
                    i++;
                    if (i == 3)
                    {
                        MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
                        mainWindow.MainFrame.Navigate(new MainPage());
                        return;
                    }
                    else return;
                }
                else if(OperationType == "Deposit")
                {
                    BankAccount account = db.BankAccount.FirstOrDefault(x => x.UserID == user.IDUser);
                    Contract contract = new Contract()
                    {
                        NumberAccount = account.NumberAccount,
                        UserID = user.IDUser,
                        Amount = Amount,
                        Period = Convert.ToInt32(Period),
                        ExpirationDate = DateTime.Today,
                        Percet =Percet * 100
                    };
                    db.Contract.Add(contract);
                    db.SaveChanges();
                }
                MainWindow mainWindow1 = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
                mainWindow1.MainFrame.Navigate(new PageCabinet(user));
            }

        }

        private string CheckData()
        {
            string message = "";
            if (string.IsNullOrWhiteSpace(tbLogin.Text)) message += "Введите логин!" + Environment.NewLine;
            if (string.IsNullOrWhiteSpace(tbPassword.Text)) message += "Введите пароль!" + Environment.NewLine;
            return message;
        }

    }
}
