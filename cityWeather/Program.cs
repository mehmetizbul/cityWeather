using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
//NuGet package to handle JSON response deserialize.
using Newtonsoft.Json;

namespace cityWeather
{
    //Class declarations to be mapped to the relevant JSON response.
    public class City
    {
        public string name { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }

    }
    public class Weather
    {
        public Location location { get; set; }
        public Forecast forecast { get; set; }
    }
    public class Location
    {
        public string name { get; set; }
    }

    public class Forecast
    {
        public Forecastday[] forecastday { get; set; }
    }

    public class Forecastday
    {
        public Day day { get; set; }
    }
    public class Day
    {
        public Condition condition { get; set; }
    }
    public class Condition
    {
        public string text { get; set; }
    }

    class Program
    {
        //The configuration parameters.
        static HttpClient client = new HttpClient();
        static string baseTUI = "https://api.musement.com/";
        static string baseApiWeather = "http://api.weatherapi.com/v1/";
        static string weatherApiKey = "dd003f4d64e04f3cb6955131221409";
        static int weatherDays = 2;

        static void Main(string[] args)
        {
            //Following 2 lines are added to fix "Could not create SSL/TLS secure channel" error.
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //Execution of the application logic.
            RunAsync().GetAwaiter().GetResult();                        
        }
        //The function returns the list of cities from TUI API.
        static async Task <List<City>> GetCityAsync()
        {                                       
            var response = await client.GetAsync(baseTUI + "api/v3/cities/");
            var responseString = await response.Content.ReadAsStringAsync();
                
            //Deserializing the JSON response into "City" class using "List".
            var cityList = JsonConvert.DeserializeObject<List<City>>(responseString);

            return cityList;                
            
        }

        //The function returns whe weather information for a specific city from "weatherapi"
        static async Task<Weather> GetWeatherAsync(string url)
        {
            var response = await client.GetAsync(baseApiWeather + url);
            var responseString = await response.Content.ReadAsStringAsync();
            var weather = JsonConvert.DeserializeObject<Weather> (responseString);

            return weather;            

        }

        //The function contains the application logic
        static async Task RunAsync()
        {                                   
            try
            {
                List<City> cities;
                Weather weather;
                Forecastday[] forecastdays;
                string resultString;
                string days;

                //Getting the cities
                cities = await GetCityAsync();
               
                //For each returned city, a weather information request made to "weatherapi"
                foreach (var city in cities)
                {
                    resultString = "Processed city {0} | ";
                    weather = await GetWeatherAsync("forecast.json?key="+ weatherApiKey + "&q="+ city.latitude + ","+ city.longitude + "&days="+ weatherDays);
                    forecastdays = weather.forecast.forecastday;

                    //Here I could have explicitly get todays value with forecastday[0].day.condition.text and tomorrow's value with forecastday[1].day.condition.text, but I decided to use more generic approach.
                    foreach (var forecastday in forecastdays)
                    {
                        days = "";
                        days += forecastday.day.condition.text + " - ";
                        resultString = resultString + days;
                        
                    }
                    resultString = resultString.TrimEnd(' ');
                    resultString = resultString.TrimEnd('-');

                    Console.WriteLine(resultString, weather.location.name);
                }                               

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }        
    }
}
