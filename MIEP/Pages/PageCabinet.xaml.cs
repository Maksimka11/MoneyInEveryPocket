using MIEP.Model;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace MIEP.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageCabinet.xaml
    /// </summary>
    public partial class PageCabinet : Page
    {
        public PageCabinet(User user)
        {
            InitializeComponent();
            lblFIO.Content = user.Surname + " " + user.Name + " " + user.Patronymic;
            this.user = user;
        }

        User user;

        string Type;


        private void btnAccount_Click(object sender, RoutedEventArgs e)
        {
            Type = "Account";
            Count = 0;
            using (MIEPEntities db = new MIEPEntities())
            {
                dgOperations.Visibility = Visibility.Collapsed;
                dgAccount.Visibility = Visibility.Visible;
                db.BankAccount.Load();
                ICollectionView view = CollectionViewSource.GetDefaultView(db.BankAccount.Where(x => x.UserID == user.IDUser).Include(x => x.Type1).ToList());
                dgAccount.ItemsSource = view;

                string[] mas = new string[5];
                mas[0] = "Номер аккаунта";
                mas[1] = "ID Пользователя";
                mas[2] = "Дата открытия";
                mas[3] = "Баланс";
                mas[4] = "Тип карты";
                cbSort.ItemsSource = mas;
            }
        }

        private void btnOperations_Click(object sender, RoutedEventArgs e)
        {
            Type = "Operations";
            Count = 0;
            dgOperations.Visibility = Visibility.Visible;
            dgAccount.Visibility = Visibility.Collapsed;
            using (MIEPEntities db = new MIEPEntities())
            {
                BankAccount account = db.BankAccount.FirstOrDefault(x => x.UserID == user.IDUser);
                db.History.Load();
                ICollectionView view = CollectionViewSource.GetDefaultView(db.History.Where(x => x.Account == account.NumberAccount).Include(x => x.BankAccount).ToList());
                dgOperations.ItemsSource = view;

                string[] mas = new string[5];
                mas[0] = "ID Операции";
                mas[1] = "Название операции";
                mas[2] = "Дата и время";
                mas[3] = "Баланс";
                mas[4] = "Номер аккаунта";
                cbSort.ItemsSource = mas;
            }
        }



        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (Type == "Operations")
            {
                using (MIEPEntities db = new MIEPEntities())
                {
                    BankAccount account = db.BankAccount.FirstOrDefault(x => x.UserID == user.IDUser);
                    db.History.Load();
                    ICollectionView view = CollectionViewSource.GetDefaultView(db.History.Where(x => x.Account == account.NumberAccount).Include(x => x.BankAccount).ToList());
                    view.Filter = o =>
                    {
                        History p = o as History;
                        return p.IDOperation.ToString().Contains(tbSearch.Text)
                               || p.C_DateTime.ToString("g").Contains(tbSearch.Text)
                               || p.NameOperation.ToString().Contains(tbSearch.Text)
                               || p.Amount.ToString().Contains(tbSearch.Text)
                               || p.Account.Contains(tbSearch.Text);
                    };
                    dgOperations.ItemsSource = view;
                }

            }
            else
            {
                using (MIEPEntities db = new MIEPEntities())
                {
                    BankAccount account = db.BankAccount.FirstOrDefault(x => x.UserID == user.IDUser);
                    db.BankAccount.Load();
                    ICollectionView view = CollectionViewSource.GetDefaultView(db.BankAccount.Where(x => x.UserID == user.IDUser).Include(x => x.Type1).ToList());
                    view.Filter = o =>
                    {
                        BankAccount p = o as BankAccount;
                        return p.UserID.ToString().Contains(tbSearch.Text)
                               || p.DateOpen.ToString("D").Contains(tbSearch.Text)
                               || p.Balance.ToString().Contains(tbSearch.Text)
                               || p.Type1.NameType.Contains(tbSearch.Text)
                               || p.NumberAccount.Contains(tbSearch.Text);
                    };
                    dgAccount.ItemsSource = view;
                }
            }

        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (Type == "Account")
            {
                using (MIEPEntities db = new MIEPEntities())
                {
                    db.BankAccount.Load();
                    ICollectionView view = CollectionViewSource.GetDefaultView(db.BankAccount.Where(x => x.UserID == user.IDUser).Include(x => x.Type1).ToList()); ;
                    switch (cbSort.SelectedIndex)
                    {
                        case (0):
                            view = CollectionViewSource.GetDefaultView(db.BankAccount.Where(x => x.UserID == user.IDUser).Include(x => x.Type1).OrderBy(x => x.NumberAccount).ToList());
                            break;
                        case (1):
                            view = CollectionViewSource.GetDefaultView(db.BankAccount.Where(x => x.UserID == user.IDUser).Include(x => x.Type1).OrderBy(x => x.UserID).ToList());
                            break;
                        case (2):
                            view = CollectionViewSource.GetDefaultView(db.BankAccount.Where(x => x.UserID == user.IDUser).Include(x => x.Type1).OrderBy(x => x.DateOpen).ToList());
                            break;
                        case (3):
                            view = CollectionViewSource.GetDefaultView(db.BankAccount.Where(x => x.UserID == user.IDUser).Include(x => x.Type1).OrderBy(x => x.Balance).ToList());
                            break;
                        case (4):
                            view = CollectionViewSource.GetDefaultView(db.BankAccount.Where(x => x.UserID == user.IDUser).Include(x => x.Type1).OrderBy(x => x.Type).ToList());
                            break;
                    }

                    view.Filter = o =>
                    {
                        BankAccount p = o as BankAccount;
                        return p.UserID.ToString().Contains(tbSearch.Text)
                               || p.DateOpen.ToString("D").Contains(tbSearch.Text)
                               || p.Balance.ToString().Contains(tbSearch.Text)
                               || p.Type1.NameType.Contains(tbSearch.Text)
                               || p.NumberAccount.Contains(tbSearch.Text);
                    };
                    dgAccount.ItemsSource = view;

                }

            }
            else
            {
                using (MIEPEntities db = new MIEPEntities())
                {
                    BankAccount account = db.BankAccount.FirstOrDefault(x => x.UserID == user.IDUser);
                    db.History.Load();
                    ICollectionView view = CollectionViewSource.GetDefaultView(db.History.Where(x => x.Account == account.NumberAccount).Include(x => x.BankAccount).ToList());
                    switch (cbSort.SelectedIndex)
                    {
                        case (0):
                            view = CollectionViewSource.GetDefaultView(db.History.Where(x => x.Account == account.NumberAccount).Include(x => x.BankAccount).OrderBy(x => x.IDOperation).ToList());
                            break;
                        case (1):
                            view = CollectionViewSource.GetDefaultView(db.History.Where(x => x.Account == account.NumberAccount).Include(x => x.BankAccount).OrderBy(x => x.NameOperation).ToList());
                            break;
                        case (2):
                            view = CollectionViewSource.GetDefaultView(db.History.Where(x => x.Account == account.NumberAccount).Include(x => x.BankAccount).OrderBy(x => x.C_DateTime).ToList());
                            break;
                        case (3):
                            view = CollectionViewSource.GetDefaultView(db.History.Where(x => x.Account == account.NumberAccount).Include(x => x.BankAccount).OrderBy(x => x.Amount).ToList());
                            break;
                        case (4):
                            view = CollectionViewSource.GetDefaultView(db.History.Where(x => x.Account == account.NumberAccount).Include(x => x.BankAccount).OrderBy(x => x.Account).ToList());
                            break;
                    }
                    view.Filter = o =>
                    {
                        History p = o as History;
                        return p.IDOperation.ToString().Contains(tbSearch.Text)
                               || p.C_DateTime.ToString("g").Contains(tbSearch.Text)
                               || p.NameOperation.ToString().Contains(tbSearch.Text)
                               || p.Amount.ToString().Contains(tbSearch.Text)
                               || p.Account.Contains(tbSearch.Text);
                    };
                    dgOperations.ItemsSource = view;
                }
            }
        }

        int Count = 1;
        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (Count % 2 == 0)
            {
                e.Row.Background = new SolidColorBrush(Colors.SteelBlue);

            }
            else
            {
                e.Row.Background = new SolidColorBrush(Colors.LightSteelBlue);
            }
            e.Row.BorderBrush = new SolidColorBrush(Colors.White);
            e.Row.BorderThickness = new Thickness(1);
        }


    }
}
