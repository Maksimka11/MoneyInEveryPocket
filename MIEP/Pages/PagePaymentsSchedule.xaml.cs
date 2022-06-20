using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MIEP.Pages
{
    /// <summary>
    /// Логика взаимодействия для PagePaymentsSchedule.xaml
    /// </summary>
    public partial class PagePaymentsSchedule : Page
    {
        public PagePaymentsSchedule(double sk, double ps, double kp)
        {
            InitializeComponent();
            Calculate(sk, ps, kp);
        }

        public List<CreditSchedule> schedules = new List<CreditSchedule>();

        private void btnFormSchedule_Click(object sender, RoutedEventArgs e)
        {
            string fileName = AppDomain.CurrentDomain.BaseDirectory + @"Отчеты\График платежей.pdf";
            for (int i = 1; i < Int16.MaxValue; i++)
            {
                if (File.Exists(fileName)) fileName = AppDomain.CurrentDomain.BaseDirectory + @"Отчеты\График платежей" + $" ({i})" + ".pdf";
                else break;
            }

            Document doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream(fileName, FileMode.Create, FileAccess.Write));

            doc.Open();
            BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);

            var ColumnCount = dgSchedule.Columns.Count;
            float[] Widths = new float[6];
            for (int i = 0; i < dgSchedule.Columns.Count; i++)
            {
                Widths[i] = (float)dgSchedule.Columns[i].ActualWidth;
            }
            var ColumnWidths = Widths;

            var table = new PdfPTable(ColumnWidths)
            {
                HorizontalAlignment = 0,
                WidthPercentage = 100,
                DefaultCell = { MinimumHeight = 22f, Border = 0},
                
            };

            var cell = new PdfPCell(new Phrase("График платежей", font))
            {
                Colspan = ColumnCount,
                HorizontalAlignment = 1,
                MinimumHeight = 30f,
                Border = 0
            };
            table.AddCell(cell);

            for (int i = 0; i < dgSchedule.Columns.Count; i++)
            {
                cell = new PdfPCell(new Phrase(dgSchedule.Columns[i].Header.ToString(), font))
                {
                    BackgroundColor = BaseColor.ORANGE,
                    Border = 0
                };
                table.AddCell(cell);
            }

            for(int i = 0; i < schedules.Count; i++)
            {             
                if(schedules[i].Id %2 == 0)
                {
                    cell = new PdfPCell(new Phrase(schedules[i].Id.ToString(), font))
                    {
                        BackgroundColor = BaseColor.PINK,
                        Border = 0
                    };
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(schedules[i].Date.ToString(), font))
                    {
                        BackgroundColor = BaseColor.PINK,
                        Border = 0
                    };
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(schedules[i].Pay.ToString(), font))
                    {
                        BackgroundColor = BaseColor.PINK,
                        Border = 0
                    };
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(schedules[i].Main.ToString(), font))
                    {
                        BackgroundColor = BaseColor.PINK,
                        Border = 0
                    };
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(schedules[i].Proc.ToString(), font))
                    {
                        BackgroundColor = BaseColor.PINK,
                        Border = 0
                    };
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(schedules[i].Remains.ToString(), font))
                    {
                        BackgroundColor = BaseColor.PINK,
                        Border = 0
                    };
                    table.AddCell(cell);
                }
                else
                {
                    cell = new PdfPCell(new Phrase(schedules[i].Id.ToString(), font))
                    {
                        Border = 0
                    };                   
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(schedules[i].Date.ToString(), font))
                    {
                        Border = 0
                    };
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(schedules[i].Pay.ToString(), font))
                    {
                        Border = 0
                    };
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(schedules[i].Main.ToString(), font))
                    {
                        Border = 0
                    };
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(schedules[i].Proc.ToString(), font))
                    {
                        Border = 0
                    };
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(schedules[i].Remains.ToString(), font))
                    {
                        Border = 0
                    };
                    table.AddCell(cell);
                }                           
            }

            var AddCell = new PdfPCell(new Phrase("Итого по кредиту: ", font))
            {
                BackgroundColor = BaseColor.ORANGE
            };
            table.AddCell(AddCell);

            double Summ = 0;
            for(int i = 0; i < schedules.Count; i++)
            {
                Summ += schedules[i].Pay;
            }
            AddCell = new PdfPCell(new Phrase(Summ.ToString(), font))
            {               
                BackgroundColor = BaseColor.ORANGE
            };
            table.AddCell(AddCell);
            Summ = 0;
            for (int i = 0; i < schedules.Count; i++)
            {
                Summ += schedules[i].Main;
            }
            AddCell = new PdfPCell(new Phrase(Summ.ToString(), font))
            {
                BackgroundColor = BaseColor.ORANGE
            };
            table.AddCell(AddCell);
            Summ = 0;
            for (int i = 0; i < schedules.Count; i++)
            {
                Summ += schedules[i].Proc;
            }
            AddCell = new PdfPCell(new Phrase(Summ.ToString(), font))
            {
                BackgroundColor = BaseColor.ORANGE
            };
            table.AddCell(AddCell);

            doc.Add(table);
            doc.Close();
            
        }

        private void btnGetCredit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            mainWindow.MainFrame.Navigate(new PageLogin());
        }

        private void Calculate(double sk,double ps,double kp)
        {
            DateTime date = DateTime.Today;
            double Paymants = Math.Round(sk * ((ps * Math.Pow(1 + ps, kp)) / (Math.Pow(1 + ps, kp) - 1)), 2);
            for (int i = 1; i <= kp; i++)
            {                   
                double proc = Math.Round(sk * ps, 2);
                if (i == kp) Paymants = Math.Round(sk + proc, 2);
                double Main = Math.Round(Paymants - proc, 2);
                sk -= Main;
                CreditSchedule credit = new CreditSchedule();
                string ActutalDate = GetDate(date);
                credit.GetData(i,ActutalDate,Paymants,Main,proc,Math.Round(sk,2));
                date = date.AddMonths(1);
                schedules.Add(credit);
                
            }
            dgSchedule.ItemsSource = schedules;
            
        }

        private string GetDate(DateTime date)
        {
            string Mounth = "";
            switch (date.Month)
            {
                case (1):
                    Mounth = "Январь";
                    break;
                case (2):
                    Mounth = "Февраль";
                    break;
                case (3):
                    Mounth = "Март";
                    break;
                case (4):
                    Mounth = "Апрель";
                    break;
                case (5):
                    Mounth = "Май";
                    break;
                case (6):
                    Mounth = "Июнь";
                    break;
                case (7):
                    Mounth = "Июль";
                    break;
                case (8):
                    Mounth = "Август";
                    break;
                case (9):
                    Mounth = "Сентябрь";
                    break;
                case (10):
                    Mounth = "Октябрь";
                    break;
                case (11):
                    Mounth = "Ноябрь";
                    break;
                case (12):
                    Mounth = "Декабрь";
                    break;
            }

            return Mounth + ", " + date.Year;
        }

        private void dgSchedule_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            CreditSchedule credit = (CreditSchedule)e.Row.DataContext;
            if (credit.Id % 2 == 0) e.Row.Background = new SolidColorBrush(Colors.PeachPuff);
            else e.Row.Background = new SolidColorBrush(Colors.White);             
        }

        
    }
    public class CreditSchedule
    {
        public double Id { get; set; }
        public string Date { get; set; }
        public double Pay { get; set; }
        public double Main { get; set; }
        public double Proc { get; set; }
        public double Remains { get; set; }

        public void GetData(double id, string date, double pay,double main,double proc,double remains)
        {
            Id = id;
            Date = date;
            Pay = pay;
            Main = main;
            Proc = proc;
            Remains = remains;
        }

    }
}
