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
        internal ParameterWindow(City city)
        {
            InitializeComponent();
        
            this.city = city;
            l_name.Content = l_name.Content + " " +city.Name;

            foreach (var year in city.PopulationThroughYears)
            {
                cb_year.Items.Add(year.Year);
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
    }
}
