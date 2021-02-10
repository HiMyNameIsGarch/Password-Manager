using Microsoft.AspNet.Identity;
using PassManager_WebApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using PassManager_WebApi.ViewModels;
using PassManager_WebApi.Enums;
using PassManager_WebApi.Models.Interfaces;

namespace PassManager_WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Passwords")]
    public class PasswordsController : ApiController, IBaseItemController<PasswordVM>
    {
        private PasswordManagerEntities db = new PasswordManagerEntities();

        //GET api/Passwords
        public IHttpActionResult Get()//get latest passwords preview
        {
            string userId = User.Identity.GetUserId();
            //bring from db just the item preview for passwords
            IEnumerable<ItemPreview> passwords = EntireItems.GetAllPasswords(db, userId); ;
            
            return Ok(passwords.GroupItems());
        }
        //GET api/Password?lastCreated=true
        public IHttpActionResult Get(bool lastCreated)
        {
            if (lastCreated)
            {
                var userId = User.Identity.GetUserId();
                string imageUrl = IconHelper.GetImageUrl(TypeOfItems.Password);
                var lastestPass = db.Passwords
                    .Where(w => w.UserId == userId)
                    .OrderByDescending(s => s.CreateDate)
                    .Take(1)
                    .Select(item => new ItemPreview() {
                        Id = item.Id, 
                        Title = item.Name, 
                        SubTitle = item.Username, 
                        ItemType = TypeOfItems.Password,
                        IconUrl = imageUrl
                    });
                return Ok(lastestPass.FirstOrDefault());
            }
            return BadRequest();
        }
        //GET api/Passwords/5
        public IHttpActionResult Get(int id)
        {
            if (id <= 0) return BadRequest(ErrorMsg.InvalidId);
            string userId = User.Identity.GetUserId();
            Password password = db.Passwords.FirstOrDefault(w => w.Id == id && w.UserId == userId);//get the current password from user
            if (password is null) return BadRequest(ErrorMsg.ItemDoesNotExist(TypeOfItems.Password));//check if that exists
            password.NumOfVisits++;//modify when is last visited
            db.SaveChanges();//save the changes
            return Ok(new PasswordVM(password));//send
        }
        //POST api/Passwords
        public IHttpActionResult Post([FromBody] PasswordVM password)
        {
            if (password is null) return BadRequest(ErrorMsg.ItemDoesNotExist(TypeOfItems.Password));
            var isModelValid = password.IsModelValid();
            if (!string.IsNullOrEmpty(isModelValid)) return BadRequest(isModelValid);
            string userId = User.Identity.GetUserId();
            db.Passwords.Add(new Password(password, userId));
            db.SaveChanges();
            return Ok();
        }
        //PUT api/Passwords/5
        public IHttpActionResult Put(int id, [FromBody] PasswordVM password)
        {
            //check if the password id and the id match
            if (password is null) return BadRequest(ErrorMsg.ItemDoesNotExist(TypeOfItems.Password));
            if (id != password.Id) return BadRequest(ErrorMsg.InvalidIdMatchingWith(TypeOfItems.Password));
            if (id <= 0) return BadRequest(ErrorMsg.InvalidId);
            //check is model is valid
            var isModelValid = password.IsModelValid();
            if (!string.IsNullOrEmpty(isModelValid)) return BadRequest(isModelValid);
            //get current user id and modify it
            string userId = User.Identity.GetUserId();
            Password passwordToBeModified = db.Passwords.FirstOrDefault(w => w.Id == id && w.UserId == userId);
            if (passwordToBeModified is null) return BadRequest(ErrorMsg.ItemNotFound(TypeOfItems.Password));
            //modify it
            passwordToBeModified.ModifyTo(password);
            db.SaveChanges();
            return Ok();
        }
        //DELETE api/Passwords/5
        public IHttpActionResult Delete(int id)
        {
            if (id <= 0) return BadRequest(ErrorMsg.InvalidId);
            string userId = User.Identity.GetUserId();
            Password passwordToBeDeleted = db.Passwords.FirstOrDefault(w => w.Id == id && w.UserId == userId);
            if (passwordToBeDeleted is null) return BadRequest(ErrorMsg.ItemNotFound(TypeOfItems.Password));
            db.Passwords.Remove(passwordToBeDeleted);
            db.SaveChanges();
            return Ok();
        }
    }
}
