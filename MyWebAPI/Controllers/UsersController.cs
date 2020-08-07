using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HRDataLayer.DTOs;
using HRDataLayer.Managers;
using HRSystem.Models.DTOs;
using MyWebAPI;

namespace WebApi.Controllers
{
   // [BasicAuthentication]
    public class UsersController : ApiController
    { 
        UserManager UserManager = new UserManager();

        [HttpPost]
        [ResponseType(typeof(UserView))]
        public HttpResponseMessage GetManagerEmployees(int id, [FromBody]PagingModel paging)
        {
           // try http with comstumize response message!
            if (!UserManager.CheckUserID(id))
            {
                var message = string.Format($"User with id = {id} not found");
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
            }

            else {
                return Request.CreateResponse(HttpStatusCode.OK, UserManager.GetManagerEmployees(id,paging));
            }
            
        }

        public HttpResponseMessage GetUserById(int id)
        {
   
            if (!UserManager.CheckUserID(id))
            {
                var message = string.Format($"User with id = {id} not found");
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
            }

            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, UserManager.GetUser(id));
            }
        }

        public HttpResponseMessage Login([FromBody] UserLogin user)
        {
            if(UserManager.CheckLogin(user))
            return Request.CreateResponse(HttpStatusCode.OK, UserManager.GetLoggedUser(user));
            else
            return Request.CreateResponse(HttpStatusCode.BadRequest, UserManager.GetLoggedUser(user));
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, UserInsertAndUpdate user)
        {
            if (id != user.UserID)
            {
                return BadRequest("No match");
            }
            //if (UserManager.CheckUserExists(user))
            //{
            try {
                UserManager.UpdatUser(id, user);
                return Ok($"User {id} updated successfully"); }
            catch (Exception ex) { return BadRequest(); }

          //  }
            //else
            //{
            //    return BadRequest();
            //}

        }

        // POST: api/Users
        [ResponseType(typeof(UserInsertAndUpdate))]
        [HttpPost]
        public IHttpActionResult PostUser([FromBody] UserInsertAndUpdate user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (UserManager.CheckUserExists(user))
            {
                UserManager.CreateNewUser(user);
                return Ok();
            }
            else {
                return BadRequest();
            }
            
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(UserInsertAndUpdate))]
        public IHttpActionResult DeleteUser(int id)
        {
            if (!UserManager.CheckUserID(id)) {
                return NotFound();
            }
            UserManager.DeleteUserAccount(id);
            return Ok($"User {id} deleted successfully");
        }


    }
}