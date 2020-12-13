using Microsoft.AspNet.Identity;
using PassManager_WebApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using PassManager_WebApi.ViewModels;

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
            IEnumerable<ItemPreview> passwords = GetCurrentUser().Passwords
                .OrderBy(p => p.LastVisited)
                .Select(item => new ItemPreview(item.Id, item.Name, item.Username, Enums.TypeOfItems.Password))
                .ToList();
            if (passwords.Count() == 0) return BadRequest("You don't have any accounts!");
            return Ok(passwords);
        }
        //GET api/Passwords/id
        public IHttpActionResult Get(int id)
        {
            if (id <= 0) return BadRequest("Id is invalid");
            PasswordVM password = GetCurrentUser().Passwords.Select(pass => new PasswordVM(pass)).FirstOrDefault(pass => pass.Id == id);
            if (password is null) return BadRequest("Password does not exist!");
            return Ok(password);
        }
        //POST api/Passwords
        public IHttpActionResult Post([FromBody] PasswordVM password)
        {
            if (password is null) return BadRequest("Password does not exist!");
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
            Password passwordToBeModified = GetCurrentPassword(id);
            if (passwordToBeModified is null) return BadRequest("Password not found!");
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
