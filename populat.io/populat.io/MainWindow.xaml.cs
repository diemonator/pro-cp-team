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
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;

namespace populat.io
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private City city;
        public MainWindow()
        {
            InitializeComponent();
            city = null;
            ChartGender.Series[0].Values = new ChartValues<double> { 50000 };
            ChartGender.Series[1].Values = new ChartValues<double> { 55000 };

            ChartAges.Series[0].Values = new ChartValues<double> { 20000 };
            ChartAges.Series[1].Values = new ChartValues<double> { 40000 };
            ChartAges.Series[2].Values = new ChartValues<double> { 30000 };
            ChartAges.Series[3].Values = new ChartValues<double> { 15000 };

            ChartUrbanization.Series[0].Values = new ChartValues<double> { 20000 };
            ChartUrbanization.Series[1].Values = new ChartValues<double> { 40000 };
            //ChartUrbanization.Series[2].Values = new ChartValues<double> { 30000 };
            //ChartUrbanization.Series[3].Values = new ChartValues<double> { 10000 };
            //ChartUrbanization.Series[4].Values = new ChartValues<double> { 5000 };

            PointLabel = chartPoint => string.Format("{0:P}", chartPoint.Participation);

            Labels = new string[] { "2017", "2018", "2019", "2020", "2021" };
            ChartPopulationCount.Series[0].Values = new ChartValues<double> { 120000, 110000, 115000, 130000, 135000 };

            // sample data
            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "2015",
                    Values = new ChartValues<double> { 10, 50, 39, 50 }
                }
            };

            SeriesCollection.Add(new ColumnSeries
            {
                Title = "2016",
                Values = new ChartValues<double> { 11, 56, 42 }
            });

            SeriesCollection[1].Values.Add(48d);

            DataContext = this;


        }

        public SeriesCollection SeriesCollection { get; set; }
        public Func<ChartPoint, string> PointLabel { get; set; }
        public string[] Labels { get; set; }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog()
            {
                Filter = "CSVFiles (.csv)|.csv",
                AddExtension = true
            };
            if (dlg.ShowDialog() == true)
            {
                CSVHelper flupke = new CSVHelper(dlg.FileName, dlg.SafeFileName);
                flupke.WriteFile();
            }
            else
            {
                MessageBox.Show("You have canceled");
            }

        }

        private void LoadCity_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == true)
            {
                CSVHelper flupke = new CSVHelper(dlg.FileName, dlg.SafeFileName);
                city = flupke.ReadFile();
                SettCharts();
            }
        }

        private void SettCharts()
        {
            foreach (Population p in city.PopulationThroughYears)
            {
                ChartGender.Series[0].Values = ChartGender.Series[0].Values = new ChartValues<double> { p.MaleRate };
                ChartGender.Series[1].Values = ChartGender.Series[1].Values = new ChartValues<double> { p.FemaleRate };
                ChartAges.Series[0].Values = new ChartValues<double> { p.Age0_17 };
                ChartAges.Series[1].Values = new ChartValues<double> { p.Age18_34 };
                ChartAges.Series[2].Values = new ChartValues<double> { p.Age35_54 };
                ChartAges.Series[3].Values = new ChartValues<double> { p.Age55_up };

                ChartUrbanization.Series[0].Values = new ChartValues<double> { p.DeathRate };
                ChartUrbanization.Series[1].Values = new ChartValues<double> { p.BirthRate };
            }
        }
    }
}
