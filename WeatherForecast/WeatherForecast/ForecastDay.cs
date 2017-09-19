using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast
{
    public class ForecastDay
    {
        public double minT { get; set; }
        public double maxT { get; set; }
        public string icon { get; set; }

        public ForecastDay(double min, double max, string icon)
        {
            maxT = max;
            minT = min;
            this.icon=icon;
        }
    }
}