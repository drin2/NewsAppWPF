using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NewsAppWPF.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;

namespace NewsAppWPF.ViewModels
{
    public class ArticleStatisticsViewModel : INotifyPropertyChanged
    {
        public PlotModel ArticleStatisticsModel { get; private set; }

        public ArticleStatisticsViewModel()
        {
            InitializeModel();
            LoadData();
        }

        private void InitializeModel()
        {
            ArticleStatisticsModel = new PlotModel { Title = "Number of Articles Published Per Month" };
            var dateAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "MMM yyyy",
                MinorIntervalType = DateTimeIntervalType.Months,
                IntervalType = DateTimeIntervalType.Months,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Title = "Month"
            };
            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                MinorGridlineStyle = LineStyle.Dot,
                MajorGridlineStyle = LineStyle.Solid,
                Title = "Number of Articles"
            };

            ArticleStatisticsModel.Axes.Add(dateAxis);
            ArticleStatisticsModel.Axes.Add(valueAxis);
        }

        private async void LoadData()
        {
            var lineSeries = new LineSeries { Title = "Articles", MarkerType = MarkerType.Circle };

            try
            {
                var articlesPerMonth = await GetArticleCountsByMonthAsync();
                foreach (var item in articlesPerMonth)
                {
                    var pointDate = new DateTime(item.Year, item.Month, 1);
                    lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(pointDate), item.Count));
                }

                ArticleStatisticsModel.Series.Add(lineSeries);
                ArticleStatisticsModel.InvalidatePlot(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to load article data: " + ex.Message);
                // Handle exceptions or errors as needed
            }
        }

        private async Task<List<ArticleCountDto>> GetArticleCountsByMonthAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7002/api/Articles/");
                var response = await client.GetAsync("CountsByMonth");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<ArticleCountDto>>(json);
                }
                throw new Exception("Failed to load data from server.");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
