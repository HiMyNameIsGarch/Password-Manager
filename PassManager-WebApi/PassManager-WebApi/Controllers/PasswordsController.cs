using Microsoft.AspNet.Identity;
using PassManager_WebApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using PassManager_WebApi.ViewModels;
using PassManager_WebApi.Enums;
using System.Diagnostics;

namespace PassManager_WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Passwords")]
    public class PasswordsController : ApiController
    {
        private PasswordManagerEntities db = new PasswordManagerEntities();

        //GET api/Passwords
        public IHttpActionResult Get()//get latest passwords preview
        {
            //bring from db just the passwords
            IEnumerable<ItemPreview> passwords = GetCurrentUser().Passwords
                .OrderByDescending(p => p.NumOfVisits)
                .ThenByDescending(p => p.Name)
                .Select(item => new ItemPreview(item.Id, item.Name, item.Username, TypeOfItems.Password)).ToList();
            return Ok(passwords);
        }
        //GET api/Password?lastCreated=true
        public IHttpActionResult Get(bool lastCreated)
        {
            if (lastCreated)
            {
                var userId = User.Identity.GetUserId();
                var lastestPass = db.Passwords
                    .Where(w => w.UserId == userId)
                    .OrderByDescending(s => s.CreateDate)
                    .Take(1)
                    .Select(item => new ItemPreview() { Id = item.Id, Title = item.Name, SubTitle = item.Username, ItemType = TypeOfItems.Password});
                return Ok(lastestPass.FirstOrDefault());
            }
            return BadRequest();
        }

        //GET api/Passwords/id
        public IHttpActionResult Get(int id)
        {
            if (id <= 0) return BadRequest("Id is invalid");
            Password password = GetCurrentUser().Passwords.FirstOrDefault(pass => pass.Id == id);//get the current password from user
            if (password is null) return BadRequest("Password does not exist!");//check if that exists
            password.NumOfVisits++;//modify when is last visited
            db.SaveChanges();//save the changes
            return Ok(new PasswordVM(password));//send
        }
        //POST api/Passwords
        public IHttpActionResult Post([FromBody] PasswordVM password)
        {
            if (password is null) return BadRequest("Password does not exist!");
            var isModelValid = password.IsModelValid();
            if (!string.IsNullOrEmpty(isModelValid)) return BadRequest(isModelValid);
            GetCurrentUser().Passwords.Add(new Password(password));
            db.SaveChanges();
            return Ok();
        }
        //PUT api/Passwords/id
        public IHttpActionResult Put(int id, [FromBody] PasswordVM password)
        {
            if (password is null) return BadRequest("Password does not exist!");
            if (id != password.Id) return BadRequest("Id does not match with the password");
            if (id <= 0) return BadRequest("Id is invalid!");
            //check is model is valid
            var isModelValid = password.IsModelValid();
            if (!string.IsNullOrEmpty(isModelValid)) return BadRequest(isModelValid);
            //get current password
            Password passwordToBeModified = GetCurrentPassword(id);
            if (passwordToBeModified is null) return BadRequest("Password not found!");
            //modify it
            passwordToBeModified.ModifyTo(password);
            db.SaveChanges();
            return Ok();
        }
        //DELETE api/Passwords/id
        public IHttpActionResult Delete(int id)
        {
            if (id <= 0) return BadRequest("Id is invalid!");
            Password passwordToBeDeleted = GetCurrentPassword(id);
            if (passwordToBeDeleted is null) return BadRequest("Password not found!");
            db.Passwords.Remove(passwordToBeDeleted);
            db.SaveChanges();
            return Ok();
        }
        private AspNetUser GetCurrentUser()
        {
            return db.AspNetUsers.Find(User.Identity.GetUserId());
        }
        private Password GetCurrentPassword(int id)
        {
            return GetCurrentUser().Passwords.FirstOrDefault(pass => pass.Id == id);
        }
    }
}
