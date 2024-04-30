using NewsAppWPF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NewsAppWPF
{
    public static class ApiHelper
    {
        private static HttpClient _httpClient = new HttpClient();
        static ApiHelper()
        {
            // Initialize HttpClient here or via DI
            _httpClient.BaseAddress = new Uri("https://localhost:7002/api/"); // Set the base URL for API
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }
        public static async Task<List<Article>> GetArticles()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("Articles");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Article>>(json); // Using Newtonsoft.Json
                }
                else
                {
                    throw new Exception("Failed to fetch articles.");
                }
            }
            catch (Exception ex)
            {
                // Handle or log exception
                Console.WriteLine(ex.Message);
                throw; // or return a default value or empty list
            }
        }
    }
}
