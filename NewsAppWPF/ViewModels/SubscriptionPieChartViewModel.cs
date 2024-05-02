using Newtonsoft.Json;
using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NewsAppWPF.Models;

namespace NewsAppWPF.ViewModels
{
    public class SubscriptionPieChartViewModel
    {
        public PlotModel PieModel { get; private set; }

        public SubscriptionPieChartViewModel()
        {
            PieModel = new PlotModel { Title = "Subscription Types" };
            LoadPieChartData();
        }

        private async void LoadPieChartData()
        {
            var subscriptionCounts = await GetSubscriptionCountsAsync();
            var pieSeries = new PieSeries { StrokeThickness = 2.0, InsideLabelPosition = 0.5, AngleSpan = 360, StartAngle = 0 };

            foreach (var count in subscriptionCounts)
            {
                pieSeries.Slices.Add(new PieSlice(count.SubscriptionType, count.Count) { IsExploded = false });
            }

            PieModel.Series.Add(pieSeries);
            PieModel.InvalidatePlot(true);
        }

        private async Task<List<SubscriptionTypeCount>> GetSubscriptionCountsAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:7002/api/Users/SubscribersBySubscription");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<SubscriptionTypeCount>>(json);
                }
                return new List<SubscriptionTypeCount>();
            }
        }
    }
}
