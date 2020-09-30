using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += async (sender, error) =>
            {
                var results = await GetWeatherAsync();
                GridView1.ItemsSource = results;
            };
        }

        private IEnumerable<WeatherForecast> GetWeather()
        {
            using (var client = new WebClient())
            {
                try
                {
                    var content = client.DownloadString($"{ConfigurationManager.AppSettings["BaseUrl"]}/WeatherForecast");

                    var data = JsonConvert.DeserializeObject<IEnumerable<WeatherForecast>>(content);

                    return data;
                }
                catch (Exception)
                {
                    throw;
                };
            }
        }

        private async Task<IEnumerable<WeatherForecast>> GetWeatherAsync()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync($"{ConfigurationManager.AppSettings["BaseUrl"]}/WeatherForecast");

                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();

                    var data = JsonConvert.DeserializeObject<IEnumerable<WeatherForecast>>(content);

                    return data;
                }
                catch (Exception)
                { 
                    throw;
                }
            }
        }
    }

    internal class WeatherForecast
    { 
            public DateTime date { get; set; }
            public int temperatureC { get; set; }
            public int temperatureF { get; set; }
            public string summary { get; set; }
    }
}
