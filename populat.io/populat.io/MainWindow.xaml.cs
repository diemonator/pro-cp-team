using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Maps.MapControl.WPF;
using Microsoft.Win32;

namespace populat.io
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private City city;
        private Random rnd;
        private Location location;
        public MainWindow()
        {
            InitializeComponent();
            city = null;
            NumberFormatInfo numberFormat = new NumberFormatInfo();
            string CultureName = Thread.CurrentThread.CurrentCulture.Name;
            CultureInfo ci = new CultureInfo(CultureName);
            if (ci.NumberFormat.NumberDecimalSeparator != ".")
            {
                // Forcing use of decimal separator for numerical values
                ci.NumberFormat.NumberDecimalSeparator = ".";
                Thread.CurrentThread.CurrentCulture = ci;
            }
            rnd = new Random();
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
                lblCityName.Content = city.Name;
                location = new Location(city.PopulationThroughYears.Last().Latitude, city.PopulationThroughYears.Last().Longitude);
                tbYear.Text = city.PopulationThroughYears.Last().Year.ToString();
                tbDelay.Text = "2";
                SetCharts();
                PlotPopulation();
                
            }
        }

        private void PlotPopulation()
        {
            MapControl.Children.Clear();
            MapControl.Center = location;
            for (int i = 0; i < city.PopulationThroughYears[0].PopulationNr / 10; i++)
            {           // divided by 10 means a pin every 10k people
                MapControl.Children.Add(new Pushpin() { Location = GenerateRandomPoint(5) });
            }
        }

        private void SetCharts()
        {                  
            ChartGender.Series[0].Values = new ChartValues<double> { city.PopulationThroughYears.Last().MaleRate };
            ChartGender.Series[1].Values = new ChartValues<double> { city.PopulationThroughYears.Last().FemaleRate };
            ChartAges.Series[0].Values = new ChartValues<double> { city.PopulationThroughYears.Last().Age0_17 };
            ChartAges.Series[1].Values = new ChartValues<double> { city.PopulationThroughYears.Last().Age18_34 };
            ChartAges.Series[2].Values = new ChartValues<double> { city.PopulationThroughYears.Last().Age35_54 };
            ChartAges.Series[3].Values = new ChartValues<double> { city.PopulationThroughYears.Last().Age55_up };
            ChartBirthDeath.Series[0].Values = new ChartValues<double> { city.PopulationThroughYears.Last().DeathRate };
            ChartBirthDeath.Series[1].Values = new ChartValues<double> { city.PopulationThroughYears.Last().BirthRate };         
            if (city.PopulationThroughYears.Count > ChartPopulationCount.Series[0].Values.Count)
            {
                ChartPopulationCount.Series[0].Values.Add(Math.Round(city.PopulationThroughYears.Last().PopulationNr));
                Labels.Add(city.PopulationThroughYears.Last().Year.ToString());
            }
            else
            {
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
                Labels.Clear();
                ChartPopulationCount.Series[0].Values = new ChartValues<double>();
                foreach (Population p in city.PopulationThroughYears)
                {
                    if (city.PopulationThroughYears.Count > 15)
                    {
                        if (p.Year % 3 == 0)
                        {
                            ChartPopulationCount.Series[0].Values.Add(Math.Round(p.PopulationNr));
                            Labels.Add(p.Year.ToString());
                        }

                    }
                    else
                    {
                        ChartPopulationCount.Series[0].Values.Add(Math.Round(p.PopulationNr));
                        Labels.Add(p.Year.ToString());
                    }
                }
            }
            if (SeriesCollection.Count == 2)
            {
                SeriesCollection.RemoveAt(1);
            }
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
            lblCurrentYear.Content = "Current year of the simulation :" + city.PopulationThroughYears.Last().Year;
        }

        async Task DelaySim()
        {
            double delay;
            if (tbDelay.Text == "")
            {
                delay = 0;
            }
            else
            {
                delay = Convert.ToDouble(tbDelay.Text) * 1000;
            }           
            await Task.Delay((int)delay);
        }

        private async void BtnSimulate_Click(object sender, RoutedEventArgs e)
        {
            if (city != null)
            {
                EnableCheckBoxes(false);
                // Clear previous simulated data
                city.PopulationThroughYears.RemoveRange(city.LastRecord, city.PopulationThroughYears.Count() - city.LastRecord);
                for (int i = city.PopulationThroughYears.Last().Year + 1; i <= Convert.ToInt32(tbYear.Text); i++)
                {
                    city.Simulate(i);
                    SetCharts();
                    await DelaySim();                  
                }
                EnableCheckBoxes(true);
                PlotPopulation();
            }
            else
            {
                lblCityName.Content = "Please load a city first";
            }
        }

        private Location GenerateRandomPoint(double radius)
        {           
            int distance = rnd.Next((int)radius);
            double angle = rnd.Next(360) / (2 * Math.PI);
            double x = (distance * Math.Cos(angle));
            double y = distance * Math.Sin(angle);
            x /= 90;
            y /= 90;
            //"51.44164199999999, 5.469722499999989"         
            return new Location(location.Latitude + x, location.Longitude +y);
        }

        private void EnableCheckBoxes(bool state)
        {
            checkB_Disease.IsEnabled = state;
            checkB_Weather.IsEnabled = state;
            checkB_Other.IsEnabled = state;
        }
    }
}

