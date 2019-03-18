using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Newtonsoft.Json;

namespace CWeatherApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// OpenWeatherMap.org - Current Weather Forecast
    /// https://www.youtube.com/watch?v=StFp9pLbcVU
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private const string API_KEY = "6067074c07a54e73b0e2b33fa76fbe5b";

        public MainWindow()
        {
            InitializeComponent();
            this.Title = "Weather API Application";
            clearAllWeatherLabels();
        }
        void clearAllWeatherLabels()
        {
            lblWeatherDescription.Content = "";
            lblWeatherHumidity.Content = "";
            lblWeatherLocation.Content = "";
            lblWeatherTemp.Content = "";
            lblWeatherWinds.Content = "";
        }

        private void btnGetWeather_Click(object sender, RoutedEventArgs e)
        {
            if(txtZipInput.Text.Length < 5)
            {
                //User must input valid zip code.
                MessageBox.Show("You must enter a 5 digit Zip Code.");
                txtZipInput.Focus();

            } else
            {
                //Call Weater API
                string zipInput = txtZipInput.Text;
                getWeatherByZip(zipInput);                
            }
            
        }

        void getWeatherByZip(string zip)
        {
            try
            {
                using (WebClient web = new WebClient())
                {
                    string url = string.Format("https://api.openweathermap.org/data/2.5/weather?" +
                        "zip=" + zip +
                        "&appid=" + API_KEY + "&units=imperial");
                    var json = web.DownloadString(url);
                    var result = JsonConvert.DeserializeObject<weatherInfo.root>(json);
                    weatherInfo.root outPut = result;

                    lblWeatherTemp.Content = string.Format("{0}", outPut.main.temp) + "°";
                    lblWeatherHumidity.Content = "Humidity - " + string.Format("{0}", outPut.main.humidity) + "%";
                    //lblWeatherDescription.Content = "Weather Description - " + string.Format("{0}", outPut.weather.description);
                    lblWeatherWinds.Content = "Wind Speeds - " + string.Format("{0}", outPut.wind.speed) + "mph";
                    lblWeatherLocation.Content = string.Format("{0}", outPut.name);
                    btnGetWeather.Visibility = Visibility.Hidden;
                    btnTryAgain.Visibility = Visibility.Visible;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: '{0}'", e);
                MessageBox.Show("An error occured");
                txtZipInput.Text = "";
                txtZipInput.Focus();
            }          
        }

        private void btnTryAgain_Click(object sender, RoutedEventArgs e)
        {
            btnGetWeather.Visibility = Visibility.Visible;
            btnTryAgain.Visibility = Visibility.Hidden;
            txtZipInput.Text = "";
            txtZipInput.Focus();
            lblWeatherLocation.Content = "";
            lblWeatherDescription.Content = "";
            lblWeatherTemp.Content = "";
            lblWeatherWinds.Content = "";
            lblWeatherHumidity.Content = "";
        }
    }
}
