using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAppWPF.Models
{
    public class AdPlacement
    {
        public int PlacementId { get; set; }

        public int? AdId { get; set; }

        public DateOnly? PlacementDate { get; set; }

    }
}
