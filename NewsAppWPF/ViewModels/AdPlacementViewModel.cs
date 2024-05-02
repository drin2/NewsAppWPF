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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace NewsAppWPF.ViewModels
{
    public class AdPlacementViewModel
    {
        public ICommand ExportToCSVCommand { get; private set; }
        public ICommand ExportToXMLCommand { get; private set; }

        public ObservableCollection<AdPlacement> AdPlacements { get; set; } = new ObservableCollection<AdPlacement>();

        public AdPlacementViewModel()
        {
            LoadAdPlacements();
            ExportToCSVCommand = new RelayCommand(ExportToCSV);
            ExportToXMLCommand = new RelayCommand(ExportToXML);
        }

        private void ExportToCSV()
        {
            StringBuilder csvContent = new StringBuilder();
            csvContent.AppendLine("Placement ID,Ad ID,Placement Date");
            foreach (var placement in AdPlacements)
            {
                csvContent.AppendLine($"{placement.PlacementId},{placement.AdId},{placement.PlacementDate:yyyy-MM-dd}");
            }
            SaveFile(csvContent.ToString(), "CSV files|*.csv");
        }

        private void ExportToXML()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<AdPlacement>));
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, AdPlacements);
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

        private async void LoadAdPlacements()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:7002/api/AdPlacements");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var placements = JsonConvert.DeserializeObject<List<AdPlacement>>(json);
                    foreach (var placement in placements)
                    {
                        AdPlacements.Add(placement);
                    }
                }
            }
        }
    }
}
