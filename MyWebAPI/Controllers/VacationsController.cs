using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HRDataLayer.DTOs;
using HRDataLayer.Entities;
using HRDataLayer.Managers;
using HRSystem.Models.DTOs;

namespace WebApi.Controllers
{
    public class VacationsController : ApiController
    {
        VacationManager vacationManager = new VacationManager();

       [HttpPost]
        public HttpResponseMessage GetVacations(int id,int role,[FromBody]PagingModel paging)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, vacationManager.DisplayVacations(id,role,paging));
            }
            
            catch (Exception ex) {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        // GET: api/Vacations/5
        [ResponseType(typeof(Vacation))]
        public HttpResponseMessage GetVacationById(int id)
        {
              return Request.CreateResponse(HttpStatusCode.OK, vacationManager.GetVacationById(id));
        }

        // PUT: api/Vacations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVacation(int id, VacationInsertAndUpdate status)
        {        
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            vacationManager.UpdateVacationStatus(id, status);
            return Ok($"User {id} updated successfully");
        }

        // POST: api/Vacations
        [ResponseType(typeof(VacationInsertAndUpdate))]
        public IHttpActionResult PostVacation(VacationInsertAndUpdate vacation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (vacationManager.CheckNumberOfDays(vacation))
            {
                if (vacationManager.AddNewVacation(vacation))
                return Ok();
                else return BadRequest();
            }
            else
                return BadRequest();
           
   
          //  return CreatedAtRoute("DefaultApi", new { id = vacation.VacationID }, vacation);
        }

        // DELETE: api/Vacations/5
        [ResponseType(typeof(VacationView))]
        public IHttpActionResult DeleteVacation(int id)
        {
            vacationManager.DeleteVacation(id);
            return Ok($"Vacation {id} deleted successfully");
        }
    }
}