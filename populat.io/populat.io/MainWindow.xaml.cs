using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Maps.MapControl.WPF;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace populat.io
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ParameterWindow child;
        private City city;
        private Random rnd;
        private Location location;
        private List<KeyValuePair<int, string>> scheduledEvents;

        public MainWindow()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
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
            ChartPopulationCount.AxisY[0].MinValue = 0;
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
                // Querry for all city names
                if (context.Database.Exists())
                {
                    var cities = (from b in context.PopulationTables
                                  select b.City).Distinct();
                    citiesList = cities.ToList();
                    foreach (var city in citiesList)
                    {
                        cb_cities.Items.Add(city);
                    }
                }
                else
                {
                    MessageBox.Show("No connection to the database at the current time!", "Warrning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    cb_cities.IsEnabled = false;
                }
            }
            cbEvent.Items.Insert(0, "--Select event--");
            cbEvent.SelectedIndex = 0;
            scheduledEvents = new List<KeyValuePair<int, string>>();
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
                scheduledEvents = new List<KeyValuePair<int, string>>();
                lbEventLog.Items.Clear();
                tbYearEvent.Text = (city.PopulationThroughYears.Last().Year + 1).ToString();
            }
        }

        private MapLayer PlacePinAtLocation(Location l)
        {
            MapLayer mapLayer = new MapLayer();
            Image myPushPin = new Image();
            myPushPin.Source = new BitmapImage(new Uri("Resources/pin.png", UriKind.Relative));
            myPushPin.Width = 45;
            myPushPin.Height = 45;
            mapLayer.AddChild(myPushPin, l, PositionOrigin.Center);
            return mapLayer;
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
                MapControl.Children.Add(PlacePinAtLocation(GenerateRandomPoint(5)));
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
            if (city != null && tbYear.Text != "")
            {
                // Clear previous simulated data
                city.PopulationThroughYears.RemoveRange(city.LastRecord, city.PopulationThroughYears.Count() - city.LastRecord);
                EventHelper eh = GenerateEventHelper();
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
                scheduledEvents = new List<KeyValuePair<int, string>>();
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
            return new Location(location.Latitude + x, location.Longitude + y);
        }

        private void Cb_cities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            city = null;
            List<string> citiesList = new List<string>();
            using (var context = new dbi359591Entities())
            {
                // Query for city
                var dbcity = (from city in context.PopulationTables
                              where city.City.Equals(((ComboBox)sender).SelectedItem.ToString())
                              select city).ToList();

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            child = new ParameterWindow(city);
            child.ParametersUpdated += CityOnRecive;
            child.Show();
        }

        internal void CityOnRecive(City city)
        {
            this.city = city;
            lblCityName.Content = city.Name;
            location = new Location(city.PopulationThroughYears.Last().Latitude, city.PopulationThroughYears.Last().Longitude);
            tbYear.Text = city.PopulationThroughYears.Last().Year.ToString();
            tbDelay.Text = "2";
            DatabaseLoadCharts();
            PlotPopulation();
            if (!cb_cities.Items.Contains(city.Name))
            {
                cb_cities.Items.Add(city.Name);
            }
        }
        private void Tb_AllowInteger(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private EventHelper GenerateEventHelper()
        {
            int disease = (bool)cbDisease.IsChecked ? Convert.ToInt32(tbDiseases.Text) : 0;
            int weather = (bool)cbWeather.IsChecked ? Convert.ToInt32(tbWeather.Text) : 0;
            int war = (bool)cbWar.IsChecked ? Convert.ToInt32(tbWar.Text) : 0;
            int immigration = (bool)cbHigherImmigration.IsChecked ? Convert.ToInt32(tbImmigration.Text) : 0;
            int medication = (bool)cbBetterMedication.IsChecked ? Convert.ToInt32(tbMedication.Text) : 0;
            int income = (bool)cbHigherIncome.IsChecked ? Convert.ToInt32(tbIncome.Text) : 0;
            return new EventHelper(disease, weather, war, immigration, medication, income, scheduledEvents);
        }

        private void BtnAddEvent_Click(object sender, RoutedEventArgs e)
        {
            if (cbEvent.SelectedIndex != 0 && tbYearEvent.Text != "")
            {
                scheduledEvents.Add(new KeyValuePair<int, string>(Convert.ToInt32(tbYearEvent.Text), cbEvent.Text));
                lbEventLog.Items.Add("Event " + cbEvent.Text + " scheduled for year " + Convert.ToInt32(tbYearEvent.Text));
            }
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            if (city != null) {
                SaveFileDialog dialog = new SaveFileDialog()
                {
                    Filter = "PDF Document(*.pdf)|*.pdf",
                    FileName = city.Name
                };

                if (dialog.ShowDialog() == true)
                {
                    PDFHelper.CreatePdf(this, dialog.FileName, city);
                }
            }
            else {
                MessageBox.Show("Please load a city first");
            }
        }
    }
}

