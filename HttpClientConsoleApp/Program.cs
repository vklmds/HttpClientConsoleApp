using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace HttpClientConsoleApp
{
    class Program
    {
        const string baseUrl = "https://localhost:7157/";
        static async Task Main(string[] args)
        {
            var serviceProvider = ConfigureServices();
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            await GetRequest(httpClientFactory,baseUrl); 
            await PostRequest(httpClientFactory, baseUrl);

        }
        static async Task GetRequest(IHttpClientFactory httpClientFactory, string endpoint)
        {
            var client = httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(baseUrl);

            HttpResponseMessage responseGet = await client.GetAsync("api/controller/getCountries");
            responseGet.EnsureSuccessStatusCode();            
               
            string responseBody = await responseGet.Content.ReadAsStringAsync();           
            Console.WriteLine(responseBody);
            //await Task.Delay(10000);
        }

        static async Task PostRequest(IHttpClientFactory httpClientFactory, string baseUrl)
        { 
            var client = httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(baseUrl);
            string countryName = "United States"; 
            string encodedName = Uri.EscapeDataString(countryName);

            HttpResponseMessage responsePost = await client.PostAsync($"api/controller/getCountryInfo?name={encodedName}", null);
            responsePost.EnsureSuccessStatusCode();            
            string responseBody = await responsePost.Content.ReadAsStringAsync();                      
            Console.WriteLine(responseBody);
            //await Task.Delay(10000);

        }
        static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddHttpClient();           
            return services.BuildServiceProvider();
        }
    }


}