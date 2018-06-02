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

namespace populat.io
{
    /// <summary>
    /// Interaction logic for Map.xaml
    /// </summary>
    public partial class Map : Window
    {
        Location pinLocation;
        ParameterWindow parameterWindow;
        private City city;
        
        internal Map(City city, ParameterWindow parameterW)
        {
            InitializeComponent();
            this.city = city;
            pinLocation = null;
            parameterWindow = parameterW;
        }

        private void MapControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Point mPosition = e.GetPosition(this);
            pinLocation = MapControl.ViewportPointToLocation(mPosition);

            Pushpin pin = new Pushpin();
            pin.Location = pinLocation;

            MapControl.Children.Add(pin);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            parameterWindow.assignLocation(pinLocation);
            this.Close();
        }
    }
}
