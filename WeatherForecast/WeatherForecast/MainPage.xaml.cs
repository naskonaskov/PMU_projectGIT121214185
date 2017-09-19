using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WeatherForecast
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            tbDay1.Text = (System.DateTime.Now.AddDays(1)).DayOfWeek.ToString();
            tbDay2.Text = (System.DateTime.Now.AddDays(2)).DayOfWeek.ToString();
            tbDay3.Text = (System.DateTime.Now.AddDays(3)).DayOfWeek.ToString();
            tbDay4.Text = (System.DateTime.Now.AddDays(4)).DayOfWeek.ToString();
            btnRefresh_Click(null,null);
        }

        private async void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Model model = await Model.Create();
            }
            catch (System.Net.WebException)
            {

                var msg = new MessageDialog("No internet connection!");
               await msg.ShowAsync();
                return;
            }
            catch (Exception)
            {
                var msg = new MessageDialog("Data can not be recieved at the moment. Please try again!");
                await msg.ShowAsync();
                return;
            }
            tbCityName.Text = Model.Weather.name;                                                                                                                                                                                                                                                                                                                                                                                                                                                                
            tbTemp.Text = Model.Weather.main.temp.ToString()+ "°C";
            tbSunrise.Text = Model.Weather.clouds.all.ToString()+"%";
            tbSunset.Text = Model.Weather.wind.speed.ToString() + "m/s";

            string url = String.Format("ms-appx://WeatherForecast/Assets/{0}.png",Model.Weather.weather[0].icon);
            img.Source = new BitmapImage(new Uri(url));

            tbT1.Text = Model.days[0].maxT.ToString() + "/" + Model.days[0].minT.ToString();
            tbT2.Text = Model.days[1].maxT.ToString() + "/" + Model.days[1].minT.ToString();
            tbT3.Text = Model.days[2].maxT.ToString() + "/" + Model.days[2].minT.ToString();
            tbT4.Text = Model.days[3].maxT.ToString() + "/" + Model.days[3].minT.ToString();
            
            img1.Source = SetImageSource(Model.days[0]);
            img2.Source = SetImageSource(Model.days[1]);
            img3.Source = SetImageSource(Model.days[2]);
            img4.Source = SetImageSource(Model.days[3]);
        }

        private BitmapImage SetImageSource(ForecastDay day)
        {
            string url = String.Format("ms-appx://WeatherForecast/Assets/{0}d.png",day.icon);
            return new BitmapImage(new Uri(url));
        }
    }
}
