using HRDataLayer.Enums;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace HRDataLayer.DTOs
{
   public class UserInsertAndUpdate
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Username { set; get; }
        public string Password { set; get; }
        public Roles Role { set; get; }
        public int ManagerID { set; get; }

        public ICollection<VacationInsertAndUpdate> Vacations { set; get; }

    }
}
