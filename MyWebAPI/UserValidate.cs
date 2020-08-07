using HRDataLayer.DTOs;
using HRDataLayer.Entities;
using HRDataLayer.Managers;
using HRSystem.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebAPI
{
    public class UserValidate
    {
            //This method is used to check the user credentials
            public static bool Login(string username, string password)
            {
                UserManager userManager = new UserManager();
            var UserLists = userManager.Get(username,password);
                return UserLists.Any(user =>
                    user.Username.Equals(username, StringComparison.OrdinalIgnoreCase)
                    && user.Password == password);
            }
        
    }
}