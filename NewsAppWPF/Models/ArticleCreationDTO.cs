using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAppWPF.Models
{
    public class ArticleCreationDTO
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string Category { get; set; }
        public int UserId { get; set; }
        public List<string> PhotoUrls { get; set; }
    }
}
