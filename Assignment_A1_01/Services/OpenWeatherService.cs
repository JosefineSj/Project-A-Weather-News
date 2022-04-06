﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json; //Requires nuget package System.Net.Http.Json
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;

using Assignment_A1_01.Models;
using System.IO;
using System.Collections;

namespace Assignment_A1_01.Services
{
    public class OpenWeatherService 
    {
        HttpClient httpClient = new HttpClient();
        readonly string apiKey = "2badc0d0b2c833da44b9966864769599"; // Your API Key
        public async Task<Forecast> GetForecastAsync(double latitude, double longitude)
        {
            //https://openweathermap.org/current
            var language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var uri = $"https://api.openweathermap.org/data/2.5/forecast?lat={latitude}&lon={longitude}&units=metric&lang={language}&appid={apiKey}";

            //Read the response from the WebApi
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            WeatherApiData wd = await response.Content.ReadFromJsonAsync<WeatherApiData>();

            Forecast forecast = new Forecast();

            try
            {
                forecast.City = wd.city.name;
                forecast.Items = wd.list.Select(item => new ForecastItem
                {
                    DateTime = UnixTimeStampToDateTime(item.dt),
                    Temperature = item.main.temp,
                    WindSpeed = item.wind.speed,
                    Description = item.weather.Select(desc => desc.description).FirstOrDefault().ToString(),
                    Icon = item.weather.Select(item => item.icon).ToString()
                }).ToList();
            }
            catch (Exception)
            {

                throw;
            }


            return forecast;

            //Your Code to convert WeatherApiData to Forecast using Linq.

            /*using (Stream s = File.Create(fname("Example.json")))
            using (TextWriter writer = new StreamWriter(s))
                writer.Write(JsonSerializer.Serialize<WeatherApiData>(wd, new JsonSerializerOptions() { WriteIndented = true }));

            Forecast forecast;
            using (Stream s = File.OpenRead(fname("Example.json")))
            using (TextReader reader = new StreamReader(s))
                forecast = JsonSerializer.Deserialize<Forecast>(reader.ReadToEnd());

            static string fname(string name)
            {
                var documentPath = Path.GetFullPath(@"C:\Users\Josef\OneDrive\Dokument\OP2\Project Part A");
                documentPath = Path.Combine(documentPath, "JSONTest");
                if (!Directory.Exists(documentPath)) Directory.CreateDirectory(documentPath);
                return Path.Combine(documentPath, name);
            }

            */
        }
        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

      
    }
}
