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
    public class EventLogViewModel
    {
        public ICommand ExportToCSVCommand { get; private set; }
        public ICommand ExportToXMLCommand { get; private set; }

        public ObservableCollection<EventLog> EventLogs { get; set; } = new ObservableCollection<EventLog>();

        public EventLogViewModel()
        {
            LoadEventLogs();
            ExportToCSVCommand = new RelayCommand(ExportToCSV);
            ExportToXMLCommand = new RelayCommand(ExportToXML);
        }

        private void ExportToCSV()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Event ID, Event Type, User ID, Timestamp");
            foreach (var log in EventLogs)
            {
                sb.AppendLine($"{log.EventId}, {log.EventType}, {log.UserId}, {log.Timestamp:yyyy-MM-dd HH:mm:ss}");
            }
            SaveFile(sb.ToString(), "CSV files|*.csv");
        }

        private void ExportToXML()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<EventLog>));
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, EventLogs);
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

        private async void LoadEventLogs()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:7002/api/EventLogs");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var logs = JsonConvert.DeserializeObject<List<EventLog>>(json);
                    foreach (var log in logs)
                    {
                        EventLogs.Add(log);
                    }
                }
            }
        }
    }
}
