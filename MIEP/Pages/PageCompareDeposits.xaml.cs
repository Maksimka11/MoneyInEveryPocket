using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MIEP.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageCompareDeposits.xaml
    /// </summary>
    public partial class PageCompareDeposits : Page
    {
        public PageCompareDeposits(double[,] mas)
        {
            InitializeComponent();
            LoadData(mas);
        }

        public List<Deposit> deposits = new List<Deposit>();

        public void LoadData(double[,] mas)
        {
            Deposit deposit = new Deposit();
            deposit.Name = "Стабильный";
            deposits.Add(deposit);
            deposit = new Deposit();
            deposit.Name = "Оптимальный";
            deposits.Add(deposit);
            deposit = new Deposit();
            deposit.Name = "Стандарт";
            deposits.Add(deposit);
            for (int i = 0; i < deposits.Count; i++)
            {
                deposits[i].Income = mas[i, 0];
                deposits[i].Summ = mas[i, 1];
                deposits[i].Proc = mas[i, 2];
                deposits[i].Term = mas[i, 3];
            }

            dgCompare.ItemsSource = deposits;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (dgCompare.SelectedItem == null) return;
            Deposit deposit = (Deposit)dgCompare.SelectedItem;
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            mainWindow.MainFrame.Navigate(new PageLogin(deposit.Summ, deposit.Term, deposit.Proc));
        }

        private void btnForm_Click(object sender, RoutedEventArgs e)
        {
            string fileName = AppDomain.CurrentDomain.BaseDirectory + @"Отчеты\Сравнение вкладов.pdf";
            for (int i = 1; i < Int16.MaxValue; i++)
            {
                if (File.Exists(fileName)) fileName = AppDomain.CurrentDomain.BaseDirectory + @"Отчеты\Сравнение вкладов" + $" ({i})" + ".pdf";
                else break;
            }

            Document doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite));

            doc.Open();
            BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);

            var ColumnCount = dgCompare.Columns.Count - 1;
            float[] Widths = new float[4];
            for (int i = 0; i < dgCompare.Columns.Count - 1; i++)
            {
                Widths[i] = (float)dgCompare.Columns[i].ActualWidth;
            }
            var ColumnWidths = Widths;

            var table = new PdfPTable(ColumnWidths)
            {
                HorizontalAlignment = 0,
                WidthPercentage = 100,
                DefaultCell = { MinimumHeight = 22f, Border = 0 },

            };

            var cell = new PdfPCell(new Phrase("Сравнение вкладов", font))
            {
                Colspan = ColumnCount,
                HorizontalAlignment = 1,
                MinimumHeight = 30f,
                Border = 0
            };
            table.AddCell(cell);

            for (int i = 0; i < dgCompare.Columns.Count - 1; i++)
            {
                cell = new PdfPCell(new Phrase(dgCompare.Columns[i].Header.ToString(), font))
                {
                    BorderWidthBottom = 1,
                    BorderWidthLeft = 0,
                    BorderWidthRight = 0,
                    BorderWidthTop = 0,
                    MinimumHeight = 30f,
                    BorderColor = BaseColor.LIGHT_GRAY

                };
                table.AddCell(cell);
            }

            for (int i = 0; i < deposits.Count; i++)
            {
                cell = new PdfPCell(new Phrase(deposits[i].Name, font))
                {
                    BorderWidthBottom = 1,
                    BorderWidthLeft = 0,
                    BorderWidthRight = 0,
                    BorderWidthTop = 0,
                    MinimumHeight = 30f,
                    BorderColor = BaseColor.LIGHT_GRAY
                };
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(Math.Round(deposits[i].Income, 2).ToString(), font))
                {
                    BorderWidthBottom = 1,
                    BorderWidthLeft = 0,
                    BorderWidthRight = 0,
                    BorderWidthTop = 0,
                    MinimumHeight = 30f,
                    BorderColor = BaseColor.LIGHT_GRAY
                };
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(Math.Round(deposits[i].Summ, 2).ToString(), font))
                {
                    BorderWidthBottom = 1,
                    BorderWidthLeft = 0,
                    BorderWidthRight = 0,
                    BorderWidthTop = 0,
                    MinimumHeight = 30f,
                    BorderColor = BaseColor.LIGHT_GRAY
                };
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(Math.Round(deposits[i].Proc*100, 2).ToString(), font))
                {
                    BorderWidthBottom = 1,
                    BorderWidthLeft = 0,
                    BorderWidthRight = 0,
                    BorderWidthTop = 0,
                    MinimumHeight = 30f,
                    BorderColor = BaseColor.LIGHT_GRAY
                };
                table.AddCell(cell);
            }

            doc.Add(table);
            doc.Close();
        }
    }

    public class Deposit
    {
        public string Name { get; set; }
        public double Income { get; set; }
        public double Summ { get; set; }
        public double Proc { get; set; }
        public double Term { get; set; }

        public void GetData(string name, double income, double summ, double proc)
        {
            Name = name;
            Income = income;
            Summ = summ;
            Proc = proc;
        }

    }
}
