using HRDataLayer.DTOs;
using HRDataLayer.Entities;
using HRDataLayer.Enums;
using HRSystem.Models;
using HRSystem.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading;

namespace HRDataLayer.Managers
{
    public class VacationManager
    {
        public HRContext context;
        public VacationManager()
        {
            context = new HRContext();
        }
        public bool AddNewVacation(VacationInsertAndUpdate vacation)
        {
            var totalDays = (vacation.EndDate - vacation.StartDate).TotalDays;
            var updateVacationBalence = (from u in context.Users
                                         where vacation.EmployeeID == u.UserID
                                         select u).Single();
            if (vacation.Type == Types.Annual)
            {
                updateVacationBalence.AnnualVacationBalance = updateVacationBalence.AnnualVacationBalance - Math.Round(totalDays);
            }
            if (CheckBalence(updateVacationBalence.AnnualVacationBalance))
            {
                context.Vacations.Add(new Vacation()
                {
                    Type = vacation.Type,
                    Status = vacation.Status,
                    Attatchment = vacation.Attatchment,
                    Description = vacation.Description,
                    StartDate = vacation.StartDate,
                    EndDate = vacation.EndDate,
                    EmployeeID = vacation.EmployeeID,

                });

                context.SaveChanges();
                return true;
            }
            else return false;
        }
        public bool CheckBalence(double balence){
            if (balence <= 0) {
                return false;
                }
        return true;
        }
        public bool CheckNumberOfDays(VacationInsertAndUpdate vacation) {
            double counter = 14;
            var totalDays = (vacation.EndDate - vacation.StartDate).TotalDays;
            var totalHours = (vacation.EndDate - vacation.StartDate).TotalHours;
            if (Math.Abs(totalDays) > 14 && vacation.Type == Types.Annual)
            {
                return false;
            }
            else if (Math.Abs(totalHours) > 8 && vacation.Type == Types.Leave) {
                return false;
            }
             counter = counter - totalDays;
            return true;
        }

        // Display all valacation for managaer's employees
        public PageRecord<VacationView> DisplayVacations(int id,int role,PagingModel paging)
        {
            IQueryable<VacationView> innerJoinQuery = null;
            if (role == 0)
            {
                innerJoinQuery = from v in context.Vacations
                                 join u in context.Users on v.EmployeeID equals u.UserID
                                 where id == u.ManagerID && v.Status != Statuses.Draft
                                 select new VacationView
                                 {
                                     EmployeeID = u.UserID,
                                     Name = u.Name,
                                     LastName = u.LastName,
                                     VacationID = v.VacationID,
                                     Description = v.Description,
                                     Attatchment = v.Attatchment,
                                     StartDate = v.StartDate,
                                     EndDate = v.EndDate,
                                     Status = v.Status,
                                     Type = v.Type,
                                     RejectionReason = v.RejectionReason

                                 };
            }
            else if (role == 1)
            {
                 innerJoinQuery = from v in context.Vacations
                                     join u in context.Users on v.EmployeeID equals u.UserID
                                     where id == v.EmployeeID
                                     select new VacationView
                                     {
                                         EmployeeID = u.UserID,
                                         Name = u.Name,
                                         LastName = u.LastName,
                                         VacationID = v.VacationID,
                                         Description = v.Description,
                                         Attatchment = v.Attatchment,
                                         StartDate = v.StartDate,
                                         EndDate = v.EndDate,
                                         Status = v.Status,
                                         Type = v.Type,
                                         RejectionReason = v.RejectionReason

                                     };
            }
            var vacation = new PageRecord<VacationView>
            {
                Data = innerJoinQuery.ToList(),
                TotalRecord = innerJoinQuery.Count(),
            };
            if (!string.IsNullOrEmpty(paging.SearchValue))
            {
                vacation.Data = vacation.Data
                        .Where(u => u.Name.ToLower().Contains(paging.SearchValue.ToLower())||
                        u.VacationID.ToString().Contains(paging.SearchValue.ToLower())||
                        u.EmployeeID.ToString().Contains(paging.SearchValue.ToLower())||
                        u.LastName.ToLower().Contains(paging.SearchValue.ToLower())||
                        u.StartDate.ToString().Contains(paging.SearchValue.ToLower()) ||
                        u.EndDate.ToString().Contains(paging.SearchValue.ToLower()) ||
                        u.Status.ToString().Contains(paging.SearchValue.ToLower()) ||
                        u.Type.ToString().Contains(paging.SearchValue.ToLower())).ToList();

            }
            vacation.TotalFilteredRecord = vacation.Data.Count();
            //  sorting
            //  userInfo = vacation.OrderBy(paging.SortCol + " " + paging.SortDir);
            vacation.Data = vacation.Data.OrderBy(u => u.VacationID).ToList();
            // paging
            vacation.Data = vacation.Data.Skip(paging.DisplayStart).Take(paging.DisplayLength).ToList();
            return vacation;

        } 

        //vacation id
        public VacationView GetVacationById(int id)
        {
            var vacationInfo = from v in context.Vacations
                           where id == v.VacationID
                           select new VacationView()
                           {
                               VacationID = v.VacationID,
                               Type = v.Type,
                               StartDate = v.StartDate,
                               EndDate = v.EndDate,
                               Attatchment = v.Attatchment,
                               Description = v.Description,
                               Status = v.Status,
                               EmployeeID = v.EmployeeID,
                           };
            return vacationInfo.FirstOrDefault();
        }

        public void UpdateVacationStatus(int id, VacationInsertAndUpdate vacation) {
            var updateStatus = (from v in context.Vacations where id == v.VacationID select v).FirstOrDefault();
            if (vacation.Status == Statuses.Rejected) {
                updateStatus.RejectionReason = vacation.RejectionReason;
            }
            updateStatus.Status = vacation.Status;
            updateStatus.Description = vacation.Description;
            updateStatus.Attatchment = vacation.Attatchment;
            updateStatus.Type = vacation.Type;
            updateStatus.StartDate = vacation.StartDate;
            updateStatus.EndDate = vacation.EndDate;

            context.SaveChanges();
        }

        public void DeleteVacation(int id)
        {
            var deleteVacation = (from v in context.Vacations where id == v.VacationID select v).Single();
            context.Vacations.Remove(deleteVacation);
            context.SaveChanges();
        }

    }
}
