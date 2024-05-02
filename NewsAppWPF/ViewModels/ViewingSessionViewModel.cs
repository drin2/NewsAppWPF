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
    public class ViewingSessionViewModel
    {
        public ICommand ExportToCSVCommand { get; private set; }
        public ICommand ExportToXMLCommand { get; private set; }

        public ObservableCollection<ViewingSession> Sessions { get; set; } = new ObservableCollection<ViewingSession>();

        public ViewingSessionViewModel()
        {
            LoadSessions();
            ExportToCSVCommand = new RelayCommand(ExportToCSV);
            ExportToXMLCommand = new RelayCommand(ExportToXML);
        }

        private void ExportToCSV()
        {
            StringBuilder csvContent = new StringBuilder();
            csvContent.AppendLine("Session ID,User ID,Article ID,Start Time");
            foreach (var session in Sessions)
            {
                csvContent.AppendLine($"{session.SessionId},{session.UserId},{session.ArticleId},{session.StartTime:yyyy-MM-dd HH:mm:ss}");
            }
            SaveFile(csvContent.ToString(), "CSV files|*.csv");
        }

        private void ExportToXML()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<ViewingSession>));
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, Sessions);
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

        private async void LoadSessions()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:7002/api/ViewingSessions");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var sessions = JsonConvert.DeserializeObject<List<ViewingSession>>(json);
                    foreach (var session in sessions)
                    {
                        Sessions.Add(session);
                    }
                }
            }
        }
    }

}
