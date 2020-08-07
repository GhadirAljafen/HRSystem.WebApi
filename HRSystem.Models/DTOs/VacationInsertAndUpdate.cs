using HRDataLayer.Enums;
using System;

namespace HRDataLayer.DTOs
{
   public class VacationInsertAndUpdate
    {
        public int VacationID { set; get; }
        public Types Type { set; get; }
        public string Attatchment { set; get; }
        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }
        public string Description { set; get; }
        public Statuses Status { set; get; } //0 1 2 
        public int EmployeeID { set; get; }
        public string RejectionReason { set; get; }
        //public UserInsertAndUpdate User { set; get; }
    }
}
