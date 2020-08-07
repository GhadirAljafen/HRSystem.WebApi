using HRDataLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Models.DTOs
{
    public class UserLogin
    {
        public int UserID { get; set; }
        public string Username { set; get; }
        public string Password { set; get; }
        public int Role { set; get; }
    }
}
