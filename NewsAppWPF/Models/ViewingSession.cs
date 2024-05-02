using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAppWPF.Models
{
    public class ViewingSession
    {
        public int SessionId { get; set; }

        public int? UserId { get; set; }

        public int? ArticleId { get; set; }

        public DateTime? StartTime { get; set; }
    }
}
