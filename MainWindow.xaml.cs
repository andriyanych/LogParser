using LogParser.data;
using LogParser.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace LogParser
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //SizeChanged += (o, e) =>
            //{
            //    var r = SystemParameters.WorkArea;
            //    Left = r.Right - ActualWidth;
            //    Top = r.Bottom - ActualHeight;
            //};
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new LogdataViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "";
            dlg.DefaultExt = ".log";
            dlg.Filter = "Текстовые файлы | *.log";

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;

                
                Logdata.FromFileToBase(filename);
                DataContext = new LogdataViewModel();
                //string[] lines = File.ReadAllLines(@filename);
                //_ = MessageBox.Show("Выбран файл: " + filename);
            }
        }
    }
}
