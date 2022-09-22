using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
// NuGet package to handle JSON response deserialize.
using Newtonsoft.Json;

namespace CityWeather
{
    // Class declarations to be mapped to the relevant JSON responses.
    public class City
    {

        public string Name { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }
    }

    public class Weather
    {
        public Location Location { get; set; }

        public Forecast Forecast { get; set; }
    }

    public class Location
    {
        public string Name { get; set; }
    }

    public class Forecast
    {
        public ForecastDay[] Forecastday { get; set; }
    }

    public class ForecastDay
    {
        public Day Day { get; set; }
    }

    public class Day
    {
        public Condition Condition { get; set; }
    }

    public class Condition
    {
        public string Text { get; set; }
    }

    class Program
    {
        // The configuration parameters.
        private static HttpClient client = new HttpClient();
        private static string baseTUI = "https://api.musement.com/";
        private static string baseApiWeather = "http://api.weatherapi.com/v1/";
        private static string weatherApiKey = "dd003f4d64e04f3cb6955131221409";
        private static int weatherDays = 2;

        static void Main(string[] args)
        {
            // Following 2 lines are added to fix "Could not create SSL/TLS secure channel" error.
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Execution of the application logic.
            RunAsync().GetAwaiter().GetResult();                        
        }
        // The function returns the City object list from TUI API response.
        private static async Task <List<City>> GetCityAsync()
        {      
            var response = await client.GetAsync(baseTUI + "api/v3/cities/");
            // Reading the response string
            var responseString = await response.Content.ReadAsStringAsync();
            // Deserializing the JSON response into "City" class using "List".
            return JsonConvert.DeserializeObject<List<City>>(responseString);                                     
        }

        // The function returns the weather information for a specific city into Weather object from "weatherapi"
        private static async Task<Weather> GetWeatherAsync(string url)
        {      
            var response = await client.GetAsync(baseApiWeather + url);
            // Reading the response string
            var responseString = await response.Content.ReadAsStringAsync();
            // Deserializing the JSON response into "Weather" class.
            return JsonConvert.DeserializeObject<Weather>(responseString);                    
        }

        // The function contains the application logic
        private static async Task RunAsync()
        {                                   
            try
            {
                // Class instance declarations
                List<City> cities = new List<City>();
                Weather weather = new Weather();
                ForecastDay[] forecastdays;

                string resultString;
                string days;

                // Getting the cities
                cities = await GetCityAsync();                
               
                // For each returned city, a weather information request made to "weatherapi"
                foreach (City city in cities)
                {
                    resultString = "Processed city {0} | ";
                    weather = await GetWeatherAsync("forecast.json?key="+ weatherApiKey + "&q="+ city.Latitude + ","+ city.Longitude + "&days="+ weatherDays);
                    // Assign the result to forecastday class
                    forecastdays = weather.Forecast.Forecastday;

                    // Here I could have get todays value with forecastday[0].day.condition.text and tomorrow's value with forecastday[1].day.condition.text with indexing, but I decided to use more generic approach.
                    foreach (ForecastDay forecastday in forecastdays)
                    {
                        days = string.Empty;
                        days += forecastday.Day.Condition.Text + " - ";
                        resultString = resultString + days;                        
                    }
                    // Removing the "- and " " at the end of the result string.
                    resultString = resultString.TrimEnd(' ');
                    resultString = resultString.TrimEnd('-');
                    Console.WriteLine(resultString, weather.Location.Name);
                }                               
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }        
    }
}
