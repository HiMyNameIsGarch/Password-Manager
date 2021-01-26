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
            var previews = GetAllItems(GetCurrentUser());

            return Ok(previews);
        }
        //GET API/EntireItems/searchString
        public IHttpActionResult Get(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return BadRequest("Your item is null or empty!");
            }
            var previews = GetAllItems(GetCurrentUser(), searchString);
            return Ok(previews);
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
                .Select(item => new ItemPreview(item.Id, item.Name, "Wi-Fi", TypeOfItems.Wifi)));

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
