using NewsAppWPF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NewsAppWPF.Services
{
    public class EventService
    {
        private static readonly HttpClient client = new HttpClient();
        private const string ApiBaseUrl = "https://localhost:7002/api/EventLogs"; // Adjust the API URL accordingly

        public static async Task LogEvent(string eventType, int userId)
        {
            try
            {
                var newEventLogDTO = new EventLogDTO
                {
                    EventType = eventType,
                    UserId = userId,
                    Timestamp = DateTime.Now
                };

                var response = await client.PostAsJsonAsync(ApiBaseUrl, newEventLogDTO);
                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    //MessageBox.Show($"Event log failed. Status: {response.StatusCode}, Details: {errorResponse}");
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Event log exception: {ex.Message}");
            }
        }

    }
}
