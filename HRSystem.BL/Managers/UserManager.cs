using HRDataLayer.DTOs;
using HRDataLayer.Entities;
using HRDataLayer.Enums;
using HRSystem.Models;
using HRSystem.Models.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace HRDataLayer.Managers
{
    public class UserManager
    {
        public HRContext context;
        public UserManager() {
            context = new HRContext();
        }
        public bool IsManager(string username)
        {
            var query = (from u in context.Users
                         where u.Username == username
                         && u.Role == Roles.Manager select u).FirstOrDefault();
            return query != null;
        }

        public bool CheckLogin(UserLogin user)
        {
            var login = (from u in context.Users
                         where (u.Username == user.Username)
                         && (u.Password == user.Password)
                         select u).FirstOrDefault();

            return login != null;

        }
        public IEnumerable<UserLogin> Get(string username, string password) {
            var login = from u in context.Users
                         where (u.Username == username)
                         && (u.Password == password)
                         select new UserLogin()
                         {
                             UserID = u.UserID,
                             Username = u.Username,
                             Password = u.Password,
                         };
            return login;
        }
        public IEnumerable<UserLogin> GetLoggedUser(UserLogin user)
        {

            if (IsManager(user.Username))
            {
                user.Role = (int)Roles.Manager;
            }
            else
                user.Role = (int)Roles.Employee;

            var LoggedUser = from u in context.Users
                        where (u.Username == user.Username)
                         && (u.Password == user.Password)
                        select new UserLogin()
                        {
                            UserID = u.UserID,
                            Username = u.Username,
                            Password = u.Password,
                            Role = user.Role
                        };
            return LoggedUser;

        }

        public void CreateNewUser(UserInsertAndUpdate user)
        {
                context.Users.Add(new User()
                {
                    Name = user.Name,
                    LastName = user.LastName,
                    Username = user.Username,
                    Email = user.Email,
                    Password = user.Password.GetHashCode().ToString(),
                    Mobile = user.Mobile,
                    JobTitle = user.JobTitle,
                    ManagerID = user.ManagerID,
                    // alter later so we can add manger
                    Role = Roles.Employee
                });

                context.SaveChanges();

        }
        public bool CheckUserExists(UserInsertAndUpdate user) {
            bool userNameAlreadyExists = context.Users.Any(x => x.Username == user.Username);
            bool EmailAlreadyExists = context.Users.Any(x => x.Email == user.Email);
            bool MobileAlreadyExists = context.Users.Any(x => x.Mobile == user.Mobile);
            if (!userNameAlreadyExists && !EmailAlreadyExists && !MobileAlreadyExists) {
                return true;
            }
            return false;
        }
    
        // take manager id as argument
        public PageRecord<UserView> GetManagerEmployees(int id,PagingModel paging)
        {
 
            var userInfo = from u in context.Users
                           where id == u.ManagerID
                           select new UserView()
                           {
                               UserID = u.UserID,
                               Name = u.Name,
                               LastName = u.LastName,
                               JobTitle = u.JobTitle,
                               Email = u.Email,
                               Mobile = u.Mobile,
                           };

            var user = new PageRecord<UserView>
            {
                Data = userInfo.ToList(),
                TotalRecord = userInfo.Count(),
            };
            if (!string.IsNullOrEmpty(paging.SearchValue))
            {
                user.Data = user.Data
                        .Where(u => u.Name.ToLower().Contains(paging.SearchValue.ToLower()) ||
                         u.LastName.ToLower().Contains(paging.SearchValue.ToLower()) ||
                        u.JobTitle.ToLower().Contains(paging.SearchValue.ToLower()) ||
                         u.Email.ToLower().Contains(paging.SearchValue.ToLower()) ||
                          u.Mobile.ToLower().Contains(paging.SearchValue.ToLower())).ToList();

            }
            user.TotalFilteredRecord = user.Data.Count();
            //  sorting
          //  user.Data = user.Data.OrderBy(paging.SortCol + " " + paging.SortDir).ToList;
            user.Data = user.Data.OrderBy(u=>u.UserID).ToList();
            // paging
            user.Data = user.Data.Skip(paging.DisplayStart).Take(paging.DisplayLength).ToList();
            return user;
        }

        public UserView GetUser(int id)
        {
            var userInfo = from u in context.Users
                           where id == u.UserID
                           select new UserView()
                           {
                               UserID = u.UserID,
                               Name = u.Name,
                               LastName = u.LastName,
                               Username = u.Username,
                               JobTitle = u.JobTitle,
                               Email = u.Email,
                               Mobile = u.Mobile,
                           };
            return userInfo.FirstOrDefault();
        }
        public void UpdatUser(int id, UserInsertAndUpdate user)
        {
            var updateUser = (from u in context.Users where id == u.UserID
                              select u).Single();
            updateUser.Name = user.Name;
            updateUser.LastName = user.LastName;
            updateUser.JobTitle = user.JobTitle;
            if (updateUser.Username != user.Username) { 
            updateUser.Username = user.Username; }
            if (updateUser.Email != user.Email)
            {
                updateUser.Email = user.Email;
            }
            if (updateUser.Mobile != user.Mobile)
            {
                updateUser.Mobile = user.Mobile;
            }
                context.SaveChanges();
            }  

        public void DeleteUserAccount(int id) {
            var deleteUser = (from u in context.Users where id == u.UserID select u).Single();
            context.Users.Remove(deleteUser);
            context.SaveChanges();
        }
      
        public bool CheckUserID(int id)
        {
            var user = context.Users.Find(id);

            if (user == null)
            {
                return false;
            }
            return true;
        }
    }
}
