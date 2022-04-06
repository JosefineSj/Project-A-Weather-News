using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json; //Requires nuget package System.Net.Http.Json
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;

using Assignment_A1_01.Models;
using Assignment_A1_01.Services;

namespace Assignment_A1_01
{
    class Program
    {
        static void Main(string[] args)
        {
            double latitude = 59.3166654;  
            double longitude = 18.0666664;

            var t1 = new OpenWeatherService().GetForecastAsync(latitude, longitude);

            Console.WriteLine($"Weather forecast for {t1.Result.City}");

            foreach (var item in t1.Result.Items.GroupBy(x => x.DateTime.Date.ToShortDateString()))
            {
                Console.WriteLine(item.Key);
                foreach (var thing in item)
                {
                    Console.WriteLine($"   - {thing.DateTime.ToString("HH-mm")}: {thing.Description}, Temperatur: {thing.Temperature}°C, Wind: {thing.WindSpeed} m/s");
                }
            }
        }
    }
}
