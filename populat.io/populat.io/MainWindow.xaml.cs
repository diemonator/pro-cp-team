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

            PointLabel = chartPoint => string.Format("{0:P}", chartPoint.Participation);

            Labels = new List<string> { "2017", "2018", "2019", "2020", "2021" };
            ChartPopulationCount.Series[0].Values = new ChartValues<double> { 120000, 110000, 115000, 130000, 135000 };

            LabelsColumns = new List<string> { "Birth", "Death", "Emigration", "Immigration" };

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
        public List<string> Labels { get; set; }
        public List<string> LabelsColumns { get; set; }

        private void SaveCity_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog()
            {
                Filter = "CSV|.csv",
                AddExtension = true,
                FileName = city.Name
            };
            if (dlg.ShowDialog() == true)
            {
                CSVHelper flupke = new CSVHelper(dlg.FileName, dlg.SafeFileName);
                flupke.WriteFile(city);
            }
            else
            {
                MessageBox.Show("You have canceled");
            }
        }

        private void LoadCity_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog()
            {
                Filter = "CSV File (*.csv)|*.csv"
            };
            if (dlg.ShowDialog() == true)
            {
                CSVHelper flupke = new CSVHelper(dlg.FileName, dlg.SafeFileName);
                city = flupke.ReadFile();
                SetCharts();
            }
        }

        private void SetCharts()
        {
            Labels.Clear();
            ChartPopulationCount.Series[0].Values = new ChartValues<double>();
            ChartGender.Series[0].Values = new ChartValues<double> { city.PopulationThroughYears.Last().MaleRate };
            ChartGender.Series[1].Values = new ChartValues<double> { city.PopulationThroughYears.Last().FemaleRate };
            ChartAges.Series[0].Values = new ChartValues<double> { city.PopulationThroughYears.Last().Age0_17 };
            ChartAges.Series[1].Values = new ChartValues<double> { city.PopulationThroughYears.Last().Age18_34 };
            ChartAges.Series[2].Values = new ChartValues<double> { city.PopulationThroughYears.Last().Age35_54 };
            ChartAges.Series[3].Values = new ChartValues<double> { city.PopulationThroughYears.Last().Age55_up };
            ChartBirthDeath.Series[0].Values = new ChartValues<double> { city.PopulationThroughYears.Last().DeathRate };
            ChartBirthDeath.Series[1].Values = new ChartValues<double> { city.PopulationThroughYears.Last().BirthRate };
            SeriesCollection.Clear();
            SeriesCollection.Add
            (
                new ColumnSeries
                {
                    Title = city.PopulationThroughYears.First().Year.ToString(),
                    Values = new ChartValues<double>
                    {
                        city.PopulationThroughYears.First().BirthRate,
                        city.PopulationThroughYears.First().DeathRate,
                        city.PopulationThroughYears.First().EmigrationRate,
                        city.PopulationThroughYears.First().ImmigrationRate
                    }
                }
            );
            SeriesCollection.Add
            (
                new ColumnSeries
                {
                    Title = city.PopulationThroughYears.Last().Year.ToString(),
                    Values = new ChartValues<double>
                    {
                        city.PopulationThroughYears.Last().BirthRate,
                        city.PopulationThroughYears.Last().DeathRate,
                        city.PopulationThroughYears.Last().EmigrationRate,
                        city.PopulationThroughYears.Last().ImmigrationRate
                    }
                }
            );
            foreach (Population p in city.PopulationThroughYears)
            {
                ChartPopulationCount.Series[0].Values.Add(p.PopulationNr);
                Labels.Add(p.Year.ToString());
            }
        }
    }
}
