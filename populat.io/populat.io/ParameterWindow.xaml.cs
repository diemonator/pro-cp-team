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
using System.Windows.Shapes;
using Microsoft.Maps.MapControl.WPF;
using Microsoft.Win32;

namespace populat.io
{
    /// <summary>
    /// Interaction logic for ParameterWindow.xaml
    /// </summary>
    public partial class ParameterWindow : Window
    {
        private City city;
        internal delegate void ParameterUpdateHandler(City city);
        internal event ParameterUpdateHandler ParametersUpdated;
        private Map map;
        public Location cityLocation;
        internal ParameterWindow(City city)
        {
            InitializeComponent();
        
            this.city = city;
            //-l_name.Content = l_name.Content + " " +city.Name;
            if (city != null)
            {
                foreach (var year in city.PopulationThroughYears)
                {
                    cb_year.Items.Add(year.Year);
                }
            }

            if(cityLocation == null)
            {
                export_city.IsEnabled = false;
            }
        }

        private void Btn_setParameters_Click(object sender, RoutedEventArgs e)
        {
            foreach (var data in city.PopulationThroughYears)
            {
                if (cb_year.SelectedItem.ToString() == data.Year.ToString())
                {
                    try
                    {
                        data.Age0_17 = double.Parse(tb_age0_17.Text);
                        data.Age18_34 = double.Parse(tb_age18_34.Text);
                        data.Age35_54 = double.Parse(tb_age35_54.Text);
                        data.Age55_up = double.Parse(tb_age55_up.Text);
                        data.AverageAge = double.Parse(tb_averageAge.Text);
                        data.BirthRate = double.Parse(tb_birthRate.Text);
                        data.DeathRate = double.Parse(tb_deathRate.Text);
                        data.EmigrationRate = double.Parse(tb_emigration.Text);
                        data.FemaleRate = double.Parse(tb_femaleRate.Text);
                        data.GrowthRate = double.Parse(tb_growthRate.Text);
                        data.ImmigrationRate = double.Parse(tb_immigration.Text);
                        data.MaleRate = double.Parse(tb_maleRate.Text);
                        data.PopulationNr = double.Parse(tb_populationNr.Text);
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Incorrect format! Please, enter the values correctly","Warrning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }

        private void Cb_year_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var data in city.PopulationThroughYears)
            {
                if (cb_year.SelectedItem.ToString() == data.Year.ToString())
                {
                    tb_age0_17.Text = data.Age0_17.ToString();
                    tb_age18_34.Text = data.Age18_34.ToString();
                    tb_age35_54.Text = data.Age35_54.ToString();
                    tb_age55_up.Text = data.Age55_up.ToString();
                    tb_averageAge.Text = data.AverageAge.ToString();
                    tb_birthRate.Text = data.BirthRate.ToString();
                    tb_deathRate.Text = data.DeathRate.ToString();
                    tb_emigration.Text = data.EmigrationRate.ToString();
                    tb_femaleRate.Text = data.FemaleRate.ToString();
                    tb_growthRate.Text = data.GrowthRate.ToString();
                    tb_immigration.Text = data.ImmigrationRate.ToString();
                    tb_maleRate.Text = data.MaleRate.ToString();
                    tb_populationNr.Text = data.PopulationNr.ToString();
                }
            }
        }

        private void Btn_simulate_Click(object sender, RoutedEventArgs e)
        {
            ParametersUpdated.Invoke(city);
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            map = new Map(this.city, this);
            map.Show();
        }

        internal void assignLocation(Location loc)
        {
            this.cityLocation = loc;
            if(cityLocation != null)
            {
                export_city.IsEnabled = true;
            }
        }

        private void btn_exportCsv(object sender, RoutedEventArgs e)
        {
            Population p = new Population
            {
                Year = int.Parse(new_year.Text),
                BirthRate = double.Parse(new_birthRate.Text)/ 100,
                DeathRate = double.Parse(new_deathRate.Text)/ 100,
                PopulationNr = double.Parse(new_populationNr.Text)/100,
                MaleRate = double.Parse(new_maleRate.Text)/ 100,
                FemaleRate = double.Parse(new_femaleRate.Text)/ 100,
                EmigrationRate = double.Parse(new_emigration.Text)/ 100,
                ImmigrationRate = double.Parse(new_immigration.Text)/ 100,
                AverageAge = double.Parse(new_averageAge.Text),
                Age0_17 = double.Parse(new_age0_17.Text)/ 100,
                Age18_34 = double.Parse(new_age18_34.Text)/ 100,
                Age35_54 = double.Parse(new_age35_54.Text)/ 100,
                Age55_up = double.Parse(new_age55_up.Text)/ 100
            };
            p.Latitude = cityLocation.Latitude;
            p.Longitude = cityLocation.Longitude;
            p.GrowthRate = double.Parse(new_growthRate.Text) / 100;
            List<Population> populations = new List<Population>();
            populations.Add(p);
            City newCity = new City(new_name.Text, populations);

            SaveFileDialog dlg = new SaveFileDialog()
            {
                Filter = "CSV|.csv",
                AddExtension = true,
                FileName = newCity.Name
            };
            if (dlg.ShowDialog() == true)
            {
                CSVHelper flupke = new CSVHelper(dlg.FileName, dlg.SafeFileName);
                flupke.WriteFile(newCity);
            }
            else
            {
                MessageBox.Show("You have canceled");
            }
            cleanFields();
        }

        public void cleanFields()
        {
            new_year.Clear();
            new_name.Clear();
            new_age0_17.Clear();
            new_age18_34.Clear();
            new_age35_54.Clear();
            new_age55_up.Clear();
            new_averageAge.Clear();
            new_birthRate.Clear();
            new_deathRate.Clear();
            new_emigration.Clear();
            new_femaleRate.Clear();
            new_growthRate.Clear();
            new_immigration.Clear();
            new_maleRate.Clear();
            new_populationNr.Clear();
        }
    }
}
