using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAppWPF.Models
{
    public class Advertisement
    {
        public int AdId { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }

        public int Duration { get; set; }

        public string OrderersEmail { get; set; }
    }
}
