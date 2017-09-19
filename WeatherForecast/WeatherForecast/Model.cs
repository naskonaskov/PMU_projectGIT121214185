using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;
using static WeatherForecast.Forecast;

namespace WeatherForecast
{
    public class Model
    {
        public static List<ForecastDay> days = new List<ForecastDay>();
        private static double kelvinConst = 273.15;
        private static City currentCity;
        private static double latitude;
        private static double longitude;
        private static CurrentWeather.RootObject weather;
        private static Forecast.RootObject forecas;

        public static City CurrentCity { get => currentCity; private set => currentCity = value; }
        public static double Latitude { get => latitude; private set => latitude = value; }
        public static double Longitude { get => longitude; private set => longitude = value; }
        public static CurrentWeather.RootObject Weather { get => weather; private set => weather = value; }
        public static Forecast.RootObject Forecas { get => forecas; private set => forecas = value; }

        private async static void GetCoordinates()
        {
            try
            {
                var geoLocator = new Geolocator();
                geoLocator.DesiredAccuracy = PositionAccuracy.High;
                var pos = await geoLocator.GetGeopositionAsync();
                Latitude = pos.Coordinate.Latitude;
                Longitude = pos.Coordinate.Latitude;
            }
            catch (UnauthorizedAccessException)
            {

                var messageDialog = new MessageDialog("Location is turned off. Please switch it on.");
            }

        }


        private static void FindCityID(double lat, double lon)
        {

            List<City> items;
            using (StreamReader r = File.OpenText("Assets/city.list.json"))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<City>>(json);

            }

            double curDistance;
            double distance = 0;
            int index = 0;
            for (int i = 0; i < items.Count; i++)
            {
                curDistance = Math.Sqrt(Math.Pow((items[i].coord.lat - lat), 2) + Math.Pow((items[i].coord.lon - lon), 2));
                if (curDistance < distance || distance == 0)
                {
                    distance = curDistance;
                    index = i;
                }
            }
            CurrentCity = items[index];
        }


        public static async Task<Model> Create()
        {
            GetCoordinates();
            FindCityID(Latitude, Longitude);
            await GetWeatherInfo();
            return new Model();
        }


        private static async Task GetWeatherInfo()
        {

            //Current Weather                                                               /id    
            string urlWeather = String.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&APPID=24a0f8866eff886f1bd065b3ea374ee1", "Sofia,bg");// CurrentCity.id);
            var request = WebRequest.Create(urlWeather);
            var response = await request.GetResponseAsync();
            string json;
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                json = sr.ReadToEnd();
            }
            CurrentWeather.RootObject modelWeather = JsonConvert.DeserializeObject<CurrentWeather.RootObject>(json);
            EditWeatherInfo(modelWeather);
            //Forecast                                                                         //id
            string urlForecast = String.Format("http://api.openweathermap.org/data/2.5/forecast?q={0}&APPID=24a0f8866eff886f1bd065b3ea374ee1", "Sofia,bg");// CurrentCity.id);
            request = WebRequest.Create(urlForecast);
            response = await request.GetResponseAsync();
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                json = sr.ReadToEnd();
            }
            Forecast.RootObject modelForecast = JsonConvert.DeserializeObject<Forecast.RootObject>(json);
            EditForecastInfo(modelForecast);
            Forecas = modelForecast;
        }

        private static void EditForecastInfo(Forecast.RootObject modelForecast)
        {
            double minT = 9999;
            double maxT = -9999;
            string icon = "0";
            List<double> hourlyT = new List<double>();

            foreach (var item in modelForecast.list)
            {
                if (DateTime.Now.AddDays(1).Day == DateTime.Parse(item.dt_txt).Day)
                {

                    hourlyT.Add(item.main.temp);
                    string temp = item.weather[0].icon;
                    temp = temp.TrimEnd(temp[temp.Length - 1]);
                    if (int.Parse(temp) > int.Parse(icon))
                    {
                        icon = temp;
                    }
                }
            }

            minT = Math.Round(hourlyT.Min() - kelvinConst);
            maxT = Math.Round(hourlyT.Max() - kelvinConst);
            hourlyT.Clear();
            days.Add(new ForecastDay(minT, maxT, icon));
            icon = "0";
            foreach (var item in modelForecast.list)
            {
                if (DateTime.Now.AddDays(2).Day == DateTime.Parse(item.dt_txt).Day)
                {
                    hourlyT.Add(item.main.temp);
                    string temp = item.weather[0].icon;
                    temp = temp.TrimEnd(temp[temp.Length - 1]);
                    if (int.Parse(temp) > int.Parse(icon))
                    {
                        icon = temp;
                    }
                }
            }

            minT = Math.Round(hourlyT.Min() - kelvinConst);
            maxT = Math.Round(hourlyT.Max() - kelvinConst);
            hourlyT.Clear();
            days.Add(new ForecastDay(minT, maxT, icon));
            icon = "0";
            foreach (var item in modelForecast.list)
            {
                if (DateTime.Now.AddDays(3).Day == DateTime.Parse(item.dt_txt).Day)
                {
                    hourlyT.Add(item.main.temp);
                    string temp = item.weather[0].icon;
                    temp = temp.TrimEnd(temp[temp.Length - 1]);
                    if (int.Parse(temp) > int.Parse(icon))
                    {
                        icon = temp;
                    }
                }
            }

            minT = Math.Round(hourlyT.Min() - kelvinConst);
            maxT = Math.Round(hourlyT.Max() - kelvinConst);
            hourlyT.Clear();
            days.Add(new ForecastDay(minT, maxT, icon));
            icon = "0";
            foreach (var item in modelForecast.list)
            {
                if (DateTime.Now.AddDays(4).Day == DateTime.Parse(item.dt_txt).Day)
                {
                    hourlyT.Add(item.main.temp);
                    string temp = item.weather[0].icon;
                    temp = temp.TrimEnd(temp[temp.Length - 1]);
                    if (int.Parse(temp) > int.Parse(icon))
                    {
                        icon = temp;
                    }
                }
            }
            minT = Math.Round(hourlyT.Min() - kelvinConst);
            maxT = Math.Round(hourlyT.Max() - kelvinConst);
            hourlyT.Clear();
            days.Add(new ForecastDay(minT, maxT, icon));

        }

        private static void EditWeatherInfo(CurrentWeather.RootObject modelWeather)
        {
            modelWeather.main.temp = Math.Round(modelWeather.main.temp - kelvinConst);
            weather = modelWeather;
        }
    }
}
