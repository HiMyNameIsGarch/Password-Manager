using Microsoft.AspNet.Identity;
using PassManager_WebApi.Enums;
using PassManager_WebApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PassManager_WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/EntireItems")]
    public class EntireItemsController : ApiController
    {
        private PasswordManagerEntities db = new PasswordManagerEntities();

        //GET api/EntireItems
        public IHttpActionResult Get()//get latest passwords preview
        {
            IEnumerable<ItemPreview> previews = GetAllItems(GetCurrentUser());

            return Ok(previews);
        }
        //GET API/EntireItems/searchString
        public IHttpActionResult Get(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return BadRequest("Your item is null or empty!");
            }
            IEnumerable<ItemPreview> previews = GetAllItems(GetCurrentUser(), searchString);
            return Ok(previews);
        }
        //GET API/EntireItems/updateType
        public IHttpActionResult Get(TypeOfUpdates updateType)
        {
            var user = GetCurrentUser();
            if (updateType != TypeOfUpdates.Create && updateType != TypeOfUpdates.Modify)
            {
                return BadRequest("UpdateType is invalid!");
            }
            //take passwords
            var passwords = user.Passwords
            .OrderByDescending(s => updateType == TypeOfUpdates.Create ? s.CreateDate : s.LastModified)
            .Select(item => new ItemPreview(item.Id, item.Name, item.Username, TypeOfItems.Password)).Take(1)
            .Union
            (user.Wifis
            .OrderByDescending(s => updateType == TypeOfUpdates.Create ? s.CreateDate : s.LastModified)
            .Select(item => new ItemPreview(item.Id, item.Name, TypeOfItems.Wifi.ToString(), TypeOfItems.Wifi)).Take(1))
            .FirstOrDefault();
            if (passwords is null)
            {
                return BadRequest("No item were updated!");
            }
            return Ok(passwords);
        }
        private IEnumerable<ItemPreview> GetAllItems(AspNetUser user, string searchString = "")
        {
            if(user != null)
            {
                //base list, where all the items will be stored

                //take passwords
                var items = user.Passwords
                .OrderByDescending(p => p.NumOfVisits)
                .ThenBy(p => p.Name)
                .Select(item => new ItemPreview(item.Id, item.Name, item.Username, TypeOfItems.Password))
                .Union//take wifis
                (user.Wifis
                .OrderByDescending(p => p.NumOfVisits)
                .ThenBy(p => p.Name)
                .Select(item => new ItemPreview(item.Id, item.Name, TypeOfItems.Wifi.ToString(), TypeOfItems.Wifi)));

                if (!string.IsNullOrEmpty(searchString))
                {
                    items = items.Where(i => i.Title.Contains(searchString));
                }

                return items;
            }
            return null;
        }
        private AspNetUser GetCurrentUser()
        {
            return db.AspNetUsers.Find(User.Identity.GetUserId());
        }
    }
}
