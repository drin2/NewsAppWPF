using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAppWPF.Models
{
    public class EventLogDTO
    {
        public string? EventType { get; set; }
        public int? UserId { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
