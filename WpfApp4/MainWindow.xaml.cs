using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
using Excel = Microsoft.Office.Interop.Excel;

namespace WpfApp4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Danger> dangers = new List<Danger>();
        List<string> changedDangers = new List<string>();
        int numRowsOnPage = 15;
        int maxPages;
        bool isParsed = false;
        bool isFirstDownload = true;
        public MainWindow()
        {
            InitializeComponent();
            
            ParseExcelToDataGrid();
        }
        private void ParseExcelToDataGrid()
        {
            try
            {
                Program.ParseExcel(ref numRowsOnPage, ref maxPages, ref isParsed, dangers, changedDangers);
                isFirstDownload = false;
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Локальной копии не найдено");
                return;
            }
            TextBoxNumOfPage.Text = "1";
            grid.ItemsSource = Program.UpdateDataGridAccordingToPage(Int32.Parse(TextBoxNumOfPage.Text), ref numRowsOnPage, dangers);
        }

        private void grid_MouseDoubleClick_GetMoreInformation(object sender, MouseButtonEventArgs e)
        {
            Danger path = grid.SelectedItem as Danger;
            if (path != null)
            {
                WindowOfInformation windowOfInformation = new WindowOfInformation(path.ToString());
                windowOfInformation.Show();
            }
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            if (isParsed && Int32.Parse(TextBoxNumOfPage.Text) > 1)
            {
                int numPage = Int32.Parse(TextBoxNumOfPage.Text);
                TextBoxNumOfPage.Text = $"{--numPage}";
                
                grid.ItemsSource = Program.UpdateDataGridAccordingToPage(Int32.Parse(TextBoxNumOfPage.Text), ref numRowsOnPage, dangers);
            }
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            if (isParsed && Int32.Parse(TextBoxNumOfPage.Text) < maxPages)
            {
                int numPage = Int32.Parse(TextBoxNumOfPage.Text);
                TextBoxNumOfPage.Text = $"{++numPage}";

                grid.ItemsSource = Program.UpdateDataGridAccordingToPage(Int32.Parse(TextBoxNumOfPage.Text), ref numRowsOnPage, dangers);
            }
        }

        private void Button_Click_GetLocalDataGrid(object sender, RoutedEventArgs e)
        {
            //if (!isParsed)
            ParseExcelToDataGrid();
            
            MessageBox.Show($"Успешно загружена таблица из локального файла");
            //else
            //    MessageBox.Show("Таблица уже загружена");
        }

        private void Button_Click_DownloadFromInternet(object sender, RoutedEventArgs e)
        {
            try
            {
                Program.DownloadDataFromInternet();
                ParseExcelToDataGrid();
                string changedFilesReport = "";
                if (!isFirstDownload)
                    foreach (var item in changedDangers)
                    {
                        changedFilesReport += item + '\n';
                    }
                else 
                    isFirstDownload = false;
                MessageBox.Show($"Успешно загружена БД из интернета\n{changedFilesReport}");
            } catch(System.Net.WebException)
            {
                MessageBox.Show("Не удалось загрузить файл, сайт не работает или был изменен адрес");
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) { }
    }

}
