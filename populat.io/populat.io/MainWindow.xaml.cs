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
            WindowState = WindowState.Maximized;
            city = null;
            CultureInfo ci = new CultureInfo(Thread.CurrentThread.CurrentCulture.Name);
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
            List<string> citiesList = new List<string>();
            using (var context = new dbi359591Entities())
            {
                var cities = (from b in context.PopulationTables
                              select b.City).Distinct();
                citiesList = cities.ToList();
            }
            foreach (var city in citiesList)
            {
                cb_cities.Items.Add(city);
            }
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
                //Adding the cities
                /*using (var context = new dbi359591Entities())
                {
                    foreach (var c in city.PopulationThroughYears)
                    {
                        var cityTable = new PopulationTable();
                        cityTable.City = city.Name;
                        cityTable.Year = c.Year;
                        cityTable.Age0_17 = c.Age0_17;
                        cityTable.Age18_34 = c.Age18_34;
                        cityTable.Age35_54 = c.Age35_54;
                        cityTable.Age55_Up = c.Age55_up;
                        cityTable.AverageAge = c.AverageAge;
                        cityTable.BirthRate = c.BirthRate;
                        cityTable.DeathRate = c.DeathRate;
                        cityTable.EmigrationRate = c.EmigrationRate;
                        cityTable.ImmigrationRate = c.ImmigrationRate;
                        cityTable.Latitude = c.Latitude;
                        cityTable.Longitude = c.Longitude;
                        cityTable.MaleRate = c.MaleRate;
                        cityTable.FemaleRate = c.FemaleRate;
                        cityTable.GrowthRate = c.GrowthRate;
                        cityTable.PopulationNr = c.PopulationNr;
                        context.PopulationTables.Add(cityTable);
                        context.SaveChanges();
                    }
                }*/
            }
        }

        private void AddNewPolygon()
        {
            MapPolygon polygon = new MapPolygon();
            polygon.Fill = new SolidColorBrush(Colors.White);
            polygon.Stroke = new SolidColorBrush(Colors.Red);
            polygon.StrokeThickness = 5;
            polygon.Opacity = 0.4;
            polygon.Locations = new LocationCollection() {
                new Location(location.Latitude + 0.03, location.Longitude - 0.07),
                new Location(location.Latitude + 0.05, location.Longitude - 0.05),
                new Location(location.Latitude + 0.06, location.Longitude), //
                new Location(location.Latitude + 0.05, location.Longitude + 0.05),
                new Location(location.Latitude + 0.03, location.Longitude + 0.07),
                new Location(location.Latitude, location.Longitude + 0.08), //
                new Location(location.Latitude - 0.03, location.Longitude + 0.07),
                new Location(location.Latitude - 0.05, location.Longitude + 0.05),
                new Location(location.Latitude - 0.06, location.Longitude), //
                new Location(location.Latitude - 0.05, location.Longitude - 0.05),
                new Location(location.Latitude - 0.03, location.Longitude - 0.07),
                new Location(location.Latitude, location.Longitude - 0.08)
            };
            MapControl.Children.Add(polygon);
        }

        private void PlotPopulation()
        {
            MapControl.Children.Clear();
            MapControl.Center = location;
            for (int i = 0; i < city.PopulationThroughYears[0].PopulationNr / 10; i++)
            {           // divided by 10 means a pin every 10k people
                MapControl.Children.Add(new Pushpin() { Location = GenerateRandomPoint(5) });
            }
            AddNewPolygon();
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
                var check = city.PopulationThroughYears.First().BirthRate;
                SeriesCollection.Add(new ColumnSeries
                {
                    Title = city.PopulationThroughYears.First().Year.ToString(),
                    Values = new ChartValues<double>
                    {
                        city.PopulationThroughYears.First().BirthRate,
                        city.PopulationThroughYears.First().DeathRate,
                        city.PopulationThroughYears.First().EmigrationRate,
                        city.PopulationThroughYears.First().ImmigrationRate
                    }
                });
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
            SeriesCollection.Add(new ColumnSeries
            {
                Title = city.PopulationThroughYears.Last().Year.ToString(),
                Values = new ChartValues<double>
                {
                    city.PopulationThroughYears.Last().BirthRate,
                    city.PopulationThroughYears.Last().DeathRate,
                    city.PopulationThroughYears.Last().EmigrationRate,
                    city.PopulationThroughYears.Last().ImmigrationRate
                }
            });
            lblCurrentYear.Content = "Current year of the simulation: " + city.PopulationThroughYears.Last().Year;
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
                // Clear previous simulated data
                city.PopulationThroughYears.RemoveRange(city.LastRecord, city.PopulationThroughYears.Count() - city.LastRecord);
                EventHelper eh = new EventHelper((bool)cbDisease.IsChecked, (bool)cbWeather.IsChecked, (bool)cbWar.IsChecked,
                    (bool)cbHigherImmigration.IsChecked, (bool)cbBetterMedication.IsChecked, (bool)cbHigherIncome.IsChecked);
                lbEventLog.Items.Clear();
                for (int i = city.PopulationThroughYears.Last().Year + 1; i <= Convert.ToInt32(tbYear.Text); i++)
                {
                    List<string> outcomes = city.Simulate(i, eh);
                    foreach (string s in outcomes)
                    {
                        lbEventLog.Items.Add(s);
                    }
                    SetCharts();
                    await DelaySim();
                }
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
            return new Location(location.Latitude + x, location.Longitude + y);
        }

        private void Cb_cities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            city = null;
            List<string> citiesList = new List<string>();
            using (var context = new dbi359591Entities())
            {
                // Query for all blogs with names starting with B 
                var dbcity = (from b in context.PopulationTables
                              where b.City.Equals(((ComboBox)sender).SelectedItem.ToString())
                              select b).ToList();

                List<Population> tempList = new List<Population>();
                foreach (var c in dbcity)
                {
                    tempList.Add(new Population()
                    {
                        Year = c.Year,
                        Age0_17 = c.Age0_17,
                        Age18_34 = c.Age18_34,
                        Age35_54 = c.Age35_54,
                        Age55_up = c.Age55_Up,
                        AverageAge = c.AverageAge,
                        BirthRate = c.BirthRate,
                        DeathRate = c.DeathRate,
                        EmigrationRate = c.EmigrationRate,
                        ImmigrationRate = c.ImmigrationRate,
                        Latitude = c.Latitude,
                        Longitude = c.Longitude,
                        MaleRate = c.MaleRate,
                        FemaleRate = c.FemaleRate,
                        GrowthRate = c.GrowthRate,
                        PopulationNr = c.PopulationNr
                    });
                }
                city = new City(dbcity.First().City, tempList);
            }
            lblCityName.Content = city.Name;
            location = new Location(city.PopulationThroughYears.Last().Latitude, city.PopulationThroughYears.Last().Longitude);
            tbYear.Text = city.PopulationThroughYears.Last().Year.ToString();
            tbDelay.Text = "2";
            DatabaseLoadCharts();
            PlotPopulation();
        }
        // Normal Method SetCharts() doesn't work here
        private void DatabaseLoadCharts()
        {
            ChartGender.Series[0].Values = new ChartValues<double> { city.PopulationThroughYears.Last().MaleRate };
            ChartGender.Series[1].Values = new ChartValues<double> { city.PopulationThroughYears.Last().FemaleRate };
            ChartAges.Series[0].Values = new ChartValues<double> { city.PopulationThroughYears.Last().Age0_17 };
            ChartAges.Series[1].Values = new ChartValues<double> { city.PopulationThroughYears.Last().Age18_34 };
            ChartAges.Series[2].Values = new ChartValues<double> { city.PopulationThroughYears.Last().Age35_54 };
            ChartAges.Series[3].Values = new ChartValues<double> { city.PopulationThroughYears.Last().Age55_up };
            ChartBirthDeath.Series[0].Values = new ChartValues<double> { city.PopulationThroughYears.Last().DeathRate };
            ChartBirthDeath.Series[1].Values = new ChartValues<double> { city.PopulationThroughYears.Last().BirthRate };
            ChartPopulationCount.Series[0].Values.Add(Math.Round(city.PopulationThroughYears.Last().PopulationNr));
            Labels.Add(city.PopulationThroughYears.Last().Year.ToString());

            SeriesCollection.Clear();
            var check = city.PopulationThroughYears.First().BirthRate;
            SeriesCollection.Add(new ColumnSeries
            {
                Title = city.PopulationThroughYears.First().Year.ToString(),
                Values = new ChartValues<double>
                    {
                        city.PopulationThroughYears.First().BirthRate,
                        city.PopulationThroughYears.First().DeathRate,
                        city.PopulationThroughYears.First().EmigrationRate,
                        city.PopulationThroughYears.First().ImmigrationRate
                    }
            });
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
            if (SeriesCollection.Count == 2)
            {
                SeriesCollection.RemoveAt(1);
            }
            SeriesCollection.Add(new ColumnSeries
            {
                Title = city.PopulationThroughYears.Last().Year.ToString(),
                Values = new ChartValues<double>
                {
                    city.PopulationThroughYears.Last().BirthRate,
                    city.PopulationThroughYears.Last().DeathRate,
                    city.PopulationThroughYears.Last().EmigrationRate,
                    city.PopulationThroughYears.Last().ImmigrationRate
                }
            });
            lblCurrentYear.Content = "Current year of the simulation: " + city.PopulationThroughYears.Last().Year;
        }
    }
}

