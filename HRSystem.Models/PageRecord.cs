using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Models
{
    //[ComplexType]
    public class PageRecord<T>
    {
        public List<T> Data { get; set; }
        public int TotalRecord { get; set; }
        public int TotalFilteredRecord { get; set; }
    }
}
