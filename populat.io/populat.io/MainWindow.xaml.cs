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

namespace populat.io
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ChartGender.Series[0].Values = new ChartValues<double> { 50000 };
            ChartGender.Series[1].Values = new ChartValues<double> { 55000 };

            ChartAges.Series[0].Values = new ChartValues<double> { 20000 };
            ChartAges.Series[1].Values = new ChartValues<double> { 40000 };
            ChartAges.Series[2].Values = new ChartValues<double> { 30000 };
            ChartAges.Series[3].Values = new ChartValues<double> { 15000 };

            ChartUrbanization.Series[0].Values = new ChartValues<double> { 20000 };
            ChartUrbanization.Series[1].Values = new ChartValues<double> { 40000 };
            ChartUrbanization.Series[2].Values = new ChartValues<double> { 30000 };
            ChartUrbanization.Series[3].Values = new ChartValues<double> { 10000 };
            ChartUrbanization.Series[4].Values = new ChartValues<double> { 5000 };

            PointLabel = chartPoint => string.Format("{0:P}", chartPoint.Participation);

            Labels = new string[] { "2017", "2018", "2019", "2020", "2021" };
            ChartPopulationCount.Series[0].Values = new ChartValues<double> { 120000, 110000, 115000, 130000, 135000 };
            
            DataContext = this;

        }

        public Func<ChartPoint, string> PointLabel { get; set; }
        public string[] Labels { get; set; }
    }
}
