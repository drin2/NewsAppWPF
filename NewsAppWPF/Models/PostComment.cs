using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAppWPF.Models
{
    public class PostComment
    {
        public string? Text { get; set; }

        public DateOnly? CommentDate { get; set; }

        public int? UserId { get; set; }

        public int? ArticleId { get; set; }
    }
}
