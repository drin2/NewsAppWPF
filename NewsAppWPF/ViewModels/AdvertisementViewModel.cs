using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using NewsAppWPF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace NewsAppWPF.ViewModels
{
    public class AdvertisementViewModel
    {
        public ObservableCollection<Advertisement> Advertisements { get; set; } = new ObservableCollection<Advertisement>();
        public Advertisement SelectedAd { get; set; }

        public ICommand PlaceAdCommand { get; private set; }
        public ICommand DeleteAdCommand { get; private set; }
        public ICommand ExportToCSVCommand { get; private set; }
        public ICommand ExportToXMLCommand { get; private set; }

        public AdvertisementViewModel()
        {
            PlaceAdCommand = new RelayCommand(PlaceAd);
            DeleteAdCommand = new RelayCommand(DeleteAd);
            ExportToCSVCommand = new RelayCommand(ExportToCSV);
            ExportToXMLCommand = new RelayCommand(ExportToXML);
            LoadAdvertisements();
        }

        private void ExportToCSV()
        {
            StringBuilder csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("Ad ID,Title,Duration,Orderer's Email,Text");
            foreach (var ad in Advertisements)
            {
                csvBuilder.AppendLine($"{ad.AdId},{ad.Title},{ad.Duration},{ad.OrderersEmail},{ad.Text}");
            }
            SaveFile(csvBuilder.ToString(), "CSV files|*.csv");
        }

        private void ExportToXML()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Advertisement>));
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, Advertisements);
                SaveFile(writer.ToString(), "XML files|*.xml");
            }
        }

        private void SaveFile(string data, string filter)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = filter
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, data);
            }
        }
        private async void PlaceAd()
        {
            if (SelectedAd != null)
            {
                var placement = new AdPlacement
                {
                    AdId = SelectedAd.AdId,
                    PlacementDate = DateOnly.FromDateTime(DateTime.Now)
                };

                using (var client = new HttpClient())
                {
                    var response = await client.PostAsJsonAsync("https://localhost:7002/api/AdPlacements", placement);
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Failed to place ad.");
                    }
                }
            }
        }

        private async void DeleteAd()
        {
            if (SelectedAd != null)
            {
                using (var client = new HttpClient())
                {
                    var response = await client.DeleteAsync($"https://localhost:7002/api/Advertisements/{SelectedAd.AdId}");
                    if (response.IsSuccessStatusCode)
                    {
                        Advertisements.Remove(SelectedAd);
                    }
                    else
                    {
                        Console.WriteLine("Failed to delete ad.");
                    }
                }
            }
        }
        private async void LoadAdvertisements()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:7002/api/Advertisements");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var ads = JsonConvert.DeserializeObject<List<Advertisement>>(json);
                    foreach (var ad in ads)
                    {
                        Advertisements.Add(ad);
                    }
                }
            }
        }
    }

}
