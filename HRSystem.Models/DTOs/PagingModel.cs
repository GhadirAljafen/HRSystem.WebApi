using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Models.DTOs
{
   public class PagingModel
    {
        public int DisplayLength { get; set; }
        public int DisplayStart { get; set; }
        public string SortCol { get; set; }
        public string SortDir { get; set; }
        public string SearchValue { get; set; } = null;

    }
}
