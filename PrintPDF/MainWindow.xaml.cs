using Ookii.Dialogs.Wpf;
using RawPrint.NetStd;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

[assembly: CLSCompliant(true)]
namespace PrintPDF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region [Свойства]

        private string PrintName { get; set; }
        private string PrintFolder { get; set; }
        private IList<string> PrintFiles { get; }

        #endregion [Свойства]

        #region [Конструктор]

        public MainWindow()
        {
            InitializeComponent();
            PrintName = string.Empty;
            VistaFolderBrowserDialog dialog = new()
            {
                Description = "Из какой папки взять pdf-файлы",
                UseDescriptionForTitle = true
            };

            var result = dialog.ShowDialog(this);
            if (result != true)
                Application.Current.Shutdown();

            PrintFiles = new List<string>();
            PrintFolder = dialog.SelectedPath;
            PrintButtonDialog.Dialog.Content = "Печать";
            CancelButtonDialog.Dialog.Content = "Отмена";

            CancelButtonDialog.Dialog.Click += CloseApp_Click;
            PrintButtonDialog.Dialog.Click += PrintDialog_Click;

            foreach (var item in PrinterSettings.InstalledPrinters)
            {
                RadioButton print = new()
                {
                    Content = item
                };
                print.Checked += Print_Checked;
                PrintList.Items.Add(print);
            }

            IEnumerable<string> files = from item in Directory.GetFiles(PrintFolder, "*.pdf")
                                        select Path.GetFileName(item);

            foreach (var file in files)
            {
                CheckBox check = new()
                {
                    Content = file,
                    IsChecked = true
                };
                check.Click += FileName_Clicked;
                PrintFiles.Add(file);
                PdfList.Items.Add(check);
            }
        }

        #endregion [Конструктор]

        #region [Обработчики]

        private void FileName_Clicked(object sender, RoutedEventArgs e)
        {
            CheckBox file = (CheckBox)sender;

            if (file.IsChecked == true)
            {
                PrintFiles.Add((string)file.Content);
            }
            else
            {
                PrintFiles.Remove((string)file.Content);
            }
        }

        private void PrintDialog_Click(object sender, RoutedEventArgs e)
        {
            if (PrintFiles.Count == 0 || string.IsNullOrEmpty(PrintName))
                return;

            IPrinter printer = new Printer();
            foreach (var file in PrintFiles)
                printer.PrintRawFile(PrintName, string.Format(CultureInfo.CurrentCulture,@"{0}\{1}", PrintFolder, file), file);
        }

        private void Print_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton print = (RadioButton)sender;
            if (print.IsChecked == true)
                PrintName = (string)print.Content;
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
            => Application.Current.Shutdown();

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        #endregion [Обработчики]
    }
}