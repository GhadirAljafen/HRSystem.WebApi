﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRDataLayer.DTOs
{
    public class UserView
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public int ManagaerID { get; set; }

    }
}
